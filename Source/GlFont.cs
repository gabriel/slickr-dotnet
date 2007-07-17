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
using Tao.OpenGl;
using System.Drawing;
using Tao.Platform.Windows;

namespace Slickr
{
	/// <summary>
	/// Summary description for GlFont
	/// </summary>
	public class GlFont
	{
		private int fontbase;                                            // Base Display List For The Font Set		
		
		public GlFont(IntPtr hDC, string name, int size, bool bold) { 
			BuildFont(hDC, name, size, bold);	
		}

		#region Draw
		/**
		public void DrawBorder(float x, float y, float width, float height, float margin) 
		{
			Gl.glColor4f(0.3f, 0.8f, 0.3f, 0.8f);
			Gl.glBegin(Gl.GL_QUADS);						// Draw A Quad
			Gl.glVertex3f(x-margin, y-margin, 0.0f);				// Top Left
			Gl.glVertex3f(x-margin, y + height + margin, 0.0f);				// Top Right
			Gl.glVertex3f(x + width + margin, y + height + margin, 0.0f);				// Bottom Right
			Gl.glVertex3f(x + width + margin, y-margin, 0.0f);				// Bottom Left
			Gl.glEnd();
		}*/

		public void Draw(string text, float x, float y) 
		{
			//DrawBorder(x, y, 200, 20, 5);
			// Pulsing Colors Based On Text Position
			Gl.glColor4f(0f, 0f, 0f, 1f);
			// Position The Text On The Screen
			Gl.glRasterPos2f(x, y);
			// Print GL Text To The Screen
			Print(text);
		}
		#endregion
		
		#region BuildFont()
		/// <summary>
		///     Builds our bitmap font.
		/// </summary>
		private void BuildFont(IntPtr hDC, string name, int size, bool bold) 
		{
			IntPtr font;                                                        // Windows Font ID
			IntPtr oldfont;                                                     // Used For Good House Keeping
			fontbase = Gl.glGenLists(96);                                       // Storage For 96 Characters

			font = Gdi.CreateFont(                                              // Create The Font
				-size,                                                            // Height Of Font
				0,                                                              // Width Of Font
				0,                                                              // Angle Of Escapement
				0,                                                              // Orientation Angle
				(bold ? Gdi.FW_BOLD : 400),                              // Font Weight
				false,                                                          // Italic
				false,                                                          // Underline
				false,                                                          // Strikeout
				Gdi.ANSI_CHARSET,                                               // Character Set Identifier
				Gdi.OUT_TT_PRECIS,                                              // Output Precision
				Gdi.CLIP_DEFAULT_PRECIS,                                        // Clipping Precision
				Gdi.ANTIALIASED_QUALITY,                                        // Output Quality
				Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH,                            // Family And Pitch
				name);                                                       // Font Name
			
			oldfont = Gdi.SelectObject(hDC, font);                              // Selects The Font We Want
			Wgl.wglUseFontBitmaps(hDC, 32, 96, fontbase);                       // Builds 96 Characters Starting At Character 32			
			Gdi.SelectObject(hDC, oldfont);                                     // Selects The Font We Want
			Gdi.DeleteObject(font);                                             // Delete The Font
		}
		#endregion BuildFont()

		#region Print(string text)
		/// <summary>
		///     Custom GL "print" routine.
		/// </summary>
		/// <param name="text">
		///     The text to print.
		/// </param>
		private void Print(string text) 
		{
			if(text == null || text.Length == 0) 
			{                              // If There's No Text
				return;                                                         // Do Nothing
			}

			

			Gl.glPushAttrib(Gl.GL_LIST_BIT);                                    // Pushes The Display List Bits
			Gl.glListBase(fontbase - 32);                                   // Sets The Base Character to 32
			// .NET -- we can't just pass text, we need to convert
			byte [] textbytes = new byte[text.Length];
			for (int i = 0; i < text.Length; i++) textbytes[i] = (byte) text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);        // Draws The Display List Text
			Gl.glPopAttrib();                                                   // Pops The Display List Bits
		}
		#endregion GlPrint(string text)

		#region KillFont()
		/// <summary>
		///     Delete the font list.
		/// </summary>
		private void KillFont() 
		{
			Gl.glDeleteLists(fontbase, 96);                                     // Delete All 96 Characters
		}
		#endregion KillFont()
	}
}
