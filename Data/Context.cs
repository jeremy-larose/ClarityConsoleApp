﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ClarityMailLibrary;
using FluentEmail.Core;

namespace ClarityConsole.Data
{
    /// <summary>
    /// Entity Framework context class.
    /// </summary>
    public class Context : DbContext
    {
        public DbSet<ClarityMail> Emails { get; set; }

        public Context()
        {
            // This call to the SetInitializer method is used 
            // to configure EF to use our custom database initializer class
            // which contains our app's database seed data.
            Database.SetInitializer(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Removing the pluralizing table name convention 
            // so our table names will use our entity class singular names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}