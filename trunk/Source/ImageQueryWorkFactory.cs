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
using System.Threading;


namespace Slickr
{	

	/// <summary>
	/// Factory to build image query work.
	/// </summary>
	public class ImageQueryWorkFactory
	{	
		/// <summary>
		/// Get query work based on search type.
		/// </summary>
		/// <param name="dm">Download manager</param>
		/// <param name="page">Current page</param>
		/// <returns>Query work</returns>
		public static ImageQueryWork BuildQueryWork(
            DownloadManager dm, int page) 
		{
            switch (Settings.Local.SearchType) 
			{
                //case Settings.SearchType.SEARCH: return BuildGoogleQueryWork(directory, requestListener, downloadListener);
				case Settings.SearchType.FLICKR: 
				{
					if (Settings.Local.FlickrMode == Settings.FlickrMode.LOCAL) return null;
					return BuildFlickrQueryWork(dm, page);
				}
				default: return null;
			}
		}	
		
		/**
		public static ImageQueryWork BuildGoogleQueryWork(
            ImageDirectory directory,
			DownloadManager.RequestListener requestListener,
			DownloadManager.DownloadListener downloadListener) 
		{
			String phrase = GoogleImageQuery.GetRandomCameraFileName();				
			ImageQueryWork work = new ImageQueryWork(
                new GoogleImageQuery(phrase, Settings.Local.Safeness, "xxlarge", "jpg", 0),
                directory, 
				requestListener,
				downloadListener);
			return work;
		}*/

		/// <summary>
		/// Build flickr query work.
		/// </summary>
		/// <param name="dm">Download manager</param>
		/// <param name="page">Page</param>
		/// <returns>Query work</returns>
		public static ImageQueryWork BuildFlickrQueryWork(DownloadManager dm, int page) 
		{
			ImageQueryWork work = new ImageQueryWork(
				new FlickrImageQuery(),
				dm, page);
			return work;
		}
	}
}
