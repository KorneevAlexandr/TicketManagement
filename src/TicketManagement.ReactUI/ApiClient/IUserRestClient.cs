using RestEase;
using System.Threading.Tasks;
using TicketManagement.ClientModels.Users;

namespace TicketManagement.ReactUI.ApiClient
{
	public interface IUserRestClient
	{
		[Get("user/User")]
		Task<UserTicketsModel> GetUser([Header("Token")] string token);

		[Put("user/UserBalance")]
		Task TopUpUserBalance([Body] UserModel model, [Header("Token")] string token);

		[Get("user/UserSettings")]
		Task<UserModel> GetUserSettings([Header("Token")] string token);

		[Put("user/UserSettings")]
		Task UpdateUserSettings([Body] UserModel model, [Header("Token")] string token);
	}
}
