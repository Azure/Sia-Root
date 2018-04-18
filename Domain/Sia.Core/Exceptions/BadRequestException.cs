﻿namespace Sia.Core.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message)
        {
        }

        public override int StatusCode => 400;
    }
}
