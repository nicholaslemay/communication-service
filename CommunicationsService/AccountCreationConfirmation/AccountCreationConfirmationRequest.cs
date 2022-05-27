namespace CommunicationsService.AccountCreationConfirmation;

public record AccountCreationConfirmationRequest
{
    public string Email { get; init; }
    public string Name { get; init; }
}