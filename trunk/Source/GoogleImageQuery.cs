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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using Slickr.Util;

namespace Slickr
{
	/// <summary>
	/// Google image query.
	/// This is an implementation of the RandomWeb logic.
	/// </summary>
	public class GoogleImageQuery : ImageQuery
	{
		private String phrase;
        private Settings.SearchSafeness safeness; // active, images, off
		private String size; // xxlarge, xlarge
		private String fileType;
		private int start;

		private int lastErrorCode = 0;

		private static readonly String[] randomCameraFileNames = new String[] {
																				  "dcp0####", "dsc0####","dscn####","mvc-###","mvc00###",
																				  "IM00####","EX0000##", "P101####","100-####","dscf####",
																				  "pdrm####","pict####","CIMG####","img_####","imgp####"
																			  };  

		private static Random random = new Random(DateTime.Now.Millisecond);		
		
		public static String GetRandomCameraFileName() 
		{
			int index = random.Next(randomCameraFileNames.Length-1);
			String fileTemplate = randomCameraFileNames[index];
			while(fileTemplate.IndexOf('#') != -1) 
			{
				int num = random.Next(9);
				fileTemplate = Utils.ReplaceFirst(fileTemplate, '#', String.Format("{0}", num)[0]);
			}
			return fileTemplate;
		}

		public int LastErrorCode { get { return lastErrorCode; } }

		public override string ToString()
		{			
			return phrase + "[" + safeness + "/" + size + "/" + fileType + "/" + start + "]";			
		}


		public GoogleImageQuery(String phrase, Settings.SearchSafeness safeness, String size, String fileType, int start)
		{			
			this.phrase = phrase;
			this.safeness = safeness;
			this.size = size;
			this.fileType = fileType;
			this.start = start;
		}

		public SearchResults Query(DownloadManager dm, int page) 
		{
			String data = dm.Download(GetUri());
			return Parse(data);
		}

		public Uri GetUri() 
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("http://images.google.com/images?");
			if (phrase != null) sb.Append("q=" + phrase);
			if (fileType != null) sb.Append("+filetype:" + fileType);
			sb.Append("&hl=en&lr=lang_en&ie=UTF-8");
			
            switch (safeness)
            {
                case Settings.SearchSafeness.OFF: sb.Append("&safe=off"); break;
                case Settings.SearchSafeness.MODERATE: sb.Append("&safe=images"); break;
                case Settings.SearchSafeness.STRICT: sb.Append("&safe=active"); break;
            }            
			
            sb.Append("&sa=G");
			if (size != null) sb.Append("&imgsz=" + size);
			if (start > 0) sb.Append("&start=" + start);

			return new Uri(sb.ToString());
		}

		public SearchResults Parse(String data) 
		{
			GoogleSearchResults results = new GoogleSearchResults();
			ArrayList urlList = new ArrayList();

			// /imgres?imgurl=www.replayer.com/other/2000-09-09_elaine%27s_shower/DCP00987.jpg&
			Regex regexUrl = new Regex(@"\/imgres\?imgurl\=(.*?)&", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			Match matchUrl = regexUrl.Match(data);
			while (matchUrl.Success) 
			{
				Group g = matchUrl.Groups[1];				
				urlList.Add(g.Value);				
				matchUrl = matchUrl.NextMatch();
			}

			// 600 x 600 pixels - 52k
			Regex regexSize = new Regex(@"(\d*) x (\d*) pixels - (\d*)k", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			Match matchSize = regexSize.Match(data);
			int count = 0;
			while (matchSize.Success) 
			{
				if (count >= urlList.Count)
					throw new ApplicationException("Extra matches from size regex");

				String url = (String)urlList[count];

				try 
				{
					int width = Convert.ToInt32(matchSize.Groups[1].Value, 10);
					int height = Convert.ToInt32(matchSize.Groups[2].Value, 10);
					int size = Convert.ToInt32(matchSize.Groups[3].Value, 10);

                    if (width >= Settings.Local.MinWidth && height >= Settings.Local.MinHeight) 
					{
						GoogleSearchResult result = new GoogleSearchResult(url, width, height, size);
						results.Add(result);
					}
				} 
				catch(FormatException fe) 
				{
					throw new ApplicationException("Unable to extract size from regex result", fe);
				}
				count++;
				matchSize = matchSize.NextMatch();
			}

			// Results <b>1</b> - <b>20</b> of about <b>3,050,000</b> for <b><b>PHRASE</b> </b>
			Regex regexResults = new Regex(@"Results <b>(.*?)</b> - <b>(.*?)</b>.*?<b>(.*?)</b>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			Match matchResults = regexResults.Match(data);
			if (matchResults.Success) 
			{
				try 
				{
					Log.Debug("Start: " + matchResults.Groups[1].Value.Replace(",", ""));
					Log.Debug("End: " + matchResults.Groups[2].Value.Replace(",", ""));
					Log.Debug("Total: " + matchResults.Groups[3].Value.Replace(",", ""));
					
					int start = Convert.ToInt32(matchResults.Groups[1].Value.Replace(",", ""), 10);
					int end = Convert.ToInt32(matchResults.Groups[2].Value.Replace(",", ""), 10);
					int total = Convert.ToInt32(matchResults.Groups[3].Value.Replace(",", ""), 10);

					results.SetStats(start, end, total, urlList.Count);
				} 
				catch(FormatException fe) 
				{
					throw new ApplicationException("Unable to extract result stats from regex result", fe);
				}
			} else throw new ApplicationException("No matches for result regex");

			return results;
		}
	}
}
