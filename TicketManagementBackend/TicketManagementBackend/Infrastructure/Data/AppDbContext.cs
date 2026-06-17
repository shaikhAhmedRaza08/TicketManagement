using TicketManagement.Domain;
using Microsoft.EntityFrameworkCore;

namespace TicketManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketComment> Comments => Set<TicketComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var project = modelBuilder.Entity<Project>();
        project.HasKey(p => p.Id);
        project.Property(p => p.Name).IsRequired().HasMaxLength(120);
        project.Property(p => p.Key).IsRequired().HasMaxLength(10);
        project.HasIndex(p => p.Key).IsUnique();

        var ticket = modelBuilder.Entity<Ticket>();
        ticket.HasKey(t => t.Id);
        ticket.Property(t => t.Title).IsRequired().HasMaxLength(200);
        // Store enums as readable strings rather than ints.
        ticket.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
        ticket.Property(t => t.Priority).HasConversion<string>().HasMaxLength(20);
        ticket.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
        // Indexes on the columns we filter by most often.
        ticket.HasIndex(t => t.Status);
        ticket.HasIndex(t => t.ProjectId);

        ticket.HasOne(t => t.Project)
              .WithMany(p => p.Tickets)
              .HasForeignKey(t => t.ProjectId)
              .OnDelete(DeleteBehavior.Cascade);

        var comment = modelBuilder.Entity<TicketComment>();
        comment.HasKey(c => c.Id);
        comment.Property(c => c.Author).IsRequired().HasMaxLength(120);
        comment.Property(c => c.Body).IsRequired();
        comment.HasOne(c => c.Ticket)
               .WithMany(t => t.Comments)
               .HasForeignKey(c => c.TicketId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
