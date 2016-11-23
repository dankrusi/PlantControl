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
using System.IO.Ports;
using System.Threading;

namespace PlantControl.Controller
{
	public class PlantController
	{
		private Model.DataModelContext _db;
		private Thread _thread;
		private ConsoleLog _log;
		private HAL.IMicrocontrollerInterface _interface;
		bool _continue;

		public PlantController()
		{
			// Init log
			_log = new ConsoleLog();
			// Init interface
			if(Config.GetStringValue("MicrocontrollerInterfaceType") == "SerialArduinoInterface") {
				_log.InfoFormat("Using SerialArduinoInterface");
				_interface = new PlantControl.HAL.SerialArduinoInterface(Config.GetStringValue("SerialArduinoInterfacePort"),Config.GetIntValue("SerialArduinoInterfaceBaudRate"));
			} else if (Config.GetStringValue("MicrocontrollerInterfaceType") == "SimulatedArduinoInterface") {
				_log.InfoFormat("Using SimulatedArduinoInterface");
				_interface = new PlantControl.HAL.SimulatedArduinoInterface();
			} else {
				throw new Exception("Unknown MicrocontrollerInterfaceType");
			}
		}

		public void Start() {
			_log.DebugFormat("Starting Controller...");
			// Open interface
			_interface.Start();
			// Start controller thread
			_thread = new Thread(_process);
			_continue = true;
			_thread.Start();
			_log.DebugFormat("Controller Started!");
		}

		public void Stop() {
			_log.DebugFormat("Stopping Controller...");
			_continue = false;
			_thread.Join();
			_log.DebugFormat("Controller Stopped!");
		}

		public void _process(){
			bool toggle = true;
			while (_continue) {
				try {
					//string message = _serialPort.ReadLine();
					//Console.WriteLine(message);

					if(false) {
						var ret = _interface.AnalogRead(0);
						_log.DebugFormat("Controller: analogread = "+ret);
						_interface.AnalogWrite(0,100);
					}

					if(false) {
						_interface.SetPinMode(13,PlantControl.HAL.PinMode.OUTPUT);
						toggle = !toggle;
						if(toggle) {
							_interface.DigitalWrite(13,PlantControl.HAL.PinValue.HIGH);
						} else {
							_interface.DigitalWrite(13,PlantControl.HAL.PinValue.LOW);
						}
						_log.DebugFormat("Controller: digital read = "+_interface.DigitalRead(13));
					}

					if(true) {
						_interface.SetPinMode(0,PlantControl.HAL.PinMode.INPUT);
						var ret = _interface.AnalogRead(0);
						_log.DebugFormat("Controller: analogread = "+ret);
						_interface.AnalogWrite(0,42);
					}

					Thread.Sleep(1000);
				} catch (Exception e) { 
					_log.WarnFormat("!!!!!!!!!!!" + e.Message);
					Thread.Sleep(3000);
				}
			}
		}
	}
}

