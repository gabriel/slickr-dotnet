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
using System.IO;
using System.Threading;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Runtime.InteropServices; // So API functions can be called.
using Microsoft.Win32;
using Tao.OpenGl;
using Tao.DevIl;


namespace Slickr.Util
{
		

	/// <summary>
	/// Alignment constants for positioning.
	/// </summary>
	public enum Alignment : int
	{
		LEFT = 1,
		CENTER = 2,
		RIGHT = 3,
		TOP = 4,
		BOTTOM = 5
	}

	/// <summary>
	/// Request status states
	/// </summary>
	public enum RequestStatus : int
	{
		CONNECTING = 1,
		DOWNLOADING = 2,
		FINISHED = 3,
		ABORTED = 4,
        WAITING = 5,
		EXISTS = 6
	}
	
	/// <summary>
	/// An instance of this struct is passed GetClientRect API to get the size of the preview window.<br></br>
	/// A struct of ints that contains the location and size info for a screen region.<br></br>
	/// It is passed to GetClientRect API call as a ref variable so the members can be filled in.
	/// </summary>
	public struct Rect
	{
		public int left; 
		public int top; 
		public int right;
		public int bottom;

		public Rect(int l, int t, int r, int b)
		{
			left = l;
			top = t;
			right = r;
			bottom = b;
		}
	}	

	public struct Vector
	{
		public float x; 
		public float y; 
		public float z;

		public Vector(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}	
	
	public struct PointFloat
	{
		public float x;
		public float y;

		public PointFloat(float x, float y)
		{
			this.x = x;
			this.y = y;			
		}
	}      

	/// <summary>
	/// Comparator for files based on last write time
	/// </summary>
	public class FilesWriteDateComparer: IComparer
	{
		public int Compare (object f1, object f2)
		{
			FileInfo file1 = (FileInfo)f1;
			FileInfo file2 = (FileInfo)f2;

			if(file1.LastWriteTime == file2.LastWriteTime) return 0;
			else if(file1.CreationTime > file2.LastWriteTime) return 1;
			else return -1;
		}
	} 

	/// <summary>
	/// Comparator for files based on create time
	/// </summary>
	public class FilesCreateDateComparer: IComparer
	{
		bool asc;

		public FilesCreateDateComparer(bool asc) 
		{
			this.asc = asc;
		}

		public int Compare (object f1, object f2)
		{
			FileInfo file1 = (FileInfo)f1;
			FileInfo file2 = (FileInfo)f2;

			if(file1.CreationTime == file2.CreationTime) return 0;
			else if(file1.CreationTime > file2.CreationTime) return (asc ? 1 : -1);
			else return (asc ? -1 : 1);
		}

	} 

	/// <summary>
	/// A class of commonly used functions.
	/// </summary>
	public class Utils
	{

		/// <summary>
		/// A private instance of a random number generator for use by all functions in this class.
		/// </summary>
		private Random RandIntGenerator = new Random(); // Create an instance of a random number generator.
		/// <summary>
		/// Returns a non-negative integer between 0 and 32767 inclusive.
		/// </summary>
		/// <returns>a int positive random number between 0 and 32767 inclusive.</returns>
		public int rand() 
		{
			return RandIntGenerator.Next(32768); // Used 32767 Because it returns 0 to Parameter - 1;
		} 

		/// <summary>
		/// Returns an Int that seems to be in the range of approximately -(v/2)+1 to (v/2)-1 
		/// </summary>
		/// <param name="v"></param>
		/// <returns>an Int that seems to be in the range of approximately -(v/2)+1 to (v/2)-1.</returns>
		public int RAND(int v)
		{
			return ((RandIntGenerator.Next(32768)%(v))-((v)/2)); // WE use 32768 because we want this to work with a random in between 0 to 32767 inclusive.
		}
		// ----------------------------------------------------
		/// <summary>
		/// Returns an integer in the range of MinVal to MaxVal inclusive.
		/// </summary>
		/// <param name="MinVal"></param>
		/// <param name="MaxVal"></param>
		/// <returns>A random int between MinVal and MaxVal inclusive</returns>
		public int randInRange(int MinVal, int MaxVal)
		{
			return RandIntGenerator.Next(MinVal,MaxVal+1);
		}
		// ----------------------------------------------------
		// This does not seem to yield accurate results, but very close. 
		/// <summary>
		/// Convert an HSB to RGB color. Thanks to George Shepherd's site..
		/// http://www.syncfusion.com/FAQ/WindowsForms/FAQ_c85c.aspx#q982q
		/// </summary>
		/// <param name="h"></param>
		/// <param name="s"></param>
		/// <param name="v"></param>
		/// <param name="r"></param>
		/// <param name="g"></param>
		/// <param name="b"></param>
		private void ConvertHSBToRGB(float h, float s, float v, out float r, out float g, out float b) 
		{ 
			if (s == 0f) 
 			{ 
 			// if s = 0 then h is undefined 
 				r = v; 
 				g = v; 
 				b = v; 
 			} 
 			else 
 			{ 
 				float hue = (float)h; 
 				if (h == 360.0f) 
 				{ 
 					hue = 0.0f; 
 				} 
 				hue /= 60.0f; 
 				int i = (int)Math.Floor((double)hue); 
 				float f = hue - i; 
 				float p = v * (1.0f - s); 
 				float q = v * (1.0f - (s * f)); 
 				float t = v * (1.0f - (s * (1 - f))); 
 				switch(i) 
 				{ 
 					case 0: r = v; g = t; b = p; break; 
 					case 1: r = q; g = v; b = p; break; 
 					case 2: r = p; g = v; b = t; break; 
 					case 3: r = p; g = q; b = v; break; 
 					case 4: r = t; g = p; b = v; break; 
 					case 5: r = v; g = p; b = q; break; 
 					default: r = 0.0f; g = 0.0f; b = 0.0f; break; 
				} 
			} 
		} 
		// ----------------------------------------------------
		/// 
		/// Adjusts the specified Fore Color's brightness based on the specified back color and preferred contrast. <br>
		/// Thanks to George Shepherd's site..<br>
		/// http://www.syncfusion.com/FAQ/WindowsForms/FAQ_c85c.aspx#q982q<br>
		/// The fore Color to adjust. <br>
		/// The back Color for reference.<br> 
		/// Preferred contrast level. <br>
		/// This method checks if the current contrast in brightness between the 2 colors is <br>
		/// less than the specified contrast level. If so, it brightens or darkens the fore color appropriately. 
		/// 
	public void AdjustForeColorBrightnessForBackColor(ref Color foreColor, Color backColor, float prefContrastLevel) 
 	{ 
		float fBrightness = foreColor.GetBrightness(); 
 		float bBrightness = backColor.GetBrightness(); 
 		float curContrast = fBrightness - bBrightness; 
 		//float delta = prefContrastLevel - (float)Math.Abs(curContrast); 
		if((float)Math.Abs(curContrast) < prefContrastLevel) 
		{ 
			if(bBrightness < 0.5f) 
			{ 
				fBrightness = bBrightness + prefContrastLevel; 
				if(fBrightness > 1.0f) 
					fBrightness = 1.0f; 
			} 
			else 
			{ 
				fBrightness = bBrightness - prefContrastLevel; 
				if(fBrightness < 0.0f) 
					fBrightness = 0.0f; 
			} 
			float newr, newg, newb; 
			ConvertHSBToRGB(foreColor.GetHue(), foreColor.GetSaturation(), fBrightness, out newr, out newg, out newb); 
			foreColor = Color.FromArgb(foreColor.A, (int)Math.Floor(newr * 255f), (int)Math.Floor(newg * 255f), (int)Math.Floor(newb * 255f)); 
		}
	} 
 
		// <summary>
		// Fills a ref byte array with random numbers 0 to 255 inclusive per element. 
		// </summary>
		// <param name="rgb">R for Red, G for Green, B for Blue. The higher the number, the more intense the color.</param>
//		public void RandRGB(byte[] rgb) // No longer in use.
//		{
//			RandIntGenerator.NextBytes(rgb);
//			rgb[0] |= 64;
//			rgb[1] |= 64;
//			rgb[2] |= 64;
//		}

		// Calculate normal from vertices
		public static Vector CalculateNormal(Vector v1, Vector v2, Vector v3) 
		{
			double v1x, v1y, v1z, v2x, v2y, v2z;
			double nx,ny,nz;
			double vLen;

			Vector result;

			// Calculate vectors
			v1x = v1.x - v2.x;
			v1y = v1.y - v2.y;
			v1z = v1.z - v2.z;

			v2x = v2.x - v3.x;
			v2y = v2.y - v3.y;
			v2z = v2.z - v3.z;
	  
			// Get cross product of vectors
			nx = (v1y * v2z) - (v1z * v2y);
			ny = (v1z * v2x) - (v1x * v2z);
			nz = (v1x * v2y) - (v1y * v2x);

			// Normalise final vector
			vLen = Math.Sqrt((nx * nx) + (ny * ny) + (nz * nz));

			result.x = (float)(nx / vLen);
			result.y = (float)(ny / vLen);
			result.z = (float)(nz / vLen);

			return result;
		}

		// replacement for gluPerspective
		public static void SetPerspective(double fovy, double aspect, double zNear, double zFar)
		{
			double pi180 = 0.017453292519943295769236907684886;
			double top, bottom, left, right;
			top = zNear * Math.Tan(pi180*(fovy/2));
			bottom = -top;
			right = aspect*top;
			left = -right;

			Gl.glFrustum(left, right, bottom, top, zNear, zFar);			
		}

		/// <summary>
		/// Get DeIl error string.
		/// </summary>
		/// <param name="error">Error code</param>
		/// <returns>Error string</returns>
		public static String GetIlErrorString(int error) 
		{
			switch(error) 
			{
				case Il.IL_NO_ERROR: return "No detectable error has occured."; 
				case Il.IL_INVALID_ENUM:
					return "An unacceptable enumerated value was passed to a function.";  
				case Il.IL_OUT_OF_MEMORY:
					return "Could not allocate enough memory in an operation.";  
				case Il.IL_FORMAT_NOT_SUPPORTED:
					return "The format a function tried to use was not able to be used by that function.";  
				case Il.IL_INTERNAL_ERROR:
					return "A serious error has occurred. Please e-mail DooMWiz with the conditions leading up to this error being reported.";  
				case Il.IL_INVALID_VALUE:
					return "An invalid value was passed to a function or was in a file.";  
				case Il.IL_ILLEGAL_OPERATION:
					return "The operation attempted is not allowable in the current state. The function returns with no ill side effects.";  
				case Il.IL_ILLEGAL_FILE_VALUE:
					return "An illegal value was found in a file trying to be loaded.";  
				case Il.IL_INVALID_FILE_HEADER:
					return "A file's header was incorrect.";  
				case Il.IL_INVALID_PARAM:
					return "An invalid parameter was passed to a function, such as a NULL pointer.";  
				case Il.IL_COULD_NOT_OPEN_FILE:
					return "Could not open the file specified. The file may already be open by another app or may not exist.";  
				case Il.IL_INVALID_EXTENSION:
					return "The extension of the specified filename was not correct for the type of image-loading function.";  
				case Il.IL_FILE_ALREADY_EXISTS:
					return "The filename specified already belongs to another file. To overwrite files by default, call ilEnable with the IL_FILE_OVERWRITE parameter.";  
				case Il.IL_OUT_FORMAT_SAME:
					return "Tried to convert an image from its format to the same format.";  
				case Il.IL_STACK_OVERFLOW:
					return "One of the internal stacks was already filled, and the user tried to add on to the full stack.";  
				case Il.IL_STACK_UNDERFLOW:
					return "One of the internal stacks was empty, and the user tried to empty the already empty stack."; 
				case Il.IL_INVALID_CONVERSION:
					return "An invalid conversion attempt was tried.";  
				case Il.IL_LIB_JPEG_ERROR:
					return "An error occurred in the libjpeg library.";  
				case Il.IL_LIB_PNG_ERROR:
					return "An error occurred in the libpng library.";  
				case Il.IL_UNKNOWN_ERROR:
					return "No function sets this yet, but it is possible (not probable) it may be used in the future.";  
				case Ilut.ILUT_NOT_SUPPORTED:
					return "A type is valid but not supported in the current build."; 
				default: 
					return "Unknown";
			}					
		}

		/// <summary>
		/// Find the next power of 2 >= n. Start at min and do not go above max.
		/// </summary>
		/// <param name="n">N</param>
		/// <param name="min">Min</param>
		/// <param name="max">Max</param>
		/// <returns>Power of 2 >= N where N >= min and N &lt;= max</returns>
		public static int FindNextPowerOfTwo(int n, int min, int max) 
		{
			int p = (int)Math.Log(min, 2);
			int r = 0;
			while(r < max) 
			{				
				r = (int)Math.Pow(2, p);
				if (r >= n) return r;
				p++;
			}
			return max;
		}		

		/// <summary>
		/// Replace first occurance of c in source with replace.
		/// </summary>
		/// <param name="source">Source</param>
		/// <param name="c">C</param>
		/// <param name="replace">Replace</param>
		/// <returns>Modified string</returns>
		public static string ReplaceFirst(string source, char c, char replace)
		{
			int index = source.IndexOf(c);
			if (index == -1) 
			{
				return source;
			}
			return source.Substring(0, index) + replace + ((index+1) < source.Length ? source.Substring(index+1) : "");
		}

		/// <summary>
		/// Scramble the list object positions.
		/// </summary>
		/// <param name="random">Random</param>
		/// <param name="list">List</param>
		public static void Scramble(Random random, IList list) 
		{
			for(int i = 0; i < list.Count; i++) 
			{
				int index = random.Next(list.Count);
				Object obj = list[index];
				list.RemoveAt(index);			
				list.Add(obj);
			}
		}		

        /// <summary>
        /// Encode using URL encode and remove chars that cannot be using in a file path.
        /// </summary>
        /// <param name="s">Source string</param>
        /// <returns>String encoded (eg. with characters replaced by percent codes)</returns>
        public static String Encode(String s)
        {
            s = System.Web.HttpUtility.UrlEncode(s);
            s.Replace("*", "");
			s.Replace("<", "");
			s.Replace(">", "");
			s.Replace("?", "");
			s.Replace("|", "");
			s.Replace(":", "");
			s.Replace("\\", "");
			s.Replace("\"", "");
			
            return s;
        }

        /// <summary>
        /// Decode using URL decode.
        /// </summary>
        /// <param name="s">Source string</param>
        /// <returns>String decoded (without percent codes)</returns>
        public static String Decode(String s)
        {
            return System.Web.HttpUtility.UrlDecode(s);
        }

		/// <summary>
		/// Get image from resource.
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>Bitmap</returns>
		public static Bitmap LoadResourceBitmap(String path) 
		{
			Assembly a = Assembly.GetExecutingAssembly();
			return new Bitmap(a.GetManifestResourceStream("Slickr." + path));
		}

		/// <summary>
		/// Get image from resource.
		/// </summary>
		/// <param name="path">Path</param>
		/// <returns>Icon</returns>
		public static Icon LoadResourceIcon(String path) 
		{
			Assembly a = Assembly.GetExecutingAssembly();
			return new Icon(a.GetManifestResourceStream("Slickr." + path));
		}

        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="pObject">Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        public static String SerializeObject(Object pObject)
        {
            String XmlizedString = null;
            MemoryStream memoryStream = new MemoryStream();
            XmlSerializer xs = new XmlSerializer(pObject.GetType());
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

            xs.Serialize(xmlTextWriter, pObject);
            memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
            XmlizedString = Encoding.UTF8.GetString(memoryStream.ToArray());
            return XmlizedString;
        }

        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static Object DeserializeObject(String pXmlizedString, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(pXmlizedString));

            return xs.Deserialize(memoryStream);
        }        		
		// ------------------------------------------- End methods. ----------------------------------------------

	} // Class End
}
