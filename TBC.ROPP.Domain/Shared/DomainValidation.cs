using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Domain.Shared;

public class DomainValidation(string code, string message)
{
    public string Code { get; } = code;
    public string Message { get; } = message;

    public void Deconstruct(out string code, out string message)
    {
        code = Code;
        message = Message;
    }

}

public static class DomainValidationExtensions
{

    public static Task<ApplicationResult<T, ApplicationError>> ToApplicationResultAsync<T>(this DomainValidation validation)
    {
        return Task.FromResult(new ApplicationResult<T, ApplicationError>(new ApplicationError(new[] { new Error(validation.Code, validation.Message) })));
    }
}