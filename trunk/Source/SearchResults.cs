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

namespace Slickr
{
	/// <summary>
	/// Search results interface.
	/// </summary>
	public interface SearchResults
	{
		String GetUrl(int index);		
		String GetDestinationFileName(int index);
		int Count { get; }
        bool ShouldDownload(int index);
        void DidFinishDownloading();
        String GetDesc(int index);
		String GetId(int index);
        String GetOwner(int index);		
	}
	
}
