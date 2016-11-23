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
	[Table("DataPoints")]
	public class DataPoint : Entity
	{
		[Editable,Readonly]
		public DateTime Created  { get; set; }

		[Editable,Readonly]
		public int SensorId { get; set; }

		[Editable,Readonly]
		public int? PlantId { get; set; }

		[Editable,Readonly]
		public int SensorType { get; set; }

		[Editable,Readonly]
		public int RawValue { get; set; }

		[Editable,Readonly]
		public int AdjustedValue { get; set; }
	}
}

