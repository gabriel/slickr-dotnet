using System;
using System.IO;
using System.Collections;

namespace Slickr 
{
	/// <summary>
	/// The main class in the library, and the only one that's creatable.
	/// </summary>
	public class ArgParser : IDisposable 
	{
		#region Private Members
		private const string FILE_SPECIFIER = "@";
		private VVPairs _pairs = new VVPairs();
		#endregion

		#region ctors
		/// <summary>
		/// Constructor to be called when only the parameter array needs to be
		/// supplied.  It just calls the more elaborate constructor with the
		/// default variable specifier character and value separator characters.
		/// </summary>
		/// <param name="args">String array passed to the running executable from the
		/// command shell.</param>
		public ArgParser(string[] args) : this(args, '/', ':') {}

		/// <summary>
		/// Constructor called when the non-default variable specifier character
		/// and value separator characters are wanted.
		/// </summary>
		/// <param name="args">String array passed to the running executable from the
		/// command shell.</param>
		/// <param name="argDelimitter">A character used to identify the start of a 
		/// variable name in the argument list.</param>
		/// <param name="valueDelimitter">A character used to identify the start
		/// of the variable value in the variable-value pair string.</param>
		public ArgParser(string[] args,	char argDelimiter, char valueDelimiter)
		{
			if (args.Length > 0) 
			{
				ArrayList parameters = new ArrayList();
            
				// Do a pass through the parameters, looking for argument 
				// definition files.  Phase I pass.
				foreach(string rawData in args) 
				{
					if (rawData.IndexOf(FILE_SPECIFIER) == 0) 
					{
						// A file has been found
						string file = rawData.Substring(1);
                  
						if (File.Exists(file)) 
							this.ParseFile(file, argDelimiter, parameters);
					} 
					else // Doesn't start with "@".
						if (rawData.Trim().IndexOf(argDelimiter.ToString()) == 0)
						// A regular parameter.  Add it to the list to parse.
						// Trim off the first character, which just marks it
						// as the start of a variable.
						parameters.Add(rawData.Substring(1));
				}

				// Now the command line is fully expanded from any parameter files
				// that may have been present into the arraylist.
				foreach (string rawData in parameters) 
				{
					if (rawData.Length > 0) 
					{
						Int32 end = rawData.IndexOf(valueDelimiter.ToString());
						string variable = string.Empty;
						string value = string.Empty;

						// Split the variable name from the value, using the 
						// parameter delimiter to decide where in the raw string.
						if (end > 0) 
						{
							variable = rawData.Substring(0, end);

							if ((end + 1) < rawData.Length)
								value = rawData.Substring(end + 1);
						} 
						else
							variable = rawData;
   					
						_pairs.Add(variable, value);
					}
				}
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Function to determine if a parameter exists in the current list.
		/// </summary>
		/// <param name="srVariable">The name of the parameter to search for.</param>
		/// <returns>True if it is in the list, false otherwise.</returns>
		public bool Exists(string variable) 
		{
			foreach (VVPair pair in _pairs)
				if (pair.Variable == variable)
					return true;
			
			return false;
		}
		#endregion

		#region Private Methods
		private void ParseFile(string fileName,	char argDelimiter, ArrayList parameters) 
		{
			StreamReader streamFile  = new StreamReader(fileName);
			string[] RSPFileData = streamFile.ReadToEnd().Split('\n');
                     
			streamFile.Close();
                     
			// Go through the file, and expand the listed parameters
			// into the ArrayList of existing parameters.
			foreach (string line in RSPFileData) 
			{
				string trim = line.Trim();

				if (trim.IndexOf(argDelimiter.ToString()) == 0)
					// A variable found.  Trim off the variable
					// marker character.
					parameters.Add(trim.Substring(1));
			}
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Accessor method to get the value of an expected parameter
		/// from its name.  The accessor method is usually on the collection
		/// class, but it isn't exposed anywhere.  It's put here as a shortcut.
		/// </summary>
		public VVPair this [string	variable] 
		{
			get 
			{
				return (VVPair)_pairs[variable];
			}
		}

		/// <summary>
		/// Indexer to allow the parameters to be read by index, rather
		/// than name.  Just returns the collection class numerical indexer.
		/// </summary>
		public VVPair this[Int32 index] 
		{
			get 
			{
				return (VVPair)_pairs[index];
			}
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Called when the class is to be destroyed.
		/// </summary>
		public void Dispose() 
		{
			_pairs.Dispose();
			_pairs = null;
		}
		#endregion
	}
	
	/// <summary>
	/// Class used to hold the variable name and value of the 
	/// command line parameter.
	/// </summary>
	public class VVPair : IDisposable 
	{
		#region Private Members
		private	string _variable = string.Empty;
		private string _value = string.Empty;
		#endregion

		#region ctors
		/// <summary>
		/// Default constructor.  Can only be created by the YACLAP
		/// namespace.
		/// </summary>
		internal VVPair() {}

		/// <summary>
		/// Constructor called when the name of the variable and its value
		/// is known.
		/// </summary>
		/// <param name="srVariable">The name of the variable to add.</param>
		/// <param name="srValue">The value of the variable.</param>
		internal VVPair(string variable, string value) : this() 
		{
			_variable = variable;
			_value = value;
		}
		#endregion

		#region Public Properties
		/// <summary>
		/// Name of the variable.  Can only be set by the constructor.
		/// </summary>
		public string Variable 
		{
			get { return _variable; }
		}

		/// <summary>
		/// Value of the variable.  Can be set after construction.
		/// </summary>
		public string Value 
		{
			get { return _value; }
			set { _value = value; }
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Do nothing.  No objects to destroy.
		/// </summary>
		public void Dispose() {}
		#endregion
	}

	/// <summary>
	/// Collection class of variable-value pair objects. Only the Add() method
	/// is overridden.  It's internal because it's never exposed as a property
	/// or parameter anywhere in the library.
	/// </summary>
	internal class VVPairs : System.Collections.CollectionBase, IDisposable 
	{
		#region ctors
		/// <summary>
		/// Default constructor.  Just added to make the class non-createable
		/// outside of the library.
		/// </summary>
		internal VVPairs() : base() {}
		#endregion

		#region Public Methods
		/// <summary>
		/// Method called to add a new variable-value argument pair to 
		/// the collection.
		/// </summary>
		/// <param name="variable">The name of the variable to add.</param>
		/// <param name="value">The value of the variable.</param>
		public	void  	Add
			(string		variable,
			string		value) 
		{
			// Check to see if the variable exists already.  If it does,
			// change the value and return.
			foreach (VVPair pair in this.List) 
			{
				if (pair.Variable == variable) 
				{
					pair.Value = value;
         
					return;
				}
			}

			// Otherwise add it.
			VVPair	newPair	= new VVPair(variable, value);
			
			this.List.Add(newPair);
		}
		#endregion

		#region Accessors
		public VVPair this [string	variable] 
		{
			get 
			{
				foreach (VVPair pair in this.List)
					if (pair.Variable == variable)
						return pair;
				
				// If the name wasn't found in the collection, return an
				// empty object so the caller doesn't have to trap a null.
				// They'll just get a zero-length string for the value.
				return new VVPair();
			}
		}

		public VVPair this[Int32 index] 
		{
			get {return (VVPair)this.List[index];}
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Destroy the variable value pair objects in the collection.
		/// </summary>
		public void Dispose() 
		{
			foreach (VVPair pair in this.List)
				pair.Dispose();

			this.List.Clear();
		}
		#endregion
	}
}