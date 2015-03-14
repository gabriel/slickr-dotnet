# API Key & Shared secret #

If you want to build and run the source, you'll need to obtain a Flickr API key and shared secret. (http://www.flickr.com/services/api/keys/)

Once you do this update: Source/FlickrManager.cs

```
  private static String apiKey = "[API KEY HERE]";        
  private static String sharedSecret = "[SHARED SECRET HERE]";
```

Update: This was changed to point to Settings.Local.ApiKey and Settings.Local.SharedSecret by default.

# Files #

  * Source/ - Main source files for the library (Slickr.dll)
  * ScreenSaver/ - Source files for the screen saver
  * SlickrConsole/ - Source files for the console

## Visual Studio Project Files ##

  * Source/slickr.sln - Visual Studio .Net Solution (includes all the projects)
  * Source/slickr.csproj - Slickr library project file
  * ScreenSaver/ScreenSaver.csproj - Screensaver project file
  * SlickrConsole/SlickrConsole.csproj - Console project file

## NAnt Build Scripts ##

  * Slickr.build - NAnt script for building Slickr
  * SlickrMono.build - NAnt script for building Slickr under Mono

## ChangeLog ##

  * http://slickr-dotnet.googlecode.com/svn/trunk/ChangeLog

# Libraries #
  * Tao - TAO C# OpenGL libary http://www.taoframework.com/Home
  * DevIL - Cross platform image library http://openil.sourceforge.net/
  * FlickrNet - Flickr .Net API library http://www.codeplex.com/FlickrNet
  * freeglut - OpenGL utility toolkit http://freeglut.sourceforge.net/
  * NSpring http://sourceforge.net/projects/nspring/