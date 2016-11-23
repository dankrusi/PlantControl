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
using System.Reflection;

using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Log;
using Unosquare.Labs.EmbedIO.Modules;

using CommandLine;

namespace PlantControl
{
	class MainClass
	{
		public static void Main(string[] args) {
			
			string serverURL = Config.GetStringValue("WebServerURL");
			string htmlDir = Config.GetPathValue("WebServerHTMLPath");

			// https://github.com/unosquare/litelib
			var dbContext = new Model.DataModelContext();
			dbContext.Init();

			bool enableController = true;
			if(enableController) {
				var controller = new Controller.PlantController();
				controller.Start();
			}

			// Our web server is disposable. Note that if you don't want to use logging,
			// there are alternate constructors that allow you to skip specifying an ILog object.
			// https://github.com/unosquare/embedio
			//using (var server = new WebServer(url))
			using (var server = new WebServer(serverURL, new ConsoleLog(), RoutingStrategy.Regex))
			{
				// First, we will configure our web server by adding Modules.
				// Please note that order DOES matter.
				// ================================================================================================
				// If we want to enable sessions, we simply register the LocalSessionModule
				// Beware that this is an in-memory session storage mechanism so, avoid storing very large objects.
				// You can use the server.GetSession() method to get the SessionInfo object and manupulate it.
				// You could potentially implement a distributed session module using something like Redis
				server.RegisterModule(new LocalSessionModule());

				// API
				server.RegisterModule(new WebApiModule());
				server.Module<WebApiModule>().RegisterController<API.PlantController>();
				server.Module<WebApiModule>().RegisterController<API.AccountController>();
				server.Module<WebApiModule>().RegisterController<API.EditController>();

				// Here we setup serving of static files
				server.RegisterModule(new StaticFilesModule(htmlDir));
				// The static files module will cache small files in ram until it detects they have been modified.
				server.Module<StaticFilesModule>().UseRamCache = Config.GetBoolValue("WebServerUseRamCache");
				server.Module<StaticFilesModule>().DefaultExtension = ".htm";
				// We don't need to add the line below. The default document is always index.html.
				server.Module<StaticFilesModule>().DefaultDocument = "index.htm";


				// Once we've registered our modules and configured them, we call the RunAsync() method.
				// This is a non-blocking method (it return immediately) so in this case we avoid
				// disposing of the object until a key is pressed.
				//server.Run();
				server.RunAsync();


				// Wait for any key to be pressed before disposing of our web server.
				// In a service we'd manage the lifecycle of of our web server using
				// something like a BackgroundWorker or a ManualResetEvent.
				Console.ReadKey(true);
			}
		}
	}
}
