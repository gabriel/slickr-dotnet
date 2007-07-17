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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using Tao.OpenGl;
using Tao.DevIl;
using Tao.Platform.Windows;
using Slickr.Util;
using Slickr.Util.OpenGL;
using Microsoft.Win32;

namespace Slickr 
{   

    #region Class Documentation
    /// <summary>
    ///     Open GL Screensaver Form
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Original Author: Gabriel Handford
    ///     </para>
    /// </remarks>
    #endregion Class Documentation
    public sealed class SlickrOpenGLForm : Form, SlickrWindow 
    {                

        #region Private Fields
		private IntPtr hDC;                                              // Private GDI Device Context
		private IntPtr hRC;                                              // Permanent Rendering Context        
		private Point MouseXY;		
		private SlickrOpenGL openGL;
		private Rectangle oldBounds = new Rectangle(-1, -1, -1, -1);

        #endregion Private Fields

        // --- Constructors & Destructors ---
        #region SlickrOpenGLForm    

        /// <summary>
        ///     Creates a new instance.
        /// </summary>
        public SlickrOpenGLForm(int screen, int width, int height, bool fullScreen, bool standAlone) 
        {
			// TODO: Fix icon
            //System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SlickrOpenGLForm));         			
			Icon = Utils.LoadResourceIcon("App.ico");

            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SlickrOpenGLForm";
                    
            this.Text = "Slickr";               
        
			oldBounds = new Rectangle(0, 0, width, height);			

			/**
            if (fullScreen) 
            {
				FormBorderStyle = FormBorderStyle.None;                    // No Border
                Bounds = Screen.AllScreens[screen].Bounds;              								
            } 
            else 
            {
				//FormBorderStyle = FormBorderStyle.SizableToolWindow;//FormBorderStyle.Sizable;                 // Sizable   
                Bounds = new Rectangle(0, 0, width, height);                				                          				             
            }
            Log.Debug(screen + " - Bounds: " + Bounds);     			
			*/
			
            this.CreateParams.ClassStyle = this.CreateParams.ClassStyle |       // Redraw On Size, And Own DC For Window.
                User.CS_HREDRAW | User.CS_VREDRAW | User.CS_OWNDC;
			
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);            // No Need To Erase Form Background
            this.SetStyle(ControlStyles.DoubleBuffer, true);                    // Buffer Control
            this.SetStyle(ControlStyles.Opaque, true);                          // No Need To Draw Form Background
            this.SetStyle(ControlStyles.ResizeRedraw, true);                    // Redraw On Resize
            this.SetStyle(ControlStyles.UserPaint, true);                       // We'll Handle Painting Ourselves			

            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;         
            
            this.Activated += new EventHandler(this.Form_Activated);            // On Activate Event Call Form_Activated
            this.Closing += new CancelEventHandler(this.Form_Closing);          // On Closing Event Call Form_Closing
            this.Deactivate += new EventHandler(this.Form_Deactivate);          // On Deactivate Event Call Form_Deactivate         
            this.Resize += new EventHandler(this.Form_Resize);                  // On Resize Event Call Form_Resize

            if (!standAlone) 
            {
                this.MouseMove += new MouseEventHandler(this.OnMouseMove);
            }      
            
			openGL = new SlickrOpenGL(this, screen, fullScreen, standAlone);

			FullScreen = fullScreen;  			
        }
        #endregion   

		#region Settings
		/// <summary>
		/// Set/Get full screen property
		/// </summary>
		public bool FullScreen 
		{ 
			get 
			{
				return openGL.FullScreen;
			}

			set 
			{
				openGL.FullScreen = value;
				if (value) 
				{
					oldBounds = Bounds;					
					//this.WindowState = FormWindowState.Maximized;					
					FormBorderStyle = FormBorderStyle.None;			
					Log.Debug("Screen bounds: " + Screen.AllScreens[openGL.ScreenNumber].Bounds);
					Bounds = Screen.AllScreens[openGL.ScreenNumber].Bounds;
					TopMost = true;  
				} 
				else 
				{
					FormBorderStyle = FormBorderStyle.Sizable;
					TopMost = false;   					
					if (oldBounds.Width != -1) Bounds = oldBounds;  
					Log.Debug("New bounds: " + Bounds);					
					openGL.FullScreen = false;
				}
			}
		}
		#endregion

        #region void SwapBuffers()
        /// <summary>
        ///     Swap buffers (when double buffering)
        /// </summary>
        public void SwapBuffers() 
        {
            Gdi.SwapBuffersFast(hDC);
        }
        #endregion void SwapBuffers()

        // --- Private Static Methods ---
        #region CreateGLWindow
        /// <summary>
        ///     Creates our OpenGL Window.
        /// </summary>
        /// <param name="title">
        ///     The title to appear at the top of the window.
        /// </param>
        /// <param name="bits">
        ///     The bit depth for the OpenGL setup
        /// </param>
        /// <returns>
        ///     <c>true</c> on successful window creation, otherwise <c>false</c>.
        /// </returns>
        public bool CreateGLWindow(string title, int bits) 
        {

            //GC.Collect();                                                       // Request A Collection
            // This Forces A Swap
            //Kernel.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);

            // No need to change screen mode, just use a window the size of the current resolution
            /**
            if(Settings.FullScreen) 
            {                                                    // Attempt Fullscreen Mode?
                Gdi.DEVMODE dmScreenSettings = new Gdi.DEVMODE();               // Device Mode
                // Size Of The Devmode Structure
                dmScreenSettings.dmSize = (short) Marshal.SizeOf(dmScreenSettings);
                dmScreenSettings.dmPelsWidth = width;                           // Selected Screen Width
                dmScreenSettings.dmPelsHeight = height;                         // Selected Screen Height
                dmScreenSettings.dmBitsPerPel = bits;                           // Selected Bits Per Pixel
                dmScreenSettings.dmFields = Gdi.DM_BITSPERPEL | Gdi.DM_PELSWIDTH | Gdi.DM_PELSHEIGHT;

                // Try To Set Selected Mode And Get Results.  NOTE: CDS_FULLSCREEN Gets Rid Of Start Bar.
                if(User.ChangeDisplaySettings(ref dmScreenSettings, User.CDS_FULLSCREEN) != User.DISP_CHANGE_SUCCESSFUL) 
                {
                    // If The Mode Fails, Offer Two Options.  Quit Or Use Windowed Mode.
                    if(MessageBox.Show("The Requested Fullscreen Mode Is Not Supported By\nYour Video Card.  Use Windowed Mode Instead?", "Slickr GL",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes) 
                    {
                        Settings.FullScreen = false;                                     // Windowed Mode Selected.  Fullscreen = false
                    }
                    else 
                    {
                        // Pop up A Message Box Lessing User Know The Program Is Closing.
                        MessageBox.Show("Program Will Now Close.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return false;                                           // Return false
                    }
                }
            }*/            
            
            Text = title;                                                  // Set Window Title

			string errorMessage;
			bool ok = OpenGlUtils.InitializeGL(Handle, out hDC, out hRC, bits, out errorMessage);
			
			// Check if renderer or vendor is Microsoft, GDI generic which probably means that OpenGL support is not enabled in the current driver
			// settings
			// Probably want to check version number too
			// Or use a better method of detection
			if (ok && !Settings.Local.OverrideOpenGLCheck) 
			{
				String renderer = OpenGlUtils.GetString(Gl.GL_RENDERER);
				String vendor = OpenGlUtils.GetString(Gl.GL_VENDOR);
				String version = OpenGlUtils.GetString(Gl.GL_VERSION);
				
				if (vendor == null || vendor.Trim().ToLower().StartsWith("microsoft")) 
				{
					MessageBox.Show("Slickr may not support your current video card driver or settings. Try upgrading you drivers. (Vendor: " + vendor + ")\nYou can override this check in the Settings | About tab but this is not recommended.", 
						"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					ok = false;
				} 
				else if (renderer == null || renderer.Trim().ToLower().StartsWith("gdi generic")) 
				{
					MessageBox.Show("Slickr may not support your current video card driver or settings. Try upgrading you drivers. (Renderer: " + renderer + ")\nYou can override this check in the Settings | About tab but this is not recommended.", 
						"ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
					ok = false;
				}
			}
			
			if (!ok)
			{
				KillGLWindow();                                                 // Reset The Display
				if (errorMessage != null) 
				{
					MessageBox.Show(errorMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				return false;
			}
            

            Show();                                                        // Show The Window
            Focus();                                                       // Focus The Window

			if(openGL.FullScreen) 
			{                                                    // This Shouldn't Be Necessary, But Is
				Cursor.Hide();
			} 
			else 
			{
				Cursor.Show();       
			}

            ReSizeGLScene();                                       // Set Up Our Perspective GL Screen

            if(!openGL.InitGL()) 
            {                                                     // Initialize Our Newly Created GL Window
                KillGLWindow();                                                 // Reset The Display
                MessageBox.Show("Initialization Failed.", "ERROR",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            openGL.CreateGLWindow();

            //statusFont = new GlFont(hDC, "Arial", 12, false);         
            return true;                                                        // Success
        }
        #endregion       
        
		#region ReSizeGLScene
		/// <summary>
		///     Resizes and initializes the GL window.
		/// </summary>
		private void ReSizeGLScene() 
		{
			Wgl.wglMakeCurrent(hDC, hRC);
			openGL.ReSizeGLScene(Width, Height);
		}
		#endregion

		#region SlickrOpenGL
		public SlickrOpenGL GetContext()
		{
			return openGL;
		}
		#endregion

		#region KillGLWindow
		/// <summary>
		///     Properly kill the window.
		/// </summary>
		public bool KillGLWindow() 
		{           
			if(openGL.FullScreen) 
			{                                                    // Are We In Fullscreen Mode?
				//User.ChangeDisplaySettings(IntPtr.Zero, 0);                     // If So, Switch Back To The Desktop
				Cursor.Show();                                                  // Show Mouse Pointer
			}

			string errorMessage;
			bool ok = OpenGlUtils.DeinitializeGL((!IsDisposed ? Handle : IntPtr.Zero), ref hDC, ref hRC, out errorMessage);
			if (!ok) 
			{
				if (errorMessage != null) 
					MessageBox.Show(errorMessage, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			Hide();                                                    // Hide The Window
			Close();                                                   // Close The Form                    
			return true;
		}
		#endregion

		#region DrawGLScene
		/// <summary>
		///     Here's where we do all the drawing.
		/// </summary>
		/// <returns>
		///     <c>true</c> on successful drawing, otherwise <c>false</c>.
		/// </returns>
		///         
		public bool DrawGLScene() 
		{       
			if (!openGL.Active) return true;

			Wgl.wglMakeCurrent(hDC, hRC);
			return openGL.DrawGLScene();
		}
		#endregion

		#region Properties
		/// <summary>
		/// Center window
		/// </summary>
		public void Center() 
		{
			CenterToScreen();
		}		

		/// <summary>
		/// Maximum texture size
		/// </summary>
		public int MaxTextureSize { get { return OpenGlUtils.GetInteger(Gl.GL_MAX_TEXTURE_SIZE, -1); } }
		#endregion

        #region Mouse events        
        private void OnMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {           
            if (!MouseXY.IsEmpty) // Do nothing if mouse location has not been initialized.
            {
                if ((MouseXY != new Point(e.X, e.Y)) || (e.Clicks > 0)) 
                {
                    openGL.Done = true;
                }
            }   
            MouseXY = new Point(e.X, e.Y); // Get current position of mouse.
        }
        #endregion

        // --- Private Instance Event Handlers ---
        #region Form_Activated
        /// <summary>
        ///     Handles the form's activated event.
        /// </summary>
        /// <param name="sender">
        ///     The event sender.
        /// </param>
        /// <param name="e">
        ///     The event arguments.
        /// </param>
        private void Form_Activated(object sender, EventArgs e) 
        {
            openGL.Active = true;                                                      // Program Is Active
        }
        #endregion Form_Activated

        #region Form_Closing(object sender, CancelEventArgs e)
        /// <summary>
        ///     Handles the form's closing event.
        /// </summary>
        /// <param name="sender">
        ///     The event sender.
        /// </param>
        /// <param name="e">
        ///     The event arguments.
        /// </param>
        private void Form_Closing(object sender, CancelEventArgs e) 
        {
            openGL.Done = true;                                                        // Send A Quit Message
        }
        #endregion Form_Closing(object sender, CancelEventArgs e)

        #region Form_Deactivate(object sender, EventArgs e)
        /// <summary>
        ///     Handles the form's deactivate event.
        /// </summary>
        /// <param name="sender">
        ///     The event sender.
        /// </param>
        /// <param name="e">
        ///     The event arguments.
        /// </param>
        private void Form_Deactivate(object sender, EventArgs e) 
        {
            openGL.Active = false;                                                     // Program Is No Longer Active
        }
        #endregion Form_Deactivate(object sender, EventArgs e)              

        #region Form_Resize(object sender, EventArgs e)
        /// <summary>
        ///     Handles the form's resize event.
        /// </summary>
        /// <param name="sender">
        ///     The event sender.
        /// </param>
        /// <param name="e">
        ///     The event arguments.
        /// </param>
        private void Form_Resize(object sender, EventArgs e) 
        {           
			//Log.Debug("Start Form_Resize");
			/**
            if (openGL.StandAlone 
                && this.WindowState == FormWindowState.Maximized
                && this.FormBorderStyle == FormBorderStyle.Sizable 
                && !this.TopMost) 
            {                               
                Log.Debug("Is maximize");				
                //Width = Screen.AllScreens[screen].Bounds.Width;
                //Height = Screen.AllScreens[screen].Bounds.Height;             
                this.FormBorderStyle = FormBorderStyle.None;
                this.TopMost = true;      
   				openGL.FullScreen = true;
            }*/         

            ReSizeGLScene();                             // Resize The OpenGL Window
			//Log.Debug("Done Form_Resize");
        }
        #endregion Form_Resize(object sender, EventArgs e)                                      

		private void InitializeComponent()
		{
			// 
			// SlickrOpenGLForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Name = "SlickrOpenGLForm";

		}
        
    }
}
