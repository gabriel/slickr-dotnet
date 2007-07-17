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

namespace Slickr
{
	/// <summary>
	/// Google image results.
	/// </summary>
	public class GoogleSearchResults : SearchResults
	{		
		
		private int start;
		private int end;
		private int total;
		private int pageTotal;
		private ArrayList resultList = new ArrayList();

		public GoogleSearchResults() {}

        public void DidFinishDownloading() { }

		public void Add(GoogleSearchResult result) 
		{
			resultList.Add(result);
		}

        public bool ShouldDownload(int index)
        {
            return true;
        }

		public String GetId(int index) 
		{
			return null;
		}

        public String GetOwner(int index)
        {
            return null;
        }

		public String GetUrl(int index) {  
			return ((GoogleSearchResult)resultList[index]).Url;
		}

		public String GetDestinationFileName(int index) 
		{
			return ((GoogleSearchResult)resultList[index]).GetDestinationFileName();
		}

        public String GetDesc(int index)
        {
            return GetDestinationFileName(index);
        }

		public int Count { get { return resultList.Count; } }

		public void SetStats(int start, int end, int total, int pageTotal) 
		{
			this.start = start;
			this.end = end;
			this.total = total;
			this.pageTotal = pageTotal;
		}	

		public override String ToString() 
		{
			StringBuilder sb = new StringBuilder();			
			foreach(GoogleSearchResult r in resultList) 
			{
				if (sb.Length > 0) sb.Append("\n");
				sb.Append(r.ToString());
			}
			return sb.ToString();
		}

	}

	/// <summary>
	/// Summary description for GoogleImageResult.
	/// </summary>
	public class GoogleSearchResult
	{	
		private String url;
		private int width;
		private int height;
		private int size;		

		public String Url { get { return url; } }

		public GoogleSearchResult(String url, int width, int height, int size)  
		{
			this.url = url;
			this.width = width;
			this.height = height;
			this.size = size;
		}

		public String GetDestinationFileName() 
		{
			StringBuilder sb = new StringBuilder();
			Uri uri = new Uri(url);
            sb.Append(uri.Host + Settings.Local.SegmentDelimeter);
			foreach(String s in uri.Segments) 
			{
				String segment = s;
				if (segment.Trim().Equals("/")) continue;
				if (segment.EndsWith("/")) segment = segment.TrimEnd('/');
                sb.Append(segment + Settings.Local.SegmentDelimeter);
			}			
			return sb.ToString().TrimEnd(';');
		}

		public override String ToString() 
		{
			return url + " (" + width + "x" + height + ", " + size + "k)";
		}
	}
}
