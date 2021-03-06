﻿// <auto-generated />
using System;
using DocKick.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DocKick.Data.Migrations
{
    [DbContext(typeof(CategorizableDbContext))]
    partial class CategorizableDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImageName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BlobId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Blobs");
                });

            modelBuilder.Entity("DocKick.Entities.Blobs.BlobLink", b =>
                {
                    b.Property<Guid>("BlobLinkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("ExpirationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BlobLinkId");

                    b.HasIndex("BlobId")
                        .IsUnique();

                    b.ToTable("BlobLinks");
                });

            modelBuilder.Entity("DocKick.Entities.Categories.Category", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid?>("ParentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CategoryId");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserId", "Name", "ParentId")
                        .IsUnique()
                        .HasFilter("[ParentId] IS NOT NULL");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DocKick.Entities.Blobs.Blob", b =>
                {
                    b.HasOne("DocKick.Entities.Categories.Category", "Category")
                        .WithMany("Blobs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DocKick.Entities.Blobs.BlobLink", b =>
                {
                    b.HasOne("DocKick.Entities.Blobs.Blob", "Blob")
                        .WithOne("BlobLink")
                        .HasForeignKey("DocKick.Entities.Blobs.BlobLink", "BlobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blob");
                });

            modelBuilder.Entity("DocKick.Entities.Categories.Category", b =>
                {
                    b.HasOne("DocKick.Entities.Categories.Category", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("DocKick.Entities.Blobs.Blob", b =>
                {
                    b.Navigation("BlobLink");
                });

            modelBuilder.Entity("DocKick.Entities.Categories.Category", b =>
                {
                    b.Navigation("Blobs");

                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
