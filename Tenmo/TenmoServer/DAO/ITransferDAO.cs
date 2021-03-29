using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface ITransferDAO
    {
        TransferDetails CreateTransfer(Transfer transfer);
        List<TransferDetails> GetTransfers(int fromAccount);
        bool UpdateBalance(int accountID, decimal newBalance);

        TransferDetails GetTransferDetails(int accountId, int transferId);

    }
}

