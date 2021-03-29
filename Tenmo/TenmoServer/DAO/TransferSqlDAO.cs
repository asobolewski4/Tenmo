using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class TransferSqlDAO : ITransferDAO 
    {
        private readonly string connectionString;
        public TransferSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }



        public List<TransferDetails> GetTransfers(int fromAccount)
        {
            List<TransferDetails> transfers = new List<TransferDetails>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_desc, transfer_status_desc, username, amount FROM transfers " +
                        "join transfer_types on transfers.transfer_type_id = transfer_types.transfer_type_id " +
                        "join transfer_statuses on transfers.transfer_status_id = transfer_statuses.transfer_status_id " +
                        "join accounts on transfers.account_to = accounts.account_id " +
                        "join users on accounts.user_id = users.user_id " +
                        "WHERE account_from = @fromId OR account_to = @fromId;", conn);

                    cmd.Parameters.AddWithValue("@fromId", $"{fromAccount}");
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {
                            TransferDetails transfer = GetTransferDeetsFromReader(reader);
                            transfers.Add(transfer);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            return transfers;
        }



        public TransferDetails GetTransferDetails(int accountId, int transferId)
        {
            TransferDetails returnTransfer = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("SELECT transfer_id, transfer_type_desc, transfer_status_desc, username, amount FROM transfers " +
                        "join transfer_types on transfers.transfer_type_id = transfer_types.transfer_type_id " +
                        "join transfer_statuses on transfers.transfer_status_id = transfer_statuses.transfer_status_id " +
                        "join accounts on transfers.account_to = accounts.account_id " +
                        "join users on accounts.user_id = users.user_id " +
                        "WHERE transfer_id = @transferId and account_from = @accountId;", conn);
        
                    cmd.Parameters.AddWithValue("@transferId", transferId);
                    cmd.Parameters.AddWithValue("@accountId", accountId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows && reader.Read())
                    {
                        returnTransfer = GetTransferDeetsFromReader(reader);
                    } 
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return returnTransfer;
        }



        private TransferDetails GetTransferDeetsFromReader(SqlDataReader reader)
        {
            TransferDetails tD = new TransferDetails()
            {
                ID = Convert.ToInt32(reader["transfer_id"]),
                Type = Convert.ToString(reader["transfer_type_desc"]),
                Status = Convert.ToString(reader["transfer_status_desc"]),
                ToUser = Convert.ToString(reader["username"]),
                Amount = Convert.ToDecimal(reader["amount"])
            };

            return tD;
        }

        public int GetAccountNumber(int userId)
        {
            {
                int accountId = -1;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand("SELECT account_id from accounts WHERE user_id = @userId;", conn);

                        cmd.Parameters.AddWithValue("@userId", userId);
                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows && reader.Read())
                        {
                            accountId = Convert.ToInt32(reader["account_to"]);
                        }
                    }
                }
                catch (SqlException)
                {
                    throw;
                }

                return accountId;
            }
        }
        //private bool CheckForTransfer(int accountID)
        //{

        //}

        public TransferDetails CreateTransfer(Transfer transfer)
        {
            int newTransferId = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) " +
                        "VALUES(@type, 2, @account_from, @account_to, @amount)", conn);
                    cmd.Parameters.AddWithValue("@type", transfer.TypeId);
                    cmd.Parameters.AddWithValue("@account_from", transfer.AccountFrom);
                    cmd.Parameters.AddWithValue("@account_to", transfer.AccountTo);
                    cmd.Parameters.AddWithValue("@amount", transfer.Amount);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT @@IDENTITY", conn);
                    newTransferId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (SqlException)
            {
                throw;
            }

            return GetTransferDetails(transfer.AccountFrom, newTransferId);
        }



        public bool UpdateBalance(int userId, decimal newBalance)
        {
            bool accountUpdated = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE accounts set balance = @newBalance where account_id = (SELECT account_Id from accounts WHERE user_Id = @Id)", conn);
                    cmd.Parameters.AddWithValue("@Id", userId);
                    cmd.Parameters.AddWithValue("@newBalance", newBalance);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
                accountUpdated = false;
            }
            return accountUpdated;
        }

 

        private Transfer GetTransferFromReader(SqlDataReader reader)
        {
            Transfer t = new Transfer()
            {
                Id = Convert.ToInt32(reader["transfer_id"]),
                TypeId = Convert.ToInt32(reader["transfer_type_id"]),
                StatusId = Convert.ToInt32(reader["transfer_status_id"]),
                AccountFrom = Convert.ToInt32(reader["account_from"]),
                AccountTo = Convert.ToInt32(reader["account_to"]),
                Amount = Convert.ToDecimal(reader["amount"])
            };

            return t;
        }


    }
}
