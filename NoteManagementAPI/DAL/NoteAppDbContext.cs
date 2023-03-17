using Microsoft.EntityFrameworkCore;
using NoteManagementAPI.Entities;

namespace NoteManagementAPI.DAL
{
    public class NoteAppDbContext : DbContext
    {
        public NoteAppDbContext(DbContextOptions<NoteAppDbContext> options) : base(options)
        {
        }
        public NoteAppDbContext() { }


        public DbSet<LoginRecord> LoginRecords { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginRecord>()
                .HasKey(r => new { r.LoginTime, r.UserId });

            #region Note Model Creating
            modelBuilder.Entity<Note>()
                   .Property(n => n.CreateDate)
                   .HasDefaultValueSql("getdate()")
                   .ValueGeneratedOnAdd();

            modelBuilder.Entity<Note>()
                .Property(n => n.UpdateDate)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Note>()
                .Property(n => n.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<Note>()
                .HasData(
                    new Note
                    {
                        Id = 1,
                        Title = "Title 1",
                        Content = "Lorem ipsum 1",
                        IsDeleted = false,
                        UserId = 1,
                    }, 
                    new Note
                    {
                        Id = 2,
                        Title = "Title 2",
                        Content = "Lorem ipsum 2",
                        IsDeleted = false,
                        UserId = 2,
                    }, 
                    new Note
                    {
                        Id = 3,
                        Title = "Title 3",
                        Content = "Lorem ipsum 3",
                        IsDeleted = false,
                        UserId = 3,
                    });
            #endregion

            #region User Model Creating
            modelBuilder.Entity<User>()
                .HasIndex(n => n.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .Property(n => n.CreateDate)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .Property(n => n.UpdateDate)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>()
                .Property(n => n.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);
            modelBuilder.Entity<User>()
                .Property(n => n.ImageUrl)
                .IsRequired(false);
            modelBuilder.Entity<User>()
                .Property(n => n.RoleId)
                .HasDefaultValue(RoleName.User);

            modelBuilder.Entity<User>()
                .Property(n => n.LastLoginTime)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Username = "datuwu",
                        Password = "12345678",
                        Email = "datlt.mdc@gmail.com",
                        Fullname = "Dat 1",
                        CreateDate = DateTime.Now,
                        RoleId = 2,
                        Status = UserStatus.Active,
                        IsDeleted = false,
                    },
                    new User
                    {
                        Id = 2,
                        Username = "datowo",
                        Password = "12345678",
                        Email = "dat@mail.com",
                        Fullname = "Dat 2",
                        CreateDate = DateTime.Now,
                        RoleId = 2,
                        Status = UserStatus.Active,
                        IsDeleted = false,
                    },
                    new User
                    {
                        Id = 3,
                        Username = "datltuwu",
                        Password = "12345678",
                        Email = "dat1@mail.com",
                        Fullname = "Dat 3",
                        CreateDate = DateTime.Now,
                        RoleId = 2,
                        Status = UserStatus.Active,
                        IsDeleted = false,
                    });
            #endregion

            #region Role Model Creating
            modelBuilder.Entity<Role>()
                .Property(n => n.CreateDate)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>()
                .Property(n => n.UpdateDate)
                .HasDefaultValueSql("getdate()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Role>()
                .Property(n => n.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<Role>()
                .HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Admin",
                        Status = RoleStatus.Active,
                        IsDeleted = false,
                    },
                    new Role
                    {
                        Id = 2,
                        Name = "User",
                        Status = RoleStatus.Active,
                        IsDeleted = false,
                    });
            #endregion
        }


    }
}
