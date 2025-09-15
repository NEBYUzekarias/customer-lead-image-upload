using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImageManagement.Domain;

namespace ImageManagement.Persistence.Configurations.Entities
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);
                
            builder.Property(c => c.Email)
                .HasMaxLength(200);
                
            builder.Property(c => c.Phone)
                .HasMaxLength(20);
                
            builder.Property(c => c.Address)
                .HasMaxLength(500);

            // Configure relationship with ProfileImage
            builder.HasMany(c => c.Images)
                .WithOne(i => i.Customer)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ignore computed properties
            builder.Ignore(c => c.CanAddMoreImages);
            builder.Ignore(c => c.RemainingImageSlots);

            // Seed some test data
            builder.HasData(
                new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Phone = "+1-555-0123",
                    Address = "123 Main St, Anytown, USA",
                    DateCreated = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@example.com",
                    Phone = "+1-555-0456",
                    Address = "456 Oak Ave, Somewhere, USA",
                    DateCreated = DateTime.UtcNow,
                    LastModifiedDate = DateTime.UtcNow
                }
            );
        }
    }
}
