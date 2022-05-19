﻿// <auto-generated />
using EDHWebApp.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EDHWebApp.Migrations
{
    [DbContext(typeof(EDHContext))]
    [Migration("20220502094744_NewTablesMigration")]
    partial class NewTablesMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-preview.3.22175.1");

            modelBuilder.Entity("EDHWebApp.Model.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies", (string)null);
                });

            modelBuilder.Entity("EDHWebApp.Model.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MyCompanyCompanyId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("VerifiedUser")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId");

                    b.HasIndex("MyCompanyCompanyId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("EDHWebApp.Model.Admin", b =>
                {
                    b.HasBaseType("EDHWebApp.Model.User");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("Admins", (string)null);
                });

            modelBuilder.Entity("EDHWebApp.Model.RegisteredUser", b =>
                {
                    b.HasBaseType("EDHWebApp.Model.User");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.ToTable("RegisteredUsers", (string)null);
                });

            modelBuilder.Entity("EDHWebApp.Model.User", b =>
                {
                    b.HasOne("EDHWebApp.Model.Company", "MyCompany")
                        .WithMany()
                        .HasForeignKey("MyCompanyCompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MyCompany");
                });

            modelBuilder.Entity("EDHWebApp.Model.Admin", b =>
                {
                    b.HasOne("EDHWebApp.Model.User", null)
                        .WithOne()
                        .HasForeignKey("EDHWebApp.Model.Admin", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EDHWebApp.Model.RegisteredUser", b =>
                {
                    b.HasOne("EDHWebApp.Model.User", null)
                        .WithOne()
                        .HasForeignKey("EDHWebApp.Model.RegisteredUser", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
