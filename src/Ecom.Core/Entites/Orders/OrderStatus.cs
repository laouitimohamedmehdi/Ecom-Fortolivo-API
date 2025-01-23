using System.Runtime.Serialization;

namespace Ecom.Core.Entites.Orders
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending,
        [EnumMember(Value = "Payment Received")]
        PaymentReceived,
        [EnumMember(Value = "Payment Felid")]
        PaymentFelid
    }
}