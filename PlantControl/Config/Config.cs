using System;
using System.Configuration;
using System.Collections.Generic;

namespace PlantControl
{
	public class Config
	{
		public static string GetStringValue(string key, string defaultValue = null) {
			var val = ConfigurationManager.AppSettings[key];
			if (val == null) return defaultValue;
			return val.ToString();
		}

		public static int GetIntValue(string key, int defaultValue = 0) {
			var val = GetStringValue(key, null);
			if (val == null) return defaultValue;
			return int.Parse(val);
		}

		public static bool GetBoolValue(string key, bool defaultValue = false) {
			var val = GetStringValue(key, null);
			if (val == null) return defaultValue;
			return bool.Parse(val);
		}

		/// <summary>
		/// Gets a path setting where one can use the following variables:
		/// {project-dir}
		/// </summary>
		/// <returns>The path value.</returns>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
		public static string GetPathValue(string key, string defaultValue = null) {
			var val = GetStringValue(key, null);
			if (val == null) val = defaultValue;
			if (val != null) {
				val = val.Replace("{project-dir}", GetProjectDirectory());
			}
			return val;
		}

		/// <summary>
		/// Gets the project directory path.
		/// Note that this method uses the base directory but then strips the standard bin/ paths aways.
		/// </summary>
		/// <returns>The project directory.</returns>
		public static string GetProjectDirectory() {
			var baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
			baseDir = baseDir.Replace("/bin/Debug/", "");
			baseDir = baseDir.Replace("/bin/Release/", "");
			return baseDir;
		}
	}
}
