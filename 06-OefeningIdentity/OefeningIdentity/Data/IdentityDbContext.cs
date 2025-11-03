using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OefeningIdentity.Models;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;

namespace OefeningIdentity.Data
{
    public class IdentityDbContext : IdentityDbContext<CustomUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}