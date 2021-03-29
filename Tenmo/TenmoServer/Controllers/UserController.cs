using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{

    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {

        public IAccountDAO AccountDAO;
        public IUserDAO UserDAO;

        public UserController(IAccountDAO accountDAO, IUserDAO userDAO)
        {

            AccountDAO = accountDAO;
            UserDAO = userDAO;
        }


        [HttpGet]
        public List<User> GetUsers()
        {

            return UserDAO.GetUsers();
        }


    }
}
