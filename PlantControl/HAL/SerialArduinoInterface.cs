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

namespace PlantControl.HAL
{
	public class SerialArduinoInterface : IMicrocontrollerInterface
	{
		
		private SerialPort _serialPort;
		private ConsoleLog _log;
		private string _port;
		private int _baudRate;

		public SerialArduinoInterface(string port, int baudRate = 9600) {
			_log = new ConsoleLog();
			_port = port;
			_baudRate = baudRate;
		}

		public void Start() {

			_serialPort = new SerialPort();

			// Get a default port if it was not specified
			if(_port == null) {
				foreach(string port in SerialPort.GetPortNames()) {
					_port = port;
					break;
				}
			}
			if(_port == null) {
				_log.WarnFormat("No Serial Port Found!");
			} else {
				_log.DebugFormat("Selecting Serial Port " + _port);
			}

			// Setup the serial port
			_serialPort.PortName = _port;
			_serialPort.BaudRate = _baudRate;
			//_serialPort.Parity = SetPortParity(_serialPort.Parity);
			//_serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
			//_serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
			//_serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

			// Set the read/write timeouts
			_serialPort.ReadTimeout = 1500;
			_serialPort.WriteTimeout = 1500;

			// Open it up
			_serialPort.Open();
		}

		public void Stop() {
			_serialPort.Close();
		}

		private string _sendCommand(string command) {
			_log.DebugFormat("Serial Write: " + command);
			_serialPort.WriteLine(command);
			string ret = null;
			try {
				ret = _serialRead();
			} catch(TimeoutException ex) {
				_log.DebugFormat("Serial Timeout");}
			return ret;
		}

		private string _serialRead() {
			bool didRead = false;
			string ret = null;
			while(didRead == false) {
				ret = _serialPort.ReadLine();
				if(ret != null && ret.StartsWith("debug")) {
					_log.DebugFormat("Serial Debug: " + ret);
				} else {
					didRead = true;
				}
			}
			_log.DebugFormat("Serial Read: " + ret);
			return ret.Trim();
		}

		private void _processWriteCommand(string command) {
			var ret = _sendCommand(command);
			if(ret != "success") throw new Exception("Could not process write command "+command);
		}

		public int AnalogRead(int pin) {
			var ret = _sendCommand("analogread " + pin);
			if(ret == null) return -1;
			return int.Parse(ret);
		}

		public void SetPinMode(int pin, PinMode mode) {
			string modeString = null;
			if(mode == PinMode.INPUT) modeString = "in";
			else if(mode == PinMode.OUTPUT) modeString = "out";
			else if(mode == PinMode.PULLUP) modeString = "pullup";
			_processWriteCommand("pinmode " + pin + " " + modeString);
		}

		public void AnalogWrite(int pin, int value) {
			_processWriteCommand("analogwrite " + pin + " " + value);
		}

		public void DigitalWrite(int pin, PinValue value) {
			_processWriteCommand("digitalwrite " + pin + " " + (value == PinValue.HIGH ? "high" : "low"));
		}

		public int DigitalRead(int pin) {
			var ret = _sendCommand("digitalread " + pin);
			if(ret == null) return -1;
			return int.Parse(ret);
		}


	}
}

