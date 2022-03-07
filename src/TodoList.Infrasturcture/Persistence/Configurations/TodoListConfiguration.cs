using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoList.Infrasturcture.Persistence.Configurations;

public class TodoListConfiguration : IEntityTypeConfiguration<Domain.Entities.TodoList>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.TodoList> builder)
    {
        builder.Ignore(e => e.DomainEvents);        

        builder.Property(t => t.Title)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.OwnsOne(b => b.Colour);

        builder.HasMany(x => x.Items)
            .WithOne(b => b.List)
            .HasForeignKey(b => b.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
