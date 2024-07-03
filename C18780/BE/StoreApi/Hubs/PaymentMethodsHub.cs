using Microsoft.AspNetCore.SignalR;
using StoreApi.utils;

namespace SignalRWebpack.Hubs;

public class PaymentMethodsHub : Hub
{
    public async Task PaymentMethodsChange(PaymentMethods paymentMethods) =>
        await Clients.All.SendAsync("PaymentMethodsChange", paymentMethods);
}