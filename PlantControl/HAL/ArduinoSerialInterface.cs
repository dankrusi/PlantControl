using System;
using System.IO.Ports;
using System.Threading;

namespace PlantControl.HAL
{
	public class SerialInterface
	{
		
		private SerialPort _serialPort;
		private ConsoleLog _log;

		public SerialInterface() {
			_log = new ConsoleLog();
		}

		public void Open() {

			_serialPort = new SerialPort();

			// Get the port
			string defaultPort = null;
			foreach (string port in SerialPort.GetPortNames()) {
				defaultPort = port;
				break;
			}
			if(defaultPort == null) {
				_log.WarnFormat("No Serial Port Found!");
			} else {
				_log.DebugFormat("Selecting Serial Port " + defaultPort);
			}

			// Allow the user to set the appropriate properties.
			_serialPort.PortName = defaultPort;
			_serialPort.BaudRate = 9600;
			//_serialPort.Parity = SetPortParity(_serialPort.Parity);
			//_serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
			//_serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
			//_serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

			// Set the read/write timeouts
			//_serialPort.ReadTimeout = 500;
			//_serialPort.WriteTimeout = 500;

			_serialPort.Open();
		}

		public void Close() {
			_serialPort.Close();
		}


	}
}

