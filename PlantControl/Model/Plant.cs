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
		[Editable("plant title")]
		public string Title { get; set; }

		[Editable,Readonly]
		public DateTime Created { get; set; }

		[Editable]
		public DateTime Planted { get; set; }

		[Editable("species name")]
		public string Species { get; set; }

		[Editable("hardware pin")]
		public int SensorPin { get; set; }

		[Editable("hardware pin")]
		public int PumpPin { get; set; }
		
	}

}

