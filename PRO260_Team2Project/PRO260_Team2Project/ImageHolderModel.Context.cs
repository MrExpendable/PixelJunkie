﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PRO260_Team2Project
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ImageHolderContext : DbContext
    {
        public ImageHolderContext()
            : base("name=ImageHolderContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Flag> Flags { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageOwner> ImageOwners { get; set; }
        public DbSet<ImageTag> ImageTags { get; set; }
        public DbSet<Member> Members { get; set; }
    }
}
