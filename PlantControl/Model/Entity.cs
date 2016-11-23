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

namespace PlantControl
{
	public class EditableAttribute : Attribute
	{
		public string Placeholder;

		public EditableAttribute() {}

		public EditableAttribute(string placeholder) {
			this.Placeholder = placeholder;
		}
	}

	public class ReadonlyAttribute : Attribute
	{
		public ReadonlyAttribute() { }
	}

	public abstract class Entity : LiteModel
	{
		public Entity()
		{
		}
	}
}

