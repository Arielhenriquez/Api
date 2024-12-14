﻿// <auto-generated />
using System;
using Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241214212941_AddJsonFieldsToCollaboratorAndInventoryRequest")]
    partial class AddJsonFieldsToCollaboratorAndInventoryRequest
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Api.Domain.Entities.Collaborator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Approvers")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Department")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Roles")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<string>("Supervisor")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("UserOid")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AcquisitionObjectAccount")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("InstitutionalCode")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("RequestedQuantity")
                        .HasColumnType("int");

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UnitOfMeasure")
                        .HasColumnType("longtext");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<decimal?>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("WarehouseObjectAccount")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApprovalHistory")
                        .IsRequired()
                        .HasColumnType("json");

                    b.Property<Guid>("CollaboratorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("PendingApprovalBy")
                        .HasColumnType("int");

                    b.Property<string>("RequestCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StatusChangedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("CollaboratorId");

                    b.ToTable("InventoryRequests");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryRequestItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid>("InventoryItemId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("InventoryRequestId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("InventoryItemId");

                    b.HasIndex("InventoryRequestId");

                    b.ToTable("InventoryRequestItem");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransportEntities.Driver", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("LicenseExpiration")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransportEntities.TransportRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApprovedOrRejectedBy")
                        .HasColumnType("longtext");

                    b.Property<Guid>("CollaboratorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DepartureDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DeparturePoint")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("DriverId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("NumberOfPeople")
                        .HasColumnType("int");

                    b.Property<int?>("PendingApprovalBy")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("RequestCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("RequestStatus")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StatusChangedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.Property<Guid?>("VehicleId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CollaboratorId");

                    b.HasIndex("DriverId");

                    b.HasIndex("VehicleId");

                    b.ToTable("TransportRequests");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransportEntities.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("DeletedDate")
                        .HasColumnType("datetime");

                    b.Property<string>("InsuranceType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("InsuranceValidity")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("longtext");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset?>("UpdatedDate")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryRequest", b =>
                {
                    b.HasOne("Api.Domain.Entities.Collaborator", "Collaborator")
                        .WithMany("InventoryRequest")
                        .HasForeignKey("CollaboratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collaborator");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryRequestItem", b =>
                {
                    b.HasOne("Api.Domain.Entities.InventoryEntities.InventoryItem", "InventoryItem")
                        .WithMany("InventoryRequestItems")
                        .HasForeignKey("InventoryItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Domain.Entities.InventoryEntities.InventoryRequest", "InventoryRequest")
                        .WithMany("InventoryRequestItems")
                        .HasForeignKey("InventoryRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("InventoryItem");

                    b.Navigation("InventoryRequest");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransportEntities.TransportRequest", b =>
                {
                    b.HasOne("Api.Domain.Entities.Collaborator", "Collaborator")
                        .WithMany("TransportRequests")
                        .HasForeignKey("CollaboratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Domain.Entities.TransportEntities.Driver", "Driver")
                        .WithMany("TransportRequests")
                        .HasForeignKey("DriverId");

                    b.HasOne("Api.Domain.Entities.TransportEntities.Vehicle", "Vehicle")
                        .WithMany("TransportRequests")
                        .HasForeignKey("VehicleId");

                    b.Navigation("Collaborator");

                    b.Navigation("Driver");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Api.Domain.Entities.Collaborator", b =>
                {
                    b.Navigation("InventoryRequest");

                    b.Navigation("TransportRequests");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryItem", b =>
                {
                    b.Navigation("InventoryRequestItems");
                });

            modelBuilder.Entity("Api.Domain.Entities.InventoryEntities.InventoryRequest", b =>
                {
                    b.Navigation("InventoryRequestItems");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransportEntities.Driver", b =>
                {
                    b.Navigation("TransportRequests");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransportEntities.Vehicle", b =>
                {
                    b.Navigation("TransportRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
