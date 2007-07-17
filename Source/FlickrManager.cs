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
using FlickrNet;
using System.Net;

namespace Slickr
{
	/// <summary>
	/// Manager Flickr accessor.
	/// </summary>
	public class FlickrManager
	{

		private static Flickr flickr;

		private static String apiKey = Settings.Local.ApiKey;        
		private static String sharedSecret = Settings.Local.SharedSecret;	
		private static String frob = null;

		/// <summary>
		/// Static methods only
		/// </summary>
		private FlickrManager() { }		

		/// <summary>
		/// Get flickr.
		/// </summary>
		/// <returns></returns>
		public static Flickr Flickr
		{
			get 
			{
				Init();				
				return flickr;
			}
		}

		#region Inits
		/// <summary>
		/// Initialize the Flickr object.
		/// It first checks for authorization.
		/// </summary>
		public static void Init() 
		{			
			if (flickr == null) 
			{
					
				Flickr.CacheDisabled = true;

				if (Settings.Flickr.AuthEnabled && Settings.Flickr.AuthToken != null) 
				{
					flickr = new Flickr(apiKey, sharedSecret, Settings.Flickr.AuthToken);
					if (!IsAuthorizedImpl()) 
					{
						flickr = new Flickr(apiKey, sharedSecret);					
						Settings.Flickr.AuthToken = null;
					}
				}
				else 
					flickr = new Flickr(apiKey, sharedSecret);				
				
				try 
				{
					flickr.Proxy = ConfigureProxy();
				}
				catch(System.Security.SecurityException)
				{
					// Capture SecurityException for when running in a Medium Trust environment.
				}												
			}						
		}	
	
		/// <summary>
		/// Configure proxy.
		/// </summary>
		/// <param name="flickr">Flickr instance</param>
		/// <returns>Web proxy</returns>
		public static WebProxy ConfigureProxy() 
		{
			// Overriden proxy
			if (Settings.Local.DefineProxy) 
			{
				WebProxy proxy = new WebProxy();
				proxy.Address = new Uri("http://" + Settings.Local.ProxyIpAddress + ":" + Settings.Local.ProxyPort);
				if (Settings.Local.ProxyUsername != null && Settings.Local.ProxyUsername.Length > 0 )
				{
					NetworkCredential creds = new NetworkCredential();
					creds.UserName = Settings.Local.ProxyUsername;
					creds.Password = Settings.Local.ProxyPassword;
					creds.Domain = Settings.Local.ProxyDomain;
					proxy.Credentials = creds;
				}
				return proxy;
			} 
			else 
			{
				return WebProxy.GetDefaultProxy();					
			}
		}

		/// <summary>
		/// Re-initialize the Flickr object.
		/// </summary>
		public static void Reinit() 
		{
			Deinit();
			Init();
		}

		/// <summary>
		/// De-initialize the Flickr object.
		/// </summary>
		public static void Deinit() 
		{			
			flickr = null;
		}
		#endregion		

		#region Authorization
		/// <summary>
		/// Get the authorization URL.
		/// </summary>
		/// <returns>Authorization URL</returns>
		public static String Authorize() 
		{
			Init();
			frob = flickr.AuthGetFrob();			
			String url = flickr.AuthCalcUrl(frob, AuthLevel.Read);			
			//String url = flickr.AuthCalcWebUrl(AuthLevel.Read);
			return url;
		}
				
		/// <summary>
		/// Complete authorization.
		/// </summary>
		/// <returns>True if complete was ok</returns>
		public static bool CompleteAuthorize() 
		{
			Init();
			//String frob = flickr.AuthGetFrob();
			Auth token = flickr.AuthGetToken(frob);
			if (token != null) 
			{
				Log.Debug("Saving authorize: " + token.Token);
				Settings.Flickr.AuthEnabled = true;
				Settings.Flickr.AuthToken = token.Token;
				Settings.Flickr.AuthUser = token.User.UserId;
				Settings.SaveFlickrSettings();
				Reinit();				
				//return IsAuthorized();
				return true;
			}
			return false;
		}		
	
		/// <summary>
		/// Remove authorization
		/// </summary>
		/// <returns>True if was authorized and was successfully removed</returns>
		public static bool RemoveAuthorize() 
		{						
			if (Settings.Flickr.AuthEnabled) 
			{
				frob = null;
				Log.Debug("Removing authorize");
				Settings.Flickr.AuthEnabled = false;
				Settings.Flickr.AuthToken = null;
				Settings.Flickr.AuthUser = null;
				Settings.SaveFlickrSettings();
				Deinit();
			}
			return true;
		}
		
		/// <summary>
		/// Check if authorized, forced a call to Init() and removes if authorize fails.
		/// </summary>
		/// <returns>True if authorized</returns>
		public static bool IsAuthorized() 
		{
			return IsAuthorized(false);
		}
		
		/// <summary>
		/// Check if authorized
		/// </summary>
		/// <param name="simple">If true, forces a call to Init() and removes if authorize fails</param>
		/// <returns>True if authorized</returns>
		public static bool IsAuthorized(bool simple) 
		{
			if (!simple) Init();			
			bool auth = IsAuthorizedImpl();
			if (!simple && !auth) RemoveAuthorize();
			return auth;			 			
		}		
		
		/// <summary>
		/// Check if authorized (without Init() call)
		/// </summary>
		/// <returns>True if authorized</returns>
		private static bool IsAuthorizedImpl() 
		{
			Log.Debug("AuthEnabled: " + Settings.Flickr.AuthEnabled + ", AuthToken: " + Settings.Flickr.AuthToken);
			if (!Settings.Flickr.AuthEnabled || Settings.Flickr.AuthToken == null) return false;

			Log.Debug("Checking if authorized");			
			try 
			{
				Log.Debug("Token: " + Settings.Flickr.AuthToken);
				flickr.AuthCheckToken(Settings.Flickr.AuthToken);		
				Log.Debug("Check token was ok");
				return true;
			} 
			catch(FlickrException fe) 
			{
				Log.Debug("Not authorized: " + fe);					
				return false;
			}			
		}
		#endregion
	}
}
