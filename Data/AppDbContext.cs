using HCMApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HCMApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Leave_Days> Leave_Days { get; set; }
        public DbSet<Absence_Request> Absence_Requests { get; set; }
        public DbSet<Absence_Reason> Absence_Reasons { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<WorkMonth> WorkMonths { get; set; }
        public DbSet<SalaryHistory> SalaryHistories { get; set; }
        public DbSet<LeftEmployee> LeftEmployees { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
    }
}
