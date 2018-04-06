using Sia.Core.Data;
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
                .HasOne(assoc => assoc.left)
                .WithMany(associationsFromLeft)
                .HasForeignKey(assoc => assoc.leftId);
            builder.Entity<TAssociation>()
                .HasOne(assoc => assoc.right)
                .WithMany(associationsFromRight)
                .HasForeignKey(assoc => assoc.rightId);
            builder.Entity<TAssociation>()
                .HasKey(assoc => new { assoc.leftId, assoc.rightId });
        }
    }
}
