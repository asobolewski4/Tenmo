using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using TenmoClient.Data;

namespace TenmoClient
{
    public class TransferService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();


        public List<TransferDetails> GetTransfers(int accountId)
        {
            //client.Authenticator = API_User.Token;
            if (client.Authenticator == null)
            {
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }

            RestRequest request = new RestRequest(API_BASE_URL + "transfer/" + accountId);
            IRestResponse<List<TransferDetails>> response = client.Get<List<TransferDetails>>(request);
            
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                Console.WriteLine("An error message was received: " + response.Data);
                return null;
            }
            else if (response.Data.Count < 1)
            {
                Console.WriteLine("This account is not in my collection. Or it might not have done any transfers yet." + response.Data);
                return null;
            }
            else
            {
                return response.Data;
            }
        }



            public void GetUsers()
        {
            if (client.Authenticator == null)
            {
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }

            RestRequest request = new RestRequest(API_BASE_URL + "users");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            foreach (User u in response.Data)
            {
                Console.WriteLine("UserID: " + u.UserId + "     Username: " + u.Username);
            }
        }
        


        public void GetTransfer(int accountID, int transferID)
        {
            if (client.Authenticator == null)
            {
               client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }

            RestRequest request = new RestRequest(API_BASE_URL + "transfer/" + accountID + "/" + transferID);
            IRestResponse<TransferDetails> response = client.Get<TransferDetails>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Console.WriteLine("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
            }

            else
            {
                TransferDetails t = response.Data;

                Console.WriteLine($"\n------------------------------------------");
                Console.WriteLine("Transfer Details \n------------------------------------------");
                Console.WriteLine($"Id: {t.ID}");
                Console.WriteLine($"From:  {UserService.GetUserName()}");
                Console.WriteLine($"To:  {t.ToUser}");
                Console.WriteLine($"Type:  {t.Type}");
                Console.WriteLine($"Status:  {t.Status}");
                Console.WriteLine($"Amount:  {t.Amount}");
            }
        }




        public TransferDetails TransferRequest(Transfer transfer)
        {
            if (client.Authenticator == null)
            {
                client.Authenticator = new JwtAuthenticator(UserService.GetToken());
            }

            RestRequest request = new RestRequest(API_BASE_URL + "transfer");
            request.AddJsonBody(transfer);
            IRestResponse<TransferDetails> response = client.Post<TransferDetails>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Console.WriteLine("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Console.WriteLine("\nAn error message was received: " + response.Data.Message);
                }
                else
                {
                    Console.WriteLine("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return null;
            }
            else
            {
                return response.Data;
            }
        }

    }
}

