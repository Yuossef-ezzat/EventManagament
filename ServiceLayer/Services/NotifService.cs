using DomainLayer.Abstractions;
using DomainLayer.Contract;
using DomainLayer.Models;
using DomainLayer.Models.NotificationModule;
using DomainLayer.Models.Registeration;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using ServiceAbstraction;
using Shared.Dtos;

namespace ServiceLayer.Services;

public class NotifService(IEmailService emailService,UserManager<ApplicationUser> _usermanager, IGenaricRepository<UserNotification, int> _repository, IGenaricRepository<Notification, int> _repositorynotif) : INotifService
{
    public async Task<Result<List<NotifDTO>>> GetNotifsById(int id)
    {
        var notifs = _repository.FindAllAsync<NotifDTO>(n => n.UserId == id && !n.IsRead,new string[] {"User", "Notification"});

        return Result<List<NotifDTO>>.Success(notifs.ToList());
    }

    public async Task<Result<NotifDTO>> MarkNotifAsRead(int notifid)
    {
        var notif = await _repository
            .FindAsync(n => n.NotifId == notifid);

        if (notif == null)
            return Result<NotifDTO>.Failure(new Error("Notif.NotFound", "Notification not found"));

        notif.IsRead = true;
        await _repository.Update(notif);

        return Result<NotifDTO>.Success(notif.Adapt<NotifDTO>());
    }

    public async Task<Result<object>> SendNotification(string message)
    {
        var notif = new Notification
        {
            Message = message,
            Date = DateTime.UtcNow
        };

        await _repositorynotif.AddAsync(notif);

        var users = await _usermanager.GetUsersInRoleAsync("Attende");

        foreach(var user in users)
        {
            var userNotif = new UserNotification
            {
                UserId = user.Id,
                NotifId = notif.NotifId,
                IsRead = false
            };
            await _repository.AddAsync(userNotif);
            await emailService.SendEmailAsync(user.Email!, "New Notification", message);
        }

        return Result<object>.Success(null);
    }
    public async Task<Result<object>> SendNotificationForRegisterdUserAtEvent(List<Registration> registrations,string message)
    {
        var notif = new Notification
        {
            Message = message,
            Date = DateTime.UtcNow
        };

        await _repositorynotif.AddAsync(notif);

        foreach (var user in registrations)
        {
            var userNotif = new UserNotification
            {
                UserId = user.UserId,
                NotifId = notif.NotifId,
                IsRead = false
            };
            await _repository.AddAsync(userNotif);
            await emailService.SendEmailAsync(user.User.Email!, "New Notification", message);
        }

        return Result<object>.Success(null);
    }
}
