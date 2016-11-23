// <copyright>
//   Copyright (c) 2016 All Rights Reserved
// </copyright>
// <author>Dan Krusi</author>
// <summary>
//   PlantControl
//   Full-Stack Automated Home Garden
//   https://github.com/dankrusi/PlantControl
// </summary>

using System;
using System.Net;
using System.Linq;

using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Log;
using Unosquare.Labs.EmbedIO.Modules;

using PlantControl.Model;
using System.Reflection;
using System.Collections.Generic;

namespace PlantControl.API
{
	public class EditController : Controller
	{
		internal class EditableProperty {
			public string Readonly = "";
			public string Name;
			public string Value;
			public string Placeholder;
		}

		private Entity _getObjectForContext(HttpListenerContext context) {
			// Init
			string type = context.QueryString("type");
			string id = context.QueryString("id");

			Entity obj = null;
			if(type == "plant") {
				obj = this.DB.Plants.Select("RowId = @Id",new{Id = id}).SingleOrDefault();
			} else if(type == "user") {
				obj = this.DB.Users.Select("RowId = @Id",new{Id = id}).SingleOrDefault();
			} else if(type == "datapoint") {
				obj = this.DB.DataPoints.Select("RowId = @Id",new{Id = id}).SingleOrDefault();
			}
			if(obj == null) throw new Exception("Object could not be found");

			return obj;
		}

		[WebApiHandler(HttpVerbs.Get, "/api/edit/properties")]
		public bool GetProperties(WebServer server, HttpListenerContext context)
		{
			try
			{
				// Init
				Entity obj = _getObjectForContext(context);

				// Setup a list of editable properties
				List<EditableProperty> props = new List<EditableProperty>();
				{
					EditableProperty prop = new EditableProperty();
					prop.Name = "Id";
					prop.Value = obj.RowId.ToString();
					prop.Readonly = "readonly";
					props.Add(prop);
				}

				// Populate list using reflection
				foreach (var propInfo in obj.GetType().GetProperties()) {
					object[] attributes = propInfo.GetCustomAttributes(typeof(EditableAttribute), true);
					if (attributes.Length == 1) {
						// Attribute
						EditableAttribute attribute = attributes[0] as EditableAttribute;
						// Get value
						object val = propInfo.GetValue(obj);
						// roperty with my custom attribute
						EditableProperty prop = new EditableProperty();
						prop.Name = propInfo.Name;
						prop.Value = val == null ? "" : val.ToString();
						prop.Placeholder = attribute.Placeholder;
						// Readonly?
						prop.Readonly = propInfo.GetCustomAttributes(typeof(ReadonlyAttribute), true).Length == 1 ? "readonly" : "";
						props.Add(prop);
					}
				}

				return context.JsonResponse(props);
			}
			catch (Exception ex)
			{
				return HandleError(context, ex, (int)HttpStatusCode.InternalServerError);
			}
		}


		[WebApiHandler(HttpVerbs.Get, "/api/edit/save")]
		public bool GetSave(WebServer server, HttpListenerContext context)
		{
			try
			{
				// Init
				Entity obj = _getObjectForContext(context);

				// Loop all editable properties and update the fields
				bool changed = false;
				foreach (var propInfo in obj.GetType().GetProperties()) {
					object[] attributes = propInfo.GetCustomAttributes(typeof(EditableAttribute), true);
					if (attributes.Length == 1) {
						// Readonly?
						if(propInfo.GetCustomAttributes(typeof(ReadonlyAttribute), true).Length == 1) continue;
						// Set the new value
						object val = propInfo.GetValue(obj);
						string newValue = context.QueryString("prop_"+propInfo.Name);
						string currentValue = val == null ? "" : val.ToString();
						if(newValue != null && newValue != currentValue) {
							if(propInfo.PropertyType == typeof(string)) {
								if(newValue != "") propInfo.SetValue(obj,newValue);
								else propInfo.SetValue(obj,null);
							} else if(propInfo.PropertyType == typeof(int)) {
								propInfo.SetValue(obj,int.Parse(newValue));
							} else if(propInfo.PropertyType == typeof(long)) {
								propInfo.SetValue(obj,long.Parse(newValue));
							} else if(propInfo.PropertyType == typeof(double)) {
								propInfo.SetValue(obj,double.Parse(newValue));
							} else if(propInfo.PropertyType == typeof(float)) {
								propInfo.SetValue(obj,float.Parse(newValue));
							} else if(propInfo.PropertyType == typeof(DateTime)) {
								propInfo.SetValue(obj,DateTime.Parse(newValue));
							} else {
								throw new Exception("This type is not supported.");
							}
							changed = true;
						}
					}
				}

				if(changed) {
					string type = context.QueryString("type");
					if(type == "plant") {
						this.DB.Plants.Update(obj as Plant);
					} else if(type == "user") {
						this.DB.Users.Update(obj as User);
					} else if(type == "datapoint") {
						this.DB.DataPoints.Update(obj as DataPoint);
					}
				}

				return context.JsonResponse(obj);
			}
			catch (Exception ex)
			{
				return HandleError(context, ex, (int)HttpStatusCode.InternalServerError);
			}
		}

	}
}

