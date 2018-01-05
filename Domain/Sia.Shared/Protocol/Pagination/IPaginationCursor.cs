using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public interface IPaginationCursor
    {
        IEnumerable<KeyValuePair<string, string>> SerializationTokens();
        bool IsInitialized();
    }
}
