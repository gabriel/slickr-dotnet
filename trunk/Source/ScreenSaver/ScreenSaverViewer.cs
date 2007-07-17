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
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Slickr;
using Slickr.Util;
using Slickr.Util.Windows;

namespace Slickr.ScreenSaver
{
	/// <summary>
	/// Screensaver.
	/// </summary>
	public class ScreenSaverViewer
	{
		private Viewer screenSaver;
		
		private DownloadManager dm;
		private ImageDirectory directory;		
		private ScreenSaverQueryWork queryWork;

		private Thread thread;

		private const bool QUERY_ENABLED = true;

		public ScreenSaverViewer() { }

		/// <summary>
		/// Start the screensaver.
		/// </summary>
		public void Run() 
		{			
			screenSaver = new Viewer(); //true, -1, -1, Screen.AllScreens.Length, false);			
			
			for(int i = 0; i < Screen.AllScreens.Length; i++) 
				screenSaver.GetScreen(i).KeyDown += new KeyEventHandler(this.Form_KeyDown);             // On KeyDown Event Call Form_KeyDown

			if (Settings.Local.FlickrMode == Settings.FlickrMode.LOCAL) 
			{
				directory = new ImageDirectory(Settings.BaseDirectory, null, true, false, true);				
			} 
			else 
			{
				directory = new ImageDirectory(Settings.BaseDirectory, Settings.SearchTypeLabel, false, true, false);
			}

			dm = new DownloadManager(directory, 
				new DownloadManager.RequestListener(DownloadRequestListener), 
				new DownloadManager.DownloadListener(DownloadListener));
			queryWork = new ScreenSaverQueryWork(dm);

			thread = new Thread(new ThreadStart(ThreadRun));
			thread.IsBackground = true;
			thread.Priority = ThreadPriority.Lowest;
			thread.Name = "Screen Saver Query Thread";
			if (QUERY_ENABLED) thread.Start();

			Log.Debug("Calling RunMeTillShutdown");
			//screenSaver.RunMeTillShutdown();
			screenSaver.RunMeTillShutdown();
			Log.Debug("RunMeTillShutdown finished");
		}

		/// <summary>
		/// ThreadRun method. Calls query to search for images and also initializes images.
		/// This method never exits but is a "background" daemon thread.
		/// </summary>
		public void ThreadRun() 
		{			
			directory.Init();
			bool forward = true;

			while(true) 
			{
				try 
				{
					// Oscillate between moving forward and backwards in the array traversal
					for(int i = 0; i < screenSaver.ScreenCount; i++) 						
						CheckScreen((forward ? i : (screenSaver.ScreenCount - 1) - i), queryWork);						
					forward = !forward;
				} 
				catch(Exception e) 
				{
					Log.Error("Error checking screen", e);
				}

				try 
				{
					if (Settings.Local.FlickrMode != Settings.FlickrMode.LOCAL) 
						queryWork.CheckAll();
				}
				catch(Exception e) 
				{
					Log.Error("Error checking query", e);
				}

				Thread.Sleep(1000);
			}
		}

		/// <summary>
		/// Check all the screens to make sure that have images queued and initialized and ready for viewing.
		/// </summary>
		/// <param name="index">Screen index</param>
		/// <param name="queryWork">Current query work</param>
		public void CheckScreen(int index, ScreenSaverQueryWork queryWork) 
		{
			//Log.Debug("Checking screen: " + index);
			if (screenSaver.GetScreen(index).GetContext().QueuedCount < 2) 			
			{
				bool randomFill = queryWork.LastErrorCode > 0;

				FileInfo file = directory.GetImageFile(randomFill);				
				if (file != null) 
				{				
					int Width = screenSaver.GetScreen(index).Width;
					int Height = screenSaver.GetScreen(index).Height;
					int MaxTextureSize = screenSaver.GetScreen(index).MaxTextureSize;
					Log.Debug("Prefetching: " + file + ", Width: " + Width + ", Height: " + Height);
					ImageInitializeWork imageInitializer = new ImageInitializeWork();
					imageInitializer.SetParameters(file, Width, Height, MaxTextureSize);					
					SlickrImage image = imageInitializer.Initialize();					
					if (image != null && image.Initialized) 
					{
						image.AutoExpire = false;
						screenSaver.GetScreen(index).GetContext().AddImage(image);
						screenSaver.GetScreen(index).GetContext().AutoExpireVisible();
					}
				} 
			}
		}
		
		/// <summary>
		/// Download request listener, held by download manager.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="status"></param>
		/// <param name="fi"></param>
		/// <param name="index"></param>
		/// <param name="totalCount"></param>
		public void DownloadRequestListener(Uri uri, RequestStatus status, FileInfo fi, int index, int totalCount) 
		{
			switch(status) 
			{
				case RequestStatus.CONNECTING:
					StatusManager.Status = "Connecting to Flickr";
					//StatusManager.Status = "Connecting: " + (uri != null ? uri.Host : "")
					//	   + (index != -1 && totalCount != -1 ? " [" + index + "/" + totalCount + "]" : "")); 
					break;

				case RequestStatus.WAITING:
					//SetStatus("Waiting...");
					break;

				case RequestStatus.EXISTS:
					if (fi != null) directory.AddNewImage(fi);
					break;				

				case RequestStatus.FINISHED: 
				{
					Log.Debug("Finished: " + fi);
					//if (index == totalCount) SetStatus("Finished query");
					StatusManager.Status = "";
					if (fi != null) directory.AddNewImage(fi);
					break;
				}
			}
			
		}

		/// <summary>
		/// Download listener, held by download manager.
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="bytes"></param>
		/// <param name="total"></param>
		/// <param name="fi"></param>
		/// <param name="index"></param>
		/// <param name="totalCount"></param>
		/// <param name="desc"></param>
		public void DownloadListener(Uri uri, long bytes, long total, FileInfo fi, int index, int totalCount, String desc) 
		{
			if (index != totalCount) 
			{
				//String name = (uri.Segments.Length > 0 ? uri.Segments[uri.Segments.Length-1] : "");
				StatusManager.Status = desc + String.Format(" [{0}k", bytes / 1000) + "/" + String.Format("{0}k]", total / 1000);
				//" [" + index + "/" + totalCount + "]");
			} 
			else 
			{				
				
			}
		}

		public bool Done 
		{
			get { return  screenSaver != null ? screenSaver.Done : false; }
			set { if (screenSaver != null) screenSaver.Done = value; }
		}

		#region Form_KeyDown(object sender, KeyEventArgs e)
		/// <summary>
		///     Handles the form's key down event.
		/// </summary>
		/// <param name="sender">
		///     The event sender.
		/// </param>
		/// <param name="e">
		///     The event arguments.
		/// </param>
		private void Form_KeyDown(object sender, KeyEventArgs e) 
		{
			Log.Debug("Sender: " + sender);
			switch(e.KeyCode) 
			{
				case Keys.Right:
				case Keys.N:
					for(int i = 0; i < screenSaver.ScreenCount; i++)
						screenSaver.GetScreen(i).GetContext().Skip();
					break;

				case Keys.D:
					FileInfo currentFile = screenSaver.GetScreen().GetContext().CurrentFile;
					if (currentFile != null) 
					{
						Bitmap bitmap = new Bitmap(currentFile.FullName);						
						string saveAsPath = Settings.GetPicturesDirectory() + "/desktop.bmp";
						bitmap.Save(saveAsPath, System.Drawing.Imaging.ImageFormat.Bmp);

						Log.Debug("Setting wallpaper: " + saveAsPath);
						WindowsUtils.SetWallpaper(saveAsPath);
					}
					break;

				case Keys.Space:
					String id = screenSaver.GetScreen().GetContext().CurrentId;
					Log.Debug("Got id: " + id);
					if (id != null) 
					{
						String url = FlickrImageQuery.GetUrl(id);
						System.Diagnostics.Process.Start(url);

					}
					Done = true;
					break;

				default:
					Done = true;
					break;
			}			
		}
		#endregion Form_KeyDown(object sender, KeyEventArgs e)		
	}
}
