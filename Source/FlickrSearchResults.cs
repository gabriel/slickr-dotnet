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
using System.Collections;
using System.Text;
using FlickrNet;
using System.Net;
using System.IO;
using Slickr.Util;

namespace Slickr
{
	/// <summary>
	/// Flickr search results.
	/// </summary>
	public class FlickrSearchResults : SearchResults
	{						
		private Flickr flickr;
		private Photos photos;
		private Hashtable sizes = new Hashtable();
	
		public static String ORIGINAL = "Original";
		public static String LARGE = "Large";
		public static String MEDIUM = "Medium";
		public static String SMALL = "Small";
		public static String THUMB = "Thumbnail";

		/// <summary>
		/// Construct search results for flickr.
		/// </summary>
		/// <param name="flickr">Flickr accessor</param>
		/// <param name="photos">Photos from search</param>
		public FlickrSearchResults(Flickr flickr, Photos photos) {
			this.flickr = flickr;
			this.photos = photos;
		}
		
		/// <summary>
		/// toString
		/// </summary>
		/// <returns>String</returns>
		public override String ToString() 
		{
			StringBuilder sb = new StringBuilder();			
			if (photos != null) 
			{
				foreach(Photo p in photos.PhotoCollection) 
				{
					if (sb.Length > 0) sb.Append("\n");
					sb.Append("Photo: " + p.PhotoId + ", " + p.Title);
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Check if we should download the image at index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>True if the image passes the size validation</returns>
        public bool ShouldDownload(int index)
        {
            try
            {
                Size size = GetSize(photos.PhotoCollection[index], photos.PhotoCollection[index].PhotoId);
                return (size.Width >= Settings.Local.MinWidth &&
                        size.Height >= Settings.Local.MinHeight);
            }
            catch(Exception e) 
            {
                Log.Error("Error getting size info", e);
                return false;
            }
        }

		/// <summary>
		/// Notified when the search results have been processed.
		/// </summary>
        public void DidFinishDownloading() {			
            Settings.SaveFlickrSettings();
        }

		/// <summary>
		/// Number of photos in results.
		/// </summary>		
		public int Count { get { return (photos != null ? photos.PhotoCollection.Count : 0); } }

		/// <summary>
		/// Get photo id at index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Id</returns>
		public String GetId(int index) 
		{
			Photo p = photos.PhotoCollection[index];
			return p.PhotoId;
		}

		/// <summary>
		/// Get url to photo at index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>URL</returns>
		public String GetUrl(int index) 
		{
			Photo p = photos.PhotoCollection[index];
			Size s = GetSize(p, p.PhotoId);
			if (s != null) 
			{
				return s.Source;
			}
			return null;
		}

		/// <summary>
		/// Get photo description at index
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Description</returns>
        public String GetDesc(int index)
        {
            Photo p = photos.PhotoCollection[index];
            if (p.Title != null) return p.Title;
            return p.PhotoId;
        }

        /// <summary>
        /// Get photo owner at index
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Owner</returns>
        public String GetOwner(int index)
        {
            Photo p = photos.PhotoCollection[index];
            return p.OwnerName;			
        }

		/// <summary>
		/// Check that the size is ok based on the flickr label.
		/// </summary>
		/// <param name="s">label</param>
		/// <returns>True if image is ok</returns>
		private bool CheckSize(Size s) 
		{
			if (Settings.Local.MaxSizeLabel.Equals(ORIGINAL)) return true;
			if (Settings.Local.MaxSizeLabel.Equals(LARGE) && s.Label.Equals(ORIGINAL)) return false;
			if (Settings.Local.MaxSizeLabel.Equals(MEDIUM) && 
				(s.Label.Equals(ORIGINAL) || s.Label.Equals(LARGE))) return false;
			return true;
		}

		/// <summary>
		/// Get the size of a photo to download. The biggest photo satisfying the max size rule.
		/// </summary>
		/// <param name="p">Photo</param>
		/// <param name="key">Photo id</param>
		/// <returns>The size to use</returns>
		private Size GetSize(Photo p, string key) 
		{
            Size size = (Size)sizes[key];
            if (size != null) return size;            
			
			Log.Debug("Getting size info for: " + p.PhotoId);
			Sizes photoSizes = flickr.PhotosGetSizes(p.PhotoId);			
			//Size original = null;
			foreach(Size s in photoSizes.SizeCollection) 
			{				
				//Label could be: "Thumbnail", "Small", "Medium", "Large" and "Original"					
				//Log.Debug("Size: " + s.Label + ", " + s.Source + ", " + s.Width + ", " + s.Height + ", " + s.Url);

				if (!CheckSize(s)) continue;

				if (s.Label.Equals(LARGE)) size = s;																
				else if (s.Label.Equals(ORIGINAL)) 
				{
					size = s;		
					break;					
				}
				else if (s.Label.Equals(MEDIUM) && size == null) size = s;					
			}

			/**
			if (size == null && original != null) 
			{
				Log.Debug("No size chosen and original is non-null, going to use original");
				size = original;
			}*/

			if (size != null) 
			{
				Log.Debug("Using size: " + size.Label);
				sizes[key] = size;			
			}
			return size;
		}        

		/// <summary>
		/// Get the destination path for the image. URL encode weird characters.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Destination path</returns>
		public String GetDestinationFileName(int index) 
		{
			String url = GetUrl(index);
			StringBuilder sb = new StringBuilder();

			String desc = GetDesc(index);
			if (desc == null) desc = "";
			desc = Slickr.Util.Utils.Encode(desc);
			sb.Append("[" + desc + "]");                			

            String owner = GetOwner(index);
            if (owner == null) owner = "";
            owner = Slickr.Util.Utils.Encode(owner);
            sb.Append("[" + owner + "]");
            
			String id = GetId(index);
			sb.Append("[" + id + "]");

			Uri uri = new Uri(url);
			sb.Append(uri.Segments[uri.Segments.Length-1]);						
			return sb.ToString();						
		}

	}
}
