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
using System.Threading;
using System.Collections;
using Slickr.Util;

namespace Slickr
{		

	/// <summary>
	/// Searches flickr for images.
	/// </summary>
	public class FlickrImageQuery : ImageQuery
	{				
		private static Random random = new Random(DateTime.Now.Millisecond);		

		private int lastFlickrErrorCode = 0;
		
		#region Cached Variables		
		private SortedList cache = new SortedList();
		private long cacheFavoritesCount = -1;
		private long cacheUserAllCount = -1;
		#endregion

		#region Results
		/// <summary>
		/// Internal results class to keep track of photos, page count incrementor and randomize.
		/// </summary>
		class Results
		{
			public Photos Photos;
			public int PageCount;
			public bool randomize;

			/// <summary>
			/// Results with photos and page count.
			/// </summary>
			/// <param name="photos">Photos results</param>
			/// <param name="pageCount">What to increment the page count by</param>
			public Results(Photos photos, int pageCount) : this(photos, pageCount, false) { }

			/// <summary>
			/// Results with photos and page count.
			/// </summary>
			/// <param name="photos">Photos results</param>
			/// <param name="pageCount">What to increment the page count by</param>
			/// <param name="randomize">Whether to scramble the photo indexes</param>
			public Results(Photos photos, int pageCount, bool randomize) 
			{
				this.Photos = photos;				
				this.PageCount = pageCount;
				this.randomize = randomize;
				if (randomize) 
				{					
					Slickr.Util.Utils.Scramble(random, photos.PhotoCollection);					
					pageCount = 0;
				}
			}		
		}
		#endregion				

		#region Constructors
		/// <summary>
		/// Construct flickr image query
		/// </summary>
		public FlickrImageQuery()
		{
			FlickrManager.Init();
		}
		#endregion

		#region GetURL
		public static String GetUrl(String id) 
		{
			PhotoInfo pi = FlickrManager.Flickr.PhotosGetInfo(id);
			return pi.WebUrl;
		}
		#endregion

		#region GetUser
		/// <summary>
		/// Get user with search string. (Cached)
		/// </summary>
		/// <param name="user">User name, id, or email</param>
		/// <returns>User</returns>
		public User GetUser(String user)
		{
			User userCache = (User) cache[user + "_User"];
			if (userCache != null) return userCache;
			userCache = SearchUser(user);
			cache[user + "_User"] = userCache;
			return userCache;
		}
		
		/// <summary>
		/// Get user name from id. (Cached)
		/// </summary>
		/// <param name="id">Id</param>
		/// <returns>User name</returns>
		public String GetUserName(String id) 
		{
			try 
			{
				Person person = (Person) cache[id + "_UserId"];
				if (person != null) return person.UserName;

				person = FlickrManager.Flickr.PeopleGetInfo(id);
				if (person != null) 
				{					
					cache[id + "_UserId"] = person;
					return person.UserName;
				}

			}
			catch(FlickrException fe) 
			{
				Log.Error(fe);
			}
			return null;
		}

		/// <summary>
		/// Search for user, based on id, name or email
		/// </summary>
		/// <param name="suser">Search string</param>
		/// <returns>User</returns>
		public static User SearchUser(String suser)
		{			
			suser = suser.Trim();

			StatusManager.Status = "Finding user: " + suser;
			User user = null;
			Log.Debug("Finding " + suser + " by username");
			try 
			{
				user = FlickrManager.Flickr.PeopleFindByUsername(suser);
			} 
			catch(FlickrException fe) 
			{
				Log.Error(fe);
			}

			if (user == null) 
			{
				Log.Debug("Finding " + suser + " by email");
				try 
				{
					user = FlickrManager.Flickr.PeopleFindByEmail(suser);
				} 
				catch(FlickrException fe) 
				{
					Log.Error(fe);
				}
			}
			if (user == null)
			{
				try 
				{
					Log.Debug("Finding " + suser + " by user id");
					Person person = FlickrManager.Flickr.PeopleGetInfo(suser);
					if (person != null) 
					{
						//String userName = person.UserName;
						Log.Debug("Found person at id, finding " + suser + " by username");
						user = FlickrManager.Flickr.PeopleFindByUsername(suser);
					}
				} 
				catch(FlickrException fe) 
				{
					Log.Error(fe);
				}
			}
			StatusManager.Status = (user != null ? "Found user: " + suser : "User " + suser + " not found");
			return user;
		}

		#endregion

		#region GetContacts
		/// <summary>
		/// Get contacts for user. (Cached)
		/// </summary>
		/// <param name="user">User</param>
		/// <returns>Contacts</returns>
		private Contacts GetContacts(User user) 
		{
			Contacts contactsCache = (Contacts) cache[user.UserId + "_Contacts"];
			if (contactsCache != null) return contactsCache;			
			contactsCache = FlickrManager.Flickr.ContactsGetPublicList(user.UserId);			
			cache[user.UserId + "_Contacts"] = contactsCache;
			return contactsCache;
		}
		#endregion		
						
		#region GetGroup
		/// <summary>
		/// Get group from group name (url)
		/// </summary>
		/// <param name="groupName">End part of group url</param>
		/// <returns>Group</returns>
		public static GroupInfo GetGroup(String groupName) 
		{
			StatusManager.Status = "Finding group: " + groupName;
			GroupInfo gi = FlickrManager.Flickr.UrlsLookupGroup("http://flickr.com/groups/" + groupName.Trim());
			StatusManager.Status = (gi != null ? "Found group: " + groupName : "Group: " + groupName + " not found");
			return gi;
		}
		#endregion

		#region GetPhotoCount
		/// <summary>
		/// Get the number of photos for a user.
		/// </summary>
		/// <param name="userId">User id, name or email</param>
		/// <returns>Photo count</returns>
		public int GetPhotoCount(String userId) 
		{
			Person person = FlickrManager.Flickr.PeopleGetInfo(userId);
			return person.PhotosSummary.PhotoCount;
		}
		#endregion		

		#region Randomizing
		/// <summary>
		/// Get a random page
		/// </summary>
		/// <param name="totalCount">Total number of images</param>
		/// <param name="pageSize">Page size</param>
		/// <returns>Random page number from 1 to the max page</returns>
		public int GetRandomPage(long totalCount, int pageSize) 
		{			
			int maxPage = (int)Math.Ceiling((double)totalCount / (double)pageSize);
			if (maxPage == 0) return 0;

			int randomPage = random.Next(maxPage - 1) + 1;

			Log.Debug("Random page: " + randomPage + " (max: " + maxPage + ", #photos: " + totalCount + ")");

			return randomPage;						
		}
		#endregion

		#region GetUserPhotos
		/// <summary>
		/// Get user photos.
		/// </summary>
		/// <param name="uid">User id, name or email</param>
		/// <param name="page">Page</param>
		/// <param name="pageSize">Page size</param>
		/// <returns>Photos</returns>
        public Photos GetUserPhotos(String uid, int page, int pageSize)
        {
			PhotoSearchOptions options = new PhotoSearchOptions();
            options.UserId = uid;
            options.PerPage = pageSize;
            options.Page = page;
			options.SortOrder = PhotoSearchSortOrder.InterestingnessAsc;
            return FlickrManager.Flickr.PhotosSearch(options);
        }
		#endregion

		#region GetContactsPhotos
		/// <summary>
		/// Get photos from contacts.
		/// </summary>
		/// <param name="contacts">Contacts</param>
		/// <param name="index">Current contact index</param>
		/// <param name="page">Current page</param>
		/// <param name="count">Number of photos to get</param>
		/// <returns>Photos</returns>
		private PhotoCollection GetContactsPhotos(Contacts contacts, int index, int page, int count, User source, bool includeSelf) 
		{							
			PhotoCollection pc = new PhotoCollection();
			int numContacts = contacts.ContactCollection.Length;
			if (includeSelf) numContacts++;
			int contactIndex = index % numContacts;

			Log.Debug("Index: " + contactIndex + " (" + index + "), #Contacts: " + contacts.ContactCollection.Length);
			
			String userId = null;
			String userName = null;
			
			if (contactIndex >= contacts.ContactCollection.Length) 
			{
				userId = source.UserId;
				userName = source.UserName;
			}
			else 
			{
				userId = contacts.ContactCollection[contactIndex].UserId;
				userName = contacts.ContactCollection[contactIndex].UserName;
			}
			
			int contactPhotoCount = GetPhotoCount(userId);
			if (contactPhotoCount == 0) return pc;

			int maxPage = (int)Math.Ceiling((double)contactPhotoCount / (double)count);
			int contactPage = page % maxPage;
			Log.Debug("Photo count: " + contactPhotoCount + "/" + count + ", Page: " + contactPage + " (" + page + "), MaxPage: " + maxPage);

			Log.Debug("Getting contact photos: " + userName + " (" + userId + ")");							
			StatusManager.Status = "Finding " + userName + "'s photos";
			Photos userPhotos = GetUserPhotos(userId, contactPage, count);
			pc.AddRange(userPhotos.PhotoCollection);
			Log.Debug("Found " + userPhotos.PhotoCollection.Count + " photos");																												
			return pc;
		}
		#endregion

		#region GetPhotos
		/// <summary>
		/// Get photos.
		/// </summary>
		/// <param name="localSettings">Local settings</param>
		/// <param name="flickrSettings">Flickr settings</param>
		/// <param name="page">Page</param>
		/// <returns>Results</returns>
        private Results GetPhotos(SettingsLocal localSettings, SettingsFlickr flickrSettings, int page)
        {            
			int pageSize = flickrSettings.FlickrPageSize;

            switch (localSettings.FlickrMode)
            {
				case Settings.FlickrMode.LOCAL:
					return null;

                case Settings.FlickrMode.USER:                    

					User user = GetUser(localSettings.FlickrUser);
					if (user == null) return null;

					bool randomize = localSettings.Randomize && localSettings.RandomizeOptionEnabled;

                    switch (localSettings.FlickrUserMode)
                    {						

                        case Settings.FlickrUserMode.ALL:
                        {                     							

							if (randomize) 
							{
								if (cacheUserAllCount == -1) 
									cacheUserAllCount = GetPhotoCount(user.UserId);
								page = GetRandomPage(cacheUserAllCount, pageSize);
							}

                            Log.Debug("Query Flickr/User=" + user.UserId + ", Page: " + page + " (" + pageSize + ")");
                            PhotoSearchOptions options = new PhotoSearchOptions();
                            options.UserId = user.UserId;
                            options.PerPage = pageSize;
                            options.Page = page;
							StatusManager.Status = "Finding " + user.UserName + "'s photos";
                            Photos photos = FlickrManager.Flickr.PhotosSearch(options);
							return new Results(photos, 1, randomize);
                        }

						case Settings.FlickrUserMode.CONTACTS:
						{
							Log.Debug("Query Flickr/User=" + user.UserId + "/Contacts");
							StatusManager.Status = "Getting contacts...";
							Contacts contacts = GetContacts(user);
							if (contacts == null) return null;														

							int pageSizeOverride = 4;	
							int numContacts = contacts.ContactCollection.Length;
							if (localSettings.IncludeUserInContacts) numContacts++;

							bool wrapped = flickrSettings.FlickrCurrentContactIndex >= numContacts;
							if (wrapped) flickrSettings.FlickrCurrentContactIndex = 0;
							Log.Debug("Wrapped: " + wrapped);

							PhotoCollection pc = GetContactsPhotos(contacts, flickrSettings.FlickrCurrentContactIndex++, 
								flickrSettings.FlickrContactPage, pageSizeOverride, user, localSettings.IncludeUserInContacts);
							Photos photos = new Photos();
							photos.PhotoCollection = pc;
							return new Results(photos, wrapped ? 1 : 0);
						}

                        case Settings.FlickrUserMode.FAVORITES: 
						{                            
                            Log.Debug("Query Flickr/User=" + user.UserId + "/Favorites");
                            //if (flickr.IsAuthenticated && isCurrentUser) return flickr.FavoritesGetList().PhotoCollection;
							StatusManager.Status = "Finding " + user.UserName + "'s favorite photos";
						
							Photos photos = null;

							if (FlickrManager.IsAuthorized(true)) 
							{
								if (randomize) 
								{
									if (cacheFavoritesCount == -1) 
										cacheFavoritesCount = FlickrManager.Flickr.FavoritesGetList(user.UserId, 1, 1).TotalPhotos;
									page = GetRandomPage(cacheFavoritesCount, pageSize);
								}

								photos = FlickrManager.Flickr.FavoritesGetList(user.UserId, pageSize, page);						
							}
							else
							{
								if (cacheFavoritesCount == -1) 
									cacheFavoritesCount = FlickrManager.Flickr.FavoritesGetPublicList(user.UserId, 1, 1).TotalPhotos;
								page = GetRandomPage(cacheFavoritesCount, pageSize);

								photos = FlickrManager.Flickr.FavoritesGetPublicList(user.UserId, pageSize, page);																								
							}

							if (photos != null) 
							{
								foreach(Photo photo in photos.PhotoCollection) 
									photo.OwnerName = GetUserName(photo.UserId);
								
								return new Results(photos, 1, randomize);
							}
							return null;
                        }

                        case Settings.FlickrUserMode.SET:                            
                            String fset = localSettings.FlickrUserSet.Trim().ToLower();
                            Log.Debug("Query Flickr/User=" + user.UserId + "/Set=" + fset);
							StatusManager.Status = "Finding " + user.UserName + "'s set: " + fset;
                            Photosets photoSets = FlickrManager.Flickr.PhotosetsGetList(user.UserId);
                            foreach(Photoset photoSet in photoSets.PhotosetCollection) {
                                if (photoSet.Title.ToLower().Equals(fset))
                                {
                                    Photoset pset = FlickrManager.Flickr.PhotosetsGetPhotos(photoSet.PhotosetId);                                    
									PhotoCollection pc = pset.PhotoCollection;
									Photos photos = new Photos();
									photos.PhotoCollection = pc;
									photos.TotalPhotos = pc.Count;
                                    return new Results(photos, 0);
                                }
                            }
                            break;

                        case Settings.FlickrUserMode.TAG:
                            {
								String tags = localSettings.FlickrUserTag.Trim();
                                Log.Debug("Query Flickr/User=" + user.UserId + "/Tag=" + tags);								
                                PhotoSearchOptions options = new PhotoSearchOptions();
                                options.UserId = user.UserId;
                                options.Tags = tags;
								String tagMode = localSettings.FlickrUserTagMode;
                                options.TagMode = (tagMode.Equals("All") ? TagMode.AllTags : TagMode.AnyTag);
                                options.PerPage = pageSize;
                                options.Page = page;
								Log.Debug("Page: " + flickrSettings.FlickrUserTagPage + "," + pageSize);
								StatusManager.Status = "Finding " + user.UserName + "'s tags (" + tagMode.ToLower() + "): " + options.Tags;
                                Photos photos = FlickrManager.Flickr.PhotosSearch(options);								
                                return new Results(photos, 1);
                            }
                    }
                    break;

                case Settings.FlickrMode.GROUP:
                    {
                        String groupName = localSettings.FlickrGroup;
                        Log.Debug("Query Flickr/Group=" + groupName);
                        GroupInfo groupInfo = GetGroup(groupName);
                        String groupId = groupInfo.GroupId;
						StatusManager.Status = "Finding " + groupName + "'s photos";
                        Photos photos = FlickrManager.Flickr.GroupPoolGetPhotos(groupId, pageSize, page);                        
                        Log.Debug("New group page: " + flickrSettings.FlickrGroupPage);
                        return new Results(photos, 1);
                    }					

                case Settings.FlickrMode.EVERYONE:
                    {
                        switch (localSettings.FlickrEveryoneMode)
                        {
							case Settings.FlickrEveryoneMode.RECENT: 
							{
								Log.Debug("Query Flickr/Everyone/Recent");
								// Ignore page count, since recent images are always changing
								StatusManager.Status = "Finding recent photos";
								Photos photos = FlickrManager.Flickr.PhotosGetRecent(pageSize, page);
								return new Results(photos, 0);
							}

                            case Settings.FlickrEveryoneMode.TAG:
                                {
                                    Log.Debug("Query Flickr/Everyone/Tag=" + localSettings.FlickrEveryoneTag);
                                    PhotoSearchOptions options = new PhotoSearchOptions();
                                    options.Tags = localSettings.FlickrEveryoneTag.Trim();                                    
									String tagMode = localSettings.FlickrEveryoneTagMode;
									options.TagMode = (tagMode.Equals("All") ? TagMode.AllTags : TagMode.AnyTag);
                                    options.PerPage = pageSize;
                                    options.Page = page;

									StatusManager.Status = "Finding photos with " + tagMode.ToLower() + " tag(s): " + options.Tags;
                                    Photos photos = FlickrManager.Flickr.PhotosSearch(options);									
                                    return new Results(photos, 1);
                                }
							case Settings.FlickrEveryoneMode.INTERESTINGNESS: 
							{
								Log.Debug("Query Flickr/Everyone/Interestingness, Page=" + page);

								DateTime date = localSettings.FlickrEveryoneInterestDate;
								Photos photos = null; 

								if (date == DateTime.MinValue) 
								{
									StatusManager.Status = "Finding interesting photos";
									photos = FlickrManager.Flickr.InterestingnessGetList(PhotoSearchExtras.All, pageSize, page);
								} 
								else 
								{
									StatusManager.Status = String.Format("Finding interesting photos from {0:MM-dd-yyyy}", date);
									photos = FlickrManager.Flickr.InterestingnessGetList(date, PhotoSearchExtras.All, pageSize, page);
								}
								return new Results(photos, 1);
							}
                        }
                        break;
                    }
            }

            return null;
        }
		#endregion

		#region Query
		/// <summary>
		/// Query for photos.
		/// </summary>
		/// <param name="dm">Download manager</param>
		/// <param name="page">Page</param>
		/// <returns>Search results</returns>
		public SearchResults Query(DownloadManager dm, int page)
		{		
			try 
			{				
				Log.Debug("Doing flickr search");
				Results results = GetPhotos(Settings.Local, Settings.Flickr, page);		
				Photos photos = results.Photos;
				
				if (photos != null) 
				{
					int count = photos.PhotoCollection.Count;				

					Log.Debug("Page: " + photos.PageNumber + "/" + photos.TotalPages + ", Total: " + photos.TotalPhotos 
						+ ", Per page: " + photos.PhotosPerPage + ", Photo collection count: " + photos.PhotoCollection.Count);
					StatusManager.Status = "Found " + count + " photos";
		
					if (results.PageCount > 0) 
					{
						Settings.CurrentPage = page + results.PageCount;
						Log.Debug("Set current page: " + Settings.CurrentPage);

						if (photos.TotalPages > 0 && Settings.CurrentPage > photos.TotalPages) 
						{
							Log.Debug("Resetting page to 1");
							Settings.CurrentPage = 1;				

						}
					}										 
				}
				//StatusManager.Status = "";
				FlickrSearchResults searchResults = new FlickrSearchResults(FlickrManager.Flickr, photos);
				lastFlickrErrorCode = 0;
				return searchResults;
			} 
			catch(FlickrException fe) 
			{
				lastFlickrErrorCode = fe.Code;
				throw fe;
			}
		}		        
		#endregion

		/// <summary>
		/// Get the last flickr error code (0 if not errored)
		/// </summary>
		public int LastErrorCode { get { return lastFlickrErrorCode; } }

		#region Test Methods
		public static void TestSearch() 
		{
			DownloadManager dm = new DownloadManager(null, null, null);

			Settings.Flickr.FlickrPageSize = 15;

			FlickrImageQuery query = new FlickrImageQuery();
			SearchResults results = query.Query(dm, Settings.CurrentPage);
			Log.Debug("Results #" + results.Count);
			for(int i = 0; i < results.Count; i++) 
			{
				Log.Debug("Id: " + results.GetId(i));
				//Log.Debug("Url: " + results.GetUrl(i) + " (" + results.ShouldDownload(i) + ")");				
			}

		}

		public static void TestUserAll() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.USER;
			Settings.Local.FlickrUserMode = Settings.FlickrUserMode.ALL;
			Settings.Local.FlickrUser = "Simon Lieschke";				
			TestSearch();
		}

		public static void TestSet() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.USER;
			Settings.Local.FlickrUserMode = Settings.FlickrUserMode.SET;
			Settings.Local.FlickrUser = "Simon Lieschke";
			Settings.Local.FlickrUserSet = "2005";
			TestSearch();
		}

		public static void TestContacts() 
		{					
			Settings.Local.FlickrMode = Settings.FlickrMode.USER;
			Settings.Local.FlickrUserMode = Settings.FlickrUserMode.CONTACTS;
			Settings.Local.FlickrUser = "g4b3";                			
			TestSearch();
		}

		public static void TestFavorites() 
		{					
			Settings.Local.FlickrMode = Settings.FlickrMode.USER;
			Settings.Local.FlickrUserMode = Settings.FlickrUserMode.FAVORITES;
			Settings.Local.FlickrUser = "Thomas Hawk";                			
			TestSearch();
		}

		public static void TestGroup() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.GROUP;
			Settings.Local.FlickrGroup = "flickyawards";
			TestSearch();
		}

		public static void TestTags() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.EVERYONE;
			Settings.Local.FlickrEveryoneMode = Settings.FlickrEveryoneMode.TAG;
			Settings.Local.FlickrEveryoneTag = "fish";
			TestSearch();
		}

		public static void TestRecent() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.EVERYONE;
			Settings.Local.FlickrEveryoneMode = Settings.FlickrEveryoneMode.RECENT;
			Settings.Local.FlickrEveryoneTag = "fish";
			TestSearch();
		}

		public static void TestInteresting() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.EVERYONE;
			Settings.Local.FlickrEveryoneMode = Settings.FlickrEveryoneMode.INTERESTINGNESS;
			//Settings.Local.FlickrEveryoneInterestDate = DateTime.Today;
			//Settings.Flickr.FlickrInterestPage = 1;
			TestSearch();
		}

		public static void TestUserTags() 
		{
			Settings.Local.FlickrMode = Settings.FlickrMode.USER;
			Settings.Local.FlickrUserMode = Settings.FlickrUserMode.TAG;
			Settings.Local.FlickrUser = "Simon Lieschke";  
			Settings.Local.FlickrUserTag = "weather";
			TestSearch();
		}
		#endregion

		#region Test Main
		/// <summary>
		/// Main is the entry point.
		/// </summary>
		/// <param name="args">
		/// <description>
		/// A string array that contains the arguments passed in on the command line. For more info see the source code.
		/// </description>
		/// </param>
		//[STAThread] 
		public static void Test() 
		{
			try 
			{
                //TestUserTags();
				//TestSet();
					
				Settings.Local.Randomize = true;
				Settings.Local.RandomizeOptionEnabled = true;
				TestFavorites();

				/**
				Settings.Flickr.FlickrCurrentContactIndex = 5;
				Settings.Flickr.FlickrContactPage = 300;
				TestContacts();
				Thread.Sleep(1000);
				TestContacts();
				Thread.Sleep(1000);
				TestContacts();
				Thread.Sleep(1000);
				TestContacts();
				Thread.Sleep(1000);
				TestContacts();
				Thread.Sleep(1000);
				*/
				
				//TestUserAll();
				//TestRecent();
				//TestInteresting();

			} 
			catch(Exception e) 
			{
				Log.Error("Error in flickr query", e);
			}
		}
		#endregion
	}
}
