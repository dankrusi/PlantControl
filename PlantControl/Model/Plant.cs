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
using System.ComponentModel.DataAnnotations.Schema;

using Unosquare.Labs.LiteLib;

namespace PlantControl.Model
{
	[Table("Plants")]
	public class Plant : Entity
	{
		[Editable("Plant title")]
		public string Title { get; set; }

		[Editable("Plant notes")]
		public string Notes { get; set; }

		[Editable,Readonly]
		public DateTime Created { get; set; }

		[Editable]
		public DateTime Planted { get; set; }

		[Editable("Species name")]
		public string Species { get; set; }

		[Editable()]
		public int PositionX { get; set; }
		[Editable()]
		public int PositionY { get; set; }

		[Editable("Hardware pin")]
		public int SensorPin { get; set; }

		[Editable("Calibration offset at min")]
		public int SensorCalibrationMin { get; set; }

		[Editable("Calibration offset at max")]
		public int SensorCalibrationMax { get; set; }

		[Editable("Hardware pin")]
		public int PumpPin { get; set; }

		[Editable("Number of milliliters pumped")]
		public int PumpCalibrationMilliliters { get; set; }

		[Editable("Number of milliseconds pumped")]
		public int PumpCalibrationMilliseconds { get; set; }
		
	}

}

