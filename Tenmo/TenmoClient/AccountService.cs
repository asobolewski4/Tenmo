using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient
{
    public class AccountService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        public string UNAUTHORIZED_MSG { get { return "Authorization is required for this endpoint. Please log in."; } }
        public string FORBIDDEN_MSG { get { return "You do not have permission to perform the requested action"; } }
        public string OTHER_4XX_MSG { get { return "Error occurred - received non-success response: "; } }



        private API_User user = new API_User();

        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }



        public decimal GetBalanceOfAccount(int id)
        {

            if (client.Authenticator == null)
            {
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }


            RestRequest request = new RestRequest(API_BASE_URL + "api/account/" + id + "/balance");
            IRestResponse<decimal> response = client.Get<decimal>(request);




            //if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            //{
            //    ProcessErrorResponse(response);
            //}
            //else
            //{
            //    return response.Data;
            //}



            //return 0M;

            return response.Data;


        }

        public Account GetAccount(int id)
        {
            if (client.Authenticator == null)
            {
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }
            RestRequest request = new RestRequest(API_BASE_URL + "api/account");
            IRestResponse<List<Account>> response = client.Get<List<Account>>(request);

            foreach(Account account in response.Data)
            {
                if(account.Account_Id == id)
                {

                    return account;
                }

            }

            return null;

        }


        public List<Account> GetAccounts()
        {
            if (client.Authenticator == null)
            {
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }

            RestRequest request = new RestRequest(API_BASE_URL + "api/account");
            IRestResponse<List<Account>> response = client.Get<List<Account>>(request);
            return response.Data;

        }


        //public string ProcessErrorResponse(IRestResponse response)
        //{
        //    if (response.ResponseStatus != ResponseStatus.Completed)
        //    {
        //        return "Error occurred - unable to reach server.";
        //    }
        //    else if (!response.IsSuccessful)
        //    {

        //        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        {
        //            return UNAUTHORIZED_MSG;
        //        }
        //        else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
        //        {
        //            return FORBIDDEN_MSG;
        //        }


        //        return OTHER_4XX_MSG + (int)response.StatusCode;
        //    }
        //    return "";
        //}

    }
}
