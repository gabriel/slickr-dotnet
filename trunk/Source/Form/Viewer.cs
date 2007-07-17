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
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Slickr.Util;
using Microsoft.Win32;

namespace Slickr
{
	/// <summary>
	/// This class handles the running of the viewing window(s).
	/// There may be more than one is we are doing a screensaver on multiple monitors.
	/// </summary>
	/// 
	public class Viewer
	{
		/// <summary>
		/// A count of the open screens.
		/// </summary>
		private int screenCount;

		/// <summary>
		/// False if not done running.
		/// </summary>
		private bool done = false;	
		
		/// <summary>
		/// Slickr form per display
		/// </summary>
		private SlickrOpenGLForm[] sf;

		/// <summary>
		/// Construct viewer windows for all screens, at fullcreen.
		/// </summary>
		public Viewer() : this(true, -1, -1, Screen.AllScreens.Length, false) {
		}
		
		/// <summary>
		/// Constrict viewing window(s)
		/// </summary>
		/// <param name="fullScreen">Whether fullscreen</param>
		/// <param name="width">Width of window, -1 is ignored</param>
		/// <param name="height">Height of window, -1 is ignored</param>
		/// <param name="screenCount">Number of screens (windows) to show</param>
		/// <param name="standAlone">If true, this is a standalone client mode and not a screensaver type window</param>
		public Viewer(bool fullScreen, int width, int height, int screenCount, bool standAlone)
		{
            /**
            SystemEvents.PowerModeChanged += new
                PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);

            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
             */
			StatusManager.Status = "Loading Slickr...";
			SlickrOpenGL.Init();

			this.screenCount = screenCount; // Get count of screens on system.		

			// Use an array of forms for each screen found just to get the array to create instances. 
			sf = new SlickrOpenGLForm[screenCount]; // We need to create actual instances to fill out the array.			
			Log.Debug("Screen count: " + screenCount);
			for (int i = 0; i < screenCount; i++)
			{							
				sf[i] = new SlickrOpenGLForm(i, width, height, fullScreen, standAlone);											
				if (standAlone) sf[i].GetContext().Center();
				if (!sf[i].CreateGLWindow("Slickr", 32)) 
				{
					Log.Error("Could not create GL Window on screen: " + i);
					return;
				}
			}
			StatusManager.Status = "";
		}
        /**
        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            //PowerStatus ps = new PowerStatus();
            
            switch (e.Mode)
            {
                case PowerModes.StatusChange: Log.Debug("Power StatusChange"); break;
                case PowerModes.Resume: 
                    Log.Debug("Power Resume");
                    Settings.PowerOff = false;
                    break;
                case PowerModes.Suspend: 
                    Log.Debug("Power Suspend");
                    Settings.PowerOff = true;
                    break;
            }

        }*/

        /**
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            Log.Debug("Display Settings change");
        }*/		

		public bool Done { get { return done; } set { done = value; } }

		public int ScreenCount { get { return screenCount; } }

		public SlickrOpenGLForm GetScreen(int index) { return sf[index]; }

		public SlickrOpenGLForm GetScreen() { return sf[0]; }

		public bool FullScreen { get { return sf[0].FullScreen; } set { sf[0].FullScreen = value; } }

		/// <summary>
		/// Run the viewing windows. This is a blocking call.
		/// </summary>
		public void RunMeTillShutdown() 
		{
			int interval = 25;

			while(!done) 
			{
				try 
				{
					DrawScene();				 
					Thread.Sleep(interval);
				} 
				catch(Exception e) 
				{
					Log.Error("Error in main run, will sleep 5 seconds...", e);
					Thread.Sleep(5000);
				}
			}
			Stop();
		}

		/// <summary>
		/// Stop the viewing windows.
		/// </summary>
		public void Stop() 
		{			
			// Shutdown
			for (int i = 0; i < screenCount; i++) 
			{
				Log.Debug("Killing GL Window");
				sf[i].KillGLWindow(); // Kill The Window
			}

		}

		/// <summary>
		/// Draw the scenes on all the viewing windows.
		/// </summary>
		public void DrawScene()
		{						
			for (int i = 0; i < screenCount; i++)
			{										
				if (!sf[i].DrawGLScene() || sf[i].GetContext().Done) 
				{  
					Log.Debug("Slickr is done");
					done = true;
				}
				else 
				{                                                      // Not Time To Quit, Update Screen
					sf[i].SwapBuffers();
				}					
			}
			Application.DoEvents();
		}	
			
	} // End of class ScreenSaver definition.
} // End of namespace.
