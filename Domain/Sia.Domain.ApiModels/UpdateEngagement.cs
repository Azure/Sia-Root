using System;

namespace Sia.Domain.ApiModels
{
    public class UpdateEngagement
    {
        public DateTime? TimeEngaged { get; set; }
        public DateTime? TimeDisengaged { get; set; }
    }
}
