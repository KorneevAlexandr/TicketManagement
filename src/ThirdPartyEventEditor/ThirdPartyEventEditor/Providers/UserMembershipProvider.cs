using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.Json;
using System.Web.Security;
using ThirdPartyEventEditor.Models;
using System.Web.Configuration;
using System.IO;

namespace ThirdPartyEventEditor.Providers
{
	/// <summary>
	/// Represents methods for authorize and authentication users in system.
	/// For data source use .json file.
	/// </summary>
	public class UserMembershipProvider : MembershipProvider
	{
		/// <summary>
		/// Check registered user or not registered.
		/// </summary>
		/// <param name="username">Login.</param>
		/// <param name="password">Password.</param>
		/// <returns>True - registered, false - not registered.</returns>
		public override bool ValidateUser(string username, string password)
		{
			var fileName = WebConfigurationManager.AppSettings["UsersSourcePath"];
			string path = HttpContext.Current.Server.MapPath(fileName);
			var jsonString = File.ReadAllText(path);
			var users = JsonSerializer.Deserialize<IEnumerable<User>>(jsonString);

			var user = users.FirstOrDefault(x => x.Login.Equals(username) && x.Password.Equals(password));
			if (user == null)
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Getting user by his username.
		/// </summary>
		/// <param name="username">Login or username.</param>
		/// <param name="userIsOnline">Online or offline.</param>
		/// <returns>User.</returns>
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			try
			{
				var fileName = WebConfigurationManager.AppSettings["UsersSourcePath"];
				string path = HttpContext.Current.Server.MapPath(fileName);
				var jsonString = File.ReadAllText(path);
				var users = JsonSerializer.Deserialize<IEnumerable<User>>(jsonString);
				var user = users.FirstOrDefault(x => x.Login.Equals(username));

				var membershipUser = new MembershipUser("UserMembershipProvider", user.Login, null, null, null, null, 
					false, false, new DateTime(), new DateTime(), new DateTime(), new DateTime(), new DateTime());
				return membershipUser;
			}
			catch
			{
				return null;
			}
		}

		public override bool EnablePasswordRetrieval => throw new NotImplementedException();

		public override bool EnablePasswordReset => throw new NotImplementedException();

		public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

		public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

		public override int PasswordAttemptWindow => throw new NotImplementedException();

		public override bool RequiresUniqueEmail => throw new NotImplementedException();

		public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

		public override int MinRequiredPasswordLength => throw new NotImplementedException();

		public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

		public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			throw new NotImplementedException();
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		public override string GetUserNameByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public override string ResetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}

		public override void UpdateUser(MembershipUser user)
		{
			throw new NotImplementedException();
		}
	}
}