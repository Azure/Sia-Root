using Sia.Domain.Playbook;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sia.Domain.ApiModels.Playbooks
{
    public abstract class CreateSource
    {
        public SourceObject SourceObject { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
