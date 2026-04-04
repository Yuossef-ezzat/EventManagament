using DomainLayer.Models.NotificationModule;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Shared.Dtos;

namespace PresistenceLayer.Data.Configurations;

public static class MapsterConfig
{
    public static void RegisterMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<UserNotification, NotifDTO>
            .NewConfig()
            .Map(dest => dest.Message, src => src.Notification.Message)
            .Map(dest => dest.Date, src => src.Notification.Date)
            .Map(dest => dest.UserName, src => src.User.UserName);

    }
}
