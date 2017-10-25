using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateEventType : IJsonDataObject
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public object Data { get; set; }
    }
}
