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
//using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Tao.OpenGl;
using Tao.Cg;
using Tao.Platform.Windows;

namespace Slickr.Util.OpenGL
{
	/// <summary>
	/// OpenlGlUtils are OpenGL Utilities.
	/// </summary>
	public class OpenGlUtils
	{
		public OpenGlUtils()
		{
		}

		/// <summary>
		/// Get property (string).
		/// </summary>
		/// <param name="property">Property (key)</param>
		/// <returns>String, or null if property not found</returns>
		public static string GetString(int property) 
		{
			IntPtr extStrPtr = Gl.glGetString(property);
			if (extStrPtr == IntPtr.Zero)
				return null;

			return Marshal.PtrToStringAnsi (extStrPtr);
		}

		/// <summary>
		/// Get property (integer).
		/// </summary>
		/// <param name="property">Property (key)</param>
		/// <param name="defaultValue">Default value if property is not found</param>
		/// <returns>Integer</returns>
		public static int GetInteger(int property, int defaultValue) 
		{
			int val = defaultValue;
			Gl.glGetIntegerv(property, out val);
			return val;
		}

		/// <summary>
		/// Test OpenGL capabilities
		/// </summary>
		public static void Test() 
		{
			Form form = new Form();
			IntPtr hDC;
			IntPtr hRC;
			string errorMessage;

			bool ok = InitializeGL(form.Handle, out hDC, out hRC, 32, out errorMessage);
			Log.Debug("Initialized: " + ok + (errorMessage != null ? " (" + errorMessage + ")" : ""));

			int maxTextureSize = GetInteger(Gl.GL_MAX_TEXTURE_SIZE, -1);
			int max3dSize = GetInteger(Gl.GL_MAX_3D_TEXTURE_SIZE, -1);

			int maxTextureUnits = GetInteger(Gl.GL_MAX_TEXTURE_UNITS, -1);

			String renderer = GetString(Gl.GL_RENDERER);
			String vendor = GetString(Gl.GL_VENDOR);
			String version = GetString(Gl.GL_VERSION);

			String extensions = GetString (Gl.GL_EXTENSIONS);			
			
			Log.Debug("Max texture size: " + maxTextureSize);
			Log.Debug("Max 3d texture size: " + max3dSize);
			Log.Debug("Max texture units: " + maxTextureUnits);
			Log.Debug("Renderer: " + renderer);
			Log.Debug("Vendor: " + vendor);
			Log.Debug("Version: " + version);
			Log.Debug("Extensions: " + extensions);

			ok = DeinitializeGL((!form.IsDisposed ? form.Handle : IntPtr.Zero), ref hDC, ref hRC, out errorMessage);
			Log.Debug("De-Initialized: " + ok + (errorMessage != null ? " (" + errorMessage + ")" : ""));
		}		

		/// <summary>
		/// Initialize OpenGL for handle.
		/// </summary>
		/// <param name="handle">Form handle</param>
		/// <param name="hDC">Device context, is set</param>
		/// <param name="hRC">Release context, is set</param>
		/// <param name="bits">Pixel bit depth</param>
		/// <param name="errorMessage">Error message, on error</param>
		/// <returns>Whether initialized</returns>
		public static bool InitializeGL(IntPtr handle, out IntPtr hDC, out IntPtr hRC, int bits, out string errorMessage) 
		{
			errorMessage = null;
			hDC = IntPtr.Zero;
			hRC = IntPtr.Zero;

			Gdi.PIXELFORMATDESCRIPTOR pfd = new Gdi.PIXELFORMATDESCRIPTOR();    // pfd Tells Windows How We Want Things To Be
			pfd.nSize = (short) Marshal.SizeOf(pfd);                            // Size Of This Pixel Format Descriptor
			pfd.nVersion = 1;                                                   // Version Number
			pfd.dwFlags = Gdi.PFD_DRAW_TO_WINDOW |                              // Format Must Support Window
				Gdi.PFD_SUPPORT_OPENGL |                                        // Format Must Support OpenGL
				Gdi.PFD_DOUBLEBUFFER;                                           // Format Must Support Double Buffering
			pfd.iPixelType = (byte) Gdi.PFD_TYPE_RGBA;                          // Request An RGBA Format
			pfd.cColorBits = (byte) bits;                                       // Select Our Color Depth
			pfd.cRedBits = 0;                                                   // Color Bits Ignored
			pfd.cRedShift = 0;
			pfd.cGreenBits = 0;
			pfd.cGreenShift = 0;
			pfd.cBlueBits = 0;
			pfd.cBlueShift = 0;
			pfd.cAlphaBits = 0;                                                 // No Alpha Buffer
			pfd.cAlphaShift = 0;                                                // Shift Bit Ignored
			pfd.cAccumBits = 0;                                                 // No Accumulation Buffer
			pfd.cAccumRedBits = 0;                                              // Accumulation Bits Ignored
			pfd.cAccumGreenBits = 0;
			pfd.cAccumBlueBits = 0;
			pfd.cAccumAlphaBits = 0;
			pfd.cDepthBits = 16;                                                // 16Bit Z-Buffer (Depth Buffer)
			pfd.cStencilBits = 0;                                               // No Stencil Buffer
			pfd.cAuxBuffers = 0;                                                // No Auxiliary Buffer
			pfd.iLayerType = (byte) Gdi.PFD_MAIN_PLANE;                         // Main Drawing Layer
			pfd.bReserved = 0;                                                  // Reserved
			pfd.dwLayerMask = 0;                                                // Layer Masks Ignored
			pfd.dwVisibleMask = 0;
			pfd.dwDamageMask = 0;

            
			hDC = User.GetDC(handle);                                       // Attempt To Get A Device Context
			if(hDC == IntPtr.Zero) 
			{               
				errorMessage = "Can't Create A GL Device Context";
				return false;
			}

			int pixelFormat = Gdi.ChoosePixelFormat(hDC, ref pfd);                  // Attempt To Find An Appropriate Pixel Format
			if(pixelFormat == 0) 
			{                                              // Did Windows Find A Matching Pixel Format?
				errorMessage = "Can't Find A Suitable PixelFormat";
				return false;
			}

			if(!Gdi.SetPixelFormat(hDC, pixelFormat, ref pfd)) 
			{                // Are We Able To Set The Pixel Format?
				errorMessage = "Can't Set The PixelFormat";
				return false;
			}

			hRC = Wgl.wglCreateContext(hDC);                                    // Attempt To Get The Rendering Context
			if(hRC == IntPtr.Zero) 
			{                                            // Are We Able To Get A Rendering Context?
				errorMessage = "Can't Create A GL Rendering Context";
				return false;
			}

			if(!Wgl.wglMakeCurrent(hDC, hRC)) 
			{                                 // Try To Activate The Rendering Context
				errorMessage = "Can't Activate The GL Rendering Context";
				return false;
			}
			
			return true;
		}
		
		/// <summary>
		/// De-initialize OpenGL
		/// </summary>
		/// <param name="handle">Form handle</param>
		/// <param name="hDC">Existing device context, zero'ed on success</param>
		/// <param name="hRC">Existing release context, zero'ed on success</param>
		/// <param name="isDisposed">Whether form is disposed</param>
		/// <param name="errorMessage">Error message, on error</param>
		/// <returns>Whether de-initialized</returns>
		public static bool DeinitializeGL(IntPtr handle, ref IntPtr hDC, ref IntPtr hRC, out string errorMessage) 
		{
			errorMessage = null;

			if(hRC != IntPtr.Zero) 
			{   // Do We Have A Rendering Context?
				if(!Wgl.wglMakeCurrent(IntPtr.Zero, IntPtr.Zero))
				{ 
					// Ignore
				}

				if(!Wgl.wglDeleteContext(hRC)) 
				{                                // Are We Able To Delete The RC?
					errorMessage = "Release Rendering Context Failed";
					return false;
				}

				hRC = IntPtr.Zero;                                              // Set RC To Null
			}

			if(hDC != IntPtr.Zero) 
			{                                            // Do We Have A Device Context?
				if(handle != IntPtr.Zero) 
				{                            // Do We Have A Window Handle?
					if(!User.ReleaseDC(handle, hDC)) 
					{                 // Are We Able To Release The DC?
						errorMessage = "Release Device Context Failed";
						return false;
					}
				}

				hDC = IntPtr.Zero;                                              // Set DC To Null
			}
			return true;
		}		
	}
}
