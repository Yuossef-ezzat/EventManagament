using DomainLayer.Abstractions;
using DomainLayer.Models.Registeration;
using Shared.Dtos;

namespace ServiceAbstraction;

public interface INotifService
{
    Task<Result<object>> SendNotification(string message);
    Task<Result<List<NotifDTO>>> GetNotifsById(int id);
    Task<Result<NotifDTO>> MarkNotifAsRead(int notifid);
    Task<Result<object>> SendNotificationForRegisterdUserAtEvent(List<Registration> registrations, string message);

}
