using Microsoft.AspNetCore.SignalR;

namespace TeamPro1.Hubs
{
    /// <summary>
    /// SignalR Hub for real-time notifications in Team Formation System
    /// </summary>
    public class NotificationHub : Hub
    {
        /// <summary>
        /// Send notification to a specific user (by userId)
        /// </summary>
        public async Task SendNotificationToUser(string userId, string message, string type = "info")
        {
            await Clients.User(userId).SendAsync("ReceiveNotification", message, type);
        }

        /// <summary>
        /// Send notification to all connected clients
        /// </summary>
        public async Task SendNotificationToAll(string message, string type = "info")
        {
            await Clients.All.SendAsync("ReceiveNotification", message, type);
        }

        /// <summary>
        /// Notify specific student about team request
        /// </summary>
        public async Task NotifyTeamRequest(string receiverStudentId, string senderName, string requestType)
        {
            var message = requestType switch
            {
                "received" => $"{senderName} sent you a team formation request!",
                "accepted" => $"{senderName} accepted your team request!",
                "rejected" => $"{senderName} rejected your team request.",
                _ => "You have a new team notification."
            };
            
            await Clients.User(receiverStudentId).SendAsync("ReceiveTeamRequest", message, requestType);
        }

        /// <summary>
        /// Notify team members about project progress update
        /// </summary>
        public async Task NotifyProgressUpdate(string teamId, string message)
        {
            await Clients.Group(teamId).SendAsync("ReceiveProgressUpdate", message);
        }

        /// <summary>
        /// Notify student when mentor is assigned
        /// </summary>
        public async Task NotifyMentorAssigned(string studentId, string mentorName)
        {
            var message = $"?? {mentorName} has been assigned as your team mentor!";
            await Clients.User(studentId).SendAsync("ReceiveMentorAssignment", message, mentorName);
        }

        /// <summary>
        /// Notify student when problem statement is assigned
        /// </summary>
        public async Task NotifyProblemStatementAssigned(string studentId, string problemStatement)
        {
            var message = $"?? Problem statement has been assigned to your team!";
            await Clients.User(studentId).SendAsync("ReceiveProblemStatement", message, problemStatement);
        }

        /// <summary>
        /// Notify team when faculty reviews a meeting
        /// </summary>
        public async Task NotifyMeetingReviewed(string teamId, int meetingNumber, string review)
        {
            var message = $"? Faculty reviewed Meeting #{meetingNumber}";
            await Clients.Group(teamId).SendAsync("ReceiveMeetingReview", message, meetingNumber, review);
        }

        /// <summary>
        /// Join a team group for group-specific notifications
        /// </summary>
        public async Task JoinTeamGroup(string teamId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, teamId);
        }

        /// <summary>
        /// Leave a team group
        /// </summary>
        public async Task LeaveTeamGroup(string teamId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, teamId);
        }

        /// <summary>
        /// Called when a client connects
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            // You can log connection or initialize user-specific setup here
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when a client disconnects
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // You can log disconnection or cleanup here
            await base.OnDisconnectedAsync(exception);
        }
    }
}

