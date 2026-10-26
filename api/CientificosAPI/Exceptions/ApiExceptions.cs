namespace CientificosAPI.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string mensaje)
        : base(mensaje)
    {
    }
}

public class BadRequestException : Exception
{
    public BadRequestException(string mensaje)
        : base(mensaje)
    {
    }
}