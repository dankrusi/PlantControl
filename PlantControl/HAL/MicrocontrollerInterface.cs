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

namespace PlantControl.HAL
{
	public enum PinMode
	{
		INPUT,
		OUTPUT,
		PULLUP
	}

	public enum PinValue
	{
		HIGH = 1,
		LOW = 0
	}

	public interface IMicrocontrollerInterface
	{
		
		void Start();

		void Stop();

		int AnalogRead(int pin);

		void SetPinMode(int pin, PinMode mode);

		void AnalogWrite(int pin, int value);

		void DigitalWrite(int pin, PinValue value);

		int DigitalRead(int pin);
	}
}
