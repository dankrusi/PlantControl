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
	[Table("Sensors")]
	public class Sensor : Entity
	{
		[Editable()]
		public string Title { get; set; }

		[Editable()]
		public int SensorType { get; set; }

		[Editable,Readonly]
		public DateTime Created { get; set; }

		[Editable("hardware pin")]
		public int Pin { get; set; }

	}

}

