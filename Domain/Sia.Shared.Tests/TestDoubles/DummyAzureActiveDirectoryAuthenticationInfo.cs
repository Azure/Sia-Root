using System;
using System.Collections.Generic;
using System.Text;
using Sia.Shared.Authentication;

namespace Sia.Shared.Tests.TestDoubles
{
    public class DummyAzureActiveDirectoryAuthenticationInfo : AzureActiveDirectoryAuthenticationInfo
    {
        private static string _resource = "resource";
        private static string _clientId = "clientId";
        private static string _clientSecret = "clientSecret";
        private static string _tenant = "tenant";

        public DummyAzureActiveDirectoryAuthenticationInfo()
            : base(_resource, _clientId, _clientSecret, _tenant)
        {
        }
    }
}
