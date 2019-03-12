using Microsoft.EntityFrameworkCore;
using ProjectManager.Model;
using System;

namespace ProjectManager.DataAccesslayer
{
    public class ProjectManagerDbContext : DbContext
    {
        public ProjectManagerDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public virtual DbSet<TaskDetail> Tasks { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(false);
            optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseSqlServer(@"Server = DOTNET; Database = ProjectManagerDb; Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildTaskDetailTable(modelBuilder);
            BuildProjectTable(modelBuilder);
            BuildUserTable(modelBuilder);
        }

        private static void BuildTaskDetailTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskDetail>().HasKey("Id");
            modelBuilder.Entity<TaskDetail>().ToTable("Task");
            modelBuilder.Entity<TaskDetail>().Property(t => t.Name).HasColumnName("Task").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<TaskDetail>().Property(t => t.StartDate).HasColumnName("Start_Date");
            modelBuilder.Entity<TaskDetail>().Property(t => t.EndDate).HasColumnName("End_Date");
            modelBuilder.Entity<TaskDetail>().Property(t => t.ParentId).HasColumnName("ParentId");
            modelBuilder.Entity<TaskDetail>().Property(t => t.Priority).IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.ActiveStatus).HasColumnName("Status").IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.Id).ValueGeneratedOnAdd().HasColumnName("Task_Id").IsRequired();
            modelBuilder.Entity<TaskDetail>().Property(t => t.UserId).HasColumnName("User_Id");
            modelBuilder.Entity<TaskDetail>().Property(t => t.ProjectId).HasColumnName("Project_Id");
            modelBuilder.Entity<TaskDetail>().HasOne(t => t.UserDetail).WithMany(u => u.TaskDetails).HasForeignKey(t => t.UserId);
            modelBuilder.Entity<TaskDetail>().HasOne(t => t.ProjectDetail).WithMany(u => u.TaskDetails).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Restrict);
        }

        private static void BuildProjectTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().HasKey("ProjectId");
            modelBuilder.Entity<Project>().ToTable("Project");
            modelBuilder.Entity<Project>().Property(t => t.ProjectName).HasColumnName("Project").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Project>().Property(t => t.StartDate).HasColumnName("Start_Date");
            modelBuilder.Entity<Project>().Property(t => t.EndDate).HasColumnName("End_Date");          
            modelBuilder.Entity<Project>().Property(t => t.Priority).IsRequired();
            modelBuilder.Entity<Project>().Property(t => t.ActiveStatus).HasColumnName("Status").IsRequired();
            modelBuilder.Entity<Project>().Property(t => t.ProjectId).ValueGeneratedOnAdd().HasColumnName("Project_Id").IsRequired();          
            modelBuilder.Entity<Project>().Property(t => t.UserId).HasColumnName("User_Id");
            modelBuilder.Entity<Project>().HasOne(t => t.UserDetail).WithMany(u => u.Projects).HasForeignKey(t => t.UserId);
        }

        private static void BuildUserTable(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey("UserId");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>().Property(t => t.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(100);
            modelBuilder.Entity<User>().Property(t => t.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(100); ;
            modelBuilder.Entity<User>().Property(t => t.EmployeeId).HasColumnName("Employee_Id").IsRequired();
            modelBuilder.Entity<User>().Property(t => t.UserId).ValueGeneratedOnAdd().HasColumnName("User_Id").IsRequired();
        }
    }
}
