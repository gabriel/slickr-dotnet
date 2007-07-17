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
using System.Collections;
using System.IO;
using System.Reflection;
using Tao.OpenGl;
using Tao.DevIl;
using Slickr.Util;

namespace Slickr
{
	/// <summary>
	/// Summary description for SlickrOpenGL.
	/// </summary>
	public class SlickrOpenGL
	{

		// --- Fields ---
		#region Private Static Fields       
		private static bool done = false;                                       // Bool Variable To Exit Main Loop                      
		private static bool active = false;
		private static bool movePositive = true;
		private static bool zoomIn = true;                      
		#endregion

		#region Private Fields
		private int screen;
        
		private GlFont2 glFont2;
                
		private Queue images = new Queue();
		private Queue visibleImages = new Queue();

		private FileInfo currentFile;
		private string fileStatus = ""; 
		private int currentPage = -1;
		private int currentIndex = -1;        

		private bool fullScreen = false;
		private bool standAlone = false;

		private SlickrWindow window;
		#endregion

		public SlickrOpenGL(SlickrWindow window, int screen, bool fullScreen, bool standAlone) { 
			this.window = window;
			this.screen = screen;
			this.fullScreen = fullScreen;
			this.standAlone = standAlone;
		}

		#region State
		/// <summary>
		///     Check if done set
		/// </summary>
		public bool Done { get { return done; } set { done = value; } }

		/// <summary>
		///     Check if OpenGL subsystem is active yet.
		/// </summary>		
		public bool Active { get { return active; } set { active = value; } }

		/// <summary>
		/// Queued count, number of queued images.
		/// </summary>
		public int QueuedCount { get { return images.Count; } }

		/// <summary>
		/// Check if fullscreen.
		/// </summary>
		public bool FullScreen { get { return fullScreen; } set { fullScreen = value; } }
		
		/// <summary>
		/// Check if standalone (client) mode.
		/// </summary>
		public bool StandAlone { get { return standAlone; } }

		/// <summary>
		/// Screen number
		/// </summary>
		public int ScreenNumber { get { return screen; } }
		#endregion

		#region Init
		/// <summary>
		///     Initialize global OpenGL settings.
		/// </summary>
		public static void Init() 
		{
			Log.Debug("Initing IL, ILU, and ILUT");
			Il.ilInit();
			Ilu.iluInit();
			Ilut.ilutInit();
			Ilut.ilutRenderer(Ilut.ILUT_OPENGL);    
			Log.Debug("Done initing IL");
		}
		#endregion

		#region InitTextures
		/// <summary>
		///     Initialize textures. Move image from the queue into to visible queue.
		/// </summary>
		private void InitTextures() 
		{           
			bool hasMainImage = false;
			bool showingLogo = false;
			lock(visibleImages) 
			{
				foreach(SlickrImage image in visibleImages)
				{
					if (image.Temporary && image.Name != null && image.Name.Equals("Logo")) 
						showingLogo = true;
					if (image.IsFadingIn || image.IsFullyVisible) 
						hasMainImage = true;                        
				}
			}

			if (!hasMainImage) 
			{
				SlickrImage nextImage = null;
				lock(images) 
				{
					if (images.Count > 0) 
					{
						nextImage = (SlickrImage)images.Dequeue();                          
					}
					else if (Settings.Local.ShowLogo && !showingLogo) nextImage = GetLogo();
				}
				if (nextImage != null) 
				{
					Log.Debug("Adding to visible: " + nextImage);
					nextImage = PrepareImage(nextImage);
					if (nextImage != null) 
					{
						lock(visibleImages) 
						{
							visibleImages.Enqueue(nextImage);   
							currentPage = nextImage.SourcePage;
							currentIndex = nextImage.SourceIndex;
							if (nextImage.FileInfo != null) CurrentFile = nextImage.FileInfo;
							else SetFileInfoStatus(null);
						}
					}
				}                               
			}
		}
		#endregion

		#region CreateGLWindow
		public void CreateGLWindow() 
		{
			glFont2 = new GlFont2();
		}
		#endregion

		#region GetLogo
		/// <summary>
		///     Create and get the Slickr logo
		/// </summary>
		/// <returns>
		///     The Slickr logo
		/// </returns>
		public SlickrImage GetLogo() 
		{                   
			Log.Debug("Getting logo");
			Bitmap logo = Utils.LoadResourceBitmap("slickr-large.gif");
			Log.Debug("Logo: " + logo.Width + ", " + logo.Height);
			SlickrImage image = new SlickrImage(logo);
			//image.ZoomEnabled = false;
			image.MoveEnabled = false;          
			image.ZoomAmount = 1.5f;
			image.ZoomStart = -0.5f;
			image.Temporary = true;
			image.Name = "Logo";
			image.InitializeImage(window.Width, window.Height, window.MaxTextureSize);           
			Log.Debug("Returning logo");
			return image;
		}
		#endregion
    
		#region PrepareImage
		/// <summary>
		///     Prepare image (generate and store OpenGL texture)
		/// </summary>
		/// <param name="image">
		///     Image to prepare
		/// </param>
		/// <returns>
		///     The same prepared image, or null if an error occurred
		/// </returns>
		public SlickrImage PrepareImage(SlickrImage image) 
		{       
			try 
			{
				if (image.Initialized && !image.TextureGenerated) 
				{					
					image.GenerateTexture();
					image.DeinitializeImage();
				}
				return image;
			} 
			catch(Exception e) 
			{
				Log.Debug("Error preparing image: " + e);
			}
			return null;
		}
		#endregion		

		#region SetImage
		public void SetImage(SlickrImage image) 
		{
			Skip();

			lock(images) 
			{
				while(images.Count > 0) 
					((SlickrImage)images.Dequeue()).Close();                

				AddImage(image);
			}
		}
		#endregion

		#region Skip
		public void Skip() 
		{
			Log.Debug("Skipping screen: " + screen);
			AutoExpireVisible();
			lock(visibleImages) 
			{
				foreach(SlickrImage vimage in visibleImages) 
				{                   
					if (!vimage.IsFadingOut) 
						vimage.End();
				}
			}
		}
		#endregion

		#region AutoExpireVisible
		public void AutoExpireVisible() 
		{
			lock(visibleImages) 
			{
				foreach(SlickrImage vimage in visibleImages) 
				{
					vimage.AutoExpire = true;
				}
			}
		}
		#endregion

		#region AddImage
		public void AddImage(SlickrImage image) 
		{
			lock(images) 
			{
				Log.Debug("Adding image: " + image);                
				image.MoveEnabled = Settings.Local.PanEnabled;
				image.ZoomEnabled = Settings.Local.ZoomEnabled;
				images.Enqueue(image);
			}
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
			if (!active) return true;

			Gl.glDisable(Gl.GL_DEPTH_TEST);
			Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);            
			Gl.glLoadIdentity();                                                // Reset The View           

			try 
			{
				InitTextures(); 
			} 
			catch(Exception e) 
			{
				Log.Error("Error initing, and checking", e);
			}           

			Queue endedImages = new Queue();
			lock(visibleImages) 
			{           
				int count = visibleImages.Count;                
				for(int i = 0; i < count; i++)
				{
					SlickrImage image = (SlickrImage)visibleImages.Dequeue();                                   
					if (image.Temporary 
						&& !image.IsFadingOut && images.Count > 0) image.End();
                    
					if (image.SetStarted(zoomIn, movePositive)) 
					{
						zoomIn = !zoomIn;
						movePositive = !movePositive;
					}           
        
					image.CheckReset();

					if (!image.HasEnded()) 
					{   
						DrawGLTexture(image);               
						visibleImages.Enqueue(image);
					} 
					else 
					{
						endedImages.Enqueue(image);                     
					}
				}               
			}

			DrawText();

			Gl.glEnable(Gl.GL_DEPTH_TEST);

			while(endedImages.Count > 0) 
			{               
				try 
				{
					SlickrImage image = (SlickrImage)endedImages.Dequeue();
					image.DeleteTexture();                      
				} 
				catch(Exception e) 
				{
					Log.Error("Error deleting texture", e);
				}
			}
			return true;            
		}
		#endregion  
        
		#region GetColumn
		/// <summary>
		/// Get encoded column. [column1][column2]blahblah.jpg
		/// </summary>
		/// <param name="source">Source</param>
		/// <param name="startpos">Where to start from</param>
		/// <param name="endpos">The end position of the column</param>
		/// <param name="decode">Whether to decode result string</param>
		/// <returns>Column</returns>
		public String GetColumn(String source, int startpos, out int endpos, bool decode)
		{           
			int start = source.IndexOf("[", startpos) + 1;
			int end = source.IndexOf("]", startpos);
			//Log.Debug("Start: " + start + ", End: " + end);
			if (start != -1 && end != -1 && (end - start > 0))
			{
				source = source.Substring(start, end - start);
				if (decode) source = Utils.Decode(source);
				endpos = end;
			}
			else if (start != -1 && end != -1)
			{
				source = "";
				endpos = end;
			} 
			else 
			{
				endpos = source.Length;
				source = null;              
			}

			return source;
		}
		#endregion

		#region SetFileInfoStatus
		/// <summary>
		/// Set status for file info.
		/// </summary>
		/// <param name="fi">File path</param>      
		private void SetFileInfoStatus(FileInfo fi) 
		{
			if (fi != null) 
			{
				String name = fi.Name;
				//name = name.Replace(Settings.Local.SegmentDelimeter, "/");

				int startpos = 0;
				int endpos = -1;
				String desc = GetColumn(name, startpos, out endpos, true);
				startpos = endpos + 1;
				String owner = GetColumn(name, startpos, out endpos, true);

				//Log.Debug("Desc: " + desc);
				if (desc == null) desc = name;

				if (Settings.ShowOwnerInStatus && owner != null && !owner.Equals(""))
					fileStatus = desc + " by " + owner;
				else
					fileStatus = desc;
			} 
			else 
			{
				fileStatus = "";
			}
		}
		#endregion

		#region CurrentFile
		public FileInfo CurrentFile 
		{ 
			get { return currentFile; } 
			set { currentFile = value; SetFileInfoStatus(value); } 
		}   
        
		public String CurrentId
		{
			get 
			{
				if (CurrentFile == null) return null;
				String name = CurrentFile.Name;
				int startpos = 0;
				int endpos = -1;
				String desc = GetColumn(name, startpos, out endpos, true);
				startpos = endpos + 1;
				String owner = GetColumn(name, startpos, out endpos, true);
				startpos = endpos + 1;
				String id = GetColumn(name, startpos, out endpos, false);
				Log.Debug("Desc: " + desc + ", Owner: " + owner + ", Id: " + id);
				return id;
			}
		}
		#endregion

		#region DrawText
		private void DrawText() 
		{
			if (standAlone && !FullScreen) 
			{
				DrawTextInStandAloneMode();
				return;
			}

			if (Settings.Local.ShowStatus) 
			{
                
				//string header = String.Format("{0} Page:({1}/{2})", Settings.FlickrTypeLabel.Replace("/", "::"), 
				//  (Settings.CurrentPage - 1), Settings.CurrentPageCount);                             
				//glFont2.Draw(Width, Height, header, Alignment.RIGHT, Alignment.TOP);

				if (StatusManager.Status != null)
					glFont2.Draw(window.Width, window.Height, StatusManager.Status, Alignment.RIGHT, Alignment.BOTTOM);

			}
			if (fileStatus != null && Settings.Local.ShowFileName && !fileStatus.Trim().Equals("")) 
			{
				glFont2.Draw(window.Width, window.Height, fileStatus, Alignment.CENTER, Alignment.TOP);
			}
		}

		private void DrawTextInStandAloneMode() 
		{
			int decWidth = 10;
			int decHeight = 35;
			if (Settings.Local.ShowStatus) 
			{
				// Page, index
				/**
				if (currentPage > 0) 
				{
					string header = String.Format("{1} [{0}]", currentPage, 
						currentIndex + 1 + ((currentPage-1) * Settings.Flickr.FlickrPageSize));
					float hwidth = glFont2.GetCharWidth(header);
					float hheight = glFont2.GetCharHeight(header);
					glFont2.Draw(window.Width, window.Height, header, (int)(window.Width - hwidth - decWidth), (int)(window.Height - hheight - decHeight));             
				}*/

				if (StatusManager.Status != null) 
				{
					float cwidth = glFont2.GetCharWidth(StatusManager.Status);
					float cheight = glFont2.GetCharHeight(StatusManager.Status);
					glFont2.Draw(window.Width, window.Height, StatusManager.Status, (int)(window.Width - cwidth - decWidth), 0);

				}
			}
			if (fileStatus != null && Settings.Local.ShowFileName) 
			{
				float cwidth = glFont2.GetCharWidth(fileStatus);
				float cheight = glFont2.GetCharHeight(fileStatus);
				glFont2.Draw(window.Width, window.Height, fileStatus, (int)(((window.Width - decWidth) / 2) - (cwidth / 2)), (int)(window.Height - cheight - decHeight));
			}
		}
		#endregion

		#region DrawGLTexture
		private void DrawGLTexture(SlickrImage image) 
		{
			PointFloat p = image.GetMovement(window.Width, window.Height, true);
			float scale = image.GetScale(window.Width, window.Height);            
			float fade = image.GetPercentageFade();
			int texWidth = image.TextureWidth;
			int texHeight = image.TextureHeight;

			//float zoom = image.GetZoomAmount();
			//scale += (scale * zoom);

			//scale += stepScale;

			Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
			Gl.glLoadIdentity();                                                // Reset The Projection Matrix
			float r = (window.Width/scale);
			float b = (window.Height/scale);

			Gl.glOrtho(0, r, b, 0, -1.0f, 1.0f);
			Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
			Gl.glLoadIdentity();            
            
			Gl.glEnable (Gl.GL_BLEND); // for text fading
			Gl.glBlendFunc (Gl.GL_ONE, Gl.GL_ONE_MINUS_SRC_ALPHA);
			Gl.glColor4f (fade, fade, fade, fade);                                  
                                    
			float posX = p.x; //stepX + 
			float posY = p.y; //stepY + 
			//Log.Debug("[" + image.Name + "] Pos: " + posX + "x" + posY);
            
			image.DrawAtPoint(posX, posY);
            
			Gl.glDisable(Gl.GL_BLEND);                                              
		}
		#endregion

		#region InitGL
		/// <summary>
		///     All setup for OpenGL goes here.
		/// </summary>
		/// <returns>
		///     <c>true</c> on successful initialization, otherwise <c>false</c>.
		/// </returns>
		public bool InitGL() 
		{           
			Gl.glShadeModel(Gl.GL_SMOOTH);                                      // Enable Smooth Shading
			Gl.glClearColor(0, 0, 0, 0.5f);                                     // Black Background
			Gl.glClearDepth(1);                                                 // Depth Buffer Setup
			Gl.glEnable(Gl.GL_DEPTH_TEST);                                      // Enables Depth Testing
			Gl.glDepthFunc(Gl.GL_LEQUAL);                                       // The Type Of Depth Testing To Do          
			Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);         // Really Nice Perspective Calculations         
			return true;
		}
		#endregion		

		#region ReSizeGLScene
		/// <summary>
		///     Resizes and initializes the GL window.
		/// </summary>		
		public void ReSizeGLScene(int width, int height) 
		{
			Log.Debug("Resize: " + width + "x" + window.Height);
			Gl.glViewport(0, 0, width, height);                                 // Reset The Current Viewport           
            
			Gl.glMatrixMode(Gl.GL_PROJECTION);                                  // Select The Projection Matrix
			Gl.glLoadIdentity();                                                // Reset The Projection Matrix
			//double aspectRatio = (double)height/(double)width;
			//Glu.gluPerspective(45, (double)width/(double)height, 0.1, 100);          // Calculate The Aspect Ratio Of The Window                      

			//Gl.glFrustum(-0.5f, 0.5f, -0.5f*aspectRatio, 0.5f*aspectRatio, 1f, 1.0f);
			//SetPerspective(90, (double)width/(double)height, 0, 1);
			Gl.glOrtho(0.0f, width, 0.0f, height, -1.0f, 1.0f);
                        
			Gl.glMatrixMode(Gl.GL_MODELVIEW);                                   // Select The Modelview Matrix
			Gl.glLoadIdentity();                                                // Reset The Modelview Matrix           
		}
		#endregion ReSizeGLScene(int width, int height)             

		#region Center
		public void Center() 
		{
			window.Center();
		}
		#endregion
	}
}
