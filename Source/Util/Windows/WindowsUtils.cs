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


namespace Slickr.Util.Windows
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
	public class DISPLAY_DEVICE
	{
		public int cb = 0;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string DeviceName = new String(' ', 32);

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceString = new String(' ', 128);

		public int StateFlags = 0;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceID = new String(' ', 128);

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string DeviceKey = new String(' ', 128);
	}

	/**
	[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
	public struct DISPLAY_DEVICE
	{
		public int cb;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public char[] DeviceName;        

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] DeviceString;

		public int StateFlags;
        
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] DeviceID;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
		public char[] DeviceKey;
	}*/

	public class WindowsUtils {
		//		[DllImport("gdi32.dll")]
		//		private static extern bool Rectangle(
		//			IntPtr hdc,
		//			int ulCornerX, int ulCornerY,
		//			int lrCornerX, int lrCornerY);
		//		public bool RectangleApi(IntPtr hdc, int X1, int Y1, int X2, int Y2)
		//		{
		//			return Rectangle(hdc,X1,Y1, X2, Y2);
		//		}

		[DllImport("Kernel32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
		public extern static bool GetDevicePowerState(IntPtr hDevice, out bool fOn);        

		[DllImport("user32.dll")]
		public extern static bool EnumDisplayDevices(string lpDevice,
			int iDevNum, [In, Out] DISPLAY_DEVICE lpDisplayDevice, int dwFlags);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern int SystemParametersInfo(
			int uAction, int uParam, string lpvParam, int fuWinIni);

		const int SPI_SETDESKWALLPAPER = 20;
		const int SPIF_UPDATEINIFILE = 0x01;
		const int SPIF_SENDWININICHANGE = 0x02;


		//		public static int RGB(int R,int G,int B)
		//		{
		//			return (R |(G<<8)|(B<<16));         
		//		}
		//
		//		[DllImport("gdi32.dll")] // -*** CreatePen cr = color. Stupid programmers!
		//		private static extern IntPtr CreatePen(int iStyle, int iWidth, int cr);
		//
		//		[DllImport("gdi32.dll")] // -*** MoveToEx use 0 for just plain MoveTo.
		//		private static extern bool MoveToEx(IntPtr hdc, int x, int y, IntPtr pt);
		//
		//		[DllImport("gdi32.dll")] // -*** LineTo
		//		private static extern bool LineTo(IntPtr hdc, int x, int y);		

		// ---------------------------------------------
		//		[DllImport("user32.dll")]
		//		private static extern IntPtr GetDC(IntPtr hWnd);
		//		public IntPtr GetDcApi(IntPtr hWnd)
		//		{
		//			return GetDC(hWnd);
		//		}

		//		[DllImport("gdi32.dll")]
		//		private static extern IntPtr CreateCompatibleBitmap(IntPtr HDC, int X, int Y);
		//		public IntPtr CreateCompatibleBitmapApi(IntPtr HDC, int X, int Y)
		//		{
		//			return CreateCompatibleBitmap(HDC, X, Y);
		//		}

		//		[DllImport("gdi32.dll")] // -*** CreateCompatibleDC Used to turn the htBitmap from a graphic to a real memory pointer we need.
		//		private static extern IntPtr CreateCompatibleDC(IntPtr HDC);
		//		public IntPtr CreateCompatibleDcApi(IntPtr HDC)
		//		{
		//			return CreateCompatibleDC(HDC);
		//		}
		// ----------------------------------------------------------------------------
		//		[DllImport("gdi32.dll")] // -*** BitBlt BitBltApi
		//		private static extern bool BitBlt(IntPtr HDC, int Top, int Left, 
		//			int Width, int Height, IntPtr SourceHDC, 
		//			int X, int Y, int ROP);
		//		public bool BitBltApi(IntPtr HDC, int Top, int Left, int Width, int Height, IntPtr SourceHDC, int X, int Y)
		//		{	
		//			// this value for ROP copies the buffer.
		//            bool b = BitBlt(HDC, Top, Left, Width, Height, SourceHDC, X, Y, 0x00CC0020); 
		//			return b;
		//		}
		//		// -----------------------------------------------------------------------
		//        [DllImport("gdi32.dll")]// -*** SelectObject SelectObjectApi 
		//		private static extern IntPtr SelectObject(IntPtr hMemDC, IntPtr hObject);
		//		public IntPtr SelectObjectApi(IntPtr hMemDC, IntPtr hObject)
		//		{
		//			return SelectObject(hMemDC, hObject);
		//		}
		//		// ----------------------------------------------------
		//		[DllImport("gdi32.dll")] // -*** DeleteDC DeleteDcApi
		//		private static extern bool DeleteDC(IntPtr hMemDC);
		//		public bool DeleteDcApi(IntPtr hMemDC)
		//		{
		//			return DeleteDC(hMemDC);
		//		}
		//		// --------------------------------------------------------
		//		[DllImport("gdi32.dll")]
		//		private static extern bool DeleteObject(IntPtr hpen);
		//		public bool DeleteObjectApi(IntPtr hObj)
		//		{
		//			return DeleteObject(hObj);
		//		}
		// --------------------------------------------------------
		//		[System.Runtime.InteropServices.DllImport("user32.dll")]
		//		private static extern int GetSystemMetrics(int nMetrics);
		//		public int GetSystemMetricsApi(int nMetrics)
		//		{
		//			return GetSystemMetrics(nMetrics);
		//		}
		// ---------------------------------------------
		//		[DllImport("user32.dll")] 
		//		private static extern int MessageBeep(uint n); 
		// 
		//		public void MessageBeepApi() 
		//		{ 
		//			MessageBeep(0x0);  
		//		} 
 

		/// Declares an API call to determine if a window is visible.
		/// /summary>
		/// param name="hWnd"> Handle to the window being checked for visibility.</param>
		/// returns> Returns true if the window is visible, false if not.</returns>
		[DllImport("user32.DLL",EntryPoint="IsWindowVisible")]
		private static extern bool IsWindowVisible(IntPtr hWnd);

		/// <summary>
		/// Declare an external API function call. A wrapper function calls this.
		/// </summary>
		/// <param name="hWnd"> Handle to the window being addressed.</param>
		/// <param name="rect"> Struct containing Size and Location to be filled with the client area size and loc.</param>
		/// <returns> true is success, otherwise false.</returns>
		[DllImport("user32.dll")]
		private static extern bool GetClientRect(IntPtr hWnd, ref Rect rect);

		/// <summary>
		/// Wrapper to call the IsWindowVisible api call.
		/// </summary>
		/// <param name="hWnd">Handle to the Desktop Properties window.</param>
		/// <returns></returns>
		public static bool IsWindowVisibleApi(IntPtr hWnd)
		{
			return IsWindowVisible(hWnd);
		}
		/// <summary>
		/// Wrapper to call GetClientRect API functions in user32.dll to get size of the client area of a window.
		/// </summary>
		/// <param name="hWnd"> This is a handle to the desktop properties dialog box</param>
		/// <param name="rect"> This is an instance of a RECT struct to be filled with the location & size of the client area.
		/// </param>
		/// <returns>true if successful, false if uncuccessful</returns>
		public static bool GetClientRectApi(IntPtr hWnd, ref Rect rect)
		{
			return GetClientRect(hWnd, ref rect);
		}

		//		public void CopyMemoryGraphicToScreen(Graphics screenGr, Graphics memGr, Rectangle rct)
		//		{
		//			IntPtr hdc = screenGr.GetHdc();
		//			IntPtr memHdc = memGr.GetHdc(); 
		//			IntPtr hMemdc = CreateCompatibleDC(memHdc);
		//			BitBltApi(hdc, rct.Top, rct.Left, rct.Width, rct.Height,hMemdc,rct.X,rct.Y);
		//			screenGr.ReleaseHdc(hdc);
		//			DeleteDcApi(hMemdc);
		//			memGr.ReleaseHdc(memHdc);
		//		}

		/// <summary>
		/// Set the wallpaper with image at path.
		/// </summary>
		/// <param name="path"></param>
		public static void SetWallpaper(string path) 
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", true);
			// For stretch
			key.SetValue(@"WallpaperStyle", "2");
			key.SetValue(@"TileWallpaper", "0");
			SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, path, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
		}
	}
}