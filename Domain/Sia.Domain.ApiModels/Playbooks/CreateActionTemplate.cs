using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public class CreateActionTemplate
    {
        [Required]
        public string Name { get; set; }
        public bool IsUrl { get; set; }
        public string Template { get; set; }
        public ICollection<CreateActionTemplateSource> NewSources { get; set; }
            = new List<CreateActionTemplateSource>();
        public ICollection<long> ExistingSourceIds { get; set; }
            = new List<long>();
    }
}
