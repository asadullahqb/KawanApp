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
        [Get("/kawanapis/functions/login.php")]
        Task<LoginReply> Login([Body] UserAuthentication ua);

        [Get("/kawanapis/functions/fetchAllKawanUsers.php")]
        Task<List<KawanUser>> FetchAllKawanUsers([Body] User u);

        [Get("/kawanapis/functions/fetchAllInternationalStudentUsers.php")]
        Task<List<User>> FetchAllInternationalStudentUsers([Body] User u);

        [Get("/kawanapis/functions/fetchCurrentKawanUser.php")]
        Task<KawanUser> FetchCurrentKawanUser([Body] User u);

        [Get("/kawanapis/functions/fetchListOfCountries.php")]
        Task<List<Country>> FetchListOfCountries([Body] User u);
        
        [Get("/kawanapis/functions/fetchMessages.php")]
        Task<List<ChatMessage>> FetchMessages([Body] ChatMessage cm);
        
        [Post("/kawanapis/functions/storeMessage.php")]
        Task<ReplyMessage> StoreMessage([Body] ChatMessage cm);

        [Post("/kawanapis/functions/sendFriendRequest.php")]
        Task<ReplyMessage> SendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/unsendFriendRequest.php")]
        Task<ReplyMessage> UnsendFriendRequest([Body] FriendRequest fr);
    }
}
