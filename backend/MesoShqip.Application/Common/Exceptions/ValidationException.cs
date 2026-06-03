namespace MesoShqip.Application.Common.Exceptions;

public class AppValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public AppValidationException(IDictionary<string, string[]> errors)
        : base("Ndodhën gabime validimi.")
    {
        Errors = errors;
    }
}