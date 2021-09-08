﻿// <auto-generated />
using System;
using Inflow.Services.Users.Core.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Inflow.Services.Users.Core.DAL.Migrations
{
    [DbContext(typeof(UsersDbContext))]
    [Migration("20210908193833_Users_Init")]
    partial class Users_Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Convey.MessageBrokers.Outbox.Messages.InboxMessage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("ProcessedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Inbox");
                });

            modelBuilder.Entity("Convey.MessageBrokers.Outbox.Messages.OutboxMessage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CorrelationId")
                        .HasColumnType("text");

                    b.Property<string>("Headers")
                        .HasColumnType("text");

                    b.Property<string>("MessageContextType")
                        .HasColumnType("text");

                    b.Property<string>("MessageType")
                        .HasColumnType("text");

                    b.Property<string>("OriginatedMessageId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("SerializedMessage")
                        .HasColumnType("text");

                    b.Property<string>("SerializedMessageContext")
                        .HasColumnType("text");

                    b.Property<string>("SpanContext")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Outbox");
                });

            modelBuilder.Entity("Inflow.Services.Users.Core.Entities.Role", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Permissions")
                        .HasColumnType("text");

                    b.HasKey("Name");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Inflow.Services.Users.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("RoleId")
                        .HasColumnType("character varying(100)");

                    b.Property<int>("State")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Inflow.Services.Users.Core.Entities.User", b =>
                {
                    b.HasOne("Inflow.Services.Users.Core.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Inflow.Services.Users.Core.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
