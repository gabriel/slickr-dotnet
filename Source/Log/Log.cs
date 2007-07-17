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
using NSpring.Logging;

namespace Slickr
{
	/// <summary>
	/// Logger singleton/facade
	/// </summary>
	public class Log
	{
		private static Logger logger;

        private static bool enabled = false;
		private static string fileName;

		/// <summary>
		/// Default is console logger
		/// </summary>
		static Log() 
		{
			logger = Logger.CreateConsoleLogger("{ts}{z}  [{ln:1w}]  {msg}");
			logger.Open();	
		}

		/// <summary>
		/// File name to log to, if null a console logger is used
		/// </summary>
		public static string FileName { 
			get { return fileName; } 
			set { 
				fileName = value; 
				if (logger != null) logger.Close();
				logger = Logger.CreateFileLogger(fileName, "{ts}{z}  [{ln:1w}]  {msg}");
				logger.Open();
			} 
		}

		/// <summary>
		/// Get the logger.
		/// </summary>
		/// <returns>Logger</returns>
		public static Logger getLogger() 
		{
			return logger;
		}

		/// <summary>
		/// Enabled
		/// </summary>
		public static bool Enabled { get { return enabled; } set { enabled = value; } }

		private Log() { }

		/// <summary>
		/// Destructor
		/// </summary>
		~Log() 
		{
			if (logger != null)	logger.Close();
		}

		/// <summary>
		/// Debug
		/// </summary>
		/// <param name="s">s</param>
		public static void Debug(String s) 
		{
			if (enabled) logger.Log(s);
		}

		/// <summary>
		/// Error
		/// </summary>
		/// <param name="s">s</param>
		public static void Error(String s) 
		{
			Error(s, null);
		}

		/// <summary>
		/// Error
		/// </summary>
		/// <param name="e">e</param>
		public static void Error(Exception e) 
		{
			Error(null, e);
		}

		/// <summary>
		/// Get error string.
		/// </summary>
		/// <param name="e">Exception</param>
		/// <returns>Full error message and stack trace</returns>
		public static String GetError(Exception e) 
		{
			if (e == null) return "";
			return e.GetType().FullName + " (" + e.Message + ")\n" + e.StackTrace + 
					(e.GetBaseException() != null ? "\ncaused by:\n" + e.GetBaseException().StackTrace : "");
		}

		/// <summary>
		/// Error
		/// </summary>
		/// <param name="s">s</param>
		/// <param name="e">e</param>
		public static void Error(String s, Exception e) 
		{
			if (!enabled) return;
			logger.Log((s != null ? s + " " : "") + " " + GetError(e));			
		}
	}
}
