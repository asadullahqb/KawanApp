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
        [Post("/kawanapis/functions/sessionStart.php")]
        Task<SessionReply> StartSession([Body] Session s);

        [Post("/kawanapis/functions/sessionEnd.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<SessionReply> EndSession([Body] Session s);

        [Get("/kawanapis/functions/login.php")]
        Task<LoginReply> Login([Body] UserAuthentication ua);
        
        [Post("/kawanapis/functions/signUp.php")]
        Task<ReplyMessage> SignUp([Body] KawanUser ku);

        [Post("/kawanapis/functions/edit.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> Edit([Body] KawanUser ku);
        
        [Post("/kawanapis/functions/uploadPhoto.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> UploadPhoto([Body] PhotoUpload pu);

        [Get("/kawanapis/functions/fetchAllKawanUsers.php")]
        Task<List<KawanUser>> FetchAllKawanUsers([Body] User u);

        [Get("/kawanapis/functions/fetchAllInternationalStudentUsers.php")]
        Task<List<User>> FetchAllInternationalStudentUsers([Body] User u);

        [Get("/kawanapis/functions/fetchCurrentKawanUser.php")]
        Task<KawanUser> FetchCurrentKawanUser([Body] User u);

        [Get("/kawanapis/functions/fetchListOfCountries.php")]
        Task<List<Country>> FetchListOfCountries([Body] User u);

        [Get("/kawanapis/functions/fetchAllMessages.php")]
        Task<List<ChatMessageItem>> FetchAllMessages([Body] ChatMessageRequest cmr);

        [Get("/kawanapis/functions/fetchMessages.php")]
        Task<List<ChatMessage>> FetchMessages([Body] ChatMessage cm);
        
        [Post("/kawanapis/functions/storeMessage.php")]
        Task<ReplyMessage> StoreMessage([Body] ChatMessage cm);

        [Post("/kawanapis/functions/sendFriendRequest.php")]
        Task<ReplyMessage> SendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/unsendFriendRequest.php")]
        Task<ReplyMessage> UnsendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/acceptFriendRequest.php")]
        Task<ReplyMessage> AcceptFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/rejectFriendRequest.php")]
        Task<ReplyMessage> RejectFriendRequest([Body] FriendRequest fr);

        [Get("/kawanapis/functions/fetchKawanStats.php")]
        Task<KawanStats> FetchKawanStats([Body] User u);

        [Get("/kawanapis/functions/fetchUserOnlineTimeFrequencies.php")]
        Task<int[]> FetchUserOnlineTimeFrequencies([Body] FriendRequest fr);

        [Post("/kawanapis/functions/updatePassword.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> UpdatePassword([Body] KawanUser ku);
    }
}
