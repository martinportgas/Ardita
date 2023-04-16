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

    public virtual DbSet<IdxRolePage> IdxRolePages { get; set; }

    public virtual DbSet<IdxUserRole> IdxUserRoles { get; set; }

    public virtual DbSet<MstCompany> MstCompanies { get; set; }

    public virtual DbSet<MstCompanyLog> MstCompanyLogs { get; set; }

    public virtual DbSet<MstCreator> MstCreators { get; set; }

    public virtual DbSet<MstCreatorLog> MstCreatorLogs { get; set; }

    public virtual DbSet<MstEmployee> MstEmployees { get; set; }

    public virtual DbSet<MstGmd> MstGmds { get; set; }

    public virtual DbSet<MstGmdLog> MstGmdLogs { get; set; }

    public virtual DbSet<MstMenu> MstMenus { get; set; }

    public virtual DbSet<MstPage> MstPages { get; set; }

    public virtual DbSet<MstPageDetail> MstPageDetails { get; set; }

    public virtual DbSet<MstPosition> MstPositions { get; set; }

    public virtual DbSet<MstRole> MstRoles { get; set; }

    public virtual DbSet<MstSecurityClassification> MstSecurityClassifications { get; set; }

    public virtual DbSet<MstSecurityClassificationLog> MstSecurityClassificationLogs { get; set; }

    public virtual DbSet<MstSecurityClassificationLog> MstSecurityClassificationLogs { get; set; }

    public virtual DbSet<MstSubmenu> MstSubmenus { get; set; }

    public virtual DbSet<MstTypeClassification> MstTypeClassifications { get; set; }

    public virtual DbSet<MstTypeClassificationLog> MstTypeClassificationLogs { get; set; }

    public virtual DbSet<MstUser> MstUsers { get; set; }

    public virtual DbSet<TrxArchive> TrxArchives { get; set; }

    public virtual DbSet<TrxArchiveMovement> TrxArchiveMovements { get; set; }

    public virtual DbSet<TrxArchiveUnit> TrxArchiveUnits { get; set; }

    public virtual DbSet<TrxClassification> TrxClassifications { get; set; }

    public virtual DbSet<TrxFileArchiveDetail> TrxFileArchiveDetails { get; set; }

    public virtual DbSet<TrxFloor> TrxFloors { get; set; }

    public virtual DbSet<TrxLevel> TrxLevels { get; set; }

    public virtual DbSet<TrxMediaStorage> TrxMediaStorages { get; set; }

    public virtual DbSet<TrxMediaStorageDetail> TrxMediaStorageDetails { get; set; }

    public virtual DbSet<TrxPermissionClassification> TrxPermissionClassifications { get; set; }

    public virtual DbSet<TrxRack> TrxRacks { get; set; }

    public virtual DbSet<TrxRoom> TrxRooms { get; set; }

    public virtual DbSet<TrxRow> TrxRows { get; set; }

    public virtual DbSet<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; set; }

    public virtual DbSet<TrxSubjectClassification> TrxSubjectClassifications { get; set; }

    public virtual DbSet<TrxTypeStorage> TrxTypeStorages { get; set; }

    public virtual DbSet<TrxArchiveMovement> TrxArchiveMovements { get; set; }

    public virtual DbSet<TrxArchiveUnit> TrxArchiveUnits { get; set; }

    public virtual DbSet<TrxClassification> TrxClassifications { get; set; }

    public virtual DbSet<TrxFileArchiveDetail> TrxFileArchiveDetails { get; set; }

    public virtual DbSet<TrxFloor> TrxFloors { get; set; }

    public virtual DbSet<TrxLevel> TrxLevels { get; set; }

    public virtual DbSet<TrxMediaStorage> TrxMediaStorages { get; set; }

    public virtual DbSet<TrxMediaStorageDetail> TrxMediaStorageDetails { get; set; }

    public virtual DbSet<TrxPermissionClassification> TrxPermissionClassifications { get; set; }

    public virtual DbSet<TrxRack> TrxRacks { get; set; }

    public virtual DbSet<TrxRoom> TrxRooms { get; set; }

    public virtual DbSet<TrxRow> TrxRows { get; set; }

    public virtual DbSet<TrxSubSubjectClassification> TrxSubSubjectClassifications { get; set; }

    public virtual DbSet<TrxSubjectClassification> TrxSubjectClassifications { get; set; }

    public virtual DbSet<TrxTypeStorage> TrxTypeStorages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=115.124.75.185;database=BKS.ARDITA.DEV;uid=ardita;password=Ardita@2023;TrustServerCertificate=True;Integrated Security=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdxRolePage>(entity =>
        {
            entity.HasKey(e => e.RolePageId).HasName("PK_ROLE_PAGE");

            entity.ToTable("IDX_ROLE_PAGE");

            entity.Property(e => e.RolePageId)
                .HasDefaultValueSql("(newid())")
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

            entity.HasOne(d => d.Page).WithMany(p => p.IdxRolePages)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROLE_PAGE_PAGE");

            entity.HasOne(d => d.Role).WithMany(p => p.IdxRolePages)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ROLE_PAGE_ROLE");
        });

        modelBuilder.Entity<IdxUserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK_USER_ROLE");

            entity.ToTable("IDX_USER_ROLE");

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

            entity.HasOne(d => d.Role).WithMany(p => p.IdxUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE_ROLE");

            entity.HasOne(d => d.User).WithMany(p => p.IdxUserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_USER_ROLE_USERS");
        });

        modelBuilder.Entity<MstCompany>(entity =>
        {
            entity.HasKey(e => e.CompanyId);

            entity.ToTable("MST_COMPANY");

            entity.Property(e => e.CompanyId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("company_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CompanyCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("company_code");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("company_name");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Telepone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telepone");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstCompanyLog>(entity =>
        {
            entity.HasKey(e => e.CompanyIdLog);

            entity.ToTable("MST_COMPANY_LOG");

            entity.Property(e => e.CompanyIdLog)
                .ValueGeneratedNever()
                .HasColumnName("company_id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CompanyCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("company_code");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("company_name");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Telepone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telepone");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Company).WithMany(p => p.MstCompanyLogs)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_COMPANY_ID_COMPANY_LOG");
        });

        modelBuilder.Entity<MstCreator>(entity =>
        {
            entity.HasKey(e => e.CreatorId);

            entity.ToTable("MST_CREATOR");

            entity.Property(e => e.CreatorId)
                .ValueGeneratedNever()
                .HasColumnName("creator_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatorCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("creator_code");
            entity.Property(e => e.CreatorName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("creator_name");
            entity.Property(e => e.CreatorType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("creator_type");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstCreatorLog>(entity =>
        {
            entity.HasKey(e => e.CreatorIdLog);

            entity.ToTable("MST_CREATOR_LOG");

            entity.Property(e => e.CreatorIdLog)
                .ValueGeneratedNever()
                .HasColumnName("creator_id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatorCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("creator_code");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.CreatorName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("creator_name");
            entity.Property(e => e.CreatorType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("creator_type");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Creator).WithMany(p => p.MstCreatorLogs)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CREATOR_ID_CREATOR_LOG");
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
                .HasConstraintName("FK_POSITION_ID_MST_EMPLOYEE");
        });

        modelBuilder.Entity<MstGmd>(entity =>
        {
            entity.HasKey(e => e.GmdId);

            entity.ToTable("MST_GMD");

            entity.Property(e => e.GmdId)
                .ValueGeneratedNever()
                .HasColumnName("gmd_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GmdCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gmd_code");
            entity.Property(e => e.GmdName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("gmd_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstGmdLog>(entity =>
        {
            entity.HasKey(e => e.GmdIdLog);

            entity.ToTable("MST_GMD_LOG");

            entity.Property(e => e.GmdIdLog)
                .ValueGeneratedNever()
                .HasColumnName("gmd_id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GmdCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gmd_code");
            entity.Property(e => e.GmdId).HasColumnName("gmd_id");
            entity.Property(e => e.GmdName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("gmd_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Gmd).WithMany(p => p.MstGmdLogs)
                .HasForeignKey(d => d.GmdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMD_ID_GMD_LOG");
        });

        modelBuilder.Entity<MstGmd>(entity =>
        {
            entity.HasKey(e => e.GmdId);

            entity.ToTable("MST_GMD");

            entity.Property(e => e.GmdId)
                .ValueGeneratedNever()
                .HasColumnName("gmd_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GmdCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gmd_code");
            entity.Property(e => e.GmdName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("gmd_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstGmdLog>(entity =>
        {
            entity.HasKey(e => e.GmdIdLog);

            entity.ToTable("MST_GMD_LOG");

            entity.Property(e => e.GmdIdLog)
                .ValueGeneratedNever()
                .HasColumnName("gmd_id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.GmdCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gmd_code");
            entity.Property(e => e.GmdId).HasColumnName("gmd_id");
            entity.Property(e => e.GmdName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("gmd_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Gmd).WithMany(p => p.MstGmdLogs)
                .HasForeignKey(d => d.GmdId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMD_ID_GMD_LOG");
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
            entity.Property(e => e.Sort).HasColumnName("sort");
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

        modelBuilder.Entity<MstPageDetail>(entity =>
        {
            entity.HasKey(e => e.PageDetailId);

            entity.ToTable("MST_PAGE_DETAIL");

            entity.Property(e => e.PageDetailId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("page_detail_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PageId).HasColumnName("page_id");
            entity.Property(e => e.Path)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("path");

            entity.HasOne(d => d.Page).WithMany(p => p.MstPageDetails)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MST_PAGE_DETAIL_MST_PAGE");
        });

        modelBuilder.Entity<MstPosition>(entity =>
        {
            entity.HasKey(e => e.PositionId).HasName("PK_POSITION_N");

            entity.ToTable("MST_POSITION");

            entity.HasIndex(e => e.Code, "UQ_POSITION_CODE_N").IsUnique();

            entity.Property(e => e.PositionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("position_id");
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

        modelBuilder.Entity<MstSecurityClassification>(entity =>
        {
            entity.HasKey(e => e.SecurityClassificationId).HasName("PK_MST_SECURITY_CLASIFICATION");

            entity.ToTable("MST_SECURITY_CLASSIFICATION");

            entity.Property(e => e.SecurityClassificationId)
                .ValueGeneratedNever()
                .HasColumnName("security_classification_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.SecurityClassificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("security_classification_code");
            entity.Property(e => e.SecurityClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("security_classification_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstSecurityClassificationLog>(entity =>
        {
            entity.HasKey(e => e.SecurityClassificationIdLog).HasName("PK_MST_SECURITY_CLASIFICATION_LOG");

            entity.ToTable("MST_SECURITY_CLASSIFICATION_LOG");

            entity.Property(e => e.SecurityClassificationIdLog)
                .ValueGeneratedNever()
                .HasColumnName("security_classification_id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.SecurityClassificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("security_classification_code");
            entity.Property(e => e.SecurityClassificationId).HasColumnName("security_classification_id");
            entity.Property(e => e.SecurityClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("security_classification_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.SecurityClassification).WithMany(p => p.MstSecurityClassificationLogs)
                .HasForeignKey(d => d.SecurityClassificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SECURITY_CLASSIFICATION_ID_SECURITY_CLASSIFICATION_LOG");
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

        modelBuilder.Entity<MstTypeClassification>(entity =>
        {
            entity.HasKey(e => e.TypeClassificationId);

            entity.ToTable("MST_TYPE_CLASSIFICATION");

            entity.Property(e => e.TypeClassificationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("type_classification_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.TypeClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("type_classification_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
        });

        modelBuilder.Entity<MstTypeClassificationLog>(entity =>
        {
            entity.HasKey(e => e.TypeClassificationIdLog);

            entity.ToTable("MST_TYPE_CLASSIFICATION_LOG");

            entity.Property(e => e.TypeClassificationIdLog)
                .ValueGeneratedNever()
                .HasColumnName("type_classification_id_log");
            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("action");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.TypeClassificationId).HasColumnName("type_classification_id");
            entity.Property(e => e.TypeClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("type_classification_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.TypeClassification).WithMany(p => p.MstTypeClassificationLogs)
                .HasForeignKey(d => d.TypeClassificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TYPE_CLASSIFICATION_ID_TYPE_CLASSIFICATION_LOG");
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

        modelBuilder.Entity<TrxArchive>(entity =>
        {
            entity.HasKey(e => e.ArchiveId);

            entity.ToTable("TRX_ARCHIVE");

            entity.Property(e => e.ArchiveId)
                .ValueGeneratedNever()
                .HasColumnName("archive_id");
            entity.Property(e => e.ActiveRetention).HasColumnName("active_retention");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatedDateArchive)
                .HasColumnType("datetime")
                .HasColumnName("created_date_archive");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.GmdId).HasColumnName("gmd_id");
            entity.Property(e => e.InactiveRetention).HasColumnName("inactive_retention");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Keyword)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("keyword");
            entity.Property(e => e.SecurityClassificationId).HasColumnName("security_classification_id");
            entity.Property(e => e.SubSubjectClassificationId).HasColumnName("sub_subject_classification_id");
            entity.Property(e => e.TitleArchive)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("title_archive");
            entity.Property(e => e.TypeArchive)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type_archive");
            entity.Property(e => e.TypeSender)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("type_sender");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");
            entity.Property(e => e.Volume).HasColumnName("volume");

            entity.HasOne(d => d.Creator).WithMany(p => p.TrxArchives)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_CREATOR_ID");

            entity.HasOne(d => d.Gmd).WithMany(p => p.TrxArchives)
                .HasForeignKey(d => d.GmdId)
                .HasConstraintName("FK_MST_GMD_GMD_ID");

            entity.HasOne(d => d.SecurityClassification).WithMany(p => p.TrxArchives)
                .HasForeignKey(d => d.SecurityClassificationId)
                .HasConstraintName("FK_SECURITY_CLASSIFICATION_ID");

            entity.HasOne(d => d.SubSubjectClassification).WithMany(p => p.TrxArchives)
                .HasForeignKey(d => d.SubSubjectClassificationId)
                .HasConstraintName("FK_SUB_SUBJECT_CLASSIFICATION_ID");
        });

        modelBuilder.Entity<TrxArchiveMovement>(entity =>
        {
            entity.HasKey(e => e.ArchiveMovementId);

            entity.ToTable("TRX_ARCHIVE_MOVEMENT");

            entity.Property(e => e.ArchiveMovementId)
                .ValueGeneratedNever()
                .HasColumnName("archive_movement_id");
            entity.Property(e => e.ArchiveId).HasColumnName("archive_id");
            entity.Property(e => e.ArchiveUnitId).HasColumnName("archive_unit_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.DateReceived)
                .HasColumnType("datetime")
                .HasColumnName("date_received");
            entity.Property(e => e.DateSchedule)
                .HasColumnType("datetime")
                .HasColumnName("date_schedule");
            entity.Property(e => e.DateSend)
                .HasColumnType("datetime")
                .HasColumnName("date_send");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Archive).WithMany(p => p.TrxArchiveMovements)
                .HasForeignKey(d => d.ArchiveId)
                .HasConstraintName("FK_ARCHIVE_ID");

            entity.HasOne(d => d.ArchiveUnit).WithMany(p => p.TrxArchiveMovements)
                .HasForeignKey(d => d.ArchiveUnitId)
                .HasConstraintName("FK_ARCHIVE_UNIT_ID");
        });

        modelBuilder.Entity<TrxArchiveUnit>(entity =>
        {
            entity.HasKey(e => e.ArchiveUnitId);

            entity.ToTable("TRX_ARCHIVE_UNIT");

            entity.Property(e => e.ArchiveUnitId)
                .ValueGeneratedNever()
                .HasColumnName("archive_unit_id");
            entity.Property(e => e.ArchiveUnitAddress)
                .HasMaxLength(2500)
                .IsUnicode(false)
                .HasColumnName("archive_unit_address");
            entity.Property(e => e.ArchiveUnitCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("archive_unit_code");
            entity.Property(e => e.ArchiveUnitEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("archive_unit_email");
            entity.Property(e => e.ArchiveUnitName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("archive_unit_name");
            entity.Property(e => e.ArchiveUnitPhone)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("archive_unit_phone");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Company).WithMany(p => p.TrxArchiveUnits)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_COMPANY_ID");
        });

        modelBuilder.Entity<TrxClassification>(entity =>
        {
            entity.HasKey(e => e.ClassificationId);

            entity.ToTable("TRX_CLASSIFICATION");

            entity.Property(e => e.ClassificationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("classification_id");
            entity.Property(e => e.ClassificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("classification_code");
            entity.Property(e => e.ClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("classification_name");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.TypeClassificationId).HasColumnName("type_classification_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.TypeClassification).WithMany(p => p.TrxClassifications)
                .HasForeignKey(d => d.TypeClassificationId)
                .HasConstraintName("FK_TYPE_CLASSIFICATION_ID");
        });

        modelBuilder.Entity<TrxFileArchiveDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("TRX_FILE_ARCHIVE_DETAIL");

            entity.Property(e => e.ArchiveId).HasColumnName("archive_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.FileArchiveDetailId).HasColumnName("file_archive_detail_id");
            entity.Property(e => e.FileName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("file_name");
            entity.Property(e => e.FilePath)
                .HasMaxLength(2500)
                .IsUnicode(false)
                .HasColumnName("file_path");
            entity.Property(e => e.FileType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("file_type");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Archive).WithMany()
                .HasForeignKey(d => d.ArchiveId)
                .HasConstraintName("FK_ARCHIVE_ID_ARCHIVE_DETAIL");
        });

        modelBuilder.Entity<TrxFloor>(entity =>
        {
            entity.HasKey(e => e.FloorId);

            entity.ToTable("TRX_FLOOR");

            entity.Property(e => e.FloorId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("floor_id");
            entity.Property(e => e.ArchiveUnitId).HasColumnName("archive_unit_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.FloorCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("floor_code");
            entity.Property(e => e.FloorName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("floor_name");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.ArchiveUnit).WithMany(p => p.TrxFloors)
                .HasForeignKey(d => d.ArchiveUnitId)
                .HasConstraintName("FK_ARCHIVE_UNIT_ID_FLOOR");
        });

        modelBuilder.Entity<TrxLevel>(entity =>
        {
            entity.HasKey(e => e.LevelId);

            entity.ToTable("TRX_LEVEL");

            entity.Property(e => e.LevelId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("level_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LevelCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("level_code");
            entity.Property(e => e.LevelName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("level_name");
            entity.Property(e => e.RackId).HasColumnName("rack_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Rack).WithMany(p => p.TrxLevels)
                .HasForeignKey(d => d.RackId)
                .HasConstraintName("FK_RACK_ID_LEVEL");
        });

        modelBuilder.Entity<TrxMediaStorage>(entity =>
        {
            entity.HasKey(e => e.MediaStorageId);

            entity.ToTable("TRX_MEDIA_STORAGE");

            entity.Property(e => e.MediaStorageId)
                .ValueGeneratedNever()
                .HasColumnName("media_storage_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.DifferenceVolume).HasColumnName("difference_volume");
            entity.Property(e => e.GmdId).HasColumnName("gmd_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LevelId).HasColumnName("level_id");
            entity.Property(e => e.MediaStorageCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("media_storage_code");
            entity.Property(e => e.MediaStorageName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("media_storage_name");
            entity.Property(e => e.RackId).HasColumnName("rack_id");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.RowId).HasColumnName("row_id");
            entity.Property(e => e.TotalVolume).HasColumnName("total_volume");
            entity.Property(e => e.TypeStorageId).HasColumnName("type_storage_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Creator).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_CREATOR_ID_MEDIA_STORAGE");

            entity.HasOne(d => d.Gmd).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.GmdId)
                .HasConstraintName("FK_GMD_ID_MEDIA_STORAGE");

            entity.HasOne(d => d.Level).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.LevelId)
                .HasConstraintName("FK_LEVEL_ID_MEDIA_STORAGE");

            entity.HasOne(d => d.Rack).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.RackId)
                .HasConstraintName("FK_RACK_ID_MEDIA_STORAGE");

            entity.HasOne(d => d.Room).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_ROOM_ID_MEDIA_STORAGE");

            entity.HasOne(d => d.Row).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.RowId)
                .HasConstraintName("FK_ROW_ID_MEDIA_STORAGE");

            entity.HasOne(d => d.TypeStorage).WithMany(p => p.TrxMediaStorages)
                .HasForeignKey(d => d.TypeStorageId)
                .HasConstraintName("FK_TYPE_STORAGE_ID_MEDIA_STORAGE");
        });

        modelBuilder.Entity<TrxMediaStorageDetail>(entity =>
        {
            entity.HasKey(e => e.MediaStorageDetailId);

            entity.ToTable("TRX_MEDIA_STORAGE_DETAIL");

            entity.Property(e => e.MediaStorageDetailId)
                .ValueGeneratedNever()
                .HasColumnName("media_storage_detail_id");
            entity.Property(e => e.ArchiveId).HasColumnName("archive_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.MediaStorageId).HasColumnName("media_storage_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Archive).WithMany(p => p.TrxMediaStorageDetails)
                .HasForeignKey(d => d.ArchiveId)
                .HasConstraintName("FK_ARCHIVE_ID_MEDIA_STORAGE_DETAIL");

            entity.HasOne(d => d.MediaStorage).WithMany(p => p.TrxMediaStorageDetails)
                .HasForeignKey(d => d.MediaStorageId)
                .HasConstraintName("FK_MEDIA_STORAGE_ID_MEDIA_STORAGE_DETAIL");
        });

        modelBuilder.Entity<TrxPermissionClassification>(entity =>
        {
            entity.HasKey(e => e.PermissionClassificationId);

            entity.ToTable("TRX_PERMISSION_CLASSIFICATION");

            entity.Property(e => e.PermissionClassificationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("permission_classification_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.SubSubjectClassificationId).HasColumnName("sub_subject_classification_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate).HasColumnName("updated_date");

            entity.HasOne(d => d.Position).WithMany(p => p.TrxPermissionClassifications)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_POSITION_ID_TRX_PERMISSION_CLASSIFICATION");

            entity.HasOne(d => d.SubSubjectClassification).WithMany(p => p.TrxPermissionClassifications)
                .HasForeignKey(d => d.SubSubjectClassificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SUB_SUBJECT_CALSSIFICATIONE_ID_PERMISSION_CLASSIFIFCATION");
        });

        modelBuilder.Entity<TrxRack>(entity =>
        {
            entity.HasKey(e => e.RackId);

            entity.ToTable("TRX_RACK");

            entity.Property(e => e.RackId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("rack_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.RackCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("rack_code");
            entity.Property(e => e.RackName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("rack_name");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasMaxLength(2500)
                .IsUnicode(false)
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Room).WithMany(p => p.TrxRacks)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK_ROOM_ID_RACK");
        });

        modelBuilder.Entity<TrxRoom>(entity =>
        {
            entity.HasKey(e => e.RoomId);

            entity.ToTable("TRX_ROOM");

            entity.Property(e => e.RoomId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("room_id");
            entity.Property(e => e.ArchiveRoomType)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("archive_room_type");
            entity.Property(e => e.ArchiveUnitId).HasColumnName("archive_unit_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.RoomCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("room_code");
            entity.Property(e => e.RoomName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("room_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.ArchiveUnit).WithMany(p => p.TrxRooms)
                .HasForeignKey(d => d.ArchiveUnitId)
                .HasConstraintName("FK_ARCHIVE_UNIT_ID_ROOM");
        });

        modelBuilder.Entity<TrxRow>(entity =>
        {
            entity.HasKey(e => e.RowId);

            entity.ToTable("TRX_ROW");

            entity.Property(e => e.RowId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("row_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.RackId).HasColumnName("rack_id");
            entity.Property(e => e.RowCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("row_code");
            entity.Property(e => e.RowName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("row_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Rack).WithMany(p => p.TrxRows)
                .HasForeignKey(d => d.RackId)
                .HasConstraintName("FK_RACK_ID_ROW");
        });

        modelBuilder.Entity<TrxSubSubjectClassification>(entity =>
        {
            entity.HasKey(e => e.SubSubjectClassificationId);

            entity.ToTable("TRX_SUB_SUBJECT_CLASSIFICATION");

            entity.Property(e => e.SubSubjectClassificationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("sub_subject_classification_id");
            entity.Property(e => e.BasicInformation)
                .HasMaxLength(2500)
                .IsUnicode(false)
                .HasColumnName("basic_information");
            entity.Property(e => e.ClassificationId).HasColumnName("classification_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.CreatorId).HasColumnName("creator_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.RetentionActive).HasColumnName("retention_active");
            entity.Property(e => e.RetentionInactive).HasColumnName("retention_inactive");
            entity.Property(e => e.SecurityClassificationId).HasColumnName("security_classification_id");
            entity.Property(e => e.SubSubjectClassificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sub_subject_classification_code");
            entity.Property(e => e.SubSubjectClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("sub_subject_classification_name");
            entity.Property(e => e.SubjectClassificationId).HasColumnName("subject_classification_id");
            entity.Property(e => e.TypeClassificationId).HasColumnName("type_classification_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Classification).WithMany(p => p.TrxSubSubjectClassifications)
                .HasForeignKey(d => d.ClassificationId)
                .HasConstraintName("FK_CLASSIFICATION_ID_SUB_SUBJECT_CLASSIFICATION");

            entity.HasOne(d => d.Creator).WithMany(p => p.TrxSubSubjectClassifications)
                .HasForeignKey(d => d.CreatorId)
                .HasConstraintName("FK_CREATOR_ID_SUB_SUBJECT_CLASSIFICATION");

            entity.HasOne(d => d.SecurityClassification).WithMany(p => p.TrxSubSubjectClassifications)
                .HasForeignKey(d => d.SecurityClassificationId)
                .HasConstraintName("FK_SECURITY_CLASSIFICATION_ID_SUB_SUBJECT_CLASSIFICATION");

            entity.HasOne(d => d.SubjectClassification).WithMany(p => p.TrxSubSubjectClassifications)
                .HasForeignKey(d => d.SubjectClassificationId)
                .HasConstraintName("FK_SUBJECT_CLASSIFICATION_ID_SUB_SUBJECT_CLASSIFICATION");

            entity.HasOne(d => d.TypeClassification).WithMany(p => p.TrxSubSubjectClassifications)
                .HasForeignKey(d => d.TypeClassificationId)
                .HasConstraintName("FK_TYPE_CLASSIFICATION_ID_SUB_SUBJECT_CLASSIFICATION");
        });

        modelBuilder.Entity<TrxSubjectClassification>(entity =>
        {
            entity.HasKey(e => e.SubjectClassificationId);

            entity.ToTable("TRX_SUBJECT_CLASSIFICATION");

            entity.Property(e => e.SubjectClassificationId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("subject_classification_id");
            entity.Property(e => e.ClassificationId).HasColumnName("classification_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.SubjectClassificationCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("subject_classification_code");
            entity.Property(e => e.SubjectClassificationName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("subject_classification_name");
            entity.Property(e => e.TypeClassificationId).HasColumnName("type_classification_id");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.Classification).WithMany(p => p.TrxSubjectClassifications)
                .HasForeignKey(d => d.ClassificationId)
                .HasConstraintName("FK_TRX_SUBJECT_CLASSIFICATION_TRX_CLASSIFICATION");

            entity.HasOne(d => d.TypeClassification).WithMany(p => p.TrxSubjectClassifications)
                .HasForeignKey(d => d.TypeClassificationId)
                .HasConstraintName("FK_TRX_SUBJECT_CLASSIFICATION_MST_TYPE_CLASSIFICATION");
        });

        modelBuilder.Entity<TrxTypeStorage>(entity =>
        {
            entity.HasKey(e => e.TypeStorageId);

            entity.ToTable("TRX_TYPE_STORAGE");

            entity.Property(e => e.TypeStorageId)
                .ValueGeneratedNever()
                .HasColumnName("type_storage_id");
            entity.Property(e => e.ArchiveUnitId).HasColumnName("archive_unit_id");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Length).HasColumnName("length");
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.Property(e => e.TypeStorageCode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("type_storage_code");
            entity.Property(e => e.TypeStorageName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("type_storage_name");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.ArchiveUnit).WithMany(p => p.TrxTypeStorages)
                .HasForeignKey(d => d.ArchiveUnitId)
                .HasConstraintName("FK_ARCHIVE_UNIT_ID_TYPE_STORAGE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
