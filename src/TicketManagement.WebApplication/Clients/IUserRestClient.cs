using RestEase;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Accounts;
using TicketManagement.ClientModels.Users;

namespace TicketManagement.WebApplication.Clients
{
	/// <summary>
	/// A client interface that provides methods for calling methods of the user manipulating
	/// and user-authentication service.
	/// </summary>
	public interface IUserRestClient
	{
		[Post("account/login")]
		Task<string> Login([Body] LoginModel model);

		[Post("account/register")]
		Task<string> Register([Body] RegisterModel model);

		[Get("user/User")]
		Task<UserTicketsModel> GetUser([Header("Token")] string token);

		[Get("user/UserBalance")]
		Task<UserModel> GetUserBalance([Header("Token")] string token);

		[Put("user/UserBalance")]
		Task TopUpUserBalance([Body] UserModel model, [Header("Token")] string token);

		[Get("user/UserSettings")]
		Task<UserModel> GetUserSettings([Header("Token")] string token);

		[Put("user/UserSettings")]
		Task UpdateUserSettings([Body] UserModel model, [Header("Token")] string token);

		[Get("admin/Users")]
		Task<UsersForAdminModel> GetAllUsers(int roleId, [Header("Page")] int page, [Header("Token")] string token);

		[Get("admin/User")]
		Task<UserModel> GetUserDetails(int userId, [Header("Token")] string token);

		[Put("admin/UserRole")]
		Task UpdateUserRole(int userId, [Header("RoleId")] int roleId, [Header("Token")] string token);
	}
}
