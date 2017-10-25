using Sia.Shared.Data;
using System.ComponentModel.DataAnnotations;

namespace Sia.Domain.ApiModels
{
    public class UpdateIncident
    {
        public string Title { get; set; }
        [Required]
        public Ticket PrimaryTicket { get; set; }
    }
}
