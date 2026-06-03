namespace MesoShqip.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"'{name}' me id '{key}' nuk u gjet.") { }
}