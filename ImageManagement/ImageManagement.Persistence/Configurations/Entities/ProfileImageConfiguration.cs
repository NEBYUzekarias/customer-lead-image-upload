using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImageManagement.Domain;

namespace ImageManagement.Persistence.Configurations.Entities
{
    public class ProfileImageConfiguration : IEntityTypeConfiguration<ProfileImage>
    {
        public void Configure(EntityTypeBuilder<ProfileImage> builder)
        {
            builder.HasKey(p => p.Id);
            
            builder.Property(p => p.Base64Data)
                .IsRequired();
                
            builder.Property(p => p.ContentType)
                .HasMaxLength(100);
                
            builder.Property(p => p.FileName)
                .HasMaxLength(255);
                
            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.FileSize)
                .IsRequired();

            builder.Property(p => p.IsMainImage)
                .HasDefaultValue(false);

            // Ignore computed properties
            builder.Ignore(p => p.IsValidImageType);

            // Configure relationships
            builder.HasOne(p => p.Customer)
                .WithMany(c => c.Images)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Lead)
                .WithMany(l => l.Images)
                .HasForeignKey(p => p.LeadId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add constraint to ensure image belongs to either customer or lead, but not both
            builder.ToTable(t => t.HasCheckConstraint("CK_ProfileImage_OneOwner", 
                "(CustomerId IS NOT NULL AND LeadId IS NULL) OR (CustomerId IS NULL AND LeadId IS NOT NULL)"));

            // Add index for better query performance
            builder.HasIndex(p => p.CustomerId);
            builder.HasIndex(p => p.LeadId);
            builder.HasIndex(p => p.IsMainImage);
        }
    }
}
