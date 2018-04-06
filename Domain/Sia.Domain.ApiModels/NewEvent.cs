using Sia.Core.Data;
using System;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace Sia.Domain.ApiModels
{
    public class NewEvent
        :IJsonDataObject
    {
        [Required]
        public long? EventTypeId { get; set; }
        [Required]
        public DateTime? Occurred { get; set; }
        [Required]
        public DateTime? EventFired { get; set; }
        public object Data { get; set; }
    }
}
