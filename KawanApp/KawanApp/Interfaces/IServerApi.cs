using Refit;
using KawanApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace KawanApp.Interfaces
{
    public interface IServerApi
    {

        [Get("/kawanapis/functions/fetchAllKawanUsers.php")]
        Task<List<KawanUser>> FetchAllKawanUsers();
        
        [Post("/kawanapis/functions/fetchMessages.php")]
        Task<List<ChatMessage>> FetchMessages([Body] SendingAndReceivingUsers sendingandreceivingusers);
        
        [Post("/kawanapis/functions/storeMessage.php")]
        Task<ReplyMessage> StoreMessage([Body] ChatMessage message);

    }
}
