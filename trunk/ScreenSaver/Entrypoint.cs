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
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Collections;
using System.IO;
using System.Text;
using CommonCode;
using Slickr;

namespace Slickr.ScreenSaver
{	
	/// <summary>
	/// Entry point into ScreenSaver application.
	/// </summary>
	public class EntryPoint 
	{			
	
		#region Exception Handling
		public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)		
		{
			try 
			{					
				Log.Error("Uncaught exception", e.Exception);

				DialogResult result = MessageBox.Show("Slickr encountered an unexpected error. Would you like to view the details?", 
					"Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation );
				if (result == DialogResult.Yes) 
				{
					ExceptionDialog ed = new ExceptionDialog((Exception)e.Exception);
					ed.ShowDialog();
				}
			} 
			catch(Exception ee)
			{
				Log.Error("Error showing exception dialog", ee);
			}
			Application.Exit();
		}
		#endregion

		/// <summary>
		/// A bad args parser.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="argPrefix"></param>
		/// <param name="argHandle"></param>
		private static void ParseArgsToPrefixAndArgInt(string[] args, out string argPrefix, out int argHandle)
		{
			string curArg;
			char[] SpacesOrColons =  {' ', ':'};

			switch(args.Length)
			{
				case 0: // Nothing on command line, so just start the screensaver.
					argPrefix = "/s";
					argHandle = 0;
					break;

				case 1:
					curArg = args[0];
					argPrefix = curArg.Substring(0, 2);
					curArg = curArg.Replace(argPrefix, ""); // Drop the slash /? part.
					curArg = curArg.Trim(SpacesOrColons); // Remove colons and spaces.
					argHandle = curArg == "" ? 0 : int.Parse(curArg); // if empty return zero. else get handle.
					break;

				case 2:
					argPrefix = args[0].Substring(0,2);
					argHandle = int.Parse(args[1].ToString());
					break;

				default:
					argHandle = 0;
					argPrefix = "";
					break;

			}
		}

		/// <summary>
		/// Main is the entry point.
		/// </summary>
		/// <param name="args">
		/// <description>
		/// Command line arguments.
		/// </description>
		/// </param>
		[STAThread] 
		static void Main(string[] args) 
		{
            //Log.Enabled = true;
			try 
			{
				Application.EnableVisualStyles();
				Application.DoEvents();  // This is currently required to bypass a framework 1.1 bug.
			} 
			catch(Exception e) 
			{
				Log.Error("Error enabling visual styles", e);
			}						

			// These are just some variables to argument data.
			string argPrefix;
			int argHandle; // Will be 0 if no number found.

			if (args.Length > 2) 
			{
				MessageBox.Show("Invalid arguments.");
				return;
			}

			ParseArgsToPrefixAndArgInt(args, out argPrefix, out argHandle);

			try
			{
				Log.Debug("Argument prefix: " + argPrefix);
				switch (argPrefix)
				{
					case "/a": // password dialog desired. I'll exit program instead of risking locking someone out due to a bug
					case "/A":
						break; // in my code, or a change in the OS code.

					case "/c": // Show configuration screen.
					case "/C":
						System.Windows.Forms.Application.Run(new ConfigurationForm());				
						break; 

					case "/p": 
					case "/P":
						if (argHandle == 0) goto case "/s"; // No handle found, do a fullscreen screensaver.
						else
						{
							MiniPreview mpTemp = new MiniPreview();
							mpTemp.DoMiniPreview(argHandle, true);
							mpTemp = null; // This does not execute until preview is closed.
						}
						break;

					case "/s": // Start screensaver.				
					case "/S":		
						ScreenSaverViewer screenSaver = new ScreenSaverViewer();
						screenSaver.Run();
						break;
						
					default:   
						Log.Debug("Unknown argument: " + argPrefix);
						break;
				} 
			}		
			catch (Exception e)
			{	
				Log.Error("Unhandled Error", e);				
				Environment.Exit(1);
			}            
			Environment.Exit(0);
			
		}
	} 
}