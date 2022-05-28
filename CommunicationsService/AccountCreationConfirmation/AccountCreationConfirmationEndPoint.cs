using FluentEmail.Core;

namespace CommunicationsService.AccountCreationConfirmation;

public static class AccountCreationConfirmationEndPoint
{
    public static WebApplication MapAccountCreationConfirmationEndPoints(this WebApplication app)
    {
        app.MapPost("/accountCreationConfirmation", async (IFluentEmail fluentEmail, AccountCreationConfirmationRequest request) =>
        {
            await fluentEmail
                .To(request.Email)
                .Subject("Subscription Confirmation")
                .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/AccountCreationConfirmation/AccountCreationConfirmationEmailTemplate.cshtml", 
                    new AccountCreationConfirmationEmailViewModel(request.Name))
                .SendAsync();
        }).Produces(200);
        
        return app;
    }
}