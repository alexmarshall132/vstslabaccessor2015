// ----------------------------------------------------------------------
// <copyright file="PropertyHelper.cs" company="West Peaks Consulting."> 
//     Copyright (c) West Peaks Consulting. All rights reserved. 
// </copyright> 
// ----------------------------------------------------------------------
namespace VstsLabAccessor2015
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;

	/// <summary>
	/// Utility class for helping with management of object properties.
	/// </summary>
	internal static class PropertyHelper
	{
		internal static void SetVariables(object rootObject, IEnumerable<KeyValuePair<string, object>> properties)
		{
			if (rootObject == null)
			{
				throw new ArgumentNullException("rootObject");
			}

			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			IReadFromProperties reader = rootObject as IReadFromProperties;

			if (reader != null)
			{
				reader.ReadProperties(properties);
			}
			else
			{
				SetDirectVariables(rootObject, properties);
			}
		}

		internal static void SetDirectVariables(object rootObject, IEnumerable<KeyValuePair<string, object>> properties)
		{
			if (rootObject == null)
			{
				throw new ArgumentNullException("rootObject");
			}

			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			foreach (var kvp in properties)
			{
				PropertyInfo property = rootObject.GetType().GetProperties().SingleOrDefault(p => StringComparer.InvariantCultureIgnoreCase.Equals(p.Name, kvp.Key.Replace("_", String.Empty)));

				if (property == null)
				{
					continue;
				}

				if (kvp.Value == null || (String.Empty.Equals(kvp.Value) && property.PropertyType != typeof(string)))
				{
					continue;
				}

				TypeConverter converter = TypeDescriptor.GetConverter(property.PropertyType);

				if (converter == null)
				{
					Trace.TraceError("Failed to find converter for type '{0}'", property.PropertyType.FullName);

					continue;
				}

				property.GetSetMethod(true).Invoke(rootObject, new[] {converter.ConvertFrom(kvp.Value)});
			}
		}

		internal static IEnumerable<KeyValuePair<string, object>> SelectPropertiesAndTrimName(IEnumerable<KeyValuePair<string, object>> properties, string prefix)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			if (String.IsNullOrEmpty(prefix))
			{
				throw new ArgumentException("Must not be null or empty", "prefix");
			}

			return properties.Where(x => x.Key.StartsWith(prefix)).Select(kvp => new KeyValuePair<string, object>(kvp.Key.Substring(prefix.Length), kvp.Value));
		}
	}
}