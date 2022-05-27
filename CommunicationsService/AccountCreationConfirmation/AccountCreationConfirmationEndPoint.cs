using FluentEmail.Core;

namespace CommunicationsService.AccountCreationConfirmation;

public static class AccountCreationConfirmationEndPoint
{
    public static WebApplication MapAccountCreationConfirmationEndPoints(this WebApplication app)
    {
        app.MapPost("/accountCreationConfirmation", async (IFluentEmail fluentEmail, AccountCreationConfirmationRequest request) =>
        {
            try
            {
                await fluentEmail
                    .To(request.Email)
                    .Subject("Subscription Confirmation")
                    .UsingTemplateFromFile($"{Directory.GetCurrentDirectory()}/AccountCreationConfirmation/AccountCreationConfirmationEmailTemplate.cshtml", 
                        new AccountCreationConfirmationEmailViewModel { Name = request.Name })
                    .SendAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }).Produces(200);
        
        return app;
    }
}