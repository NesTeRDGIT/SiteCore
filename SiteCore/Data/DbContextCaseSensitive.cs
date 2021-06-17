using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SiteCore.Data
{
    public static class DbContextCaseSensitive
    {
        /// <summary>
        /// Set table's name to Uppercase
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ToUpperCaseTables(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetTableName().ToUpper());
            }
        }

        /// <summary>
        /// Set column's name to Uppercase 
        /// </summary>
        /// <param name="modelBuilder"></param>
       
        public static void ToUpperCaseColumns(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().ToUpper());
                }
            }
        }

        /// <summary>
        /// Set foreignkey's name to Uppercase
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ToUpperCaseForeignKeys(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    foreach (var fk in entityType.FindForeignKeys(property))
                    {
                        fk.SetConstraintName(fk.GetConstraintName().ToUpper());
                    }
                }
            }
        }

        /// <summary>
        /// Set index's name to Uppercase
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ToUpperCaseIndexes(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var index in entityType.GetIndexes())
                {
                    index.SetName(index.GetName().ToUpper());
                }
            }
        }
    }
}
