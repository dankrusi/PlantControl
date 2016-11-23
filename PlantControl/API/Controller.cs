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
	public abstract class Controller : WebApiController
	{

		internal class NotAuthenticatedException : Exception {
			public NotAuthenticatedException(string desc) : base(desc) {}
		}


		protected DataModelContext DB = new DataModelContext();

		protected void Authenticate(WebServer server, HttpListenerContext context) {
			// Check for auth token
			try {
				// Get the cookie hash
				var authTokenCookie = context.Request.Cookies["AuthToken"];
				if(authTokenCookie == null) throw new NotAuthenticatedException("You must login.");
				var authTokenHash = authTokenCookie.Value;
				if(authTokenHash == null) throw new NotAuthenticatedException("You must login.");
				// Check token
				var token = this.DB.AuthTokens.Select("Hash = @Hash", new { Hash = authTokenHash}).SingleOrDefault();
				if(token == null) throw new NotAuthenticatedException("Invalid login.");
				// Expired?
				if(token.Expires < DateTime.UtcNow) throw new NotAuthenticatedException("Login expired.");
			} catch(Exception e) {
				HandleError(context, e, 401);
				throw e;
			}
		}

		protected bool HandleError(HttpListenerContext context, Exception ex, int statusCode = 500)
		{
			var errorResponse = new
			{
				Title = "Unexpected Error",
				ErrorCode = ex.GetType().Name,
				Description = ex.ExceptionMessage(),
			};

			context.Response.StatusCode = statusCode;
			return context.JsonResponse(errorResponse);
		}
	}
}

