using System;
using Microsoft.Extensions.DependencyInjection;
using No_Core_Auth.Services;
namespace No_Core_Auth.Email
{
    public static class SendGridExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            return services;
        }
    }
}
