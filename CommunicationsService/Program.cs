using System.Net;
using System.Net.Mail;
using CommunicationsService;
using CommunicationsService.AccountCreationConfirmation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentEmail(Config.EmailsUsername).AddRazorRenderer().AddSmtpSender(new SmtpClient
{
    Host = "smtp.gmail.com",
    Port = 587,
    Credentials = new NetworkCredential(Config.EmailsUsername, Config.Password),
    EnableSsl = true
});

builder.Build()
    .MapAccountCreationConfirmationEndPoints()
    .Run();

namespace CommunicationsService
{
    public partial class Program{}
}

