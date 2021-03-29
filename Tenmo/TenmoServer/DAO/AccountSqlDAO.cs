using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    
    public class AccountSqlDAO : IAccountDAO
    {

        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }


        public Account GetAccount(int id)
        {
            Account returnAccount = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts WHERE account_id = @account_id", conn);
                    cmd.Parameters.AddWithValue("@account_id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        returnAccount = GetAccountsFromReader(reader);
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnAccount;
        }



        public List<Account> GetAllAccounts()
        {


            List<Account> returnAccounts = new List<Account>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT account_id, user_id, balance FROM accounts", conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Account a = GetAccountsFromReader(reader);
                            returnAccounts.Add(a);
                        }

                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnAccounts;



        }



















        private Account GetAccountsFromReader(SqlDataReader reader)
        {
            Account a = new Account()
            {
                Account_Id = Convert.ToInt32(reader["account_id"]),
                User_Id = Convert.ToInt32(reader["user_id"]),
                Balance = Convert.ToDecimal(reader["balance"]),
            };

            return a;
        }
    }
}
