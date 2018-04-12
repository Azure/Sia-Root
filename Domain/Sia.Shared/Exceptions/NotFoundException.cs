﻿namespace Sia.Shared.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public override int StatusCode => 404;
    }
}
