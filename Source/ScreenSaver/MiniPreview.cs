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
using Slickr.Util;
using Slickr.Util.Windows;
using System.Windows.Forms;
using System.Drawing; 
using System.Threading;
using System.Reflection;

namespace Slickr.ScreenSaver
{
	/// <summary>
	/// Draws in the Display Properties Screensaver preview window exclusively.
	/// </summary>
	public class MiniPreview
	{
		/// <summary>
		/// Do the mini preview until the mini-preview window vanishes.
		/// </summary>
		/// <param name="argHandle"> a 10 based handle to the Display Properties window.</param>
		/// <param name="useStyleDoubleBuffer">Use double buffering</param>
		public void DoMiniPreview(int argHandle, bool useStyleDoubleBuffer)
		{
			// Pointer to windows Display Properties window.
			IntPtr ParentWindowHandle = new IntPtr(0); 
			ParentWindowHandle = (IntPtr) argHandle; // Get the pointer to Windows Display Properties dialog. 
			Rect rect = new Rect(); 

			// The Using construct is to make sure all resources used here are cleared in case of unhandled exceptions.
			using(Graphics PreviewGraphic = Graphics.FromHwnd(ParentWindowHandle)) // This is the mini-preview window from the OS. 
			{				
				WindowsUtils.GetClientRectApi(ParentWindowHandle, ref rect); // Get the dimensions and location of the preview window.
				
				DateTime dt30Seconds = DateTime.Now.AddSeconds(30);
				while (WindowsUtils.IsWindowVisibleApi(ParentWindowHandle) == false)
				{
					if (DateTime.Now > dt30Seconds) return; // If time runs out, exit program.
					Application.DoEvents(); // We don't want to ignore windows, or we might be sorry! :) Respond to events.
				}				

				// Create a bitmap for double buffering
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;
				Bitmap OffScreenBitmap = new Bitmap(width, height, PreviewGraphic); 
				Graphics OffScreenBitmapGraphic = Graphics.FromImage(OffScreenBitmap); // Create a Graphics object
				OffScreenBitmapGraphic.Clear(Color.Black);

                Bitmap image = null;
                try {
					Assembly a = Assembly.GetExecutingAssembly();
					//image = new Bitmap(this.GetType(), "Slickr.slickr.png");
					image = new Bitmap(a.GetManifestResourceStream("Slickr.slickr.png"));
                    //image = Properties.Resources.slickr;        
                    OffScreenBitmapGraphic.DrawImage(image, 0, 0);
                }
                catch (Exception e)
                {
                    Log.Error("Error loading slickr icon", e);
                    //OffScreenBitmapGraphic.Clear(Color.Red);
                }
				
				while (WindowsUtils.IsWindowVisibleApi(ParentWindowHandle) == true) // Now that the window is visible ...
				{
					//if (useStyleDoubleBuffer) // Will black out if we are using built in double-buffering.
					//	OffScreenBitmapGraphic.Clear(Color.Black); 

					Thread.Sleep(50); // Slow down the mini-preview.
					try
					{	// Draw the image created by insects.DrawWaspThenSwrm(OffscreenBitmapGraphic).
						PreviewGraphic.DrawImage(OffScreenBitmap,0,0,OffScreenBitmap.Width,OffScreenBitmap.Height); 
					}
					catch // the most likely reason we get an exception here is because
					{     // the user hits cancel button while drawing to mini-preview.
						break; // Either way we must get out of the program.
					}
					
					Application.DoEvents();
				} 

				OffScreenBitmap.Dispose();
				OffScreenBitmapGraphic.Dispose();
				PreviewGraphic.Dispose(); 
			} 
		}
	}
}
