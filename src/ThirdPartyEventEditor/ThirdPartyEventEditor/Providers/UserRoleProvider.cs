using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Web.Configuration;
using System.Web.Security;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Providers
{
	/// <summary>
	/// Represents methods for multi-users access to application.
	/// </summary>
	public class UserRoleProvider : RoleProvider
	{
		/// <summary>
		/// Getting all roles, owned by specified user.
		/// </summary>
		/// <param name="username">Username or login.</param>
		/// <returns>Roles string array.</returns>
		public override string[] GetRolesForUser(string username)
		{
			string[] roles = null;

			var fileName = WebConfigurationManager.AppSettings["UsersSourcePath"];
			string path = HttpContext.Current.Server.MapPath(fileName);
			var jsonString = File.ReadAllText(path);
			var users = JsonSerializer.Deserialize<IEnumerable<User>>(jsonString);

			var user = users.FirstOrDefault(x => x.Login.Equals(username));
			if (user != null)
			{
				roles = new string[] { user.Role.ToString() };
			}
			return roles;
		}

		/// <summary>
		/// Checked affiliation role for specified user.
		/// </summary>
		/// <param name="username">Username or login.</param>
		/// <param name="roleName">Role name.</param>
		/// <returns>True - owned by, false - no.</returns>
		public override bool IsUserInRole(string username, string roleName)
		{
			var fileName = WebConfigurationManager.AppSettings["UsersSourcePath"];
			string path = HttpContext.Current.Server.MapPath(fileName);
			var jsonString = File.ReadAllText(path);
			var users = JsonSerializer.Deserialize<IEnumerable<User>>(jsonString);

			var user = users.FirstOrDefault(x => x.Login.Equals(username));
			if (user != null && user.Role.Equals(roleName))
			{
				return true;
			}
			return false;
		}

		public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}
	}
}