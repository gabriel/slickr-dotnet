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
using Slickr;
using Slickr.Util.OpenGL;

namespace Slickr.App
{
	/// <summary>
	/// Show console viewer.
	/// </summary>
	public class EntryPoint
	{

		#region Exception Handling
		public static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)		
		{
			Handle(e.Exception);
		}

		static void OnExcept(Object sender,	UnhandledExceptionEventArgs args)
		{
			if (args != null) 
			{
				Exception e = (Exception)args.ExceptionObject;
				Handle(e);
			}
		}

		static void Handle(Exception e) 
		{
			Console.Error.WriteLine("Uncaught exception: {0}", e);
			Environment.Exit(2);
		}
		#endregion

		public EntryPoint() { }		

		/// <summary>
		/// Main is the entry point.
		/// </summary>
		/// <param name="args">
		/// <description>
		/// A string array that contains the arguments passed in on the command line. For more info see the source code.
		/// </description>
		/// </param>
		[STAThread]
		static void Main(string[] args) 
		{    
			//Log.Enabled = true;
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnExcept);
			Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);			

			Console.WriteLine("Slickr {0}", Settings.Version);		
			Console.WriteLine("Usage: /?");
			Console.WriteLine("");
			Console.Out.Flush();

			ArgParser ap = new ArgParser(args);
			if (ap.Exists("?")) 
			{
				Usage();
				Environment.Exit(0);
			}

			if (ap.Exists("Verbose")) 
			{
				Log.Enabled = true;				
			}

			if (ap.Exists("Log")) 
			{
				Log.Enabled = true;
				Log.FileName = ap["Log"].Value;
			}

			if (ap["VisualStyles"].Value != "false") 
			{
				Log.Debug("Enabling visual styles");
				Application.EnableVisualStyles();
				Application.DoEvents();  // This is currently required to bypass a framework 1.1 bug.
			}						
			
			try
			{			
				int width = ParseInt(ap, "Width", 800);
				int height = ParseInt(ap, "Height", 600);
				bool fullScreen = ap.Exists("FullScreen");

				if (ap.Exists("Config")) 
				{
					System.Windows.Forms.Application.Run(new ConfigurationForm());
				} 
				else if (ap.Exists("Window")) 
				{					
					SlickrApp app = new SlickrApp(fullScreen, width, height);
					app.Run();
				} 
				else if (ap.Exists("TestOpenGL")) 
				{
					OpenGlUtils.Test();
				}
				else 
				{
					Usage();
				}
			}
			catch (Exception e)
			{	
				Log.Error("Unhandled Error", e);				
				Environment.Exit(1);
			}            
			Environment.Exit(0);
			
		} // Main End.

		static void Usage() 
		{
			Console.WriteLine("{0,-30}", "THIS IS HIGHLY EXPERIMENTAL");
			Console.WriteLine("{0,-30}{1,-30}", "/Verbose", "Shows verbose logging");
			Console.WriteLine("{0,-30}{1,-30}", "/Log:filename", "Logs to file name");
			Console.WriteLine("{0,-30}{1,-30}", "/Config", "Configure settings");

			Console.WriteLine("{0,-30}{1,-30}", "/Window", "Start console");
			Console.WriteLine("{0,-30}{1,-30}", "/Width:n", "Set width (default: 800)");
			Console.WriteLine("{0,-30}{1,-30}", "/Height:n", "Set height (default: 600)");
			Console.WriteLine("{0,-30}{1,-30}", "/FullScreen", "Set full screen");

			Console.WriteLine("{0,-30}{1,-30}", "/VisualStyles:[true,false]", "Turn on visual styles (default: true)");
			Console.WriteLine("{0,-30}{1,-30}", "/TestOpenGL", "Test OpenGL capabilities");
			Console.WriteLine("");
		}

		static int ParseInt(ArgParser ap, string key, int dflt) 
		{
			if (ap.Exists(key)) 
			{
				return Convert.ToInt32(ap[key].Value);								
			}
			return dflt;
		}
	}
}
