﻿// <auto-generated />
using System;
using DocKick.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocKick.Data.Migrations
{
    [DbContext(typeof(CategorizableDbContext))]
    [Migration("20210211204811_RemoveBlobContainer")]
    partial class RemoveBlobContainer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("DocKick.Entities.Blobs.Blob", b =>
                {
                    b.Property<Guid>("BlobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BlobId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Blobs");
                });

            modelBuilder.Entity("DocKick.Entities.Categories.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentId")
                        .IsUnique()
                        .HasFilter("[ParentId] IS NOT NULL");

                    b.HasIndex("UserId", "Name", "ParentId")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL AND [ParentId] IS NOT NULL");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DocKick.Entities.Blobs.Blob", b =>
                {
                    b.HasOne("DocKick.Entities.Categories.Category", "Category")
                        .WithMany("Blobs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DocKick.Entities.Categories.Category", b =>
                {
                    b.HasOne("DocKick.Entities.Categories.Category", "Parent")
                        .WithOne("Child")
                        .HasForeignKey("DocKick.Entities.Categories.Category", "ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("DocKick.Entities.Categories.Category", b =>
                {
                    b.Navigation("Blobs");

                    b.Navigation("Child");
                });
#pragma warning restore 612, 618
        }
    }
}
