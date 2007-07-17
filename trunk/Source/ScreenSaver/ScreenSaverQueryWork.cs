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
using Slickr;
using Slickr.Util.Windows;

namespace Slickr.ScreenSaver
{
	/// <summary>
	/// This handles all the query work for the screensaver.
	/// If we are in a power off state then don't query.
	/// When CheckAll() is called and a query is not running and we don't have enough images queued, then a query is started.
	/// </summary>
	public class ScreenSaverQueryWork
	{

		private static ImageQueryWork queryWork;

		private static bool powerOff = false;

		private static DateTime lastCheckSecond = DateTime.MinValue;
		private static DateTime lastCheckMinute = DateTime.MinValue;

		private DownloadManager dm;
		
		/// <summary>
		/// Construct query work.
		/// </summary>
		/// <param name="dm">Download manager</param>
		public ScreenSaverQueryWork(DownloadManager dm)
		{
			this.dm = dm;			
		}

		/// <summary>
		/// Check to see if we need to run the query.
		/// If we are in a power off state then don't query.
		/// </summary>
		public void CheckAll() 
		{
			if (DateTime.Now.Subtract(lastCheckMinute).TotalSeconds > 60)
			{
				lastCheckMinute = DateTime.Now;
				bool powerOffOld = powerOff;
				Log.Debug("Checking power off...");
				powerOff = (Settings.Local.UsePowerOffSetting &&
					DateTime.Now.Subtract(Settings.Start).TotalSeconds > (SystemParams.GetPowerOffTimeout() + 60) &&
					SystemParams.GetPowerOffActive());

				Log.Debug("Power off: " + powerOff);
				if (powerOffOld && !powerOff)
				{
					Log.Debug("Resetting start time");
					Settings.Start = DateTime.Now;
				}				                
			}

			if (powerOff)
			{
				Log.Debug("POWER OFF");					
				return;
			}                				               

			if (!Settings.QueryOff)
			{
				if (queryWork != null && !queryWork.IsRunning && queryWork.Errored) 
				{
					if (LastErrorCode == 100) 
					{
						StatusManager.Status = String.Format("You need a valid application key. Go to the Control Panel | Display | Screensaver | Settings | Key tab.");
						StatusManager.Sticky = true;
						Settings.QueryOff = true;
						return;
					} 
					else 
					{
						StatusManager.Status = String.Format("Error: {0}", queryWork.Error.Message);
						Thread.Sleep(10000);
					}
					
				}

				CheckForQueryTrigger();

				/**
				if (queryWork != null) 
				{
					TimeSpan wait = queryWork.Wait;
					if (wait.TotalSeconds > 0)
					{
						StatusManager.Status = String.Format("({0:00}:{1:00})", wait.Minutes, wait.Seconds);
						//StatusManager.Status = "";
					}
				}*/
			}
			else
			{
				StatusManager.Status = String.Format("");
			}			
		}		

		/// <summary>
		/// Get last error code from image query.
		/// </summary>
		public int LastErrorCode 
		{ 
			get 
			{ 
				return (queryWork != null && queryWork.ImageQuery != null ? 
					queryWork.ImageQuery.LastErrorCode : 0);
			}
		}
		/// <summary>
		/// Check for the query trigger.
		/// If its not running and we don't have enough images queued, then start a query.
		/// </summary>
		private void CheckForQueryTrigger() 
		{		
			if (queryWork == null) 
			{
				Log.Debug("Building query work");
				queryWork = ImageQueryWorkFactory.BuildQueryWork(dm, -1);
			}

			if (queryWork == null) 
			{
				Log.Debug("No query work available");
				return;
			}

			//TimeSpan diff = DateTime.Now.Subtract(queryWork.FinishTime);
			//TimeSpan wait = new TimeSpan(0, 0, Settings.QueryWait);
			//diff.CompareTo(wait) == 1 ||			

			//Log.Debug("Query running: " + queryWork.IsRunning() + ", ImageDirectory.Count: " 
			//	+ dm.ImageDirectory.Count + ", MinCount: " + Settings.Local.MinImageCount);
			if (!queryWork.IsRunning && (dm.ImageDirectory.Count < Settings.Local.MaxRecentSize))  
			{
				Log.Debug("Starting query work");				
				queryWork.Start();				
			} 
			else 
			{
				Log.Debug("Query work waiting");
			}
		}
	}
}
