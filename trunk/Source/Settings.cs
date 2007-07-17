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
using Tao.Platform.Windows;
using System.IO;
using Slickr.Util;
using System.Windows.Forms;


namespace Slickr
{
	/// <summary>
	/// Settings.
	/// </summary>
    
	public class Settings
	{

		// File extension for settings
        private const string xmlExtention = ".config.xml";

		// File extenstion for flickr settings
        private const string xmlFlickrExtention = ".flickr.xml";		

		// Version string
		private static string version = "1.0.3";

		// Local settings
        private static SettingsLocal local = new SettingsLocal();
		
		// Flickr settings
        private static SettingsFlickr flickr = new SettingsFlickr();
		
		// Start time
        private static DateTime start = DateTime.Now;

		// Power status
        public static bool powerOff = false;                     

		// Flickr auth user id
		private static String flickAuthUserId;

		private static bool registered = false;

		// Query status
		private static bool queryOff = false;

		/// <summary>
		/// Static initilizer load
		/// </summary>
        static Settings()
        {
            Load();
        }
    
		/// <summary>
		/// Search type, Search (google) or Flickr.
		/// </summary>
		public enum SearchType : int
		{
			SEARCH = 1,
			FLICKR = 2
		}

		/// <summary>
		/// Flickr mode (User, Group, Everyone, Local)
		/// </summary>
        public enum FlickrMode : int
		{
			USER = 1,
			GROUP = 2,
            EVERYONE = 3,
			LOCAL = 4
		}

		/// <summary>
		/// Flickr user mode (for FlickrMode.USER)
		/// </summary>
        public enum FlickrUserMode : int
		{
			ALL = 1,
			FAVORITES = 2,
            CONTACTS = 3,
            SET = 4,
            TAG = 5
		}

		/// <summary>
		/// Flickr everyone mode (for FlickrMode.EVERYONE)
		/// </summary>
        public enum FlickrEveryoneMode : int
		{
			RECENT = 1,
			TAG = 2,
			INTERESTINGNESS = 3
		}

		/// <summary>
		/// Flickr contact type.
		/// </summary>
		public enum FlickrContactType : int
		{
			ALL = 1,
			FRIENDS = 2, 
			FAMILY = 3,
			BOTH = 4,
			NEITHER = 5
		}

		/// <summary>
		/// Search safenss (for google)
		/// </summary>
        public enum SearchSafeness : int {
            OFF = 1,
            MODERATE = 2,
            STRICT = 3
        }        

		public static bool Registered { get { return registered; } }

		/// <summary>
		/// Get the pictures base folder, defaults to "My Pictures" folder, otherwise returns the current directory.
		/// </summary>
		/// <returns>Pictures directory</returns>
		public static String GetPicturesDirectory() 
		{
			String picturesDirectory = CheckDirectory(
				Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));				

			if (picturesDirectory == null) picturesDirectory = Path.GetPathRoot(".");
			return picturesDirectory;
		}

		/// <summary>
		/// Get the pictures folder. This is pictures base folder + "/Slickr".
		/// </summary>
		/// <returns>Pictures folder</returns>
		private static String GetDefaultPicturesDirectory()
		{
			String picturesDirectory = GetPicturesDirectory();

			String directory = picturesDirectory + Path.DirectorySeparatorChar + "Slickr";
			return new DirectoryInfo(directory).FullName;
		}
		
		/// <summary>
		/// Get the cache directory, the user specified directory or the default.
		/// </summary>
		public static String CacheDirectory 
		{ 
			get 
			{								
				String bd = CheckDirectory(Settings.Local.CacheDirectory);
				if (bd != null) return bd;
				
				return GetDefaultPicturesDirectory();				
			}
		}

		/// <summary>
		/// Get the base directory, either the cache directory (for Flickr) or user specified directory (for Local)
		/// </summary>
		public static String BaseDirectory 
		{ 
			get 
			{				
				if (Settings.Local.FlickrMode == Settings.FlickrMode.LOCAL) 
				{
					String d = CheckDirectory(Settings.Local.LocalDirectory);
					if (d != null) return d;
					else return GetDefaultPicturesDirectory();
				}

				String bd = CheckDirectory(Settings.Local.CacheDirectory);
				if (bd != null) return bd;

                return GetDefaultPicturesDirectory();
			}
		}

		/// <summary>
		/// Check directory is ok.
		/// </summary>
		/// <param name="d">Directory</param>
		/// <returns>True if exists and not null</returns>
		public static String CheckDirectory(String d) 
		{			
	        if (d != null && !d.Equals(""))
            {
                DirectoryInfo di = new DirectoryInfo(d);
                if (di.Exists) return di.FullName;
            }
            return null;
		}

		/// <summary>
		/// Get the search type label. Can be used to generate a path.
		/// </summary>
		public static String SearchTypeLabel { 
			get { 
				switch(local.SearchType) 
				{
					case SearchType.SEARCH: return "Search"; 
					case SearchType.FLICKR: return FlickrTypeLabel;
					default: return "Unknown";
				}
			} 
		}

		/// <summary>
		/// Get query wait.
		/// </summary>
        public static int QueryWait
        {
            get
            {
                switch (local.SearchType)
                {
                    case SearchType.SEARCH: return local.SearchQueryWait;
                    case SearchType.FLICKR: return local.FlickrQueryWait;
                    default: return 600;
                }
            } 
        }

		/// <summary>
		/// Get flickr type label.
		/// This is used to generate the sub directory structure under the pictures folder.
		/// For example, User g4b3's favorites search type label is: User/g4b3/Favorites/
		/// </summary>
        public static String FlickrTypeLabel
        {
            get
            {
                switch (Settings.Local.FlickrMode)
                {
					case Settings.FlickrMode.LOCAL: return null;
                    case Settings.FlickrMode.USER:
                        String flickrUser = "User/" + Settings.Local.FlickrUser + "/";
                        switch (Settings.Local.FlickrUserMode)
                        {
                            case Settings.FlickrUserMode.ALL: return flickrUser;
                            case Settings.FlickrUserMode.CONTACTS: return flickrUser + "Contacts";
                            case Settings.FlickrUserMode.FAVORITES: return flickrUser + "Favorites";
                            case Settings.FlickrUserMode.SET: return flickrUser + "Set/" + Settings.Local.FlickrUserSet;
                            case Settings.FlickrUserMode.TAG: return flickrUser + "Tags/" + Settings.Local.FlickrUserTagMode + "/" + Settings.Local.FlickrUserTag;
                        }
                        break;

                    case Settings.FlickrMode.GROUP: return "Group/" + Settings.Local.FlickrGroup;

                    case Settings.FlickrMode.EVERYONE:
                        switch (Settings.Local.FlickrEveryoneMode)
                        {							
                            case Settings.FlickrEveryoneMode.RECENT: return "Everyone/Recent";
                            case Settings.FlickrEveryoneMode.TAG: return "Everyone/Tags/" + Settings.Local.FlickrEveryoneTagMode + "/" + Settings.Local.FlickrEveryoneTag;
							case Settings.FlickrEveryoneMode.INTERESTINGNESS: 
							{
								DateTime interestDate = Settings.Local.FlickrEveryoneInterestDate;
								if (interestDate.Equals(DateTime.MinValue)) 
									return "Everyone/Interestingness";
								else return "Everyone/Interestingness/Recent" + String.Format("{0:MM-dd-yyyy}", interestDate);
							}
                        }
                        break;
                }

                return "";
            }
        }

		/// <summary>
		/// Get the current page count (number of pages in last search).
		/// </summary>
		public static long CurrentPageCount
		{
			get
			{
				switch (Settings.Local.FlickrMode)
				{
					case Settings.FlickrMode.USER:
					switch (Settings.Local.FlickrUserMode)
					{
						case Settings.FlickrUserMode.ALL: return Settings.Flickr.FlickrUserPageSize;
						case Settings.FlickrUserMode.FAVORITES: return Settings.Flickr.FlickrFavsPageSize;
						case Settings.FlickrUserMode.TAG: return Settings.Flickr.FlickrUserTagPageSize;
					}
					break;			

					case Settings.FlickrMode.GROUP: return Settings.Flickr.FlickrGroupPageSize;

					case Settings.FlickrMode.EVERYONE:
					switch (Settings.Local.FlickrEveryoneMode)
					{							
						case Settings.FlickrEveryoneMode.TAG: return Settings.Flickr.FlickrTagPageSize;
						case Settings.FlickrEveryoneMode.INTERESTINGNESS: return Settings.Flickr.FlickrInterestPageSize;						
					}
					break;
				}				
				return 0;
			}
		}

        public static bool ShowOwnerInStatus
        {
            get {
                switch (Settings.Local.FlickrMode)
                {
                    case Settings.FlickrMode.USER:
                        switch (Settings.Local.FlickrUserMode)
                        {
                            case Settings.FlickrUserMode.CONTACTS: 
							case Settings.FlickrUserMode.FAVORITES: 
								return true;
                            default: return false;
                        }
                    case Settings.FlickrMode.GROUP:
                    case Settings.FlickrMode.EVERYONE:
                        return true;
                    default: return false;
                }
            }
        }

		/// <summary>
		/// Get the current page.
		/// </summary>
		public static int CurrentPage
		{
			get
			{
				switch (Settings.Local.FlickrMode)
				{
					case Settings.FlickrMode.USER:
					switch (Settings.Local.FlickrUserMode)
					{
						case Settings.FlickrUserMode.ALL: return Settings.Flickr.FlickrUserPage;
						case Settings.FlickrUserMode.FAVORITES: return Settings.Flickr.FlickrFavsPage;
						case Settings.FlickrUserMode.TAG: return Settings.Flickr.FlickrUserTagPage;
						case Settings.FlickrUserMode.CONTACTS: return Settings.Flickr.FlickrContactPage;
					}
						break;			

					case Settings.FlickrMode.GROUP: return Settings.Flickr.FlickrGroupPage;

					case Settings.FlickrMode.EVERYONE:
					switch (Settings.Local.FlickrEveryoneMode)
					{							
						case Settings.FlickrEveryoneMode.RECENT: return 1;
						case Settings.FlickrEveryoneMode.TAG: return Settings.Flickr.FlickrTagPage;
						case Settings.FlickrEveryoneMode.INTERESTINGNESS: return Settings.Flickr.FlickrInterestPage;						
					}
						break;
				}				
				return -1;
			}
		
			set 
			{
				switch (Settings.Local.FlickrMode)
				{
					case Settings.FlickrMode.USER:
					switch (Settings.Local.FlickrUserMode)
					{
						case Settings.FlickrUserMode.ALL: Settings.Flickr.FlickrUserPage = value; break;
						case Settings.FlickrUserMode.FAVORITES: Settings.Flickr.FlickrFavsPage = value;  break;
						case Settings.FlickrUserMode.TAG: Settings.Flickr.FlickrUserTagPage = value; break;
						case Settings.FlickrUserMode.CONTACTS: Settings.Flickr.FlickrContactPage = value; break;
					}
						break;			

					case Settings.FlickrMode.GROUP: Settings.Flickr.FlickrGroupPage = value; break;

					case Settings.FlickrMode.EVERYONE:
					switch (Settings.Local.FlickrEveryoneMode)
					{							
						case Settings.FlickrEveryoneMode.RECENT: break;
						case Settings.FlickrEveryoneMode.TAG: Settings.Flickr.FlickrTagPage = value; break;
						case Settings.FlickrEveryoneMode.INTERESTINGNESS: Settings.Flickr.FlickrInterestPage = value; break;				
					}
						break;
				}			
			}
		}

        public static String FlickAuthUserId { get { return flickAuthUserId; } set { flickAuthUserId = value; } }

		/// <summary>
		/// Local settings
		/// </summary>
        public static SettingsLocal Local { get { return local; } set { local = value; } }

		/// <summary>
		/// Flickr settings
		/// </summary>
        public static SettingsFlickr Flickr { get { return flickr; } set { flickr = value; } }

		/// <summary>
		/// Version
		/// </summary>
		public static string Version { get { return version; } }

		/// <summary>
		/// Load settings
		/// </summary>
		/// <param name="path">Path</param>
		/// <param name="type">Settings class</param>
		/// <returns>Settings</returns>
        private static object Load(String path, Type type)
        {            
            if (File.Exists(path) == false)
            {
                return null;
            }
            else
            {
                StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open));
                object obj = Utils.DeserializeObject(reader.ReadToEnd(), type);
                reader.Close();
                return obj;
            }
        }

		/// <summary>
		/// Save settings
		/// </summary>
		/// <param name="obj">Settings</param>
		/// <param name="directory">Directory</param>
		/// <param name="path">Settings file</param>
        private static void Save(object obj, String directory, String path)
        {			
			Directory.CreateDirectory(directory);			
            String s = Utils.SerializeObject(obj);
            StreamWriter writer = new StreamWriter(new FileStream(directory + "/" + path, FileMode.Create));
            writer.Write(s);
            writer.Flush();
            writer.Close();
        }

		/// <summary>
		/// Old settings path
		/// </summary>
		public static string OldSettingsPath { get { return Application.ExecutablePath + xmlExtention; } }

		/// <summary>
		/// Get settings directory
		/// </summary>
		public static string SettingsDirectory { 
			get { 
				String applicationDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				if (applicationDataDirectory == null) return OldSettingsPath;
				return applicationDataDirectory + "/Slickr";
			} 
		}

		/// <summary>
		/// Get old flickr settings path
		/// </summary>
		public static string OldFlickrSettingsPath { get { return Application.ExecutablePath + xmlFlickrExtention; } }		


		/// <summary>
		/// Get settings path
		/// </summary>
		public static string SettingsPath { get { return "Slickr.scr" + xmlExtention; } }

		/// <summary>
		/// Get flickr settings path
		/// </summary>
		public static string FlickrSettingsPath { get { return "Slickr.scr" + xmlFlickrExtention; } }

		/// <summary>
		/// Load old seettings
		/// </summary>
		/// <returns>True if old settings exist and were loaded</returns>
		public static bool OldLoad()
		{
			bool loaded = false;
			try 
			{				
				local = (SettingsLocal)Load(OldSettingsPath, typeof(SettingsLocal));
				if (local != null) loaded = true;
			}
			catch(Exception e)
			{
				Log.Error("Error loading settings", e);
			}
			if (local == null) local = new SettingsLocal();

			try 
			{				
				flickr = (SettingsFlickr)Load(OldFlickrSettingsPath, typeof(SettingsFlickr));
			} 
			catch(Exception e)
			{
				Log.Error("Error loading flickr settings", e);
			}
			if (flickr == null) flickr = new SettingsFlickr();
			return loaded;
		}

		/// <summary>
		/// Load settings
		/// </summary>
        public static void Load()
        {
			if (OldLoad()) return;

			try 
			{
				local = (SettingsLocal)Load(SettingsDirectory + "/" + SettingsPath, typeof(SettingsLocal));
			}
			catch(Exception e)
			{
				Log.Error("Error loading settings", e);
			}
            if (local == null) local = new SettingsLocal();

			try 
			{				
				flickr = (SettingsFlickr)Load(SettingsDirectory + "/" + FlickrSettingsPath, typeof(SettingsFlickr));
			} 
			catch(Exception e)
			{
				Log.Error("Error loading flickr settings", e);
			}
            if (flickr == null) flickr = new SettingsFlickr();
        }

		/// <summary>
		/// Save local settings
		/// </summary>
        public static void Save()
        {
            Save(local, SettingsDirectory, SettingsPath);            

			FileInfo oldSettings = new FileInfo(OldSettingsPath);
			if (oldSettings.Exists) oldSettings.Delete();			
		
			SaveFlickrSettings();
		}

		/// <summary>
		/// Save flickr settings
		/// </summary>
        public static void SaveFlickrSettings()
        {
            Log.Debug("Saving flickr settings");
            Save(flickr, SettingsDirectory, FlickrSettingsPath);

			FileInfo oldFlickrSettings = new FileInfo(OldFlickrSettingsPath);
			if (oldFlickrSettings.Exists) oldFlickrSettings.Delete();
        }

		/// <summary>
		/// Query should be off
		/// </summary>
		public static bool QueryOff { get { return queryOff; } set { queryOff = value; } }

		/// <summary>
		/// Application start time
		/// </summary>
        public static DateTime Start { get { return start; } set { start = value; } }
	       
	}

	/// <summary>
	/// Flickr settings
	/// </summary>
    [Serializable]	
    public class SettingsFlickr
    {
		// Page size
        private int pageSize = 20;

		// Current page
        private String currentContact = null;
		private int currentContactIndex = 0;
        private int contactPage = 1;
        private int tagPage = 1;
        private int groupPage = 1;
        private int userTagPage = 1;
        private int userPage = 1;
        private int favsPage = 1;
		private int interestPage = 1;

		// Last search page count results
		private long tagPageSize;
		private long groupPageSize;
		private long userTagPageSize;
		private long userPageSize;
		private long favsPageSize;
		private long interestPageSize;

		private bool authEnabled = false;
		private String authToken;
		private String authUser;

        public String FlickrCurrentContact { get { return currentContact; } set { currentContact = value; } }
		public int FlickrCurrentContactIndex { get { return currentContactIndex; } set { currentContactIndex = value; } }

        public int FlickrContactPage { get { return contactPage; } set { contactPage = value; } }
        public int FlickrTagPage { get { return tagPage; } set { tagPage = value; } }
        public int FlickrGroupPage { get { return groupPage; } set { groupPage = value; } }
        public int FlickrUserTagPage { get { return userTagPage; } set { userTagPage = value; } }
        public int FlickrFavsPage { get { return favsPage; } set { favsPage = value; } }
        public int FlickrUserPage { get { return userPage; } set { userPage = value; } }
		public int FlickrInterestPage { get { return interestPage; } set { interestPage = value; } }

		public long FlickrTagPageSize { get { return tagPageSize; } set { tagPageSize = value; } }
		public long FlickrGroupPageSize { get { return groupPageSize; } set { groupPageSize = value; } }
		public long FlickrUserTagPageSize { get { return userTagPageSize; } set { userTagPageSize = value; } }
		public long FlickrFavsPageSize { get { return favsPageSize; } set { favsPageSize = value; } }
		public long FlickrUserPageSize { get { return userPageSize; } set { userPageSize = value; } }
		public long FlickrInterestPageSize { get { return interestPageSize; } set { interestPageSize = value; } }
        
        public int FlickrPageSize { get { return pageSize; } set { pageSize = value; } }

		public String AuthToken { get { return authToken; } set { authToken = value; } }
		public String AuthUser { get { return authUser; } set { authUser = value; } }
		public bool AuthEnabled { get { return authEnabled; } set { authEnabled = value; } }

		/// <summary>
		/// Reset settings
		/// </summary>
        public void Reset()
        {
            currentContact = null;
            contactPage = 1;
            tagPage = 1;
            groupPage = 1;
            userTagPage = 1;
            userPage = 1;
            favsPage = 1;
			interestPage = 1;

			groupPageSize = 0;
			userTagPageSize = 0;
			userPageSize = 0;
			favsPageSize = 0;
			interestPageSize = 0;
        }
    }

	/// <summary>
	/// Local settings
	/// </summary>
    [Serializable]
    public class SettingsLocal
    {
        private String cacheDirectory = null;

        private Settings.SearchType searchType = Settings.SearchType.FLICKR;
        private Settings.FlickrMode flickrMode = Settings.FlickrMode.EVERYONE;
        private Settings.FlickrUserMode flickrUserMode = Settings.FlickrUserMode.ALL;
        private Settings.FlickrEveryoneMode flickrEveryoneMode = Settings.FlickrEveryoneMode.INTERESTINGNESS;
		private Settings.FlickrContactType flickrContactType = Settings.FlickrContactType.ALL;

        private String flickrUser = "";
        private String flickrUserSet = "";
        private String flickrUserTag = "";
        private String flickrGroup = "";
        private String flickrEveryoneTag = "";
		private String localDirectory = "";
		private DateTime flickrEveryoneInterestDate = DateTime.MinValue;

        private int minPrefetch = 2;
        private int searchQueryWait = 120;
        private int flickrQueryWait = 60;

        private int secondsFade = 5;
        private int secondsView = 20;
        private int maxTextureSize = 2048;
        private float zoomAmount = 0.12f;

        private int fillAmount = 5;
        private double historyDivider = 2d;
        private int maxHistorySize = 50;
        private double maxCacheSizeMB = 500;
        private int minImageCount = 5; //12;

        private int streamTimeout = 20;
        private int bufferSize = 4096;

        private String segmentDelimeter = ";";

        private int minWidth = 400;
        private int minHeight = 300;                

        private bool showFileName = false;
        private bool showStatus = true;

        private DateTime queryStart = DateTime.MinValue;

        private int maxRecentSize = 3;
        private int cleanWait = 15;               

        //private Settings.SearchSafeness safeness = Settings.SearchSafeness.STRICT; // active, images, off		

        private bool usePowerOffSetting = true;

		//private bool useOriginal = false;
		//private int maxOriginalSize = 1024;
		private String maxSizeLabel = "Large";

		private bool zoomEnabled = true;
		private bool panEnabled = true;
		private bool fadeEnabled = true;
		private bool showLogo = true;

		private String userTagMode = "Any";
		private String everyoneTagMode = "Any";

		private bool randomize = false;
		private bool randomizeOptionEnabled = false;

		private bool includeUserInContacts = false;

		private bool defineProxy = false;
		private String proxyIpAddress;
		private String proxyPort;
		private String proxyUsername;
		private String proxyPassword;
		private String proxyDomain;

		private String apiKey;
		private String sharedSecret;

		private bool overrideOpenGLCheck = false;

		public SettingsLocal() { }

		public bool DefineProxy { get { return defineProxy; } set { defineProxy = value; } }
		public String ProxyIpAddress { get { return proxyIpAddress; } set { proxyIpAddress = value; } }
		public String ProxyPort { get { return proxyPort; } set { proxyPort = value; } }
		public String ProxyUsername { get { return proxyUsername; } set { proxyUsername = value; } }
		public String ProxyPassword { get { return proxyPassword; } set { proxyPassword = value; } }
		public String ProxyDomain { get { return proxyDomain; } set { proxyDomain = value; } }        
		public String ApiKey { get { return apiKey; } set { apiKey = value; } }
		public String SharedSecret { get { return sharedSecret; } set { sharedSecret = value; } }
		public bool OverrideOpenGLCheck { get { return overrideOpenGLCheck; } set { overrideOpenGLCheck = value; } }

        //public Settings.SearchSafeness Safeness { get { return safeness; } set { safeness = value; } }
        public int MinWidth { get { return minWidth; } set { minWidth = value; } }
        public int MinHeight { get { return minHeight; } set { minHeight = value; } }        

        public int MinPrefetch { get { return minPrefetch; } }
        public int SearchQueryWait { get { return searchQueryWait; } }
        public int MaxTextureSize { get { return maxTextureSize; } set { maxTextureSize = value; } }
        public int SecondsFade { get { return secondsFade; } set { secondsFade = value; } }
        public int SecondsView { get { return secondsView; } set { secondsView = value; } }
        public int FillAmount { get { return fillAmount; } }
        public double HistoryDivider { get { return historyDivider; } }
        public int MaxHistorySize { get { return maxHistorySize; } }
        public double MaxCacheSizeMB { get { return maxCacheSizeMB; } set { maxCacheSizeMB = value; } }
        public float ZoomAmount { get { return zoomAmount; } }
        public int MinImageCount { get { return minImageCount; } }

        public String SegmentDelimeter { get { return segmentDelimeter; } }
        public int StreamTimeout { get { return streamTimeout; } }
        public int BufferSize { get { return bufferSize; } }
        
        public Settings.SearchType SearchType { get { return searchType; } set { searchType = value; } }
        
        public Settings.FlickrMode FlickrMode { get { return flickrMode; } set { flickrMode = value; } }
        
        public Settings.FlickrUserMode FlickrUserMode { get { return flickrUserMode; } set { flickrUserMode = value; } }
        public Settings.FlickrEveryoneMode FlickrEveryoneMode { get { return flickrEveryoneMode; } set { flickrEveryoneMode = value; } }
		public Settings.FlickrContactType FlickrContactType { get { return flickrContactType; } set { flickrContactType = value; } }

        public String FlickrUser { get { return flickrUser; } set { flickrUser = value; } }
        public String FlickrUserSet { get { return flickrUserSet; } set { flickrUserSet = value; } }
        public String FlickrUserTag { get { return flickrUserTag; } set { flickrUserTag = value; } }
        public String FlickrGroup { get { return flickrGroup; } set { flickrGroup = value; } }
        public String FlickrEveryoneTag { get { return flickrEveryoneTag; } set { flickrEveryoneTag = value; } }
		public DateTime FlickrEveryoneInterestDate { get { return flickrEveryoneInterestDate; } set { flickrEveryoneInterestDate = value; } }
		public String LocalDirectory { get { return localDirectory; } set { localDirectory = value; } }
        
        public DateTime QueryStart { get { return queryStart; } set { queryStart = value; } }
		public int FlickrQueryWait { get { return flickrQueryWait; } set { flickrQueryWait = value; } }
        
        public bool ShowFileName { get { return showFileName; } set { showFileName = value; } }
        public bool ShowStatus { get { return showStatus; } set { showStatus = value; } }

		public bool RandomizeOptionEnabled { get { return randomizeOptionEnabled; } set { randomizeOptionEnabled = value; } }
		public bool Randomize { get { return randomize; } set { randomize = value; } }

        public int MaxRecentSize { get { return maxRecentSize; } }
        public int CleanWait { get { return cleanWait; } }

        public bool UsePowerOffSetting { get { return usePowerOffSetting; } set { usePowerOffSetting = value; } }

        public String CacheDirectory { get { return cacheDirectory; } set { cacheDirectory = value; } }

		//public bool UseOriginal { get { return useOriginal; } set { useOriginal = value; } }
		//public int MaxOriginalSize { get { return maxOriginalSize; } set { maxOriginalSize = value; } }
		
		public String MaxSizeLabel { get { return maxSizeLabel; } set { maxSizeLabel = value; } }

		public bool ZoomEnabled { get { return zoomEnabled; } set { zoomEnabled = value; } }
		public bool PanEnabled { get { return panEnabled; } set { panEnabled = value; } }
		public bool FadeEnabled { get { return fadeEnabled; } set { fadeEnabled = value; } }
		public bool ShowLogo { get { return showLogo; } set { showLogo = value; } }

		public bool IncludeUserInContacts { get { return includeUserInContacts; } set { includeUserInContacts = value; } }

		public String FlickrUserTagMode { get { return userTagMode; } set { userTagMode = value; } }
		public String FlickrEveryoneTagMode { get { return everyoneTagMode; } set { everyoneTagMode = value; } }
    }
}
