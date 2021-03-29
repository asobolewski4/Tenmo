using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        public IAccountDAO AccountDAO;
        public IUserDAO UserDAO;

        public AccountController(IAccountDAO accountDAO, IUserDAO userDAO)
        {

            AccountDAO = accountDAO;
            UserDAO = userDAO;
        }


        [HttpGet("{id}/balance")]
        public ActionResult<Decimal> GetAccountBalance(int id)
        {

            Account account = AccountDAO.GetAccount(id);
            return account.Balance;


        }

        [HttpGet]
        public List<Account> GetAllAccounts()
        {

            return AccountDAO.GetAllAccounts();
        }

        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {

            Account account = AccountDAO.GetAccount(id);
            return account;


        }







    }
}
