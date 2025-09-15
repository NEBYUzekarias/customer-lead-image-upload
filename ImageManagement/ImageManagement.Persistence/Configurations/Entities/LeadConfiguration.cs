using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImageManagement.Domain;

namespace ImageManagement.Persistence.Configurations.Entities
{
    public class LeadConfiguration : IEntityTypeConfiguration<Lead>
    {
        public void Configure(EntityTypeBuilder<Lead> builder)
        {
            builder.HasKey(l => l.Id);
            
            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(l => l.Email)
                .HasMaxLength(200);
                
            builder.Property(l => l.Phone)
                .HasMaxLength(20);
                
            builder.Property(l => l.Company)
                .HasMaxLength(100);
                
            builder.Property(l => l.Source)
                .HasMaxLength(50);

            // Configure relationship with ProfileImage
            builder.HasMany(l => l.Images)
                .WithOne(i => i.Lead)
                .HasForeignKey(i => i.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore computed properties
            builder.Ignore(l => l.CanAddMoreImages);
            builder.Ignore(l => l.RemainingImageSlots);

            // Seed some test data
            builder.HasData(
                new Lead
                {
                    Id = 1,
                    Name = "Alice Johnson",
                    Email = "alice.johnson@company.com",
                    Phone = "+1-555-0789",
                    Company = "Tech Solutions Inc.",
                    Source = "Website",
                    ContactDate = DateTime.UtcNow.AddDays(-5),
                    DateCreated = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                },
                new Lead
                {
                    Id = 2,
                    Name = "Bob Wilson",
                    Email = "bob.wilson@business.com",
                    Phone = "+1-555-0321",
                    Company = "Business Corp",
                    Source = "Referral",
                    ContactDate = DateTime.UtcNow.AddDays(-2),
                    DateCreated = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                }
            );
        }
    }
}
