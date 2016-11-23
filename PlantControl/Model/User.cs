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
using System.Text;
using System.Security.Cryptography;

using Unosquare.Labs.LiteLib;

namespace PlantControl.Model
{
	[Table("Users")]
	public class User : Entity
	{
		[LiteUnique,Editable]
		public string Username { get; set; }

		public string PasswordHash { get; set; }

		[Editable,Readonly]
		public DateTime Created { get; set; }


		public static User CreateUser(string username, string password) {
			User user = new User();
			user.Username = username;
			user.SetPassword(password);
			user.Created = DateTime.UtcNow;
			return user;
		}

		public string GetPasswordHash(string password) {
			string hashstring = this.Username + password + "389uas!@ydf89ua&#q29i34y9yr3ai&*(^&uwy49tu";
			byte[] hashstringBytes = new UTF8Encoding().GetBytes(hashstring);
			byte[] hashBytes = ((HashAlgorithm) CryptoConfig.CreateFromName("MD5")).ComputeHash(hashstringBytes);
			return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
		}

		public void SetPassword(string password) {
			this.PasswordHash = GetPasswordHash(password);
		}
	}
}

