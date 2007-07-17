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
using System.Collections;
using System.IO;
using Slickr.Util;

namespace Slickr
{
	/// <summary>
	/// Summary description for ImageDirectory.
	/// </summary>
	public class ImageDirectory
	{
		// Random queue
		private Queue queue = new Queue();

		// Recent queue
		private Queue recentQueue = new Queue();

		// History
		private ArrayList history = new ArrayList();

		// Base directory info
        private DirectoryInfo baseDirectoryInfo;
		// Complete directory info
		private DirectoryInfo directoryInfo;      
  
		// Whether clean/pruning is enabled
		private bool cleanEnabled = false;
		// Directory file count
		private int totalDirCount = 0;

		// Files in directory
		private SortedList files = new SortedList();

		// Random
		private Random random;

		// File count
		private int count = 0;

		// Last image returned
        private FileInfo lastImageGet;

		// Last time a clean was performed
        private DateTime lastClean = DateTime.MinValue;

		// Whether to include sub directories
		private bool includeSubDirectories = false;
		// Whether fill is from random or recent queue
		private bool isRandomFill = false;

		/// <summary>
		/// Construct an image directory.
		/// If you specify a sub-directory:
		///  (1) The read is non-recursive
		///  (2) The directory is pruned so that its size is less than the max cache size
		/// Otherwise, the read is recursive and the directory is not pruned.
		/// </summary>
		/// <param name="directory">Base directory</param>
		/// <param name="subDirectory">Sub directory</param>
		/// <param name="includeSubDirectories">The directory read is recursive</param>
		/// <param name="cleanEnabled">If true the directory is pruned so that its size is less than the max cache size</param>
		/// <param name="isRandomFill">If true calls to GetImageFile() return a random file from the directory, otheriwise it is returned from the recent queue</param>
		public ImageDirectory(String directory, String subDirectory, bool includeSubDirectories, bool cleanEnabled, bool isRandomFill)
		{
			random = new Random((int)DateTime.Now.Ticks);
            baseDirectoryInfo = new DirectoryInfo(directory);
			this.cleanEnabled = cleanEnabled;
			this.includeSubDirectories = includeSubDirectories;
			this.isRandomFill = isRandomFill;

			if (subDirectory != null) directoryInfo = new DirectoryInfo(directory + "/" + subDirectory);											
			else directoryInfo = new DirectoryInfo(directory);							
			if (!directoryInfo.Exists) directoryInfo.Create();
		}

		/// <summary>
		/// Get the directory size (sum of all the file sizes).
		/// </summary>
		/// <returns></returns>
        public long GetDirSize()
        {
            FileInfo[] fileInfos = baseDirectoryInfo.GetFiles();
            long amount = 0;
            foreach (FileInfo fi in fileInfos)
                amount += fi.Length;

            return amount;
        }

		/// <summary>
		/// The current directory.
		/// </summary>
        public String Directory { get { return directoryInfo.FullName; } }

		/// <summary>
		/// Trim the directory to the max cache size. Deletes oldest images first.
		/// </summary>
		/// <param name="maxCacheSizeMB">Max cache size</param>
		private void CleanDirectory(double maxCacheSizeMB) 
		{
			if (!cleanEnabled) 
			{
				Log.Debug("Cleaning disabled");
				return;
			}
            Log.Debug("Cleaning directory, " + baseDirectoryInfo);
            //FileInfo[] fileInfos = baseDirectoryInfo.GetFiles();            
            ArrayList fileList = new ArrayList();
            GetAllFiles(baseDirectoryInfo, fileList);            
            Log.Debug("Total files: " + fileList.Count);            
            fileList.Sort(new FilesCreateDateComparer(false));		
			double amount = 0;
			double deleteAmount = 0;
			int deleteCount = 0;
			bool deleteOn = false;
            foreach (FileInfo fi in fileList)
            {
				if (fi.Extension.EndsWith("tmp")) 
				{
					Log.Debug("Deleting tmp file: " + fi);
					fi.Delete();
					continue;
				}

				double l = fi.Length/1000000d;
				if (deleteOn || (amount + l) > maxCacheSizeMB) 
				{
					deleteOn = true;
                    //StatusManager.Status = "Cleaning: " + fi.DirectoryName;
					deleteAmount += fi.Length/1000000d;
					deleteCount++;
					Log.Debug("Deleting old file: " + fi + " (" + fi.CreationTime + "/" + fi.DirectoryName + ")");
					fi.Delete();
				} 
				else 
				{
					amount += l;
				}				
			}
            Log.Debug("Amount: " + amount);
			Log.Debug("Deleted (for size) " + deleteCount + " files of size: " + deleteAmount);            
		}

		/// <summary>
		/// Get all files, and put them in the specified file list.
		/// </summary>
		/// <param name="directory">Directory to search</param>
		/// <param name="fileList">File list</param>
        private void GetAllFiles(DirectoryInfo directory, SortedList fileList)
        {
            Log.Debug("Reading directory, " + directory);
            FileInfo[] fileInfos = directory.GetFiles();
            //fileList.AddRange(fileInfos);

			foreach(FileInfo fi in fileInfos) 
			{
				if (!fi.Extension.EndsWith("tmp") && !fi.Extension.EndsWith("ignore"))
					files.Add(fi.FullName, fi);
			}	

            DirectoryInfo[] directories = directory.GetDirectories();
            foreach (DirectoryInfo di in directories)
                GetAllFiles(di, fileList);
        }

		/// <summary>
		/// Get all files from directory, and put them in the specified file list.
		/// </summary>
		/// <param name="directory">Directory to search</param>
		/// <param name="fileList">File list</param>
		private void GetAllFiles(DirectoryInfo directory, ArrayList fileList)
		{
			Log.Debug("Reading directory, " + directory.FullName);
			FileInfo[] fileInfos = directory.GetFiles();
			fileList.AddRange(fileInfos);

			DirectoryInfo[] directories = directory.GetDirectories();
			foreach (DirectoryInfo di in directories)
				GetAllFiles(di, fileList);

		}

		/// <summary>
		/// Get image with id.
		/// </summary>
		/// <param name="id">Id</param>
		/// <returns>Image with id</returns>
		public FileInfo GetFile(String id) 
		{
			if (id == null) return null;
			FileInfo[] files = directoryInfo.GetFiles("[*]" + id + "*.jpg");
			if (files != null && files.Length > 0) 
			{
				return files[0];
			}
			return null;
		}

		/// <summary>
		/// Check if image with id exists.
		/// </summary>
		/// <param name="id">Id</param>
		/// <returns>True if exists</returns>
		public bool Exists(String id) 
		{
			if (GetFile(id) != null) 
			{
				//Log.Debug("Exists #" + files.Length + ", 0: " + files[0].Name);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Read the directory. Images recently viewed are removed from the files listing.
		/// </summary>
		private void ReadDirectory() 
		{
            if (DateTime.Now.Subtract(lastClean).TotalMinutes > Settings.Local.CleanWait)
            {
                lastClean = DateTime.Now;
                CleanDirectory(Settings.Local.MaxCacheSizeMB);
            }
			
			if (!includeSubDirectories) 
			{
				FileInfo[] fileInfos = directoryInfo.GetFiles();
				files = new SortedList();

				foreach(FileInfo fi in fileInfos) 
				{
					if (!fi.Extension.EndsWith("tmp") && !fi.Extension.EndsWith("ignore"))
						files.Add(fi.FullName, fi);
				}	
			} 
			else 
			{
				files = new SortedList();
				GetAllFiles(directoryInfo, files);
			}								
			
			if (files.Count > 0) Log.Debug("Read " + files.Count + " from " + directoryInfo);
			count = files.Count;
			if (history.Count > 0 && files.Count > 1) 
			{
				Log.Debug("Read " + files.Count + " from " + directoryInfo);
                int maxSize = (int)Math.Ceiling(files.Count / Settings.Local.HistoryDivider);

                int maxHistorySize = (Settings.Local.MaxHistorySize < maxSize ? Settings.Local.MaxHistorySize : maxSize);

				Log.Debug("Max history size: " + maxHistorySize + ", History size: " + history.Count + ", Files size: " + files.Count);
				if (history.Count > maxHistorySize) 
				{					
					history.RemoveRange(maxHistorySize, (history.Count-maxHistorySize));
					Log.Debug("Sized history down to " + history.Count);
				}


				foreach (FileInfo info in history)
                {
                    Log.Debug("Removing image that was in history: " + info.Name);
                    files.Remove(info.FullName);
                }
			}
			if (files.Count > 0) Log.Debug(files.Count + " total files available");
			totalDirCount = files.Count;
			//Log.Debug(files.Count + " total files available");			
		}

		/// <summary>
		/// Get the number of images in the recent queue.
		/// </summary>
        public int RecentCount 
        {
            get
            {
                return recentQueue.Count;
            }
        }

		/// <summary>
		/// Get the number images queued.
		/// </summary>
		public int Count 
		{ 
			get 
			{ 
				//EnsureFilled();
				return count + recentQueue.Count;
			}
		}

		/// <summary>
		/// Initialize the image directory.
		/// Cleans entries from the directory if we are over the max cache size.
		/// </summary>
		public void Init() 
		{
			CleanDirectory(Settings.Local.MaxCacheSizeMB);
			if (IsRandomFill) EnsureFilled();
		}

		/// <summary>
		/// Whether the image directory is using a random fill.
		/// If true images returned by GetImageFile() are obtained from the random queue.
		/// </summary>
		/// <returns>True if calls to GetImageFile() return from the random queue</returns>
		public bool IsRandomFill
		{
			get { return isRandomFill; } 
		}

		/// <summary>
		/// Ensure that the queue has images.
		/// </summary>
		/// <returns>True if the queue has images</returns>
		private bool EnsureFilled() 
		{
            if (queue.Count == 0) return (FillQueue(Settings.Local.FillAmount) > 0);
			return true;
		}

		/// <summary>
		/// Get an image file, either from the recent queue or from the random queue.
		/// When this method is called it will return null if the image file from any queue is equal to the last one returned.
		/// </summary>
		/// <param name="randomFill">If true, overrides IsRandomFill setting for this call</param>
		/// <returns>File info for image</returns>
		public FileInfo GetImageFile(bool randomFill) 
		{			
            if (recentQueue.Count > 0)
            {
                FileInfo info = (FileInfo)recentQueue.Dequeue();
                history.Add(info);                
                lastImageGet = info;
                Log.Debug("Returning (hpq): " + info);
                return info;
            }            
	
        	if ((IsRandomFill || randomFill) && EnsureFilled()) 
            {
				if (totalDirCount <= 1) return null;

                FileInfo info = (FileInfo)queue.Dequeue();
                if (lastImageGet != null && info != null && info.Name.Equals(lastImageGet.Name))
                {
                    Log.Debug("Last image get equal to dequeue, returning null");
                    return null;
                }
		        history.Add(info);
                lastImageGet = info;
                Log.Debug("Returning: " + info);
			    return info;			
            }
			
            return null;
		}

		/// <summary>
		/// Add an image to the directory. Adds to the recent queue.
		/// </summary>
		/// <param name="fi">Image to add</param>
 		public void AddNewImage(FileInfo fi) 
		{
            Log.Debug("Adding to high priority queue: " + fi.Name);
			recentQueue.Enqueue(fi);
		}

		/// <summary>
		/// Fill the random queue with image files.
		/// </summary>
		/// <param name="amount">Number of images to place in the queue</param>
		/// <returns>The actual number of images placed in the queue</returns>
		private int FillQueue(int amount) 
		{			
			int count = 0;			

			if (files.Count == 0) 
				ReadDirectory();
			
			for(int i = 0; i < amount && files.Count > 0; i++) 
			{
				int index = random.Next(files.Count-1);				
				FileInfo info = (FileInfo)files.GetByIndex(index);
				files.RemoveAt(index);								
				count++;
				Log.Debug("Queueing file: " + info);
				queue.Enqueue(info);
			}
			return count;		
		}
	}
}
