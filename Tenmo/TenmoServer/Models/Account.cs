using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Account
    {
        public int Account_Id;
        public int User_Id;
        public decimal Balance;


        public Account(int account_Id, int user_Id, decimal balance)
        {

            Account_Id = account_Id;
            User_Id = user_Id;
            Balance = balance;

        }

        public Account(int account_Id)
        {

            Account_Id = account_Id;


        }
        public Account()
        {



        }
    }
}
