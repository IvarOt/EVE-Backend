﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eve_backend.data;

#nullable disable

namespace eve_backend.api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241008130415_initial-excel-file-structure")]
    partial class initialexcelfilestructure
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("eve_backend.logic.Models.ExcelFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ExcelFiles");
                });

            modelBuilder.Entity("eve_backend.logic.Models.ExcelObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("ExcelFileId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ExcelFileId");

                    b.ToTable("ExcelObjects");
                });

            modelBuilder.Entity("eve_backend.logic.Models.ExcelObject", b =>
                {
                    b.HasOne("eve_backend.logic.Models.ExcelFile", null)
                        .WithMany("excelObjects")
                        .HasForeignKey("ExcelFileId");
                });

            modelBuilder.Entity("eve_backend.logic.Models.ExcelFile", b =>
                {
                    b.Navigation("excelObjects");
                });
#pragma warning restore 612, 618
        }
    }
}