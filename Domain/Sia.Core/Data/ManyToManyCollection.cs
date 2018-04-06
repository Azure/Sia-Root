using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sia.Core.Data
{
    public class ManyToManyCollection<TItem, TAssociation, TAssociated> : ICollection<TAssociated>
            where TAssociation : IAssociation<TItem, TAssociated>, new()
    {
        private TItem _parent;
        private ICollection<TAssociation> _associations;
        private IEnumerable<TAssociated> _associated => _associations.Select(assoc => assoc.AssociatesBetween().Value);

        public ManyToManyCollection(TItem parent, ICollection<TAssociation> associations)
        {
            _parent = parent;
            _associations = associations;
        }

        public int Count => _associations.Count;

        public bool IsReadOnly => false;

        public void Add(TAssociated item)
        {
            var associationToAdd = new TAssociation();
            associationToAdd.Associate(_parent, item);
            _associations.Add(associationToAdd);
        }

        public void Clear()
        {
            _associations.Clear();
        }

        public bool Contains(TAssociated item)
        {
            return _associated.Contains(item);
        }

        public void CopyTo(TAssociated[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(arrayIndex));
            if (arrayIndex < 0) throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex, "Array Index cannot be less than 0!");
            if (array.GetLength(0) - arrayIndex > Count) throw new ArgumentException("The number of elements in the source System.Collections.Generic.ICollection`1 is greater than the available space from arrayIndex to the end of the destination array.", nameof(arrayIndex));

            _associated.ToList().CopyTo(array, arrayIndex);
        }

        public IEnumerator<TAssociated> GetEnumerator()
        {
            return _associated.GetEnumerator();
        }

        public bool Remove(TAssociated item)
        {
            var relevantAssociations = _associations.Where(assoc => assoc.AssociatesBetween().Value.Equals(item));
            bool success = true;
            foreach (var association in relevantAssociations)
            {
                if (!_associations.Remove(association))
                {
                    success = false;
                }
            }
            return success;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _associated.GetEnumerator();
        }
    }
}