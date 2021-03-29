using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class Transfer
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public int AccountFrom { get; set; }
        public int AccountTo { get; set; }
        public decimal Amount { get; set; }

        public Transfer()
        {
            //must have parameterless constructor to use as a type parameter (i.e., client.Get<Reservation>())
        }

        public bool IsValid
        {
            get
            {
                return AccountTo != 0 && AccountFrom != 0 && TypeId != 0 && StatusId != 0 && Amount != 0;
            }
        }

    }

    public class TransferDetails
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ToUser { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }

    }


}
