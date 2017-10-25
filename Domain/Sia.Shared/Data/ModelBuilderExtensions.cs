using Sia.Shared.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Microsoft.EntityFrameworkCore
{
    public static class ModelBuilderExtensions
    {
        public static void WithManyToManyAssociation<TLeft, TRight, TAssociation>(this ModelBuilder builder
            , Expression<Func<TLeft, IEnumerable<TAssociation>>> associationsFromLeft
            , Expression<Func<TRight, IEnumerable<TAssociation>>> associationsFromRight
            )
            where TAssociation : BidrectionalAssociation<TLeft, TRight>
            where TLeft : class, IEntity
            where TRight : class, IEntity
        {
            builder.Entity<TAssociation>()
                .HasOne(assoc => assoc._left)
                .WithMany(associationsFromLeft)
                .HasForeignKey(assoc => assoc._leftId);
            builder.Entity<TAssociation>()
                .HasOne(assoc => assoc._right)
                .WithMany(associationsFromRight)
                .HasForeignKey(assoc => assoc._rightId);
            builder.Entity<TAssociation>()
                .HasKey(assoc => new { assoc._leftId, assoc._rightId });
        }
    }
}
