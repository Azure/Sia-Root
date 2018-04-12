﻿using Sia.Core.Protocol;
using System;

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
                case System.Net.HttpStatusCode.BadRequest:
                    throw new BadRequestException(message);
                case System.Net.HttpStatusCode.Conflict:
                    throw new ConflictException(message);
                case System.Net.HttpStatusCode.Forbidden:
                    throw new UnauthorizedException(message);
                case System.Net.HttpStatusCode.NotFound:
                    throw new NotFoundException(message);
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException(message);
                default:
                    throw new Exception(message);
            }
        }
    }
}
