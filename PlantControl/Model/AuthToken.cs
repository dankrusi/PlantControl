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
	[Table("AuthTokens")]
	public class AuthToken : Entity
	{
		[LiteUnique]
		public string Hash { get; set; }

		public long UserId { get; set; }

		public DateTime Created { get; set; }

		public DateTime Expires { get; set; }


		public static AuthToken CreateAuthToken(User user) {
			AuthToken token = new AuthToken();
			token.UserId = user.RowId;
			token.Created = DateTime.UtcNow;
			token.Expires = DateTime.UtcNow.AddDays(30);
			token.Hash = CreateHash();
			return token;
		}

		public static string CreateHash() {
			return (Guid.NewGuid().ToString() + Guid.NewGuid().ToString()).Replace("-","");
		}
	}
}

