using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Sia.Core.Data
{
    public interface IAssociation<TLeft, TRight>
    {
        void Associate(TLeft left, TRight right);
        KeyValuePair<TLeft, TRight> AssociatesBetween();
    }

    public abstract class Association<TLeft, TRight>
        : IAssociation<TLeft, TRight>
        where TLeft : IEntity
        where TRight : IEntity
    {
        protected internal abstract long leftId { get; set; }
        protected internal abstract long rightId { get; set; }
        protected internal abstract TLeft left { get; set; }
        protected internal abstract TRight right { get; set; }
        public void Associate(TLeft left, TRight right)
        {
            this.left = left;
            this.right = right;
            leftId = left.Id;
            rightId = right.Id;
        }

        public KeyValuePair<TLeft, TRight> AssociatesBetween()
            => new KeyValuePair<TLeft, TRight>(left, right);
    }

    public abstract class BidrectionalAssociation<TLeft, TRight>
        : Association<TLeft, TRight>,
        IAssociation<TRight, TLeft>
        where TLeft : IEntity
        where TRight : IEntity
    {
        protected BidrectionalAssociation()
        {
        }

        void IAssociation<TRight, TLeft>.Associate(TRight left, TLeft right)
            => Associate(right, left);


        KeyValuePair<TRight, TLeft> IAssociation<TRight, TLeft>.AssociatesBetween()
            => new KeyValuePair<TRight, TLeft>(right, left);
    }
}