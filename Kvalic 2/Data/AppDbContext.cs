using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kvalic_2.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Master> Masters { get; set; }

    public virtual DbSet<Masterservice> Masterservices { get; set; }

    public virtual DbSet<Qualificationrequest> Qualificationrequests { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Servicecollection> Servicecollections { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Kvalic2;Username=postgres;Password=12345");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Appointmentid).HasName("appointments_pkey");

            entity.ToTable("appointments");

            entity.Property(e => e.Appointmentid).HasColumnName("appointmentid");
            entity.Property(e => e.Appointmentdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("appointmentdate");
            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Queuenumber).HasColumnName("queuenumber");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Master).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Masterid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_app_master");

            entity.HasOne(d => d.Service).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Serviceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_app_service");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_app_user");
        });

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Collectionid).HasName("collections_pkey");

            entity.ToTable("collections");

            entity.HasIndex(e => e.Name, "collections_name_key").IsUnique();

            entity.Property(e => e.Collectionid).HasColumnName("collectionid");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Master>(entity =>
        {
            entity.HasKey(e => e.Masterid).HasName("masters_pkey");

            entity.ToTable("masters");

            entity.HasIndex(e => e.Userid, "masters_userid_key").IsUnique();

            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Qualificationlevel)
                .HasDefaultValue(1)
                .HasColumnName("qualificationlevel");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithOne(p => p.Master)
                .HasForeignKey<Master>(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_master_user");
        });

        modelBuilder.Entity<Masterservice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("masterservices_pkey");

            entity.ToTable("masterservices");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");

            entity.HasOne(d => d.Master).WithMany(p => p.Masterservices)
                .HasForeignKey(d => d.Masterid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ms_master");

            entity.HasOne(d => d.Service).WithMany(p => p.Masterservices)
                .HasForeignKey(d => d.Serviceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ms_service");
        });

        modelBuilder.Entity<Qualificationrequest>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("qualificationrequests_pkey");

            entity.ToTable("qualificationrequests");

            entity.Property(e => e.Requestid).HasColumnName("requestid");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'::character varying")
                .HasColumnName("status");

            entity.HasOne(d => d.Master).WithMany(p => p.Qualificationrequests)
                .HasForeignKey(d => d.Masterid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_qr_master");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("reviews_pkey");

            entity.ToTable("reviews");

            entity.Property(e => e.Reviewid).HasColumnName("reviewid");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Master).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Masterid)
                .HasConstraintName("fk_review_master");

            entity.HasOne(d => d.Service).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Serviceid)
                .HasConstraintName("fk_review_service");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_review_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Rolename, "roles_rolename_key").IsUnique();

            entity.Property(e => e.Roleid).HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Serviceid).HasName("services_pkey");

            entity.ToTable("services");

            entity.Property(e => e.Serviceid).HasColumnName("serviceid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Lastupdated)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdated");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");
        });

        modelBuilder.Entity<Servicecollection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("servicecollections_pkey");

            entity.ToTable("servicecollections");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Collectionid).HasColumnName("collectionid");
            entity.Property(e => e.Serviceid).HasColumnName("serviceid");

            entity.HasOne(d => d.Collection).WithMany(p => p.Servicecollections)
                .HasForeignKey(d => d.Collectionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sc_collection");

            entity.HasOne(d => d.Service).WithMany(p => p.Servicecollections)
                .HasForeignKey(d => d.Serviceid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sc_service");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Transactionid).HasName("transactions_pkey");

            entity.ToTable("transactions");

            entity.Property(e => e.Transactionid).HasColumnName("transactionid");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasColumnName("amount");
            entity.Property(e => e.Transactiondate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("transactiondate");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trans_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Fullname)
                .HasMaxLength(100)
                .HasColumnName("fullname");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Roleid).HasColumnName("roleid");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
