using DomainLayer.Models;

namespace ServiceAbstraction;

public interface IPayMobService
{
    Task<string> PayWithCard(int amountCents,int attendid);
    Task<bool> PaymobCallback(PaymobCallback payload, string hmacHeader,int attendId);
}
