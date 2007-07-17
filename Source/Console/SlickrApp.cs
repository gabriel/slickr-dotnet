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

namespace Slickr.App
{
	/// <summary>
	/// Slickr Viewer.
	/// </summary>
	public class SlickrApp
	{

		private Viewer viewer;
		private Thread thread;
		private ImageDirectory directory;
		private DownloadManager dm;
		
		private int page = 1;
		private int index = 0;
		private SearchResults results;
		private bool paused = false;

		private AutoResetEvent resetEvent = new AutoResetEvent(false);
		private string action;

		private ContextMenu contextMenu;

		/// <summary>
		/// Construct Slickr Viewer.
		/// </summary>
		/// <param name="fullScreen"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public SlickrApp(bool fullScreen, int width, int height)
		{			
			viewer = new Viewer(fullScreen, width, height, 1, true);
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
			viewer.GetScreen().KeyDown += new KeyEventHandler(this.Form_KeyDown);			
			viewer.GetScreen().MouseDown += new MouseEventHandler(this.OnMouseEvent);         

			contextMenu = new ContextMenu();
			contextMenu.Popup += new EventHandler(MyPopupEventHandler);
			viewer.GetScreen().ContextMenu = contextMenu;
		}

		/// <summary>
		/// Popup handler
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MyPopupEventHandler(System.Object sender, System.EventArgs e) {			
			contextMenu.MenuItems.Clear();

			if (viewer.FullScreen) 
			{				
				MenuItem menuItem = new MenuItem("Windowed Mode", new EventHandler(GoWindowedMode));
				contextMenu.MenuItems.Add(menuItem);
			} 
			else 
			{
				MenuItem menuItem = new MenuItem("Full Screen Mode", new EventHandler(GoFullScreenMode));
				contextMenu.MenuItems.Add(menuItem);
			}			
		}	

		public void GoWindowedMode(object sender, EventArgs e) 
		{
			viewer.FullScreen = false;
		}

		public void GoFullScreenMode(object sender, EventArgs e) 
		{
			viewer.FullScreen = true;
		}

		/// <summary>
		/// Run query thread
		/// </summary>
		public void Run() 
		{			
			thread = new Thread(new ThreadStart(ThreadRun));
			thread.IsBackground = true;
			thread.Priority = ThreadPriority.Lowest;
			thread.Name = "Slickr Query Thread";
			thread.Start();

			Log.Debug("Calling RunMeTillShutdown");
			viewer.RunMeTillShutdown();
			Log.Debug("RunMeTillShutdown finished");
		}

		/// <summary>
		/// Run query, with page.
		/// </summary>
		/// <param name="page">Page</param>
		/// <param name="next">If a call to the next page, or false if to a previous page</param>
		private void RunQuery(int page, bool next) 
		{			
			Log.Debug("Running query on page: " + page);
			ImageQueryWork queryWork = ImageQueryWorkFactory.BuildQueryWork(dm, page);
			results = queryWork.Query();	
			if (next) index = -1;							
			else index = results.Count - 1;
		}

		/// <summary>
		/// ThreadRun method.
		/// </summary>
		private void ThreadRun() 
		{									
			while(true) 
			{
				
				resetEvent.WaitOne(5000, false);

				if (action != null) 
				{			
					if (results == null) RunQuery(page, true);

					if (action.Equals("next")) 
					{
						Next(false);									
					} 
					else if (action.Equals("prev")) 
					{						
						Previous(false);
					}
					action = null;
				} 
				else if (viewer.GetScreen(0).GetContext().QueuedCount < 2)
				{
					if (results == null) RunQuery(page, true);
					Next(true);
				}	
			}
		}

		/// <summary>
		/// Show next image.
		/// </summary>
		/// <param name="queue">Whether to queue next image, or skip to it</param>
		public void Next(bool queue) 
		{
			if (results == null) return;

			if (index >= (results.Count-1)) 
				RunQuery(++page, true);
							
			if (index < (results.Count-1))
				ShowImage(++index, queue);			
		}

		/// <summary>
		/// Show previous image
		/// </summary>
		/// <param name="queue">Whether to queue next image, or skip to it</param>
		public void Previous(bool queue) 
		{
			if (results == null) return;

			if (index < 1 && page > 1) 
				RunQuery(--page, false);
						
			if (index > 0)
				ShowImage(--index, queue);	
		}

		/// <summary>
		/// Show image at index.
		/// </summary>
		/// <param name="index">Index</param>
		public void ShowImage(int index, bool queue) 
		{			
			if (results == null) return;
			Log.Debug("Showing index: " + index);
			String id = results.GetId(index);
		
			//FileInfo file = directory.GetImageFile();				
			FileInfo file = directory.GetFile(id);
			if (file == null) 
			{
				file = dm.Download(results, index, results.Count);
			}

			if (file != null) 
			{				
				int Width = viewer.GetScreen().Width;
				int Height = viewer.GetScreen().Height;
				int MaxTextureSize = viewer.GetScreen().MaxTextureSize;
				Log.Debug("Prefetching: " + file + ", Width: " + Width + ", Height: " + Height);
				ImageInitializeWork imageInitializer = new ImageInitializeWork();
				imageInitializer.SetParameters(file, Width, Height, MaxTextureSize);					
				SlickrImage image = imageInitializer.Initialize();
				image.AutoExpire = !paused;				
				image.SourcePage = page;
				image.SourceIndex = index;
				if (image != null && image.Initialized) 
				{
					if (!queue) viewer.GetScreen().GetContext().SetImage(image);
					else viewer.GetScreen().GetContext().AddImage(image);
				}
			}			
		}

		/// <summary>
		/// Request listener held by DownloadManager
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
					break;

				case RequestStatus.WAITING:
					break;

				case RequestStatus.FINISHED: 
				{
					Log.Debug("Finished: " + fi);
					StatusManager.Status = "";
					break;
				}
			}
			
		}

		/// <summary>
		/// Download listener held by DownloadManager
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
				StatusManager.Status = desc + String.Format(" [{0}k", bytes / 1000) + "/" + String.Format("{0}k]", total / 1000);
			} 			
		}

		/// <summary>
		/// Key listener.
		/// Listens for next/prev, etc.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form_KeyDown(object sender, KeyEventArgs e) 
		{
			switch(e.KeyCode)
			{
				case Keys.Right:
					action = "next";
					resetEvent.Set();						
					break;

				case Keys.Left:
					action = "prev";
					resetEvent.Set();						
					break;

				case Keys.Escape:
					viewer.Done = true;
					break;

				case Keys.D:
					FileInfo currentFile = viewer.GetScreen().GetContext().CurrentFile;
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
					String id = viewer.GetScreen().GetContext().CurrentId;
					Log.Debug("Got id: " + id);
					if (id != null) 
					{
						String url = FlickrImageQuery.GetUrl(id);
						System.Diagnostics.Process.Start(url);

					}
					viewer.Done = true;
					break;
			}
		}		

		/// <summary>
		/// OnMouseEvent. Has user moved or clicked the mouse?<br>
		/// if so, set stopNow = true and close the form.<br>
		/// Paint event will stop drawinig when stopNow = true<br>
		/// and inform all insect instances to stop drawing.<br>
		/// Initialize your drawing routines in the load event or earlier.<br>
		/// Your drawing routines should go into the paint event.
		/// </summary>
		/// <param name="sender"> Unused</param>
		/// <param name="e"> Has a count of mouse clicks and mouse X, Y location.</param>
		private void OnMouseEvent(object sender, System.Windows.Forms.MouseEventArgs e)
		{

			Form form = viewer.GetScreen();

			//Log.Debug("Start OnMouseEvent");
			if (e.Button == MouseButtons.Left && e.Clicks == 2 
				//&& this.WindowState == FormWindowState.Maximized
				&& form.FormBorderStyle == FormBorderStyle.None
				&& form.TopMost)                    
			{
				viewer.FullScreen = false;
			} 
			else if (e.Button == MouseButtons.Left && e.Clicks == 2)
			{
				viewer.FullScreen = true;								
			}

		}
	}
}
