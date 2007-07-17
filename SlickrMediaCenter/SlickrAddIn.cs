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
using System.Diagnostics;
using Microsoft.MediaCenter.AddIn;
using Slickr;
using Slickr.ScreenSaver;

namespace Slickr.MediaCenter
{ 
	public class SlickrAddIn : MarshalByRefObject, IAddInModule, IAddInEntryPoint 
	{ 

		void  IAddInModule.Initialize(IDictionary dictAppInfo, IDictionary dictEntryPoint) 
		{ 
			// Write any initialization code here 
		} 

		void IAddInModule.Uninitialize() 
		{ 			
			// Write any clean up code here 
		} 

		void IAddInEntryPoint.Launch(AddInHost host) 
		{ 
			Log.Enabled = true;
			Log.FileName = "c:/slickr.log";

			// This does not work and probably never will 
			// Fuck windows media center.. fuck it ..
			//ScreenSaverViewer screenSaver = new ScreenSaverViewer();
			//screenSaver.Run();
		} 
	} 
}