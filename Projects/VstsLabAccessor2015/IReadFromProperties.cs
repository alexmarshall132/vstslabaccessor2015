// ----------------------------------------------------------------------
// <copyright file="IReadFromProperties.cs" company="West Peaks Consulting."> 
//     Copyright (c) West Peaks Consulting. All rights reserved. 
// </copyright> 
// ----------------------------------------------------------------------
namespace VstsLabAccessor2015
{
	using System.Collections.Generic;

	public interface IReadFromProperties
	{
		/// <summary>
		/// Reads properties for the object from the given <paramref name="properties"/>
		/// </summary>
		/// <param name="properties">
		///     The names and values of properties to be set on the object. Must not be null.
		/// </param>
		void ReadProperties(IEnumerable<KeyValuePair<string, object>> properties);
	}
}