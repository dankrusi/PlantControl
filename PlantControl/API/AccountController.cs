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
	public class AccountController : Controller
	{


		[WebApiHandler(HttpVerbs.Get, "/api/account/login")]
		public bool GetLogin(WebServer server, HttpListenerContext context)
		{
			try
			{
				// Init
				string username = context.QueryString("username");
				string password = context.QueryString("password");

				// User exists?
				var user = this.DB.Users.Select("Username = @Username", new { Username = username}).SingleOrDefault();
				if(user == null) throw new Exception("Invalid username.");

				// Password matches?
				if(user.GetPasswordHash(password) != user.PasswordHash) throw new Exception("Password does not match.");

				// All good

				// Create auth token
				AuthToken token = AuthToken.CreateAuthToken(user);
				this.DB.AuthTokens.Insert(token);

				return context.JsonResponse(token);
			}
			catch (Exception ex)
			{
				return HandleError(context, ex, (int)HttpStatusCode.InternalServerError);
			}
		}




	}
}

