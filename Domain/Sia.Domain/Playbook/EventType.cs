using Sia.Core.Data;
using System.Collections.Generic;


namespace Sia.Domain.Playbook
{
    public class EventType : IEntity, IJsonDataObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// MaterialUI icon to display on frontend.
        /// See https://material.io/icons/ for options
        /// Format is PascalCase combining the category and icon name
        /// For example, in the Action category, there is an 'account balance' icon
        /// To use it, this value would need to be ActionAccountBalance
        /// </summary>
        public string Icon { get; set; }
        public object Data { get; set; }
        public DisplayTemplate DisplayTemplate { get; set; }
        public ICollection<Action> Actions { get; set; }
            = new List<Action>();
    }
}
