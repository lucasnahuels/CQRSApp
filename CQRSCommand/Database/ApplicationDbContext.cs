﻿using CQRSCommand.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRSCommand.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}
