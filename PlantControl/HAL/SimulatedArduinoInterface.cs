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
using System.Collections.Generic;
using System.Threading;

namespace PlantControl.HAL
{
	public class SimulatedArduinoInterface : IMicrocontrollerInterface
	{
		private class SimulatedPin {
			public int Value;
			public PinMode Mode = PinMode.INPUT;
		}

		private ConsoleLog _log;
		private Dictionary<int, SimulatedPin> _pins;
		private Random _rnd;

		public SimulatedArduinoInterface() {
			_log = new ConsoleLog();
			_pins = new Dictionary<int, SimulatedPin>();
			_rnd = new Random((int)DateTime.Now.Ticks);
		}

		private SimulatedPin _getPin(int pin) {
			if (!_pins.ContainsKey(pin)) {
				_pins[pin] = new SimulatedPin();
			}
			var simPin = _pins[pin];
			return simPin;
		}

		public void Start() {

		}

		public void Stop() {
			
		}

		public int AnalogRead(int pin) {
			var simPin = _getPin(pin);
			if(simPin.Mode == PinMode.OUTPUT || simPin.Mode == PinMode.PULLUP) {
				return _getPin(pin).Value;
			} else {
				return _rnd.Next(0, 1024);
			}
		}

		public void SetPinMode(int pin, PinMode mode) {
			_getPin(pin).Mode = mode;
		}

		public void AnalogWrite(int pin, int value) {
			_getPin(pin).Value = value;
		}

		public void DigitalWrite(int pin, PinValue value) {
			_getPin(pin).Value = (int)value;
		}

		public int DigitalRead(int pin) {
			var simPin = _getPin(pin);
			if (simPin.Mode == PinMode.OUTPUT || simPin.Mode == PinMode.PULLUP) {
				return (_getPin(pin).Value == 0 ? 0 : 1);
			} else {
				return _rnd.Next(0, 1);
			}
		}


	}
}

