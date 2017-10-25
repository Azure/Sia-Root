using System.ComponentModel.DataAnnotations;

namespace Sia.Domain.ApiModels
{
    public class NewEngagement
    {
        [Required]
        public Participant Participant { get; set; }
    }
}
