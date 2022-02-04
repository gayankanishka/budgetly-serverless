using Budgetly.Domain.Entities;
using Budgetly.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Budgetly.Infrastructure.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.Ignore(e => e.DomainEvents);

        builder
            .Property(_ => _.Type)
            .HasConversion(
                _ => _.ToString(),
                _ => (TransactionTypes)Enum.Parse(typeof(TransactionTypes), _));
    }
}