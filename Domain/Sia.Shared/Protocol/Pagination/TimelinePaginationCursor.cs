using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Shared.Protocol.Pagination
{
    public class TimelinePaginationCursor
        : IPaginationCursor<TimelinePaginationCursor>,
        IComparable<TimelinePaginationCursor>
    {
        public long CursorId { get; set; }
        public DateTime CursorTime { get; set; }
        public int CompareTo(TimelinePaginationCursor other)
        {
            var dateTimeComparison = CursorTime.CompareTo(other.CursorTime);
            if (dateTimeComparison != 0) return dateTimeComparison;
            return CursorId.CompareTo(other.CursorId);
        }

        public bool IsInitialized()
            => !CursorId.Equals(default(long))
            && !CursorTime.Equals(default(DateTime));

        public IEnumerable<KeyValuePair<string, string>> SerializationTokens()
        {
            yield return new KeyValuePair<string, string>(nameof(CursorId), CursorId.ToString());
            yield return new KeyValuePair<string, string>(nameof(CursorTime), CursorTime.ToString());
        }
    }
}
