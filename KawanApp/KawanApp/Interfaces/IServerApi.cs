using Refit;
using KawanApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KawanApp.Interfaces
{
    public interface IServerApi
    {
        #region Sessions
        [Post("/kawanapis/demoFunctions/sessionStart.php")]
        Task<SessionReply> StartSession([Body] Session s);

        [Post("/kawanapis/demoFunctions/sessionEnd.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<SessionReply> EndSession([Body] Session s);
        #endregion

        #region Login, SignUp, and Edit
        [Get("/kawanapis/demoFunctions/login.php")]
        Task<LoginReply> Login([Body] UserAuthentication ua);
        
        [Post("/kawanapis/demoFunctions/signUp.php")]
        Task<ReplyMessage> SignUp([Body] KawanUser ku);

        [Post("/kawanapis/demoFunctions/edit.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> Edit([Body] KawanUser ku);

        [Post("/kawanapis/demoFunctions/uploadPhoto.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> UploadPhoto([Body] PhotoUpload pu);
        #endregion

        #region Fetch Profile and Country Information
        [Get("/kawanapis/demoFunctions/fetchAllKawanUsers.php")]
        Task<List<KawanUser>> FetchAllKawanUsers([Body] User u);

        [Get("/kawanapis/demoFunctions/fetchAllInternationalStudentUsers.php")]
        Task<List<User>> FetchAllInternationalStudentUsers([Body] User u);

        [Get("/kawanapis/demoFunctions/fetchCurrentKawanUser.php")]
        Task<KawanUser> FetchCurrentKawanUser([Body] User u);

        [Get("/kawanapis/demoFunctions/fetchListOfCountries.php")]
        Task<List<Country>> FetchListOfCountries([Body] User u);
        #endregion

        #region Analytics
        [Get("/kawanapis/demoFunctions/fetchKawanStats.php")]
        Task<KawanStats> FetchKawanStats([Body] User u);

        [Get("/kawanapis/demoFunctions/fetchUserOnlineTimeFrequencies.php")]
        Task<int[]> FetchUserOnlineTimeFrequencies([Body] FriendRequest fr);
        #endregion

        #region Friend Requests
        [Post("/kawanapis/demoFunctions/sendFriendRequest.php")]
        Task<ReplyMessage> SendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/demoFunctions/unsendFriendRequest.php")]
        Task<ReplyMessage> UnsendFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/demoFunctions/acceptFriendRequest.php")]
        Task<ReplyMessage> AcceptFriendRequest([Body] FriendRequest fr);

        [Post("/kawanapis/demoFunctions/rejectFriendRequest.php")]
        Task<ReplyMessage> RejectFriendRequest([Body] FriendRequest fr);
        #endregion

        #region Messages
        [Get("/kawanapis/demoFunctions/fetchAllMessages.php")]
        Task<List<ChatMessageItem>> FetchAllMessages([Body] ChatMessageRequest cmr);

        [Get("/kawanapis/demoFunctions/fetchMessages.php")]
        Task<List<ChatMessage>> FetchMessages([Body] ChatMessage cm);

        [Post("/kawanapis/demoFunctions/storeMessage.php")]
        Task<ReplyMessage> StoreMessage([Body] ChatMessage cm);
        #endregion

        #region Activities
        [Get("/kawanapis/demoFunctions/fetchListOfStudents.php")]
        Task<List<StudentForActivity>> FetchListOfStudents([Body] Session s); //Session is used because it is the smallest server key class available

        [Get("/kawanapis/demoFunctions/fetchAllActivities.php")]
        Task<List<Activity>> FetchAllActivities([Body] User u);

        [Post("/kawanapis/demoFunctions/storeActivities.php")]
        Task<ReplyMessage> StoreActivities([Body] ActivitiesForServer a);
        #endregion

        #region Satisfactory Forms
        [Get("/kawanapis/demoFunctions/fetchAllSatisfactoryForms.php")]
        Task<List<SatisfactoryForm>> FetchAllSatisfactoryForms([Body] User u);

        [Post("/kawanapis/demoFunctions/updateSatisfactoryForm.php")]
        Task<ReplyMessage> UpdateSatisfactoryForm([Body] SatisfactoryForm a);
        #endregion

        #region Notifications
        [Get("/kawanapis/demoFunctions/fetchNotifications.php")]
        Task<List<Notification>> FetchNotifications([Body] User u);
        
        [Post("/kawanapis/demoFunctions/readNotification.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> ReadNotification([Body] Notification n);
        
        [Post("/kawanapis/demoFunctions/storeNotification.php")] 
        Task<ReplyMessage> StoreNotification([Body] Notification n);
        
        [Post("/kawanapis/demoFunctions/deleteNotification.php")] //should be "Delete" but server does not accept Delete requests for some reason
        Task<ReplyMessage> DeleteNotification([Body] Notification n);
        #endregion

        #region Other
        [Post("/kawanapis/demoFunctions/updatePassword.php")] //should be "Put" but server does not accept Put requests for some reason
        Task<ReplyMessage> UpdatePassword([Body] KawanUser ku);
        #endregion
    }
}
