#region License
/*
 *  Copyright (C) 2006-2007 Gabriel Handford <gabrielh@gmail.com>
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation; either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 *  MA 02110-1301, USA.
 */
#endregion License

using System;
using System.Net;
using System.IO;
using Slickr.Util;
using System.Threading;
using System.Collections;


namespace Slickr
{
	/// <summary>
	/// Download manager.
	/// </summary>
	public class DownloadManager
	{

		public delegate void RequestListener(Uri uri, RequestStatus status, FileInfo fi, int index, int totalCount);
		public delegate void DownloadListener(Uri uri, long bytes, long total, FileInfo fi, int index, int totalCount, String desc);        

		private ImageDirectory imageDirectory;
        private IList requestListeners = new ArrayList();
		private DownloadListener listener;

		// If true, the downloader will automatically pause if directory recent queue has alot of images.
        private bool waitOnRecent = true;
		// Sleep between heavy calls (for Flickr)
		private int delay = 1000;

		/// <summary>
		/// Constrcut download manager.
		/// </summary>
		/// <param name="imageDirectory">Image directory</param>
		/// <param name="requestListener">Request listener</param>
		/// <param name="listener">Download listener</param>
        public DownloadManager(ImageDirectory imageDirectory, RequestListener requestListener, DownloadListener listener)
		{
			this.imageDirectory = imageDirectory;			
			this.listener = listener;
            AddListener(requestListener);
		}

		/// <summary>
		/// Get the image directory.
		/// </summary>
		public ImageDirectory ImageDirectory { get { return imageDirectory; } }

		/// <summary>
		/// Add request listener.
		/// </summary>
		/// <param name="listener"></param>
        public void AddListener(RequestListener listener)
        {
            requestListeners.Add(listener);
        }

		/// <summary>
		/// Notify delegates.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="status"></param>
		/// <param name="fi"></param>
		/// <param name="index"></param>
		/// <param name="totalCount"></param>
        public void NotifyRequestListeners(Uri uri, RequestStatus status, FileInfo fi, int index, int totalCount)
        {
            foreach(RequestListener requestListener in requestListeners)
                requestListener(uri, status, fi, index, totalCount);
        }

		/// <summary>
		/// Download URI to text.
		/// </summary>
		/// <param name="uri">URI</param>
		/// <returns></returns>
		public String Download(Uri uri) 
		{
			WebRequest request = WebRequest.Create(uri);
            NotifyRequestListeners(uri, RequestStatus.CONNECTING, null, -1, -1);
			WebResponse response = request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
            NotifyRequestListeners(uri, RequestStatus.DOWNLOADING, null, -1, -1);
			String s = reader.ReadToEnd();
            NotifyRequestListeners(uri, RequestStatus.FINISHED, null, -1, -1);
			return s;
		}		

		/// <summary>
		/// Download search results.
		/// </summary>
		/// <param name="results">Search results</param>
		public void Download(SearchResults results) 
		{
			int total = results.Count;
            int i = 0;
			while(i < total)
			{
                if (waitOnRecent && imageDirectory != null &&
                    imageDirectory.RecentCount >= Settings.Local.MaxRecentSize)
                {
                    Thread.Sleep(delay);
					StatusManager.Status = "";
                    NotifyRequestListeners(null, RequestStatus.WAITING, null, i + 1, total);
                    continue;
                }

				if (!imageDirectory.Exists(results.GetId(i)))
				{
					Thread.Sleep(delay);
					if (results.ShouldDownload(i)) 
					{
						Thread.Sleep(delay);
						StatusManager.Status = "Photo: " + results.GetDesc(i);
						Download(results, i, total);
					}
					else 
					{
						StatusManager.Status = "Photo: " + results.GetDesc(i) + " [Too small]";
						Log.Debug("Shouldn't download index: " + i);
					}
				} 
				else 
				{					
					String id = results.GetId(i);
					Log.Debug("Photo id: " + id + " exists");
					FileInfo fi = imageDirectory.GetFile(id);
					NotifyRequestListeners(null, RequestStatus.EXISTS, fi, i + 1, total);
				}
                i++;
			}
			StatusManager.Status = "";
		}

		/// <summary>
		/// Search load search results at index.
		/// </summary>
		/// <param name="r">Results</param>
		/// <param name="index">Index to download</param>
		/// <param name="totalCount">Total results count</param>
		/// <returns>Downloaded file</returns>
		public FileInfo Download(SearchResults r, int index, int totalCount) 
		{
            String path = null;
            FileInfo fi = null;
            String url = null;
            Uri uri = null;
            String desc = null;

            try
            {
                path = (imageDirectory != null ? imageDirectory.Directory + "/" : "") 
                    + r.GetDestinationFileName(index);
                fi = new FileInfo(path);

                url = r.GetUrl(index);
                uri = new Uri(url);

                desc = r.GetDesc(index);
            }
            catch (Exception e)
            {
                Log.Error("Error getting url and path information: " + path, e);
                return null;
            }
			
			try 
			{ 				
				if (fi.Exists) 
				{
					Log.Debug("Image already exists, aborting...");
                    NotifyRequestListeners(uri, RequestStatus.ABORTED, fi, index + 1, totalCount);
					return null;
				}

				String tmpPath = path + ".tmp";
				if (File.Exists(tmpPath)) File.Delete(tmpPath);

                NotifyRequestListeners(uri, RequestStatus.CONNECTING, fi, index + 1, totalCount);
				WebRequest request = WebRequest.Create(url);
				request.Timeout = Settings.Local.StreamTimeout * 1000;
				WebResponse response = request.GetResponse();
                NotifyRequestListeners(uri, RequestStatus.DOWNLOADING, fi, index + 1, totalCount);					

				FileStream fs = new FileStream(tmpPath, FileMode.CreateNew);
				BinaryWriter w = new BinaryWriter(fs);        
				Stream rs = response.GetResponseStream();
				int temp = 0;
				int total = 0;
				int bytesRead = 1;
				DateTime lastStatusTime = DateTime.MinValue;
				while(bytesRead > 0) 
				{
                    byte[] data = new byte[Settings.Local.BufferSize];
                    bytesRead = rs.Read(data, 0, Settings.Local.BufferSize);												
					if (bytesRead > 0) 
					{
						w.Write(data, 0, bytesRead);
						total += bytesRead;
						temp += bytesRead;

						if (listener != null) 
						{
							TimeSpan diff = DateTime.Now.Subtract(lastStatusTime);
							if ((diff.Ticks/TimeSpan.TicksPerMillisecond) > 333) 
							{
								listener(uri, total, response.ContentLength, fi, index+1, totalCount, desc);
								lastStatusTime = DateTime.Now;
							}
						}
					}
				}		
				fs.Close();			
				File.Move(tmpPath, fi.FullName);
				listener(uri, total, response.ContentLength, fi, index+1, totalCount, desc);
                NotifyRequestListeners(uri, RequestStatus.FINISHED, fi, index + 1, totalCount);
				response.Close();
			} 
			catch(Exception e) 
			{						   
                try {
				    if (fi != null) fi.Delete();
				} 
				catch(Exception ne) {
					Log.Error("Error cleaning up file", ne);
				}

                NotifyRequestListeners(uri, RequestStatus.ABORTED, fi, index + 1, totalCount);
				Log.Error("Error dowloading", e);
				return null;
			}
			return fi;
		}
	}
}
