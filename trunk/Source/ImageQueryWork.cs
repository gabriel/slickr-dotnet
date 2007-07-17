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


namespace Slickr
{	

	/// <summary>
	/// Image query worker.
	/// </summary>
	public class ImageQueryWork
	{

		private static int threadCount = 1;
		private Thread thread;

		private DownloadManager dm;
		private int page;
		
		private DateTime finishTime = DateTime.MinValue;

		private Exception error;

		private bool isRunning = false;

		private ImageQuery query;
		private SearchResults results;
		
		/// <summary>
		/// Construct query worker.
		/// </summary>
		/// <param name="query">Query</param>
		/// <param name="dm">Download manager</param>
		/// <param name="page">Page</param>
		public ImageQueryWork(ImageQuery query, DownloadManager dm, int page)
		{
			this.query = query;
			this.dm = dm;
			this.page = page;
		}

		/// <summary>
		/// Get the image query.
		/// </summary>
		public ImageQuery ImageQuery { get { return query; } }

		/// <summary>
		/// Start work (asynchronous).
		/// </summary>
		public void Start() 
		{
			if (thread != null && thread.IsAlive) throw new ApplicationException("Previous thread is still alive");
			thread = new Thread(new ThreadStart(Run));
			thread.IsBackground = true;
			thread.Priority = ThreadPriority.BelowNormal;
			thread.Name = "Image Query Work-" + threadCount++;
			isRunning = true;
			thread.Start();
		}

		/// <summary>
		/// Do the work.
		/// </summary>
		/// <returns>Search results</returns>
		public SearchResults Query() 
		{
			SearchResults results = query.Query(dm, (page == -1 ? Settings.CurrentPage : page));
			return results;
		}

		/// <summary>
		/// Delegate for ThreadStart
		/// </summary>
		public void Run() 
		{						
			Log.Debug("Running Query");
			
			try 
			{
				// Do work
				results = Query();
				if (results != null) dm.Download(results);
                results.DidFinishDownloading();
			} 
			catch(Exception e) 
			{
                Log.Error("Error in query", e);
				error = e;
			}
			finishTime = DateTime.Now;
			Log.Debug("Query work finished at: " + finishTime);
			isRunning = false;			
		}

		/// <summary>
		/// Check if running
		/// </summary>		
		public bool IsRunning { get { return isRunning || (thread != null && thread.IsAlive); } }

		/// <summary>
		/// Check if errored
		/// </summary>
		public bool Errored { get { return error != null; } }

		/// <summary>
		/// Get when work finished
		/// </summary>
		public DateTime FinishTime { get { return finishTime; } }

		/// <summary>
		/// Get error
		/// </summary>
		public Exception Error { get { return error; } }								

		/// <summary>
		/// Get wait time for next query
		/// </summary>
		public TimeSpan Wait 
		{ 
			get 
			{ 
				TimeSpan diff = DateTime.Now.Subtract(FinishTime);
                TimeSpan wait = new TimeSpan(0, 0, Settings.QueryWait);
                return wait.Subtract(diff); 
			} 
		}		
	}
}
