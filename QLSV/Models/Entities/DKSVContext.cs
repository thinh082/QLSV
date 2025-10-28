using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLSV.Models.Entities;

public partial class DKSVContext : DbContext
{
    public DKSVContext()
    {
    }

    public DKSVContext(DbContextOptions<DKSVContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DangKyHoc> DangKyHocs { get; set; }

    public virtual DbSet<Diem> Diems { get; set; }

    public virtual DbSet<GiangVien> GiangViens { get; set; }

    public virtual DbSet<LopHocPhan> LopHocPhans { get; set; }

    public virtual DbSet<MonHoc> MonHocs { get; set; }

    public virtual DbSet<SinhVien> SinhViens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThoiKhoaBieu> ThoiKhoaBieus { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:Connection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DangKyHoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DangKyHo__3214EC075428FE62");

            entity.ToTable("DangKyHoc");

            entity.HasIndex(e => new { e.SinhVienId, e.LopHocPhanId }, "UK_DangKyHoc").IsUnique();

            entity.HasOne(d => d.LopHocPhan).WithMany(p => p.DangKyHocs)
                .HasForeignKey(d => d.LopHocPhanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DangKyHoc__LopHo__38996AB5");

            entity.HasOne(d => d.SinhVien).WithMany(p => p.DangKyHocs)
                .HasForeignKey(d => d.SinhVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DangKyHoc__SinhV__37A5467C");
        });

        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Diem__3214EC07E0DB0AE4");

            entity.ToTable("Diem");

            entity.HasIndex(e => e.DangKyHocId, "UQ__Diem__32401B03EA685FD9").IsUnique();

            entity.Property(e => e.DiemCk).HasColumnName("DiemCK");
            entity.Property(e => e.DiemGk).HasColumnName("DiemGK");
            entity.Property(e => e.DiemTp).HasColumnName("DiemTP");

            entity.HasOne(d => d.DangKyHoc).WithOne(p => p.Diem)
                .HasForeignKey<Diem>(d => d.DangKyHocId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Diem__DangKyHocI__3C69FB99");
        });

        modelBuilder.Entity<GiangVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GiangVie__3214EC07B36A1C74");

            entity.ToTable("GiangVien");

            entity.HasIndex(e => e.MaGv, "UQ__GiangVie__2725AEF233611FC8").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaGv)
                .HasMaxLength(10)
                .HasColumnName("MaGV");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");

            entity.HasOne(d => d.TaiKhoan).WithMany(p => p.GiangViens)
                .HasForeignKey(d => d.TaiKhoanId)
                .HasConstraintName("FK__GiangVien__TaiKh__2C3393D0");
        });

        modelBuilder.Entity<LopHocPhan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LopHocPh__3214EC07AEF3CA20");

            entity.ToTable("LopHocPhan");

            entity.HasIndex(e => e.MaLhp, "UQ__LopHocPh__3B9B969141AD7212").IsUnique();

            entity.Property(e => e.HocKy).HasMaxLength(20);
            entity.Property(e => e.MaLhp)
                .HasMaxLength(10)
                .HasColumnName("MaLHP");
            entity.Property(e => e.NamHoc).HasMaxLength(10);

            entity.HasOne(d => d.GiangVien).WithMany(p => p.LopHocPhans)
                .HasForeignKey(d => d.GiangVienId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LopHocPha__Giang__33D4B598");

            entity.HasOne(d => d.MonHoc).WithMany(p => p.LopHocPhans)
                .HasForeignKey(d => d.MonHocId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LopHocPha__MonHo__32E0915F");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MonHoc__3214EC07AF48F3A2");

            entity.ToTable("MonHoc");

            entity.HasIndex(e => e.MaMh, "UQ__MonHoc__2725DFD8BD9D2315").IsUnique();

            entity.Property(e => e.MaMh)
                .HasMaxLength(10)
                .HasColumnName("MaMH");
            entity.Property(e => e.TenMh)
                .HasMaxLength(50)
                .HasColumnName("TenMH");
        });

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SinhVien__3214EC0748EBE3E8");

            entity.ToTable("SinhVien");

            entity.HasIndex(e => e.MaSv, "UQ__SinhVien__2725081BADF34598").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(10);
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.MaSv)
                .HasMaxLength(10)
                .HasColumnName("MaSV");
            entity.Property(e => e.Sdt)
                .HasMaxLength(15)
                .HasColumnName("SDT");

            entity.HasOne(d => d.TaiKhoan).WithMany(p => p.SinhViens)
                .HasForeignKey(d => d.TaiKhoanId)
                .HasConstraintName("FK__SinhVien__TaiKho__286302EC");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TaiKhoan__3214EC07DBD96E1A");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC07BF28E3F").IsUnique();

            entity.Property(e => e.LoaiTk)
                .HasMaxLength(20)
                .HasColumnName("LoaiTK");
            entity.Property(e => e.MatKhau).HasMaxLength(50);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);
        });

        modelBuilder.Entity<ThoiKhoaBieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ThoiKhoa__3214EC072801D974");

            entity.ToTable("ThoiKhoaBieu");

            entity.Property(e => e.PhongHoc).HasMaxLength(20);
            entity.Property(e => e.Thu).HasMaxLength(10);

            entity.HasOne(d => d.LopHocPhan).WithMany(p => p.ThoiKhoaBieus)
                .HasForeignKey(d => d.LopHocPhanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThoiKhoaB__LopHo__440B1D61");
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ThongBao__3214EC076F7E5918");

            entity.ToTable("ThongBao");

            entity.Property(e => e.LoaiTb)
                .HasMaxLength(20)
                .HasColumnName("LoaiTB");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(50);

            entity.HasOne(d => d.GiangVien).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.GiangVienId)
                .HasConstraintName("FK__ThongBao__GiangV__412EB0B6");

            entity.HasOne(d => d.LopHocPhan).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.LopHocPhanId)
                .HasConstraintName("FK__ThongBao__LopHoc__403A8C7D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
