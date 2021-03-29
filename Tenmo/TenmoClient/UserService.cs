using RestSharp;
using System.Collections.Generic;
using TenmoClient.Data;

namespace TenmoClient
{
    public class UserService
    {

        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private static API_User user = new API_User();
        private readonly IRestClient client = new RestClient();


        public List<API_User> GetUsers()
        {

            RestRequest request = new RestRequest(API_BASE_URL + "user");
            IRestResponse<List<API_User>> response = client.Get<List<API_User>>(request);
            return response.Data;

        }

        



        public static void SetLogin(API_User u)
        { 
            user = u;
        }

        public static int GetUserId()
        {
            return user.UserId;
        }

        public static string GetUserName()
        {
            return user.Username;
        }

        //public static string GetUserName()
        //{

        //}

        public static bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(user.Token);
        }

        public static string GetToken()
        {
            return user?.Token ?? string.Empty;
        }

    }
}
