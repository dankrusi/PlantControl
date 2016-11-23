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

namespace PlantControl.API
{
	public class PlantController : Controller
	{


		[WebApiHandler(HttpVerbs.Get, "/api/plants")]
		public bool GetPlants(WebServer server, HttpListenerContext context)
		{
			this.Authenticate(server,context);

			try
			{
				return context.JsonResponse(this.DB.Plants.SelectAll());
			}
			catch (Exception ex)
			{
				return HandleError(context, ex, (int)HttpStatusCode.InternalServerError);
			}
		}

		[WebApiHandler(HttpVerbs.Get, "/api/plant/{id}")]
		public bool GetPlant(WebServer server, HttpListenerContext context, int id)
		{
			this.Authenticate(server,context);

			try
			{
				//if (People.Any(p => p.Key == id))
				//{
				//	return context.JsonResponse(People.FirstOrDefault(p => p.Key == id));
				//}
				return false;
			}
			catch (Exception ex)
			{
				return HandleError(context, ex, (int)HttpStatusCode.InternalServerError);
			}
		}


	}
}

