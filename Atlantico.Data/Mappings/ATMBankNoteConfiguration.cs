using Atlantico.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Atlantico.Data.Mappings
{
    class ATMBankNoteConfiguration : IEntityTypeConfiguration<ATMBankNote>
    {
        public void Configure(EntityTypeBuilder<ATMBankNote> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.BankNote).IsRequired();
            builder.Property(c => c.Count).IsRequired();
            builder.HasOne(c => c.ATM);
        }
    }
}