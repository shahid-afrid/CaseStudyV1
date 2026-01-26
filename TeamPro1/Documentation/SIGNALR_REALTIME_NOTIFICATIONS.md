# ?? SignalR Real-Time Notifications Guide

## ?? **Overview**

SignalR is integrated into TeamPro1 to provide **real-time, bidirectional communication** between the server and clients. This enables instant notifications without page refreshes.

---

## ?? **Implementation Details**

### **1. Backend Setup (Already Configured)**

#### **NotificationHub.cs** (`TeamPro1/Hubs/NotificationHub.cs`)
The SignalR hub that handles all real-time communication.

**Available Methods:**
- ? `SendNotificationToUser(userId, message, type)` - Send to specific user
- ? `SendNotificationToAll(message, type)` - Broadcast to all
- ? `NotifyTeamRequest(receiverStudentId, senderName, requestType)` - Team request notifications
- ? `NotifyProgressUpdate(teamId, message)` - Project progress updates
- ? `NotifyMentorAssigned(studentId, mentorName)` - Mentor assignment alerts
- ? `NotifyProblemStatementAssigned(studentId, problemStatement)` - Problem statement alerts
- ? `NotifyMeetingReviewed(teamId, meetingNumber, review)` - Faculty review notifications
- ? `JoinTeamGroup(teamId)` - Join team-specific group
- ? `LeaveTeamGroup(teamId)` - Leave team group

#### **Program.cs Configuration**
```csharp
// Add SignalR service
builder.Services.AddSignalR();

// Map SignalR endpoint
app.MapHub<NotificationHub>("/notifyHub");
```

**Hub Endpoint:** `https://localhost:[port]/notifyHub`

---

## ?? **Frontend Integration**

### **Step 1: Add SignalR JavaScript Library**

Add to your `_Layout.cshtml` (before closing `</body>` tag):

```html
<!-- SignalR JavaScript Library -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js"></script>

<!-- Initialize SignalR Connection -->
<script>
    // Create connection to SignalR hub
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notifyHub")
        .withAutomaticReconnect() // Auto-reconnect if connection drops
        .build();

    // Start connection
    connection.start()
        .then(() => {
            console.log("? SignalR Connected!");
            
            // Join team group if user is part of a team
            const teamId = "@ViewBag.TeamId"; // Pass from controller
            if (teamId) {
                connection.invoke("JoinTeamGroup", teamId);
            }
        })
        .catch(err => console.error("? SignalR Connection Error:", err));

    // Handle reconnection
    connection.onreconnected((connectionId) => {
        console.log("?? SignalR Reconnected!");
    });

    // === LISTEN FOR NOTIFICATIONS ===

    // 1. General Notifications
    connection.on("ReceiveNotification", (message, type) => {
        showNotification(message, type);
    });

    // 2. Team Request Notifications
    connection.on("ReceiveTeamRequest", (message, requestType) => {
        showNotification(message, "info");
        // Update UI - show badge, refresh requests list, etc.
        updateTeamRequestsBadge();
    });

    // 3. Progress Update Notifications
    connection.on("ReceiveProgressUpdate", (message) => {
        showNotification(message, "success");
        // Refresh progress section
        refreshProgressSection();
    });

    // 4. Mentor Assignment Notifications
    connection.on("ReceiveMentorAssignment", (message, mentorName) => {
        showNotification(message, "success");
        // Update mentor display
        updateMentorDisplay(mentorName);
    });

    // 5. Problem Statement Notifications
    connection.on("ReceiveProblemStatement", (message, problemStatement) => {
        showNotification(message, "success");
        // Update problem statement display
        updateProblemStatement(problemStatement);
    });

    // 6. Meeting Review Notifications
    connection.on("ReceiveMeetingReview", (message, meetingNumber, review) => {
        showNotification(message, "info");
        // Refresh meeting section
        refreshMeetingSection(meetingNumber);
    });

    // === NOTIFICATION DISPLAY FUNCTION ===
    function showNotification(message, type = "info") {
        // Create notification element
        const notification = document.createElement("div");
        notification.className = `alert alert-${type} notification-popup`;
        notification.innerHTML = `
            <i class="fas fa-bell"></i>
            <span>${message}</span>
            <button onclick="this.parentElement.remove()" class="close-btn">×</button>
        `;
        
        // Add to page
        document.body.appendChild(notification);
        
        // Auto-remove after 5 seconds
        setTimeout(() => notification.remove(), 5000);
        
        // Play notification sound (optional)
        playNotificationSound();
    }

    function playNotificationSound() {
        const audio = new Audio('/sounds/notification.mp3');
        audio.play().catch(err => console.log("Sound play failed:", err));
    }

    // === UI UPDATE FUNCTIONS ===
    function updateTeamRequestsBadge() {
        // Increment badge count
        const badge = document.querySelector('.team-requests-badge');
        if (badge) {
            badge.textContent = parseInt(badge.textContent || 0) + 1;
            badge.style.display = 'inline-block';
        }
    }

    function updateMentorDisplay(mentorName) {
        const mentorElement = document.querySelector('.mentor-name');
        if (mentorElement) {
            mentorElement.textContent = mentorName;
        }
    }

    function updateProblemStatement(problemStatement) {
        const psElement = document.querySelector('.problem-statement');
        if (psElement) {
            psElement.textContent = problemStatement;
        }
    }

    function refreshProgressSection() {
        // Reload progress section via AJAX
        fetch('/Student/GetProgressPartial')
            .then(response => response.text())
            .then(html => {
                document.querySelector('#progress-section').innerHTML = html;
            });
    }

    function refreshMeetingSection(meetingNumber) {
        // Reload specific meeting
        fetch(`/Student/GetMeetingPartial/${meetingNumber}`)
            .then(response => response.text())
            .then(html => {
                document.querySelector(`#meeting-${meetingNumber}`).innerHTML = html;
            });
    }
</script>

<!-- Notification Styles -->
<style>
    .notification-popup {
        position: fixed;
        top: 80px;
        right: 20px;
        z-index: 9999;
        min-width: 300px;
        max-width: 400px;
        padding: 15px 20px;
        border-radius: 10px;
        box-shadow: 0 4px 20px rgba(0,0,0,0.3);
        display: flex;
        align-items: center;
        gap: 10px;
        animation: slideIn 0.3s ease;
        background: white;
        border-left: 4px solid #17a2b8;
    }

    .notification-popup.alert-success {
        border-left-color: #28a745;
        background: #d4edda;
    }

    .notification-popup.alert-info {
        border-left-color: #17a2b8;
        background: #d1ecf1;
    }

    .notification-popup.alert-warning {
        border-left-color: #ffc107;
        background: #fff3cd;
    }

    .notification-popup .close-btn {
        margin-left: auto;
        background: none;
        border: none;
        font-size: 24px;
        cursor: pointer;
        opacity: 0.5;
    }

    .notification-popup .close-btn:hover {
        opacity: 1;
    }

    @keyframes slideIn {
        from {
            transform: translateX(400px);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
</style>
```

---

## ?? **Using SignalR in Controllers**

### **Example 1: Send Team Request Notification**

```csharp
using Microsoft.AspNetCore.SignalR;
using TeamPro1.Hubs;

public class StudentController : Controller
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly AppDbContext _context;

    public StudentController(AppDbContext context, IHubContext<NotificationHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    [HttpPost]
    public async Task<IActionResult> SendTeamRequest(int receiverId)
    {
        // ... create team request ...

        // Get sender and receiver info
        var sender = await _context.Students.FindAsync(senderId);
        var receiver = await _context.Students.FindAsync(receiverId);

        // ? Send real-time notification
        await _hubContext.Clients.User(receiverId.ToString())
            .SendAsync("ReceiveTeamRequest", 
                $"{sender.FullName} sent you a team request!", 
                "received");

        return Json(new { success = true });
    }
}
```

### **Example 2: Notify When Mentor is Assigned (Faculty Controller)**

```csharp
[HttpPost]
public async Task<IActionResult> AssignMentor(int teamId, int facultyId)
{
    // ... assign mentor logic ...

    var team = await _context.Teams
        .Include(t => t.Student1)
        .Include(t => t.Student2)
        .FirstOrDefaultAsync(t => t.Id == teamId);

    var faculty = await _context.Faculties.FindAsync(facultyId);

    // ? Notify both team members
    await _hubContext.Clients.Users(new[] { 
        team.Student1Id.ToString(), 
        team.Student2Id.ToString() 
    }).SendAsync("ReceiveMentorAssignment", 
        $"?? {faculty.FullName} has been assigned as your mentor!", 
        faculty.FullName);

    return Json(new { success = true });
}
```

### **Example 3: Notify Team When Meeting is Reviewed**

```csharp
[HttpPost]
public async Task<IActionResult> ReviewMeeting(int meetingId, string review)
{
    var meeting = await _context.TeamMeetings
        .Include(m => m.Team)
        .FirstOrDefaultAsync(m => m.Id == meetingId);

    meeting.FacultyReview = review;
    await _context.SaveChangesAsync();

    // ? Notify team group
    await _hubContext.Clients.Group(meeting.TeamId.ToString())
        .SendAsync("ReceiveMeetingReview", 
            $"? Faculty reviewed Meeting #{meeting.MeetingNumber}", 
            meeting.MeetingNumber, 
            review);

    return Json(new { success = true });
}
```

---

## ?? **Use Cases in TeamPro1**

| Scenario | SignalR Method | Who Gets Notified |
|----------|---------------|-------------------|
| **Team Request Sent** | `NotifyTeamRequest` | Receiver student |
| **Team Request Accepted** | `NotifyTeamRequest` | Sender student |
| **Team Request Rejected** | `NotifyTeamRequest` | Sender student |
| **Mentor Assigned** | `NotifyMentorAssigned` | Both team members |
| **Problem Statement Assigned** | `NotifyProblemStatementAssigned` | Both team members |
| **Meeting Reviewed** | `NotifyMeetingReviewed` | Team group |
| **Progress Updated** | `NotifyProgressUpdate` | Team group |
| **General Announcement** | `SendNotificationToAll` | All connected users |

---

## ?? **Visual Notification Examples**

### **Success Notification** (Green)
```
? Meeting #1 added successfully! Completion: 25%
```

### **Info Notification** (Blue)
```
?? John Doe sent you a team formation request!
```

### **Warning Notification** (Yellow)
```
?? Your team meeting #2 is due tomorrow!
```

---

## ?? **Security Considerations**

1. **Authentication**: SignalR automatically uses the same authentication as your app
2. **User-Specific Notifications**: Use `Clients.User(userId)` for private messages
3. **Group Notifications**: Use `Clients.Group(teamId)` for team-specific messages
4. **Connection Tracking**: SignalR tracks connections per user automatically

---

## ?? **Testing SignalR**

### **Test in Browser Console:**
```javascript
// Send test notification
connection.invoke("SendNotificationToUser", "123", "Test message", "info")
    .catch(err => console.error(err));

// Join team group
connection.invoke("JoinTeamGroup", "1")
    .catch(err => console.error(err));
```

---

## ?? **Official Documentation**

- **ASP.NET Core SignalR**: https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction
- **JavaScript Client**: https://learn.microsoft.com/en-us/aspnet/core/signalr/javascript-client
- **Hubs**: https://learn.microsoft.com/en-us/aspnet/core/signalr/hubs
- **Groups**: https://learn.microsoft.com/en-us/aspnet/core/signalr/groups

---

## ? **Current Status**

| Component | Status |
|-----------|--------|
| **NotificationHub.cs** | ? Created with full methods |
| **Program.cs SignalR Setup** | ? Configured |
| **Hub Endpoint** | ? `/notifyHub` |
| **Frontend Integration** | ? Add to _Layout.cshtml |
| **Controller Usage** | ? Inject `IHubContext` as needed |

---

## ?? **Next Steps**

1. ? **Add SignalR script to _Layout.cshtml** (see Step 1 above)
2. ? **Inject IHubContext in controllers** that need notifications
3. ? **Call SignalR methods** when events occur (team requests, mentor assignment, etc.)
4. ? **Test** real-time notifications!

---

**Your SignalR setup is complete and ready to use!** ??
