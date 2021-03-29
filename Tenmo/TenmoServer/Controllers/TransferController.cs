using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;
using System.Net.Http;
using System.Net;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {
        public ITransferDAO transferDAO;
        public IUserDAO userDAO;
        public IAccountDAO accountDAO;


        public TransferController(ITransferDAO _transferDAO, IUserDAO _userDAO, IAccountDAO _accountDAO)
        {
            transferDAO = _transferDAO;
            userDAO = _userDAO;
            accountDAO = _accountDAO;
        }

        [HttpGet("{accountId}")]

        public List<TransferDetails> GetTransferHistory(int accountID)
        {
            return transferDAO.GetTransfers(accountID);
        }

        [HttpGet("{accountId}/{transferId}")]

        public ActionResult<TransferDetails> GetTransfer(int accountId, int transferId)
        {
            //Should I do this instead of NotFound?
            //IActionResult result = BadRequest(new { message = "Transfer ID not found" });

            TransferDetails transfer = transferDAO.GetTransferDetails(accountId, transferId);

            if (transfer == null || transfer.ID == 0)
            {
                return NotFound(new { message = "No transfer was found with that Id" });
            }
            else
            {   
                return transfer;
            }
        }

        [HttpGet("/users")]

        public List<User> GetListOfUsers()
        {
            return userDAO.GetUsers();
        }

        [HttpPost]

        public ActionResult<TransferDetails> Transfer(Transfer transfer)
        {
            decimal toAccountBalance = accountDAO.GetAccount(transfer.AccountTo).Balance;
            decimal fromAccountBalance = accountDAO.GetAccount(transfer.AccountFrom).Balance;

            ActionResult result;


            if (fromAccountBalance < transfer.Amount)
            {

                result = BadRequest(new { message = "Aw, shucks. You've not enough bucks! Transfer failed. Next time better lucks!" });
            }

            else
            {
                decimal newToBalance = toAccountBalance + transfer.Amount;
                bool updateSuccessful = transferDAO.UpdateBalance(transfer.AccountTo, newToBalance);

                decimal newFromBalance = fromAccountBalance - transfer.Amount;
                updateSuccessful = transferDAO.UpdateBalance(transfer.AccountFrom, newFromBalance);

                if (updateSuccessful)
                {
                    TransferDetails added = transferDAO.CreateTransfer(transfer);
                    result = Created($"/transfer/{added.ID}", added);
                }
                else
                {
                    result = BadRequest(new { message = "Sorry, something didn't work. The transfer didn't work." });
                }
                
            }
            return result;
        }

    }
}
