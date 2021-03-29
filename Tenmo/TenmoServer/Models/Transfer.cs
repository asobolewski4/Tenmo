using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TenmoServer.Models
{
    public class Transfer
    {
        //Need access to current user account for Account id? mh
        //Account userAccount = new Account();
        //Does the Id (unique key) get created by Sql? 
        public int? Id { get; set; }

        //[Required(ErrorMessage = "The field `Transfer or Request` is required.")]
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int AccountFrom { get; set; }
        
        //[Required(ErrorMessage = "The account to transfer to is required.")]
        public int AccountTo { get; set; }
        //[Range(1, double.MaxValue, ErrorMessage = "The minimum amount for a transfer is $1.00.")]
        public decimal Amount { get; set; }

    }

    public class TransferDetails
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ToUser { get; set; }
        public decimal Amount { get; set; }
    }
}
