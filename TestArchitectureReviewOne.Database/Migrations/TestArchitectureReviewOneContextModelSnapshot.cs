﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestArchitectureReviewOne.Database;

#nullable disable

namespace TestArchitectureReviewOne.Database.Migrations
{
    [DbContext(typeof(TestArchitectureReviewOneContext))]
    partial class TestArchitectureReviewOneContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("TestArchitectureReviewOne.Domain.Entities.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("AlteradoEm")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("Ativo")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("CriadoEm")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasComment("E-mail do usúario");

                    b.Property<bool>("ExigirNovaSenha")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasComment("Nome do usúario");

                    b.Property<string>("Senha")
                        .HasColumnType("longtext")
                        .HasComment("Senha do usúario");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasComment("Telefone do usúario");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("UnqUsuarioEmail");

                    b.ToTable("Usuario", (string)null);

                    b.HasComment("Tabela reposável por organizar os usuários");

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8");
                });
#pragma warning restore 612, 618
        }
    }
}
