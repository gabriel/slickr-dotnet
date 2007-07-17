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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Tao.DevIl;
using Tao.OpenGl;
using Slickr.Util;

namespace Slickr
{
	/// <summary>
	/// Slickr image.
	/// Contains pointer to OpenGL texture, bitmap and file data.
	/// </summary>
	public class SlickrImage
	{
		private FileInfo fileInfo;
		private Bitmap sourceImage;

		private int width;
		private int height;
		private int textureWidth;
		private int textureHeight;
		private string name;
		
		private int texture;
		private Bitmap image;

		private DateTime startForFade = DateTime.MinValue;
		private DateTime start = DateTime.MinValue;

		private int secondsView = Settings.Local.SecondsView;
		private int secondsFade = Settings.Local.SecondsFade;
		private float zoomAmount = Settings.Local.ZoomAmount;
		private float zoomStart = 0;
		
		private bool movePositive = false;		
		private bool zoomIn = false;

		//private int maxTextureSize = 1024;		

		private bool initialized = false;
		private bool textureGenerated = false;

		private bool moveEnabled = true;
		private bool zoomEnabled = true;		

		private bool autoExpire = true;
		private bool temporary = false;
		private bool overExpired = false;
		private bool started = false;

		private int sourcePage = -1;
		private int sourceIndex = -1;

		/// <summary>
		/// Slickr image from path
		/// </summary>
		/// <param name="fileInfo">File</param>
		public SlickrImage(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;			
		}

		/// <summary>
		/// Slickr image from bitmap
		/// </summary>
		/// <param name="image">Bitmap</param>
		public SlickrImage(Bitmap image)
		{
			this.sourceImage = image;			
		}

		/// <summary>
		/// Close/De-initialize image
		/// </summary>
		public void Close()
		 {
			DeinitializeImage();
			DeleteTexture();
		 }

		/// <summary>
		/// If an image autoexpires it will automatically de-initialize after its seconds view is reached
		/// </summary>
		public bool AutoExpire 
		{ 
			get { return autoExpire; } 
			set { 
				autoExpire = value; 
				if (value && overExpired) 
				{
					End();
				}
			} 
		}

		public bool Initialized { get { return initialized; } }
		public bool TextureGenerated { get { return textureGenerated; } }
		public FileInfo FileInfo{ get { return fileInfo; } }
		public string Name { get { return name; } set { name = value; } }

		public int SourcePage { get { return sourcePage; } set { sourcePage = value; } }
		public int SourceIndex { get { return sourceIndex; } set { sourceIndex = value; } }
		public bool MoveEnabled { get { return moveEnabled; } set { moveEnabled = value; } }
		public bool ZoomEnabled { get { return zoomEnabled; } set { zoomEnabled = value; } }
		public bool Temporary { get { return temporary; } set { temporary = value; } }
		public float ZoomAmount { get { return zoomAmount; } set { zoomAmount = value; } }
		public float ZoomStart { get { return zoomStart; } set { zoomStart = value; } }

		public int Width { get { return width; } }
		public int Height { get { return height; } }
		public int TextureWidth { get { return textureWidth; } }
		public int TextureHeight { get { return textureHeight; } }	

		/// <summary>
		/// Initialize the image.
		/// Re-size image, generate the texture.
		/// </summary>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// <param name="maxTextureSize">Max texture size</param>
		public void Initialize(int screenWidth, int screenHeight, int maxTextureSize) 
		{
			InitializeImage(screenWidth, screenHeight, maxTextureSize);			
			GenerateTexture();			
			DeinitializeImage();
		}

		/// <summary>
		/// Initialize image.
		/// </summary>
		/// <param name="maxTextureSize">Max texture size</param>
		public void InitializeImage(int maxTextureSize) 
		{
			InitializeImage(-1, -1, maxTextureSize);
		}		

		/// <summary>
		/// Initialize image. Resize to power of 2 square with "non-square" parts alpha blended out.
		/// </summary>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// /// <param name="maxTextureSize">Max texture size</param>
		public void InitializeImage(int screenWidth, int screenHeight, int maxTextureSize) 
		{				
			
			if (maxTextureSize == -1) 
			{
				if (screenWidth == -1 || screenHeight == -1) maxTextureSize = 1024;
				else if (screenWidth > screenHeight) maxTextureSize = Utils.FindNextPowerOfTwo(screenWidth, 512, Settings.Local.MaxTextureSize);
				else maxTextureSize = Utils.FindNextPowerOfTwo(screenHeight, 512, Settings.Local.MaxTextureSize);
			}

			Log.Debug("Max texture size: " + maxTextureSize);

			/**
			// Load image (DevIL)
			Il.ilGenImages(1, out imageHandle);
			Il.ilBindImage(imageHandle);
			if (!Il.ilLoadImage(fileInfo.FullName)) return;

			width = Il.ilGetInteger(Il.IL_IMAGE_WIDTH);
			height = Il.ilGetInteger(Il.IL_IMAGE_HEIGHT);
			*/
			
			Bitmap bitmap = null;
			if (sourceImage == null && fileInfo != null) 
			{				
				try 
				{
					bitmap = new Bitmap(fileInfo.FullName);
				} 
				catch(Exception e) 
				{
					throw new ApplicationException("Could not build image from file: " + fileInfo.FullName, e);
				}
			} 
			else 
			{
				bitmap = sourceImage;
			}

			if (bitmap == null) throw new ApplicationException("No image set");

			width = bitmap.Width;
			height = bitmap.Height;

			float aspectRatio = (float)width / (float)height;

			int size;
			int resizeHeight;
			int resizeWidth;

			if (width > height) 
			{
				resizeWidth = Utils.FindNextPowerOfTwo(width, 256, maxTextureSize);
				float increase = (float)resizeWidth / (float)width;
				resizeHeight = (int)Math.Round(height * increase);				
				size = resizeWidth;
			} 
			else 
			{
				resizeHeight = Utils.FindNextPowerOfTwo(height, 256, maxTextureSize);
				float increase = (float)resizeHeight / (float)height;
				resizeWidth = (int)Math.Round(width * increase);				
				size = resizeHeight;
			}

			Log.Debug("Scaling " + width + "x" + height + " to " + resizeWidth + "x" + resizeHeight + " and squared to " + size);
			long t1 = DateTime.Now.Ticks;
			image = Resize(bitmap, resizeWidth, resizeHeight, size, size);
			width = resizeWidth;
			height = resizeHeight;
			textureWidth = size;
			textureHeight = size;
			Log.Debug("Done: " + ((DateTime.Now.Ticks - t1)/TimeSpan.TicksPerSecond));;

			/**
			if (width != resizeWidth || height != resizeHeight) 
			{
				Log.Debug(("Scaling " + width + "x" + height + " to " + resizeWidth + "x" + resizeHeight);
				long t1 = DateTime.Now.Ticks;
				
				if (!Ilu.iluScale(resizeWidth, resizeHeight, 32)) 
					throw new ApplicationException(Utils.GetIlErrorString(Il.ilGetError()));					
							
				width = resizeWidth;
				height = resizeHeight;
				Log.Debug(("Done: " + ((DateTime.Now.Ticks - t1)/10000000f));
			}
			if (width != size || height != size) 
			{
				Il.ilClearColour(0f, 0f, 0f, 0f);
				Log.Debug(("Enlarging canvas " + resizeWidth + "x" + resizeHeight + " to " + size + "x" + size);
				long t1 = DateTime.Now.Ticks;
				Ilu.iluImageParameter(Ilu.ILU_NEAREST, Ilu.ILU_CENTER);
				if (!Ilu.iluEnlargeCanvas(size, size, 32)) 
					throw new ApplicationException(Utils.GetIlErrorString(Il.ilGetError()));
				textureWidth = size;
				textureHeight = size;
				Log.Debug(("Done: " + ((DateTime.Now.Ticks - t1)/10000000f));
			}*/
			initialized = true;
		}		
	
		/// <summary>
		/// Resize image.
		/// </summary>
		/// <param name="bitmap">Bitmap</param>
		/// <param name="destWidth">Desired width</param>
		/// <param name="destHeight">Desired height</param>
		/// <param name="canvasWidth">Canvas width</param>
		/// <param name="canvasHeight">Canvas height</param>
		/// <returns></returns>
		public Bitmap Resize(Bitmap bitmap, int destWidth, int destHeight, int canvasWidth, int canvasHeight) 
		{
			int sourceWidth = bitmap.Width;
			int sourceHeight = bitmap.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = (int)Math.Round((canvasWidth-destWidth)/2d);
			int destY = (int)Math.Round((canvasHeight-destHeight)/2d);

			Bitmap bmSized = new Bitmap(canvasWidth, canvasHeight, PixelFormat.Format32bppArgb);
			bmSized.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

			Graphics gr = Graphics.FromImage(bmSized);
			gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

			DateTime time = DateTime.Now;
			gr.DrawImage(bitmap, 
				new Rectangle(destX, destY, destWidth, destHeight),
				new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
				GraphicsUnit.Pixel);
			Log.Debug("Resize time: " + DateTime.Now.Subtract(time));

			gr.Dispose();
			return bmSized;
		}
	
		/// <summary>
		/// De-initialize image. (Dispose bitmap)
		/// </summary>
		public void DeinitializeImage()
		{
			Log.Debug("Deinitializing image: " + fileInfo);
			//Ilu.iluDeleteImage(imageHandle);
			if (image != null) image.Dispose();
			if (sourceImage != null) sourceImage.Dispose();
			initialized = false;
		}

		/// <summary>
		/// Delete the texture.
		/// </summary>
		public void DeleteTexture() 
		{
			if (textureGenerated) 
			{
				Log.Debug("Deleting texture: " + fileInfo);
				Gl.glDeleteTextures(1, new int[] { texture });				
				textureGenerated = false;
			}
		}

		/// <summary>
		/// Generate texture.
		/// </summary>
		public void GenerateTexture()
		{
			// Flip The Bitmap Along The Y-Axis
			//textureImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
			// Rectangle For Locking The Bitmap In Memory
			Rectangle rectangle = new Rectangle(0, 0, textureWidth, textureHeight);
			// Get The Bitmap's Pixel Data From The Locked Bitmap
			BitmapData bitmapData = image.LockBits(rectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			// Create OpenGL texture
			Gl.glGenTextures(1, out texture);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_S, Gl.GL_CLAMP);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_WRAP_T, Gl.GL_CLAMP);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR);
			Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR);
			//Ilut.ilutGLTexImage(0);			

			Gl.glTexImage2D(Gl.GL_TEXTURE_2D, 0, Gl.GL_RGBA, textureWidth, textureHeight, 0, Gl.GL_BGRA, Gl.GL_UNSIGNED_BYTE, bitmapData.Scan0);
			Log.Debug("Texture generated");
			textureGenerated = true;
		}

		/// <summary>
		/// Draw the image at the speicifed position.
		/// </summary>
		/// <param name="posX">X</param>
		/// <param name="posY">Y</param>
		public void DrawAtPoint(float posX, float posY) 
		{
			Gl.glEnable(Gl.GL_TEXTURE_2D);
			Gl.glBindTexture(Gl.GL_TEXTURE_2D, texture);

			Gl.glBegin(Gl.GL_QUADS);			
			Gl.glTexCoord2f(0, 0); Gl.glVertex3f(posX, posY, 0f);
			Gl.glTexCoord2f(0, 1); Gl.glVertex3f(posX, posY + textureHeight, 0f);
			Gl.glTexCoord2f(1, 1); Gl.glVertex3f(posX + textureWidth, posY + textureHeight, 0f);
			Gl.glTexCoord2f(1, 0); Gl.glVertex3f(posX + textureWidth, posY, 0f);						
			Gl.glEnd();		
							
			Gl.glFlush();								

			// Back to normal
			Gl.glDisable(Gl.GL_TEXTURE_2D);
		}					

		public override String ToString() 
		{
			return fileInfo + " (" + width + ", " + height + ") [" + textureWidth + ", " + textureHeight + "], init: " + initialized + ", gen: " + textureGenerated;
		}

		/// <summary>
		/// Check if the image should be filled to width of screen (and panned up and down), or the opposite.
		/// </summary>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// <param name="width">Image width</param>
		/// <param name="height">Image height</param>
		/// <returns></returns>
		private static bool IsWidthFill(int screenWidth, int screenHeight, int width, int height) 
		{										
			float ratio = (float)screenWidth / (float)screenHeight;
			float w = (ratio > 1.0 ? width/ratio : width);
			float h = (ratio < 1.0 ? height*ratio : height);
			return (w < h);
		}		

		/// <summary>
		/// Get the correct scaling for the screen size.
		/// If panning (movement is enabled), the image will fill the screen.
		/// </summary>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// <returns>Scale factor</returns>
		public float GetScale(int screenWidth, int screenHeight) 
		{
			return GetScale(screenWidth, screenHeight, moveEnabled);
		}

		/// <summary>
		/// Get the correct scaling for the screen size.
		/// </summary>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// <param name="fillScreen">Fill screen</param>
		/// <returns>Scale factor</returns>
		private float GetScale(int screenWidth, int screenHeight, bool fillScreen) 
		{								
			float scale = 1f;
			if (IsWidthFill(screenWidth, screenHeight, width, height)) 
			{
				if (fillScreen) scale = ((float)screenWidth/(float)width);
				else scale = ((float)screenHeight/(float)height);
			} 
			else 
			{
				if (fillScreen) scale = ((float)screenHeight/(float)height);
				else scale = ((float)screenWidth/(float)width);
			}

			if (zoomEnabled) 
			{
				float zoom = GetZoomAmount();
				scale += (scale * zoom);
			}
			return scale;			
		}

		/// <summary>
		/// Check if the non-autoexpired positioning needs to be reset and toggled.
		/// </summary>
		public void CheckReset() 
		{
			if (!autoExpire && IsFadingOut)
			{
				startForFade = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, secondsFade));
			}

			if (!autoExpire && GetPercentageDone(start) >= 1) 
			{
				zoomIn = !zoomIn;
				movePositive = !movePositive;
				start = DateTime.Now;
				startForFade = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, secondsFade));
				overExpired = true;
			}								  										
		}

		/// <summary>
		/// Zoom amount based on percentage done viewing.
		/// </summary>
		/// <returns></returns>
		public float GetZoomAmount() 
		{			
            if (zoomIn) return (GetPercentageDone(start) * zoomAmount) + zoomStart;
            else return ((1 - GetPercentageDone(start)) * zoomAmount) + zoomStart;
		}
		
		/// <summary>
		/// Get translated position.
		/// </summary>
		/// <returns></returns>
		public PointFloat GetTranslate() 
		{			
			float imageX = -(Math.Abs(textureWidth - width))/2f;
			float imageY = -(Math.Abs(textureHeight - height))/2f;
			
			return new PointFloat(imageX, imageY);
		}

		/// <summary>
		/// Get movement from the start based on done viewing completion.
		/// </summary>
		/// <param name="screenWidth">Screen width</param>
		/// <param name="screenHeight">Screen height</param>
		/// <param name="translate">Whether to translate</param>
		/// <returns>Position</returns>
		public PointFloat GetMovement(int screenWidth, int screenHeight, bool translate)
		{			
			float imageX = 0;
			float imageY = 0;
			
			float scale = GetScale(screenWidth, screenHeight);

			if (!moveEnabled) 
			{
				imageX = ((width * scale) - screenWidth)/scale;	
				imageY = ((height * scale) - screenHeight)/scale;	
			} 
			else if (IsWidthFill(screenWidth, screenHeight, width, height)) 
			{
				imageY = ((height * scale) - screenHeight)/scale;					
			} 
			else 
			{
				imageX = ((width * scale) - screenWidth)/scale;					
			}
			
			float pd = 0.5f;
			if (moveEnabled) 
			{
				pd = movePositive ? GetPercentageDone(start) : 1 - GetPercentageDone(start);
			}

			//float pd = (!moveEnabled ? 0.5f : (movePositive ? GetPercentageDone() : 1 - GetPercentageDone()));
			//Log.Debug(("d: " + pd + ", x: " + (imageX * pd) + ", y: " + (imageY * pd));
			PointFloat trans = GetTranslate();
			if (translate) return new PointFloat(-(imageX * pd) + trans.x, -(imageY * pd) + trans.y);
			else return new PointFloat(-(imageX * pd), -(imageY * pd));
		}

		/**
		public void Reset() 
		{
			Log.Debug("Resetting");
			startForFade = DateTime.MinValue;
			start = DateTime.MinValue;
		}*/

		/// <summary>
		/// Set the image viewing started.
		/// </summary>
		/// <param name="zoomIn">Whether to zoom in</param>
		/// <param name="movePositive">Whether to move left to right</param>
		/// <returns>True if this actually triggered the start, false if already started</returns>
		public bool SetStarted(bool zoomIn, bool movePositive) 
		{
			if (!started) 
			{				
				started = true;
				this.zoomIn = zoomIn;
				this.movePositive = movePositive;
				startForFade = DateTime.Now;
				start = DateTime.Now;
				Log.Debug("Set started");				
				return true;
			}
			return false;
		}		

		/// <summary>
		/// Get number of milliseconds from start time.
		/// </summary>
		/// <param name="start">Image view start</param>
		/// <returns>Millis</returns>
		public int GetMillisFromStarted(DateTime start) 
		{
			if (start == DateTime.MinValue) return 0;
			TimeSpan diff = DateTime.Now.Subtract(start);
			//Log.Debug(("Ms from started: " + (diff.Ticks/TimeSpan.TicksPerMillisecond));
			return (int)(diff.Ticks/TimeSpan.TicksPerMillisecond);
		}

		/// <summary>
		/// End this image. (Flag as ending)
		/// </summary>
		public void End() 
		{		
			Log.Debug("Ending");
			autoExpire = true;

			int diffStart = DateTime.Now.Subtract(startForFade).Seconds;
			if (diffStart < secondsFade) 
			{
				startForFade = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, secondsView-diffStart));
			} 
			else 
			{
				startForFade = DateTime.Now.Subtract(new TimeSpan(0, 0, 0, secondsView-secondsFade, 0));			
				//TimeSpan ts = DateTime.Now.Subtract(start);
				//secondsView = ts.Seconds + secondsFade;
			}
		}

		/// <summary>
		/// Check if image has ended.
		/// </summary>
		/// <returns></returns>
		public bool HasEnded()
		{
			if (!autoExpire) return false;
			return (GetMillisFromStarted(startForFade) - (secondsView * 1000)) > 0;
		}

		/// <summary>
		/// Get done viewing percentage.
		/// </summary>
		/// <param name="start">Image view start</param>
		/// <returns></returns>
		public float GetPercentageDone(DateTime start)
		{
            float diff = (secondsView * 1000) - GetMillisFromStarted(start);
            float perDone = (1 - (diff / (secondsView * 1000)));
			return perDone;
		}

		/// <summary>
		/// Get percentage fade based on done viewing percentage.
		/// </summary>
		/// <returns>Fade</returns>
		public float GetPercentageFade() 
		{			
			if (!Settings.Local.FadeEnabled) return 1;						
            
			float diffStart = (secondsFade * 1000) - GetMillisFromStarted(startForFade);

			if (!autoExpire && diffStart < 0) return 1;

            float diffEnd = ((secondsView - secondsFade) * 1000) - GetMillisFromStarted(startForFade);

            if (diffStart >= 0 && diffEnd > 0) return (1 - (diffStart / (secondsFade * 1000)));
            else if (diffEnd <= 0) return (1 - (-diffEnd / (secondsFade * 1000)));
			return 1;
		}

		/// <summary>
		/// Check if fully visible.
		/// </summary>		
		public bool IsFullyVisible
		{
			get { return GetPercentageFade() >= 1; }
		}

		/// <summary>
		/// Check if fading in.
		/// </summary>
		public bool IsFadingIn 
		{
			get 
			{
				float diffEnd = (secondsFade * 1000) - GetMillisFromStarted(startForFade);
				return (diffEnd > 0);
			}
		}

		/// <summary>
		/// Check if fading out
		/// </summary>
		public bool IsFadingOut 
		{
			get 
			{
				float diffEnd = ((secondsView - secondsFade) * 1000) - GetMillisFromStarted(startForFade);
				return (diffEnd <= 0);
			}
		}
	}
}
