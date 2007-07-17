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
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using Slickr.Util;

namespace Slickr
{
	/// <summary>
	/// Summary description for GlFont2.
	/// </summary>
	public class GlFont2
	{
		private int texture;
		private int fontbase;

		private float charWidth = 10f;
		private float charHeight = 16f;
		private float scale = 1f;

		public GlFont2()
		{			
			LoadGLTextures();
			BuildFont();
		}

		#region BuildFont
		/// <summary>
		///     Build our font display list.
		/// </summary>
		private void BuildFont() 
		{
			float cx;                                                           // Holds Our X Character Coord
			float cy;                                                           // Holds Our Y Character Coord
			fontbase = Gl.glGenLists(256);                                      // Creating 256 Display Lists
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);                     // Select Our Font Texture
			for(int loop = 0; loop < 256; loop++) 
			{                                 // Loop Through All 256 Lists
				cx = ((float) (loop % 16)) / 16.0f;                             // X Position Of Current Character
				cy = ((float) (loop / 16)) / 16.0f;                             // Y Position Of Current Character
				Gl.glNewList(fontbase + loop, Gl.GL_COMPILE);                   // Start Building A List
				Gl.glBegin(Gl.GL_QUADS);                                    // Use A Quad For Each Character
				Gl.glTexCoord2f(cx, 1 - cy - 0.0625f);                  // Texture Coord (Bottom Left)
				Gl.glVertex2i(0, 0);                                    // Vertex Coord (Bottom Left)
				Gl.glTexCoord2f(cx + 0.0625f, 1 - cy - 0.0625f);        // Texture Coord (Bottom Right)
				Gl.glVertex2i(16, 0);                                   // Vertex Coord (Bottom Right)
				Gl.glTexCoord2f(cx + 0.0625f, 1 - cy);                  // Texture Coord (Top Right)
				Gl.glVertex2i(16, 16);                                  // Vertex Coord (Top Right)
				Gl.glTexCoord2f(cx, 1 - cy);                            // Texture Coord (Top Left)
				Gl.glVertex2i(0, 16);                                   // Vertex Coord (Top Left)
				Gl.glEnd();                                                 // Done Building Our Quad (Character)
				Gl.glTranslated(10, 0, 0);                                  // Move To The Right Of The Character
				Gl.glEndList();                                                 // Done Building The Display List
			}                                                                   // Loop Until All 256 Are Built
		}
		#endregion

		#region Font Metrics
		/// <summary>
		/// Get character width.
		/// </summary>
		/// <param name="text">Text</param>
		/// <returns>Width</returns>
		public float GetCharWidth(String text) 
		{
			return (charWidth * text.Length) + 2;
		}

		/// <summary>
		/// Get character height.
		/// </summary>
		/// <param name="text">Text</param>
		/// <returns>Height</returns>
		public float GetCharHeight(String text) 
		{
			return charHeight;
		}
		#endregion

		#region Draw

		/// <summary>
		/// Draw text at horizontal alignment.
		/// </summary>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="text"></param>
		/// <param name="y"></param>
		/// <param name="a"></param>
		public void Draw(float screenWidth, float screenHeight, String text, int y, Alignment a) 
		{
			int x = 0;
			screenHeight *= scale;
			screenWidth *= scale;
			switch(a) 
			{
				case Alignment.RIGHT: x = (int)Math.Round(screenWidth - GetCharWidth(text)); break;
			}
			Draw(screenWidth, screenHeight, text, x, y);
		}

		/// <summary>
		/// Draw text at horizontal and vertical alignment.
		/// </summary>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="text"></param>
		/// <param name="h"></param>
		/// <param name="v"></param>
		public void Draw(float screenWidth, float screenHeight, String text, Alignment h, Alignment v) 
		{
			int x = 0;
			int y = 0;
			screenHeight *= scale;
			screenWidth *= scale;
			switch(h) 
			{
				case Alignment.RIGHT: x = (int)Math.Round(screenWidth - GetCharWidth(text)); break;
				case Alignment.LEFT: x = 0; break;
				case Alignment.CENTER: x = (int)Math.Round((screenWidth/2f) - (GetCharWidth(text)/2f)); break;
			}

			switch(v) 
			{
				case Alignment.TOP: y = (int)Math.Round(screenHeight - charHeight); break;
				case Alignment.CENTER: y = (int)Math.Round((screenHeight/2f) - (charHeight/2f)); break;
				case Alignment.BOTTOM: y = 0; break;
			}
			Draw(screenWidth, screenHeight, text, x, y);
		}

		/// <summary>
		/// Draw text at position.
		/// </summary>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		/// <param name="text"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Draw(float screenWidth, float screenHeight, string text, int x, int y) 
		{						
			GlPrint(x, y, text, 0, screenWidth * scale, screenHeight * scale);
		}

		/// <summary>
		/// Draw border around text.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <param name="margin"></param>
		public void DrawBorder(float x, float y, String text, float margin) 
		{			
			Gl.glEnable(Gl.GL_BLEND); // for text fading
			Gl.glBlendFunc(Gl.GL_ONE, Gl.GL_ONE_MINUS_SRC_ALPHA); // ditto
			float height = 16.0f;
			float width = GetCharWidth(text);

			float widthPad = 6;

			Gl.glColor4f(0.3f, 0.8f, 0.3f, 0.5f);
			Gl.glBegin(Gl.GL_QUADS);						// Draw A Quad
			Gl.glVertex3f(x-margin, y-margin, 0.0f);				// Top Left
			Gl.glVertex3f(x-margin, y + height + margin, 0.0f);				// Top Right
			Gl.glVertex3f(x + width + margin + widthPad, y + height + margin, 0.0f);				// Bottom Right
			Gl.glVertex3f(x + width + margin + widthPad, y-margin, 0.0f);				// Bottom Left
			Gl.glEnd();
		}
		
		/// <summary>
		/// OpenGl print method.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="text"></param>
		/// <param name="charset"></param>
		/// <param name="screenWidth"></param>
		/// <param name="screenHeight"></param>
		private void GlPrint(int x, int y, string text, int charset, float screenWidth, float screenHeight) 
		{
			if(charset > 1) 
			{
				charset = 1;
			}
			Gl.glLoadIdentity();
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);                     // Select Our Font Texture
			Gl.glDisable(Gl.GL_DEPTH_TEST);                                     // Disables Depth Testing
			Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
			Gl.glLoadIdentity();                                            // Reset The Projection Matrix
			float w = screenWidth;
			float h = screenHeight;
			if (scale != 1) 
			{				
				w = screenWidth * scale;
				h = screenHeight * scale;
			}

			Gl.glOrtho(0, w, 0, h, -1, 1);                              // Set Up An Ortho Screen
			Gl.glMatrixMode(Gl.GL_MODELVIEW);                               // Select The Modelview Matrix
			Gl.glLoadIdentity();                                        // Reset The Modelview Matrix			
	
			DrawBorder(x, y, text, 1);
			Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE);
			Gl.glEnable(Gl.GL_BLEND);                                           // Enable Blending			
			  			
			Gl.glColor3f(1.0f, 1.0f, 1.0f);
			// Print GL Text To The Screen
			Gl.glTranslated(x, y, 0);                                   // Position The Text (0,0 - Bottom Left)
			Gl.glListBase(fontbase - 32 + (128 * charset));             // Choose The Font Set (0 or 1)
			// .NET: We can't draw text directly, it's a string!
			byte [] textbytes = new byte [text.Length];
			for (int i = 0; i < text.Length; i++) textbytes[i] = (byte) text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);// Write The Text To The Screen
			
			Gl.glDisable(Gl.GL_TEXTURE_2D);
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
		}
		#endregion

		#region KillFont
		/// <summary>
		///     Delete the font from memory.
		/// </summary>
		private void KillFont() 
		{
			Gl.glDeleteLists(fontbase, 256);                                    // Delete All 256 Display Lists
		}
		#endregion				

		#region LoadGLTextures
		/// <summary>
		///     Load bitmaps and convert to textures.
		/// </summary>
		/// <returns>
		///     <c>true</c> on success, otherwise <c>false</c>.
		/// </returns>
		private bool LoadGLTextures() 
		{

			// get a reference to the current assembly
			Assembly a = Assembly.GetExecutingAssembly();        
			// get a list of resource names from the manifest
			//string [] resNames = a.GetManifestResourceNames();			

			bool status = false;                                                // Status Indicator
			Bitmap textureImage = new Bitmap(a.GetManifestResourceStream("Slickr.Font.bmp"));                // Load The Bitmap
			
			// Check For Errors, If The Bitmaps Are Not Found, Quit
			if(textureImage != null) 
			{
				status = true;                                                  // Set The Status To True

				Gl.glGenTextures(1, out texture);                            // Create Two Textures

				// Flip The Bitmap Along The Y-Axis
				textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
				// Rectangle For Locking The Bitmap In Memory
				Rectangle rectangle = new Rectangle(0, 0, textureImage.Width, textureImage.Height);
				// Get The Bitmap's Pixel Data From The Locked Bitmap
				BitmapData bitmapData = textureImage.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

				// Typical Texture Generation Using Data From The Bitmap
				Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
				Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
				Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGB8, textureImage.Width, textureImage.Height, 0, Gl.GL_BGR, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);

				if(textureImage != null) 
				{                            // If Texture Exists
					textureImage.UnlockBits(bitmapData);              // Unlock The Pixel Data From Memory
					textureImage.Dispose();                           // Dispose The Bitmap
				}
			}
			return status;                                                      // Return The Status
		}
		#endregion
	}
}
