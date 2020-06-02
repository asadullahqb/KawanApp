using Refit;
using KawanApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KawanApp.Interfaces
{
    public interface IServerApi
    {
        #region Sessions
        [Post("/kawanapis/functions/sessionStart.php")]
        Task<SessionReply> StartSession([Body] Session s);

        [Post("/kawanapis/functions/sessionEnd.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<SessionReply> EndSession([Body] Session s);
        #endregion

        #region Login, SignUp, and Edit
        [Get("/kawanapis/functions/login.php")]
        Task<LoginReply> Login([Body] UserAuthentication ua);
        
        [Post("/kawanapis/functions/signUp.php")]
        Task<ReplyMessage> SignUp([Body] KawanUser ku);

        [Post("/kawanapis/functions/edit.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> Edit([Body] KawanUser ku);

        [Post("/kawanapis/functions/uploadPhoto.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> UploadPhoto([Body] PhotoUpload pu);
        #endregion

        #region Fetch Profile and Country Information
        [Get("/kawanapis/functions/fetchAllKawanUsers.php")]
        Task<List<KawanUser>> FetchAllKawanUsers([Body] User u);

        [Get("/kawanapis/functions/fetchAllInternationalStudentUsers.php")]
        Task<List<User>> FetchAllInternationalStudentUsers([Body] User u);

        [Get("/kawanapis/functions/fetchCurrentKawanUser.php")]
        Task<KawanUser> FetchCurrentKawanUser([Body] User u);

        [Get("/kawanapis/functions/fetchListOfCountries.php")]
        Task<List<Country>> FetchListOfCountries([Body] User u);
        #endregion

        #region Analytics
        [Get("/kawanapis/functions/fetchKawanStats.php")]
        Task<KawanStats> FetchKawanStats([Body] User u);

        [Get("/kawanapis/functions/fetchUserOnlineTimeFrequencies.php")]
        Task<int[]> FetchUserOnlineTimeFrequencies([Body] FriendRequest fr);
        #endregion

        #region Friend Requests
        [Post("/kawanapis/functions/sendFriendRequest.php")]
        Task<ReplyMessage> SendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/unsendFriendRequest.php")]
        Task<ReplyMessage> UnsendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/acceptFriendRequest.php")]
        Task<ReplyMessage> AcceptFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/functions/rejectFriendRequest.php")]
        Task<ReplyMessage> RejectFriendRequest([Body] FriendRequest fr);
        #endregion

        #region Messages
        [Get("/kawanapis/functions/fetchAllMessages.php")]
        Task<List<ChatMessageItem>> FetchAllMessages([Body] ChatMessageRequest cmr);

        [Get("/kawanapis/functions/fetchMessages.php")]
        Task<List<ChatMessage>> FetchMessages([Body] ChatMessage cm);

        [Post("/kawanapis/functions/storeMessage.php")]
        Task<ReplyMessage> StoreMessage([Body] ChatMessage cm);
        #endregion

        #region Activities
        [Get("/kawanapis/functions/fetchListOfStudents.php")]
        Task<List<StudentForActivity>> FetchListOfStudents([Body] Session s); //Session is used because it is the smallest server key class available

        [Get("/kawanapis/functions/fetchAllActivities.php")]
        Task<List<Activity>> FetchAllActivities([Body] User u);

        [Post("/kawanapis/functions/storeActivities.php")]
        Task<ReplyMessage> StoreActivities([Body] ActivitiesForServer a);
        #endregion

        #region Satisfactory Forms
        [Get("/kawanapis/functions/fetchAllSatisfactoryForms.php")]
        Task<List<SatisfactoryForm>> FetchAllSatisfactoryForms([Body] User u);

        [Post("/kawanapis/functions/updateSatisfactoryForm.php")]
        Task<ReplyMessage> UpdateSatisfactoryForm([Body] SatisfactoryForm a);
        #endregion

        #region Notifications
        [Get("/kawanapis/functions/fetchNotifications.php")]
        Task<List<Notification>> FetchNotifications([Body] User u);
        
        [Post("/kawanapis/functions/readNotification.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> ReadNotification([Body] Notification n);
        
        [Post("/kawanapis/functions/storeNotification.php")] 
        Task<ReplyMessage> StoreNotification([Body] Notification n);
        
        [Post("/kawanapis/functions/deleteNotification.php")] //should be "Delete" but server does not accept Delete requests for some reason
        Task<ReplyMessage> DeleteNotification([Body] Notification n);
        #endregion

        #region Other
        [Post("/kawanapis/functions/updatePassword.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> UpdatePassword([Body] KawanUser ku);
        #endregion
    }
}
