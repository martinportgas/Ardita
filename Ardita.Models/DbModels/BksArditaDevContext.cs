using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ardita.Models.DbModels;

public partial class BksArditaDevContext : DbContext
{
    public BksArditaDevContext()
    {
    }

    public BksArditaDevContext(DbContextOptions<BksArditaDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MstCompany> MstCompanies { get; set; }

    public virtual DbSet<MstEmployee> MstEmployees { get; set; }

    public virtual DbSet<MstMenu> MstMenus { get; set; }

    public virtual DbSet<MstPage> MstPages { get; set; }

    public virtual DbSet<MstPosition> MstPositions { get; set; }

    public virtual DbSet<MstRole> MstRoles { get; set; }

    public virtual DbSet<MstRolePage> MstRolePages { get; set; }

    public virtual DbSet<MstSubmenu> MstSubmenus { get; set; }

    public virtual DbSet<MstUser> MstUsers { get; set; }

    public virtual DbSet<MstUserRole> MstUserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=115.124.75.185;database=BKS.ARDITA.DEV;uid=ardita;password=Ardita@2023;TrustServerCertificate=True;Integrated Security=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId);

            entity.ToTable("MST_COMPANY");

            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Address)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CompanyCode)
                .IsUnicode(false)
                .HasColumnName("company_code");
            entity.Property(e => e.CompanyName)
                .IsUnicode(false)
                .HasColumnName("company_name");
            entity.Property(e => e.Email)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Telepone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telepone");
        });

        modelBuilder.Entity<MstEmployee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK_EMPLOYEE");

            entity.ToTable("MST_EMPLOYEE");

            entity.HasIndex(e => e.Nik, "IX_EMPLOYEE").IsUnique();

            entity.Property(e => e.EmployeeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("employee_id");
            entity.Property(e => e.Address)
                .HasMaxLength(2500)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("date_of_birth");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeLevel)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("employee_level");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Nik)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nik");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PlaceOfBirth)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("place_of_birth");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("profile_picture");
            entity.Property(e => e.UnitArchiveId).HasColumnName("unit_archive_id");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.Position).WithMany(p => p.MstEmployees)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMPLOYEE_POSITION");
        });

        modelBuilder.Entity<MstMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK_MENU");

            entity.ToTable("MST_MENU");

            entity.Property(e => e.MenuId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("menu_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("icon");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("path");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<MstPage>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("PK_PAGE");

            entity.ToTable("MST_PAGE");

            entity.Property(e => e.PageId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("page_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("path");
            entity.Property(e => e.SubmenuId).HasColumnName("submenu_id");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.Submenu).WithMany(p => p.MstPages)
                .HasForeignKey(d => d.SubmenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PAGE_SUBMENU");
        });

        modelBuilder.Entity<MstPosition>(entity =>
        {
            entity.HasKey(e => e.PosittionId).HasName("PK_POSITION");

            entity.ToTable("MST_POSITION");

            entity.HasIndex(e => e.Code, "UQ_POSITION_CODE").IsUnique();

            entity.Property(e => e.PosittionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("posittion_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<MstRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_ROLE");

            entity.ToTable("MST_ROLE");

            entity.HasIndex(e => e.Code, "UQ_ROLE_CODE").IsUnique();

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("role_id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
        });

        modelBuilder.Entity<MstRolePage>(entity =>
        {
            entity.HasKey(e => e.RolePageId).HasName("PK_ROLE_PAGE");

            entity.ToTable("MST_ROLE_PAGE");

            entity.Property(e => e.RolePageId)
                .ValueGeneratedNever()
                .HasColumnName("role_page_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.Page).WithMany(p => p.MstRolePages)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROLE_PAGE_PAGE");

            entity.HasOne(d => d.Role).WithMany(p => p.MstRolePages)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROLE_PAGE_ROLE");
        });

        modelBuilder.Entity<MstSubmenu>(entity =>
        {
            entity.HasKey(e => e.SubmenuId).HasName("PK_SUBMENU");

            entity.ToTable("MST_SUBMENU");

            entity.Property(e => e.SubmenuId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("submenu_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Path)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("path");
            entity.Property(e => e.Sort).HasColumnName("sort");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");

            entity.HasOne(d => d.Menu).WithMany(p => p.MstSubmenus)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUBMENU_SUBMENU");
        });

        modelBuilder.Entity<MstUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_USERS");

            entity.ToTable("MST_USER");

            entity.HasIndex(e => e.Username, "IX_USERNAME").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LastLogin)
                .HasColumnType("datetime")
                .HasColumnName("last_login");
            entity.Property(e => e.LastLogout)
                .HasColumnType("datetime")
                .HasColumnName("last_logout");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PasswordLast)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password_last");
            entity.Property(e => e.PasswordLastChanged)
                .HasColumnType("datetime")
                .HasColumnName("password_last_changed");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.Employee).WithMany(p => p.MstUsers)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EMPLOYEEID");
        });

        modelBuilder.Entity<MstUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK_USER_ROLE");

            entity.ToTable("MST_USER_ROLE");

            entity.Property(e => e.UserRoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("user_role_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.MstUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE_ROLE");

            entity.HasOne(d => d.User).WithMany(p => p.MstUserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE_USERS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
