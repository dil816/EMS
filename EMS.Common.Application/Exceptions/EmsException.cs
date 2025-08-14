using EMS.Common.Domain;

namespace EMS.Common.Application.Exceptions;
public sealed class EmsException : Exception
{
    public EmsException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}

