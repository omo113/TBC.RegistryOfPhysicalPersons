namespace TBC.ROPP.Shared.ApplicationInfrastructure;

public record ApplicationError(IEnumerable<Error> Errors);

public record Error(string Recourse, string Message);