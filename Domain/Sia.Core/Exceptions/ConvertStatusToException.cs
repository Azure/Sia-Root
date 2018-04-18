using Sia.Core.Protocol;
using System;
using System.Net;

namespace Sia.Core.Exceptions
{
    public static class ConvertStatusToException
    {
        public static void ThrowExceptionOnUnsuccessfulStatus(this IResponse response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }
            var message = response.Content;
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestException(message);
                case HttpStatusCode.Conflict:
                    throw new ConflictException(message);
                case HttpStatusCode.Forbidden:
                    throw new UnauthorizedException(message);
                case HttpStatusCode.NotFound:
                    throw new NotFoundException(message);
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(message);
                // Details of 500-series server errors within microservices are hidden from external requestors
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.NotImplemented:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                case HttpStatusCode.HttpVersionNotSupported:
                    throw new ServerErrorException();
                default:
                    throw new Exception(message);
            }
        }
    }
}
