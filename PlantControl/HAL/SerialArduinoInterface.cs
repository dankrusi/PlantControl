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

using ArduinoDriver;

namespace PlantControl.HAL
{
	public class SerialArduinoInterface : IMicrocontrollerInterface
	{
		
		private ConsoleLog _log;
		private string _device;
		private string _port;
		private int _baudRate;
		private ArduinoDriver.ArduinoDriver _driver;

		public SerialArduinoInterface(string device, string port, int baudRate = 9600) {
			_log = new ConsoleLog();
			_device = device;
			_port = port;
			_baudRate = baudRate;
		}

		public void Start() {

			// Get model
			ArduinoDriver.ArduinoModel model;
			if (_device == "Mega2560") model = ArduinoDriver.ArduinoModel.Mega2560;
			else if (_device == "Micro") model = ArduinoDriver.ArduinoModel.Micro;
			else if (_device == "NanoR3") model = ArduinoDriver.ArduinoModel.NanoR3;
			else if (_device == "UnoR3") model = ArduinoDriver.ArduinoModel.UnoR3;
			else throw new Exception("Arduino model unknown");

			// Init driver
			_driver = new ArduinoDriver.ArduinoDriver(model, _port);
			//_driver = new ArduinoDriver.ArduinoDriver(model, false);

		}

		public void Stop() {
			_driver.Dispose();
		}

		public int AnalogRead(int pin) {
			var ret = _driver.Send(new ArduinoDriver.SerialProtocol.AnalogReadRequest((byte)pin));
			return ret.PinValue;
		}

		public void AnalogWrite(int pin, int value) {
			_driver.Send(new ArduinoDriver.SerialProtocol.AnalogWriteRequest((byte)pin, (byte)value));
		}

		public void DigitalWrite(int pin, PinValue value) {
			DigitalValue digitalValue = value == 0 ? DigitalValue.Low : DigitalValue.High;
			_log.DebugFormat("DigitalWrite pin "+pin+"="+value.ToString());
			_driver.Send(new ArduinoDriver.SerialProtocol.DigitalWriteRequest((byte)pin, digitalValue));
		}

		public int DigitalRead(int pin) {
			var ret = _driver.Send(new ArduinoDriver.SerialProtocol.DigitalReadRequest((byte)pin));
			if (ret.PinValue == DigitalValue.High) return 1;
			else return 0;
		}
			             
		public void SetPinMode(int pin, PinMode mode) {
			ArduinoDriver.SerialProtocol.PinMode mode2 = ArduinoDriver.SerialProtocol.PinMode.Input;
			if (mode == PinMode.INPUT) mode2 = ArduinoDriver.SerialProtocol.PinMode.Input;
			else if (mode == PinMode.OUTPUT) mode2 = ArduinoDriver.SerialProtocol.PinMode.Output;
			else if (mode == PinMode.PULLUP) mode2 = ArduinoDriver.SerialProtocol.PinMode.InputPullup;
			_driver.Send(new ArduinoDriver.SerialProtocol.PinModeRequest((byte)pin, mode2));
		}

	}
}

