using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

using PlantControl.Model;

namespace PlantControl
{
	public class Config
	{
		public static void SetDBStringValue(string key, string val) {
			using (DataModelContext db = new DataModelContext()) {
				var setting = db.Settings.Select("Key = @Key", new { Key = key }).SingleOrDefault();
				if (setting == null) {
					setting = new Setting();
					setting.Key = key;
					setting.Value = val;
					db.Settings.Insert(setting);
				} else {
					setting.Value = val;
					db.Settings.Update(setting);
				}
			}
		}

		public static string GetDBStringValue(string key, string defaultVal) {
			using (DataModelContext db = new DataModelContext()) {
				var setting = db.Settings.Select("Key = @Key", new { Key = key }).SingleOrDefault();
				if (setting == null || setting.Value == null) {
					return defaultVal;
				} else {
					return setting.Value;
				}
			}
		}

		public static string GetRequiredDBStringValue(string key, string defaultValue = null) {
			var val = GetDBStringValue(key, defaultValue);
			if (val == null) throw new Exception("The db configuration " + key + " is required.");
			return val;
		}

		public static string GetStringValue(string key, string defaultValue = null) {
			var val = ConfigurationManager.AppSettings[key];
			if (val == null) return defaultValue;
			return val.ToString();
		}

		public static string GetRequiredStringValue(string key, string defaultValue = null) {
			var val = GetStringValue(key, defaultValue);
			if (val == null) throw new Exception("The configuration "+key+" is required.");
			return val;
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
