﻿// <auto-generated />
using System;
using Diploma.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Diploma.Persistence.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230328221537_AddChatUsers")]
    partial class AddChatUsers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Diploma.Persistence.Models.Attachment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ContentType")
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<string>("Folder")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("folder");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("attachments");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.Chat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.ToTable("chats");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.ChatUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ChatId")
                        .HasColumnType("uuid")
                        .HasColumnName("chat_id");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("chat_users");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.UserPrivateKey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("key");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_private_keys");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.UserPublicKey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("X")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("x");

                    b.Property<string>("Y")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("y");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("user_public_keys");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.Attachment", b =>
                {
                    b.HasOne("Diploma.Persistence.Models.User", "User")
                        .WithMany("Attachments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.ChatUser", b =>
                {
                    b.HasOne("Diploma.Persistence.Models.Chat", "Chat")
                        .WithMany("ChatUsers")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Diploma.Persistence.Models.User", "User")
                        .WithMany("ChatUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.UserPrivateKey", b =>
                {
                    b.HasOne("Diploma.Persistence.Models.User", "User")
                        .WithOne("PrivateKey")
                        .HasForeignKey("Diploma.Persistence.Models.UserPrivateKey", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.UserPublicKey", b =>
                {
                    b.HasOne("Diploma.Persistence.Models.User", "User")
                        .WithOne("PublicKey")
                        .HasForeignKey("Diploma.Persistence.Models.UserPublicKey", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.Chat", b =>
                {
                    b.Navigation("ChatUsers");
                });

            modelBuilder.Entity("Diploma.Persistence.Models.User", b =>
                {
                    b.Navigation("Attachments");

                    b.Navigation("ChatUsers");

                    b.Navigation("PrivateKey");

                    b.Navigation("PublicKey");
                });
#pragma warning restore 612, 618
        }
    }
}
