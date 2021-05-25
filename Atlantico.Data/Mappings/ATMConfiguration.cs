using Atlantico.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Atlantico.Data.Mappings
{
    class ATMConfiguration : IEntityTypeConfiguration<ATM>
    {
        public void Configure(EntityTypeBuilder<ATM> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Actve).IsRequired();
            builder.HasMany(c => c.ATMBankNotes);
        }
    }
}
