using System.Net;
using System.Net.Mail;
using CommunicationsService;
using FluentEmail.Core;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddFluentEmail(Config.EmailsUsername).AddRazorRenderer().AddSmtpSender(new SmtpClient
// {
//     Host = "smtp.gmail.com",
//     Port = 587,
//     Credentials = new NetworkCredential(Config.EmailsUsername, Config.Password),
//     EnableSsl = true
// });

builder.Services.AddFluentEmail(Config.EmailsUsername).AddRazorRenderer().AddSmtpSender(new SmtpClient
{
    Host = "127.0.0.1",
    Port = 9999,
});
    
    
var app = builder.Build();

app.MapGet("/", async (IFluentEmail fluentEmail) =>
{
    await fluentEmail
        .To(Config.DestinationEmail)
        .Subject("Subscription Confirmation")
        .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/Templates/MyTemplate.cshtml", new TestViewModel
            { Name = "Johnny Primo" })
        .SendAsync();
});

app.Run();

public partial class Program{}