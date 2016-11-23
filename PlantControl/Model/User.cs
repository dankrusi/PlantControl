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
			string salt1 = Config.GetRequiredDBStringValue("UserSalt1");
			string salt2 = Config.GetRequiredDBStringValue("UserSalt2");
			string salt3 = Config.GetRequiredDBStringValue("UserSalt3");
			string hashstring1 = Util.Hash.GetHashedString(password + salt1);
			string hashstring2 = Util.Hash.GetHashedString(hashstring1 + salt2);
			string hashstring3 = Util.Hash.GetHashedString(hashstring2 + salt3);
			return hashstring3;
		}

		public void SetPassword(string password) {
			this.PasswordHash = GetPasswordHash(password);
		}
	}
}

