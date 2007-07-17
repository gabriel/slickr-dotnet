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
using System.Threading;
using System.IO;

namespace Slickr
{	

	public delegate void ImageInitialized(SlickrImage image);	

	/// <summary>
	/// Worker for initializing images, asynchronously.
	/// Builds a SlickrImage object from a file path.
	/// </summary>
	public class ImageInitializeWork
	{
		private static int threadCount = 1;
		private Thread thread;

		private ImageInitialized addImage;

		private SlickrImage image;

		private FileInfo fileInfo;
		private int screenWidth;
		private int screenHeight;
		private int maxTextureSize;

		private bool isRunning = false;		

		public ImageInitializeWork() { }

		/// <summary>
		/// Construct work.
		/// </summary>
		/// <param name="addImage">Callback delegate</param>
		public ImageInitializeWork(ImageInitialized addImage) { 
			this.addImage = addImage;
		}
		
		/// <summary>
		/// Set the work parameters.
		/// </summary>
		/// <param name="fileInfo">File</param>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// <param name="maxTextureSize">Max texture size</param>
		public void SetParameters(FileInfo fileInfo, int screenWidth, int screenHeight, int maxTextureSize)
		{
			this.fileInfo = fileInfo;
			this.screenWidth = screenWidth;
			this.screenHeight = screenHeight;
			this.maxTextureSize = maxTextureSize;
		}

		/// <summary>
		/// Start work asynchronously
		/// </summary>
		public void Start() 
		{
			Start(ThreadPriority.Lowest);
		}

		/// <summary>
		/// Start work asynchronously
		/// </summary>
		/// <param name="priority">Work with priority</param>
		public void Start(ThreadPriority priority) 
		{
			if (thread != null && thread.IsAlive) throw new ApplicationException("Previous thread is still alive");
			thread = new Thread(new ThreadStart(Run));
			thread.IsBackground = true;
			thread.Priority = priority;
			thread.Name = "Image Initialize Work-" + threadCount++;
			isRunning = true;
			thread.Start();
		}

		/// <summary>
		/// Run delegate for ThreadStart
		/// </summary>
		public void Run() 
		{
			Initialize();
		}

		/// <summary>
		/// Do with work (synchronous)
		/// </summary>
		/// <returns>Slickr image</returns>
		public SlickrImage Initialize() 
		{
            image = new SlickrImage(fileInfo);			
			try 
			{
				Log.Debug("Initializing image: " + fileInfo);
				image.InitializeImage(screenWidth, screenHeight, maxTextureSize);			
				if (addImage != null) addImage(image);			
			} 
			catch(Exception e) 
			{
				Log.Error("Error initializing image", e);
				image = null;
			}
			isRunning = false;			
			return image;
		}

		/// <summary>
		/// Get image.
		/// </summary>
		public SlickrImage Image { get { return image; } }

		/// <summary>
		/// Check if work is running.
		/// </summary>
		/// <returns></returns>
		public bool IsRunning() { return isRunning || (thread != null && thread.IsAlive); }
	}
}
