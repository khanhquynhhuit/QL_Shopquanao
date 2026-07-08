CREATE DATABASE QLShopQuanAo
ON PRIMARY
(
    NAME = QLShopQuanAo_PRIMARY,
    FILENAME = 'E:\SQL\HỆ quản trị cơ sơ dữ liệu\đồ án nhóm\QLShopQuanAo_primary.mdf',
    SIZE = 50MB,
    MAXSIZE = 200MB,
    FILEGROWTH = 10MB
)
LOG ON
(
    NAME = QLShopQuanAo_LOG,
    FILENAME = 'D:\LOG\QLShopQuanAo_log.ldf',
    SIZE = 20MB,
    MAXSIZE = 100MB,
    FILEGROWTH = 10%
)


USE QLShopQuanAo;
GO

------------------------------------------------
--XỬ LÝ CHO PHIÊN ĐĂNG NHẬP
------------------------------------------------
CREATE TABLE SESSION_GLOBAL (
    IsActive BIT NOT NULL
)
INSERT INTO SESSION_GLOBAL (IsActive) VALUES (0);  

select *from SESSION_GLOBAL

CREATE PROCEDURE sp_BatSession
AS
BEGIN
    UPDATE SESSION_GLOBAL SET IsActive = 1;
END

CREATE PROCEDURE sp_TatSession
AS
BEGIN
    UPDATE SESSION_GLOBAL SET IsActive = 0;
END

CREATE PROCEDURE sp_KiemTraSession
AS
BEGIN
    SELECT IsActive FROM SESSION_GLOBAL;
END


------------------------------------------------

CREATE TABLE KHACHHANG (
    MAKH CHAR(10),
    HOTEN NVARCHAR(50) NOT NULL,
	GIOITINH NVARCHAR(50),
    DIACHI NVARCHAR(50),
    SDT CHAR(10),
    EMAIL NVARCHAR(50),
    CONSTRAINT PK_KHACHHANG PRIMARY KEY (MAKH)
)

CREATE TABLE NHANVIEN (
    MANV CHAR(10),
    HOTEN NVARCHAR(50) NOT NULL,
	SDT CHAR(10),
	GIOITINH NVARCHAR(5),
    CHUCVU NVARCHAR(50),
    NGAYVAOLAM DATE,
    LUONG DECIMAL(18,2) NOT NULL,
    CONSTRAINT PK_NHANVIEN PRIMARY KEY (MANV)
);

CREATE TABLE THENHANVIEN (
    MATHE CHAR(10) NOT NULL,
	MANV CHAR(10) NOT NULL,
    NGAYCAP DATE,
    CONSTRAINT PK_THENHANVIEN PRIMARY KEY ( MANV)
);


CREATE TABLE SANPHAM (
    MASP CHAR(10),
    TENSP NVARCHAR(50) NOT NULL,
    LOAI NVARCHAR(50),
	SIZE VARCHAR(5),
    GIA DECIMAL(18,2) NOT NULL,
    SOLUONG INT NOT NULL,
    CONSTRAINT PK_SANPHAM PRIMARY KEY (MASP)
);

CREATE TABLE HOADON (
    MAHD CHAR(10) NOT NULL,
    MAKH CHAR(10) NOT NULL,
    MANV CHAR(10) NOT NULL,
    NGAYLAP DATE,
    TONGTIEN DECIMAL(18,2),
    CONSTRAINT PK_HOADON PRIMARY KEY (MAHD)
);

CREATE TABLE CHITIETHOADON (
    MAHD CHAR(10) NOT NULL,
    MASP CHAR(10) NOT NULL,
    SOLUONG INT,
    DONGIA DECIMAL(18,2),
    CONSTRAINT PK_CHITIETHOADON PRIMARY KEY (MAHD, MASP)
);


-------------------------------------------------
-- KHÓA NGOẠI
-------------------------------------------------
ALTER TABLE THENHANVIEN
ADD CONSTRAINT FK_THENHANVIEN_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV);

ALTER TABLE HOADON
ADD CONSTRAINT FK_HOADON_KHACHHANG FOREIGN KEY (MAKH) REFERENCES KHACHHANG(MAKH);

ALTER TABLE HOADON
ADD CONSTRAINT FK_HOADON_NHANVIEN FOREIGN KEY (MANV) REFERENCES NHANVIEN(MANV);

ALTER TABLE CHITIETHOADON
ADD CONSTRAINT FK_CTHD_HOADON FOREIGN KEY (MAHD) REFERENCES HOADON(MAHD);

ALTER TABLE CHITIETHOADON
ADD CONSTRAINT FK_CTHD_SANPHAM FOREIGN KEY (MASP) REFERENCES SANPHAM(MASP);

-------------------------------------------------
--RÀNG BUỘC
-------------------------------------------------
ALTER TABLE KHACHHANG
ADD CONSTRAINT UQ_KHACHHANG_SDT UNIQUE (SDT);

ALTER TABLE NHANVIEN
ADD CONSTRAINT UQ_NHANVIEN_SDT UNIQUE (SDT);

ALTER TABLE NHANVIEN
ADD CONSTRAINT CK_NHANVIEN_LUONG CHECK (LUONG >= 3000000);

ALTER TABLE THENHANVIEN
ADD CONSTRAINT UQ_THENHANVIEN_NGAYCAP DEFAULT GETDATE() FOR NGAYCAP;

ALTER TABLE THENHANVIEN
ADD CONSTRAINT UQ_THENHANVIEN_MATHE UNIQUE (MATHE);

ALTER TABLE SANPHAM
ADD CONSTRAINT CK_SANPHAM_GIA CHECK (GIA > 0);

ALTER TABLE SANPHAM
ADD CONSTRAINT CK_SANPHAM_SOLUONG CHECK (SOLUONG >= 0);

ALTER TABLE SANPHAM
ADD CONSTRAINT DF_SANPHAM_SOLUONG DEFAULT 0 FOR SOLUONG;

ALTER TABLE SANPHAM
ADD CONSTRAINT CK_SANPHAM_SIZE CHECK (SIZE IN ('S','M','L','XL', 'XXL'));

ALTER TABLE HOADON
ADD CONSTRAINT DF_HOADON_NGAYLAP DEFAULT GETDATE() FOR NGAYLAP;

ALTER TABLE HOADON
ADD CONSTRAINT CK_HOADON_TONGTIEN CHECK (TONGTIEN >= 0);

ALTER TABLE CHITIETHOADON
ADD CONSTRAINT CK_CTHD_SOLUONG CHECK (SOLUONG > 0);

ALTER TABLE CHITIETHOADON
ADD CONSTRAINT CK_CTHD_DONGIA CHECK (DONGIA > 0);

-------------------------------------------------
-- INSERT DATA
-------------------------------------------------
INSERT INTO KHACHHANG VALUES 
('KH001', N'Nguyễn Văn An', N'nam', N'Hà Nội', '0912345678', 'an.nguyen@example.com'),
('KH002', N'Trần Thị Bình', N'nữ', N'Hồ Chí Minh', '0912345679', 'binh.tran@example.com'),
('KH003', N'Lê Văn Cường', N'nam', N'Đà Nẵng', '0912345680', 'cuong.le@example.com'),
('KH004', N'Phạm Thị Dung', N'nữ', N'Hải Phòng', '0912345681', 'dung.pham@example.com'),
('KH005', N'Hoàng Văn Minh', N'nam', N'Cần Thơ', '0912345682', 'minh.hoang@example.com');

INSERT INTO NHANVIEN VALUES
('NV001', N'Nguyễn Văn Nam', '0987654321', N'nam', N'nhân viên', '2022-01-15', 5000000),
('NV002', N'Trần Thị Hoa', '0987654322', N'nữ', N'nhân viên', '2021-03-20', 4500000),
('NV003', N'Lê Văn Hải', '0987654323', N'nam', N'nhân viên', '2020-06-10', 6000000),
('NV004', N'Phạm Thị Lan', '0987654324', N'nữ', N'quản lý', '2019-09-01', 8000000),
('NV005', N'Hoàng Văn Tuấn', '0987654325', N'nam', N'nhân viên', '2023-02-05', 3500000);

INSERT INTO THENHANVIEN  VALUES
('TH001', 'NV001', '2023-01-15'),
('TH002', 'NV002', '2023-02-20'),
('TH003', 'NV003', '2023-03-10'),
('TH004', 'NV004', '2023-04-05'),
('TH005', 'NV005', '2023-05-18');


INSERT INTO SANPHAM VALUES
('SP001', N'Áo Thun Nam', N'Áo', 'M', 150000, 50),
('SP002', N'Áo Sơ Mi Nữ', N'Áo', 'L', 200000, 30),
('SP003', N'Quần Jean Nam', N'Quần', 'XL', 350000, 40),
('SP004', N'Váy Nữ', N'Váy', 'M', 400000, 20),
('SP005', N'Áo Khoác Nam', N'Áo Khoác', 'XXL', 500000, 15);

INSERT INTO HOADON VALUES
('HD001', 'KH001', 'NV001', '2023-05-01', 500000),
('HD002', 'KH002', 'NV002', '2023-05-02', 400000),
('HD003', 'KH003', 'NV003', '2023-05-03', 700000),
('HD004', 'KH004', 'NV004', '2023-05-04', 350000),
('HD005', 'KH005', 'NV005', '2023-05-05', 600000);

INSERT INTO CHITIETHOADON VALUES
('HD001', 'SP001', 2, 150000),
('HD001', 'SP002', 1, 200000),
('HD002', 'SP003', 1, 350000),
('HD003', 'SP004', 2, 400000),
('HD004', 'SP005', 1, 500000);

---------------------------------------------------
--TẠO VIEW
---------------------------------------------------
SELECT *
FROM SANPHAM; 
SELECT *
FROM CHITIETHOADON ; 

SELECT 
    SANPHAM.MASP,
    SANPHAM.TENSP,SANPHAM.SOLUONG,
    SANPHAM.SOLUONG - (
        SELECT ISNULL(SUM(CHITIETHOADON.SOLUONG), 0)
        FROM CHITIETHOADON 
        WHERE CHITIETHOADON.MASP = SANPHAM.MASP
    )
FROM SANPHAM ;

SELECT 
    SANPHAM.MASP,
    SANPHAM.TENSP,SANPHAM.SOLUONG AS SoLuongBanDau,
    SANPHAM.SOLUONG - (
        SELECT ISNULL(SUM(CHITIETHOADON.SOLUONG), 0)
        FROM CHITIETHOADON 
        WHERE CHITIETHOADON.MASP = SANPHAM.MASP
    ) AS SoLuongConLai
FROM SANPHAM ;

CREATE VIEW V_TonKho 
AS
SELECT 
    SANPHAM.MASP,
    SANPHAM.TENSP,SANPHAM.SOLUONG AS SoLuongBanDau,
    SANPHAM.SOLUONG - (
        SELECT ISNULL(SUM(CHITIETHOADON.SOLUONG), 0)
        FROM CHITIETHOADON 
        WHERE CHITIETHOADON.MASP = SANPHAM.MASP
    ) AS SoLuongConLai
FROM SANPHAM ;

-------------------------------------------------------
--HÀM THỦ TỤC
-------------------------------------------------------
--XEM DS CÁC SẢN PHẨM
CREATE PROC sp_XemSanPham
AS
BEGIN
    SELECT MASP, TENSP, LOAI, SIZE, GIA, SOLUONG
    FROM SANPHAM;
END
--KHÔNG THAM SỐ
Create PROC SP_LietKeNhanVien
AS
BEGIN
    SELECT MANV, HOTEN, SDT, GIOITINH, CHUCVU, NGAYVAOLAM, LUONG, HESOLUONG
    FROM NHANVIEN;
END;
EXEC SP_LietKeNhanVien

--CÓ INPUT, KHÔNG OUTPUT
CREATE PROCEDURE SP_ThemSanPham
    @MASP CHAR(10),
    @TENSP NVARCHAR(50),
    @LOAI NVARCHAR(50),
    @SIZE VARCHAR(5),
    @GIA DECIMAL(18,2),
    @SOLUONG INT
AS
BEGIN
    INSERT INTO SANPHAM(MASP, TENSP, LOAI, SIZE, GIA, SOLUONG)
    VALUES (@MASP, @TENSP, @LOAI, @SIZE, @GIA, @SOLUONG);
END;

SELECT * FROM SANPHAM
EXEC SP_ThemSanPham 'SP006', N'Áo Khoác Nam', N'Áo Khoác', 'XXL', 500000, 15 

--CÓ INPUT, OUTPUT
CREATE PROCEDURE sp_TimThongTinKhachHang
    @MaKH NVARCHAR(10),
    @HoTen NVARCHAR(100) OUTPUT,
    @GioiTinh NVARCHAR(10) OUTPUT,
    @DienThoai NVARCHAR(15) OUTPUT,
    @Email NVARCHAR(100) OUTPUT,
    @DiaChi NVARCHAR(200) OUTPUT
AS
BEGIN
    SELECT 
        @HoTen = HoTen,
        @GioiTinh = GioiTinh,
        @DienThoai = sdt,
        @Email = Email,
        @DiaChi = DiaChi
    FROM KHACHHANG
    WHERE MaKH = @MaKH;
END;
GO

DECLARE 
    @HoTen NVARCHAR(100),
    @GioiTinh NVARCHAR(10),
    @DienThoai NVARCHAR(15),
    @Email NVARCHAR(100),
    @DiaChi NVARCHAR(200);

EXEC sp_TimThongTinKhachHang
    @MaKH = N'KH001',              
    @HoTen = @HoTen OUTPUT,
    @GioiTinh = @GioiTinh OUTPUT,
    @DienThoai = @DienThoai OUTPUT,
    @Email = @Email OUTPUT,
    @DiaChi = @DiaChi OUTPUT;

PRINT '--- THÔNG TIN KHÁCH HÀNG ---';
PRINT N'Họ tên: ' + @HoTen;
PRINT N'Giới tính: ' + @GioiTinh;
PRINT N'Điện thoại: ' + @DienThoai;
PRINT N'Email: ' + @Email;
PRINT N'Địa chỉ: ' + @DiaChi;

--KHÔNG INPUT, CÓ OUTPUT
CREATE PROCEDURE sp_HoaDonMoiNhat
    @MaHD NVARCHAR(20) OUTPUT,
    @MaKH NVARCHAR(20) OUTPUT,
    @MaNV NVARCHAR(20) OUTPUT,
    @NgayLap DATETIME OUTPUT,
    @TongTien DECIMAL(18,2) OUTPUT
AS
BEGIN
    SELECT TOP 1
        @MaHD = MaHD,
        @MaKH = MaKH,
        @MaNV = MaNV,
        @NgayLap = NgayLap,
        @TongTien = TongTien
    FROM HOADON
    ORDER BY NgayLap DESC, MaHD DESC;
END;


DECLARE 
    @MaHD NVARCHAR(20),
    @MaKH NVARCHAR(20),
    @MaNV NVARCHAR(20),
    @NgayLap DATETIME,
    @TongTien DECIMAL(18,2);

EXEC sp_HoaDonMoiNhat
    @MaHD = @MaHD OUTPUT,
    @MaKH = @MaKH OUTPUT,
    @MaNV = @MaNV OUTPUT,
    @NgayLap = @NgayLap OUTPUT,
    @TongTien = @TongTien OUTPUT;

PRINT N'--- HOÁ ĐƠN MỚI NHẤT ---';
PRINT N'Mã HĐ: ' + @MaHD;
PRINT N'Mã KH: ' + @MaKH;
PRINT N'Mã NV: ' + @MaNV;
PRINT N'Ngày lập: ' + CONVERT(NVARCHAR(30), @NgayLap, 103);
PRINT N'Tổng tiền: ' + CAST(CAST(@TongTien AS INT) AS NVARCHAR(30));


---THỦ TỤC THÊM NHÂN VIÊN 
CREATE PROCEDURE sp_ThemNhanVien
    @MaNV CHAR(10),
    @HoTen NVARCHAR(50),
    @SDT CHAR(10),
    @GioiTinh NVARCHAR(5),
    @ChucVu NVARCHAR(50),
    @NgayVaoLam DATE,
    @Luong DECIMAL(18,2)
AS
BEGIN
    -- Kiểm tra trùng mã nhân viên
    IF EXISTS (SELECT 1 FROM NHANVIEN WHERE MANV = @MaNV)
    BEGIN
        PRINT N'Mã nhân viên đã tồn tại!';
        RETURN;
    END;

    INSERT INTO NHANVIEN (MANV, HOTEN, SDT, GIOITINH, CHUCVU, NGAYVAOLAM, LUONG)
    VALUES (@MaNV, @HoTen, @SDT, @GioiTinh, @ChucVu, @NgayVaoLam, @Luong);

    PRINT N'Thêm nhân viên thành công!';
END;
EXEC sp_ThemNhanVien 
    @MaNV = 'NV01', 
    @HoTen = N'Nguyễn Văn A', 
    @SDT = '0987654321', 
    @GioiTinh = N'Nam', 
    @ChucVu = N'Quản lý', 
    @NgayVaoLam = '2023-01-10', 
    @Luong = 15000000;
----XÓA NHÂN VIÊN THEO MÃ
Create PROCEDURE sp_XoaNhanVien
    @MaNV CHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM NHANVIEN WHERE MANV = @MaNV)
    BEGIN
        PRINT N'Không tìm thấy mã nhân viên cần xóa!';
        RETURN;
    END;
    DELETE FROM NHANVIEN WHERE MANV = @MaNV;
END;

----THỦ TỤC CHỈNH SỬA THÔNG TIN NHÂN VIÊN
CREATE PROCEDURE sp_SuaNhanVien
    @MaNV CHAR(10),
    @HoTen NVARCHAR(50),
    @SDT CHAR(10),
    @GioiTinh NVARCHAR(5),
    @ChucVu NVARCHAR(50),
    @NgayVaoLam DATE,
    @Luong DECIMAL(18,2)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM NHANVIEN WHERE MANV = @MaNV)
    BEGIN
        PRINT N'Không tìm thấy nhân viên để cập nhật!';
        RETURN;
    END;

    UPDATE NHANVIEN
    SET HOTEN = @HoTen,
        SDT = @SDT,
        GIOITINH = @GioiTinh,
        CHUCVU = @ChucVu,
        NGAYVAOLAM = @NgayVaoLam,
        LUONG = @Luong
    WHERE MANV = @MaNV;

    PRINT N'Cập nhật thông tin nhân viên thành công!';
END;

--LIỆT KÊ SẢN PHẨM
CREATE PROCEDURE sp_LietKeSanPham
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        MASP,
        TENSP,
        LOAI,
        SIZE,
        GIA,
        SOLUONG
    FROM SANPHAM;
END;
GO

--SỬA SẢN PHẨM
CREATE PROCEDURE sp_SuaSanPham
    @MASP NVARCHAR(20),
    @TENSP NVARCHAR(100),
    @LOAI NVARCHAR(50),
    @SIZE NVARCHAR(10),
    @GIA DECIMAL(18,2),
    @SOLUONG INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE SANPHAM
    SET 
        TENSP = @TENSP,
        LOAI = @LOAI,
        SIZE = @SIZE,
        GIA = @GIA,
        SOLUONG = @SOLUONG
    WHERE MASP = @MASP;
END;
GO
--XEM CHI TIẾT hóa đơn
Create PROCEDURE sp_ChiTietHoaDon
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        HD.MAHD AS [Mã hóa đơn],
        KH.HOTEN AS [Khách hàng],
        NV.HOTEN AS [Nhân viên lập],
        HD.NGAYLAP AS [Ngày lập],
        SP.TENSP AS [Sản phẩm],
        CTHD.SOLUONG AS [Số lượng],
        CTHD.DONGIA AS [Đơn giá],
        (CTHD.SOLUONG * CTHD.DONGIA) AS [Thành tiền]
    FROM HOADON HD
    INNER JOIN CHITIETHOADON CTHD ON HD.MAHD = CTHD.MAHD
    INNER JOIN SANPHAM SP ON CTHD.MASP = SP.MASP
    INNER JOIN KHACHHANG KH ON HD.MAKH = KH.MAKH
    INNER JOIN NHANVIEN NV ON HD.MANV = NV.MANV
    ORDER BY HD.MAHD;
END;
GO


--------------------------------------------------------
--HÀM FUNCTION
--------------------------------------------------------
--vô hướng
--Lấy số lượng còn lại của 1 sản phẩm
CREATE FUNCTION fn_SoLuong (@MASP CHAR(10))
RETURNS INT
AS
BEGIN
    DECLARE @SL INT;

    SELECT @SL = SOLUONG 
    FROM SANPHAM 
    WHERE MASP = @MASP;

    RETURN ISNULL(@SL, 0);
END;
GO

Create PROCEDURE sp_XemSoLuong
    @MASP CHAR(10)
AS
BEGIN
    DECLARE @KQ INT;
    SET @KQ = dbo.fn_SoLuong(@MASP);
    SELECT 
        @MASP AS MaSanPham, 
        @KQ AS SoLuong;
END;
GO

--Trả về tổng số nhân viên
CREATE FUNCTION fn_TongSoNhanVien()
RETURNS INT
AS
BEGIN
    DECLARE @SoNV INT;
    SELECT @SoNV = COUNT(*) FROM NHANVIEN;
    RETURN ISNULL(@SoNV,0);
END;
GO

CREATE PROCEDURE sp_XemTongSoNhanVien
AS
BEGIN
    DECLARE @KQ INT;
    SET @KQ = dbo.fn_TongSoNhanVien();
    SELECT @KQ AS TongSoNhanVien;
END;
GO

--table
-- Hàm trả về bảng sản phẩm bán chạy
CREATE FUNCTION f_TopSanPhamBanChay2(@TopN INT)
RETURNS @SanPham TABLE 
(
    MaSP NVARCHAR(10),
    TenSP NVARCHAR(100),
    TongSoLuong INT
)
AS
BEGIN
    INSERT INTO @SanPham
    SELECT TOP (@TopN) sp.MaSP, sp.TenSP, SUM(ct.SoLuong) AS TongSoLuong
    FROM CHITIETHOADON ct
    JOIN SANPHAM sp ON ct.MaSP = sp.MaSP
    GROUP BY sp.MaSP, sp.TenSP
    ORDER BY SUM(ct.SoLuong) DESC;

    RETURN;
END;
GO

CREATE PROCEDURE SP_TOPSP @TopN INT
AS
BEGIN
    SELECT * FROM f_TopSanPhamBanChay2(@TopN);
END;
GO
--Khách hàng gần đây
CREATE FUNCTION fn_KhachHangMoiNhat()
RETURNS @KH TABLE
(
    MaKH NVARCHAR(10),
    HoTen NVARCHAR(100),
    DienThoai NVARCHAR(15),
    Email NVARCHAR(100),
    DiaChi NVARCHAR(200)
)
AS
BEGIN
    INSERT INTO @KH
    SELECT TOP 1 MaKH, HoTen, SDT, Email, DiaChi
    FROM KHACHHANG
    ORDER BY MaKH DESC; 
    RETURN;
END;

CREATE PROC SP_KHMOINHAT
AS
BEGIN
    SELECT * FROM dbo.fn_KhachHangMoiNhat();
END;
----------------------------------------------------
--TRIGGER
----------------------------------------------------
--Trigger cho Nhân Viên
Create TRIGGER TR_THEMNHANVIEN
ON NHANVIEN
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaxMaThe INT;

    SELECT @MaxMaThe = ISNULL(MAX(CAST(SUBSTRING(MATHE, 4, 3) AS INT)), 0)
    FROM THENHANVIEN;
 
    INSERT INTO THENHANVIEN (MATHE, MANV, NGAYCAP)
    SELECT 
        'TH' + RIGHT('00' + CAST(@MaxMaThe + ROW_NUMBER() OVER (ORDER BY MANV) AS VARCHAR(3)), 3),
        MANV,
        GETDATE()
    FROM inserted i
    WHERE NOT EXISTS (
        SELECT 1 FROM THENHANVIEN t WHERE t.MANV = i.MANV
    );

    PRINT N'Đã tự động cấp thẻ nhân viên mới!';
END;
drop trigger TR_THEMNHANVIEN

--Thêm bằng INSTEAD OF INSERT
CREATE TRIGGER TR_THEMNHANVIEN_INSTEAD
ON NHANVIEN
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MaxMaThe INT;

    -- Lấy mã thẻ cao nhất hiện có trong bảng THENHANVIEN
    SELECT @MaxMaThe = ISNULL(MAX(CAST(SUBSTRING(MATHE, 3, 3) AS INT)), 0)
    FROM THENHANVIEN;

    -- B1: Thêm nhân viên mới vào bảng NHANVIEN
    INSERT INTO NHANVIEN (MANV, HOTEN, SDT, GIOITINH, CHUCVU, NGAYVAOLAM, LUONG)
    SELECT MANV, HOTEN, SDT, GIOITINH, CHUCVU, NGAYVAOLAM, LUONG
    FROM inserted;

    -- B2: Tự động tạo thẻ nhân viên tương ứng
    INSERT INTO THENHANVIEN (MATHE, MANV, NGAYCAP)
    SELECT 
        'TH' + RIGHT('00' + CAST(@MaxMaThe + ROW_NUMBER() OVER (ORDER BY i.MANV) AS VARCHAR(3)), 3),
        i.MANV,
        GETDATE()
    FROM inserted i
    WHERE NOT EXISTS (
        SELECT 1 FROM THENHANVIEN t WHERE t.MANV = i.MANV
    );

    PRINT N'Đã thêm nhân viên và tự động cấp thẻ!';
END;
GO


---------------------------------------------------------------------------


Create TRIGGER TR_CAPNHATNHANVIEN
ON NHANVIEN
for UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN deleted d ON i.MANV = d.MANV
        WHERE i.LUONG > d.LUONG * 1.1
    )
    BEGIN
        -- Ném lỗi ra cho WinForm nhận
        RAISERROR(N'Lỗi: Lương nhân viên không được tăng quá 10%%!', 16, 1);
        ROLLBACK TRANSACTION;  -- rollback sau
        RETURN;
    END
    ELSE
    BEGIN
        RAISERROR(N'Cập nhật nhân viên thành công!', 10, 1);
    END
END;


create TRIGGER TR_XOANHANVIEN
ON NHANVIEN
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (
        SELECT 1
        FROM deleted
        WHERE LOWER(LTRIM(RTRIM(CHUCVU))) = N'quản lý'
    )
    BEGIN
        RAISERROR(N'Lỗi: Không thể xóa nhân viên có chức vụ Quản lý!', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END;

    PRINT N' Đã xóa nhân viên thành công!';
END;

--INSTEAD OF DELETE
create TRIGGER TR_XOA_NHANVIEN_THE
ON NHANVIEN
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- 1️⃣ Xóa thẻ nhân viên trước
    DELETE FROM THENHANVIEN
    WHERE MANV IN (SELECT MANV FROM deleted);

    -- 2️⃣ Sau đó mới xóa nhân viên
    DELETE FROM NHANVIEN
    WHERE MANV IN (SELECT MANV FROM deleted);

    PRINT N'Đã xóa nhân viên và thẻ nhân viên liên quan!';
END;

--Đồng bộ dữ liệu giữa bảng SANPHAM và bảng HOADONCHITIET ( IF UPDATE )
CREATE TRIGGER trg_Update_SanPham
ON SANPHAM
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Nếu cập nhật giá (GIA) thì cập nhật DONGIA trong CHITIETHOADON
    IF UPDATE(GIA)
    BEGIN
        UPDATE CTHD
        SET CTHD.DONGIA = I.GIA
        FROM CHITIETHOADON CTHD
        INNER JOIN inserted I ON CTHD.MASP = I.MASP;
    END

    -- Nếu cập nhật tên sản phẩm thì cập nhật lại trong SANPHAM (thực ra không cần, nhưng giữ nguyên cấu trúc)
    IF UPDATE(TENSP)
    BEGIN
        UPDATE S
        SET S.TENSP = I.TENSP
        FROM SANPHAM S
        INNER JOIN inserted I ON S.MASP = I.MASP;
    END

    -- Nếu cập nhật số lượng thì cập nhật lại trong SANPHAM (giữ nguyên)
    IF UPDATE(SOLUONG)
    BEGIN
        UPDATE S
        SET S.SOLUONG = I.SOLUONG
        FROM SANPHAM S
        INNER JOIN inserted I ON S.MASP = I.MASP;
    END

    PRINT N'Cập nhật sản phẩm và chi tiết hóa đơn thành công!';
END;
GO
--INSTEAD OF UPDATE ( cập nhật sản phẩm )
CREATE TRIGGER trg_Update_SanPham_INSTEAD
ON SANPHAM
INSTEAD OF UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- 1️⃣ Kiểm tra điều kiện cập nhật đặc biệt
    IF EXISTS (
        SELECT 1 
        FROM INSERTED i
        JOIN DELETED d ON i.MASP = d.MASP
        WHERE 
            -- Nếu là Quần short nam thì giá bán phải < 100000
            (i.TENSP = N'Quần short nam' AND i.GIA >= 100000)
            OR
            -- Nếu là Váy tua rua festival thì số lượng không vượt quá 30
            (i.TENSP = N'Váy tua rua festival' AND i.SOLUONG > 30)
    )
    BEGIN
        RAISERROR(N'Lỗi: Cập nhật không hợp lệ theo quy định sản phẩm!', 16, 1);
        RETURN;
    END;

    -- 2️⃣ Cập nhật lại dữ liệu sản phẩm (nếu hợp lệ)
    UPDATE s
    SET 
        s.TENSP = i.TENSP,
        s.LOAI = i.LOAI,
        s.GIA = i.GIA,
        s.SOLUONG = i.SOLUONG
    FROM SANPHAM s
    JOIN INSERTED i ON s.MASP = i.MASP;

    PRINT N'Cập nhật sản phẩm thành công!';
END;
GO


-----------------------------------------------------------------
--CON TRỎ
-----------------------------------------------------------------

--CẬP NHẬT HỆ SỐ LƯƠNG
CREATE TRIGGER TR_CAPNHAT_HESO_CURSOR
ON NHANVIEN
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MANV CHAR(10), @CHUCVU NVARCHAR(50), @HESO FLOAT;
    DECLARE cur CURSOR LOCAL FORWARD_Only FOR SELECT MANV, CHUCVU FROM inserted;
    OPEN cur; FETCH NEXT FROM cur INTO @MANV, @CHUCVU;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @CHUCVU = LTRIM(RTRIM(ISNULL(@CHUCVU,N'')));
        IF LOWER(@CHUCVU) LIKE N'nhân viên' SET @HESO = 1.1
        ELSE IF LOWER(@CHUCVU) LIKE N'quản lý'  SET @HESO = 1.3
        ELSE SET @HESO = 1.0;
        UPDATE NHANVIEN SET HESOLUONG = @HESO WHERE MANV = @MANV;
        FETCH NEXT FROM cur INTO @MANV, @CHUCVU;
    END
    CLOSE cur; DEALLOCATE cur;
END
GO

--XEM LƯƠNG THỰC NHẬN
CREATE PROCEDURE sp_HienThiLuongThucNhan
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @MANV CHAR(10),
        @HOTEN NVARCHAR(50),
        @LUONG DECIMAL(18,2),
        @HESO FLOAT,
        @THUCNHAN DECIMAL(18,2);

    -- Bảng tạm chứa kết quả
    DECLARE @KQ TABLE (
        MANV CHAR(10),
        HOTEN NVARCHAR(50),
        LUONGCoBan DECIMAL(18,2),
        HESOLUONG FLOAT,
        LuongThucNhan DECIMAL(18,2)
    );

    -- Con trỏ duyệt toàn bộ nhân viên
    DECLARE cur CURSOR LOCAL FORWARD_ONLY READ_ONLY FOR
        SELECT MANV, HOTEN, ISNULL(LUONG,0), ISNULL(HESOLUONG,1.0)
        FROM NHANVIEN
        ORDER BY MANV;

    OPEN cur;
    FETCH NEXT FROM cur INTO @MANV, @HOTEN, @LUONG, @HESO;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @THUCNHAN = ROUND(@LUONG * CAST(@HESO AS DECIMAL(18,4)), 0); -- làm tròn nếu cần

        INSERT INTO @KQ (MANV, HOTEN, LUONGCoBan, HESOLUONG, LuongThucNhan)
        VALUES (@MANV, @HOTEN, @LUONG, @HESO, ISNULL(@THUCNHAN,0));

        FETCH NEXT FROM cur INTO @MANV, @HOTEN, @LUONG, @HESO;
    END

    CLOSE cur;
    DEALLOCATE cur;

    -- Trả về kết quả
    SELECT MANV, HOTEN, LUONGCoBan AS [Lương cơ bản], HESOLUONG AS [Hệ số], LuongThucNhan AS [Lương thực nhận]
    FROM @KQ
    ORDER BY MANV;
END
GO
--Tồng lương thực lãnh
CREATE FUNCTION fn_TongLuongThucLanh()
RETURNS DECIMAL(18,2)
AS
BEGIN
    DECLARE 
        @TongLuong DECIMAL(18,2) = 0,
        @Luong DECIMAL(18,2),
        @HeSo FLOAT;

    -- Tạo con trỏ duyệt từng nhân viên
    DECLARE cur CURSOR LOCAL FOR
        SELECT ISNULL(LUONG, 0), ISNULL(HESOLUONG, 1.0)
        FROM NHANVIEN;

    OPEN cur;
    FETCH NEXT FROM cur INTO @Luong, @HeSo;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @TongLuong += (@Luong * @HeSo);

        FETCH NEXT FROM cur INTO @Luong, @HeSo;
    END

    CLOSE cur;
    DEALLOCATE cur;

    RETURN ISNULL(@TongLuong, 0);
END;
GO
-----Đăng ký tài khoản
USE master;
GO
CREATE PROCEDURE sp_TaoTaiKhoanTuDong
    @MatKhau NVARCHAR(50),
    @VaiTro NVARCHAR(20) = N'NhanVien',  -- Mặc định là NhânViên
    @NguoiTao NVARCHAR(50) = NULL,       -- Ai tạo tài khoản này
    @TenDangNhapOUT NVARCHAR(50) OUTPUT  -- Output: tên đăng nhập được tạo
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. TÌM SỐ THỨ TỰ TIẾP THEO CHO VAI TRÒ
        DECLARE @SoThuTu INT;
        DECLARE @TienTo NVARCHAR(10);
        
        IF @VaiTro = N'QuanLy'
            SET @TienTo = N'ql';
        ELSE
            SET @TienTo = N'nv';

        -- Tìm số thứ tự cao nhất hiện có
        SELECT @SoThuTu = ISNULL(MAX(CAST(
            CASE 
                WHEN name LIKE @TienTo + '%' 
                THEN SUBSTRING(name, LEN(@TienTo) + 1, LEN(name)) 
                ELSE '0'
            END AS INT
        )), 0)
        FROM sys.server_principals 
        WHERE type_desc = 'SQL_LOGIN' 
        AND name LIKE @TienTo + '%';

        -- Tăng lên 1
        SET @SoThuTu = @SoThuTu + 1;

        -- 2. TẠO TÊN ĐĂNG NHẬP TỰ ĐỘNG
        DECLARE @TenDangNhap NVARCHAR(50);
        SET @TenDangNhap = @TienTo + RIGHT('000' + CAST(@SoThuTu AS NVARCHAR(10)), 3);
        SET @TenDangNhapOUT = @TenDangNhap;

        -- 3. KIỂM TRA TÊN ĐĂNG NHẬP ĐÃ TỒN TẠI CHƯA (để chắc chắn)
        IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @TenDangNhap)
        BEGIN
            RAISERROR(N'Tên đăng nhập đã tồn tại! Vui lòng thử lại.', 16, 1);
            RETURN;
        END

        -- 4. TẠO LOGIN
        DECLARE @Sql NVARCHAR(MAX);
        SET @Sql = N'CREATE LOGIN [' + @TenDangNhap + N'] WITH PASSWORD = N''' + @MatKhau + N''', DEFAULT_DATABASE = QLShopQuanAo';
        EXEC(@Sql);

        -- 5. TẠO USER TRONG DATABASE
        SET @Sql = N'USE QLShopQuanAo; CREATE USER [' + @TenDangNhap + N'] FOR LOGIN [' + @TenDangNhap + N']';
        EXEC(@Sql);

        -- 6. GÁN ROLE THEO VAI TRÒ
        IF @VaiTro = N'QuanLy'
        BEGIN
            SET @Sql = N'USE QLShopQuanAo; ALTER ROLE QuanLy ADD MEMBER [' + @TenDangNhap + N']';
            EXEC(@Sql);
            
            -- Ghi log
            IF @NguoiTao IS NOT NULL
                PRINT N'Admin [' + @NguoiTao + N'] đã tạo tài khoản QUẢN LÝ: ' + @TenDangNhap;
        END
        ELSE
        BEGIN
            -- Mặc định gán role NhânViên
            SET @Sql = N'USE QLShopQuanAo; ALTER ROLE NhanVien ADD MEMBER [' + @TenDangNhap + N']';
            EXEC(@Sql);
            
            -- Ghi log
            IF @NguoiTao IS NULL
                PRINT N'Đã tạo tài khoản NHÂN VIÊN mới (tự đăng ký): ' + @TenDangNhap;
            ELSE
                PRINT N'Admin [' + @NguoiTao + N'] đã tạo tài khoản NHÂN VIÊN: ' + @TenDangNhap;
        END

        COMMIT TRANSACTION;
        PRINT N'Tạo tài khoản thành công! Tên đăng nhập: ' + @TenDangNhap + N', Vai trò: ' + @VaiTro;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END


----------------------------
-- 1️⃣ Tạo các ROLE trong SQL
----------------------------
-- Tạo role Quản lý
CREATE ROLE QuanLy;
GRANT CONTROL ON DATABASE::QLShopQuanAo TO QuanLy;  -- Toàn quyền

-- Tạo role Nhân viên
CREATE ROLE NhanVien;
-- QUYỀN XEM DỮ LIỆU
GRANT SELECT ON KHACHHANG TO NhanVien;
GRANT SELECT ON SANPHAM TO NhanVien;
GRANT SELECT ON HOADON TO NhanVien;
GRANT SELECT ON CHITIETHOADON TO NhanVien;
GRANT SELECT ON NHANVIEN TO NhanVien;
GRANT SELECT ON V_TonKho TO NhanVien;

-- QUYỀN THÊM DỮ LIỆU (cho nghiệp vụ bán hàng)
GRANT INSERT ON HOADON TO NhanVien;
GRANT INSERT ON CHITIETHOADON TO NhanVien;
GRANT INSERT ON KHACHHANG TO NhanVien;

-- QUYỀN SỬA DỮ LIỆU (hạn chế)
GRANT UPDATE ON HOADON TO NhanVien;
GRANT UPDATE ON CHITIETHOADON TO NhanVien;
GRANT UPDATE ON KHACHHANG TO NhanVien;

-- QUYỀN EXECUTE STORED PROCEDURES
GRANT EXECUTE ON sp_XemSanPham TO NhanVien;
GRANT EXECUTE ON sp_LietKeSanPham TO NhanVien;
GRANT EXECUTE ON sp_ChiTietHoaDon TO NhanVien;
GRANT EXECUTE ON sp_XemSoLuong TO NhanVien;
GRANT EXECUTE ON sp_XemTongSoNhanVien TO NhanVien;
GRANT EXECUTE ON sp_HoaDonMoiNhat TO NhanVien;
GRANT EXECUTE ON sp_HienThiLuongThucNhan TO NhanVien;

-- QUYỀN SỬ DỤNG FUNCTIONS. các hàm trả về bảng
GRANT SELECT ON f_TopSanPhamBanChay2 TO NhanVien;
GRANT SELECT ON fn_KhachHangMoiNhat TO NhanVien; 

-----Hiển thị tất cả users và nhóm quyền của users đó trong datagridview
ALTER PROCEDURE sp_GetUsersWithRoles
AS
BEGIN
    SELECT 
        u.name AS UserName,
        COALESCE(r.name, N'Chưa phân quyền') AS RoleName,
        u.create_date AS CreatedDate,
        'User' AS UserType
    FROM sys.database_principals u
    LEFT JOIN sys.database_role_members rm ON u.principal_id = rm.member_principal_id
    LEFT JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id AND r.type = 'R'
    WHERE u.type IN ('S', 'U')  -- SQL user và Windows user
        AND u.name NOT LIKE '##%'  -- Loại bỏ system internal users
        AND u.name NOT IN (
            'dbo', 'guest', 'INFORMATION_SCHEMA', 'sys',
            'sa', 'public'
        )
        AND u.is_fixed_role = 0  -- Loại bỏ fixed roles
        AND u.principal_id > 4   -- Loại bỏ system principals
    ORDER BY 
        CASE 
            WHEN r.name = 'QuanLy' THEN 1
            WHEN r.name = 'NhanVien' THEN 2
            ELSE 3
        END,
        u.name;
END
GO

-----Xóa user
CREATE OR ALTER PROCEDURE sp_XoaUser
    @TenDangNhap NVARCHAR(50),
    @NguoiThucHien NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. KIỂM TRA USER CÓ TỒN TẠI KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @TenDangNhap)
        BEGIN
            RAISERROR(N'User không tồn tại trong database!', 16, 1);
            RETURN;
        END

        -- 2. KHÔNG CHO XÓA USER SYSTEM QUAN TRỌNG
        IF @TenDangNhap IN ('dbo', 'sa', 'guest', 'INFORMATION_SCHEMA', 'sys')
        BEGIN
            RAISERROR(N'Không được xóa user hệ thống!', 16, 1);
            RETURN;
        END

        -- 3. KHÔNG CHO TỰ XÓA CHÍNH MÌNH
        IF @TenDangNhap = USER_NAME()
        BEGIN
            RAISERROR(N'Không thể xóa chính tài khoản đang đăng nhập!', 16, 1);
            RETURN;
        END

        -- 4. XÓA USER KHỎI CÁC ROLE TRƯỚC
        DECLARE @RoleName NVARCHAR(50);
        DECLARE role_cursor CURSOR FOR
        SELECT r.name
        FROM sys.database_role_members rm
        JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
        JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
        WHERE u.name = @TenDangNhap;

        OPEN role_cursor;
        FETCH NEXT FROM role_cursor INTO @RoleName;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @Sql NVARCHAR(MAX) = N'ALTER ROLE [' + @RoleName + N'] DROP MEMBER [' + @TenDangNhap + N']';
            EXEC(@Sql);
            FETCH NEXT FROM role_cursor INTO @RoleName;
        END
        
        CLOSE role_cursor;
        DEALLOCATE role_cursor;

        -- 5. XÓA USER TRONG DATABASE
        DECLARE @DropUserSql NVARCHAR(MAX) = N'DROP USER [' + @TenDangNhap + N']';
        EXEC(@DropUserSql);

        -- 6. XÓA LOGIN (nếu có)
        IF EXISTS (SELECT 1 FROM sys.server_principals WHERE name = @TenDangNhap)
        BEGIN
            DECLARE @DropLoginSql NVARCHAR(MAX) = N'DROP LOGIN [' + @TenDangNhap + N']';
            EXEC(@DropLoginSql);
        END

        -- 7. GHI LOG (nếu có thông tin người thực hiện)
        IF @NguoiThucHien IS NOT NULL
        BEGIN
            PRINT N'User [' + @NguoiThucHien + N'] đã xóa user: ' + @TenDangNhap;
        END

        COMMIT TRANSACTION;
        PRINT N'Xóa user thành công: ' + @TenDangNhap;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END


----Lấy ra tất cả các nhóm quyền
CREATE OR ALTER PROCEDURE sp_GetAllRoles
AS
BEGIN
    SELECT 
        name AS RoleName,
        name AS RoleDisplayName
    FROM sys.database_principals 
    WHERE type = 'R'  -- Chỉ lấy roles
        AND name NOT IN (
            'public', 'db_owner', 'db_accessadmin', 'db_securityadmin',
            'db_ddladmin', 'db_backupoperator', 'db_datareader', 
            'db_datawriter', 'db_denydatareader', 'db_denydatawriter'
        )
        AND name IN ('QuanLy', 'NhanVien', 'KhachHang')  -- Chỉ lấy roles của hệ thống
    ORDER BY 
        CASE name
            WHEN 'QuanLy' THEN 1
            WHEN 'NhanVien' THEN 2
            WHEN 'KhachHang' THEN 3
            ELSE 4
        END;
END
----Load user lên combobox
CREATE OR ALTER PROCEDURE sp_GetUsersForComboBox
AS
BEGIN
    SELECT 
        name AS UserName,
        name AS DisplayName
    FROM sys.database_principals 
    WHERE type IN ('S', 'U')  -- SQL users và Windows users
        AND name NOT LIKE '##%'  -- Loại bỏ system internal users
        AND name NOT IN (
            'dbo', 'guest', 'INFORMATION_SCHEMA', 'sys',
            'sa', 'public'
        )
        AND is_fixed_role = 0
        AND principal_id > 4
    ORDER BY name;
END
----load table lên combobox
CREATE OR ALTER PROCEDURE sp_GetTablesForComboBox
AS
BEGIN
    SELECT 
        name AS TableName,
        name AS DisplayName
    FROM sys.tables 
    WHERE name NOT LIKE '%_TMP%'
        AND name NOT LIKE 'sys%'
        AND name NOT IN ('SESSION_GLOBAL')  -- 🚫 LOẠI BỎ SESSION_GLOBAL
        AND type = 'U'  -- User tables
    ORDER BY name;
END

----Lấy các quyền hiện tại của user
ALTER PROCEDURE sp_GetUserPermissions
    @UserName NVARCHAR(50)
AS
BEGIN
    -- Kiểm tra quyền sử dụng sys.database_permissions
    SELECT 
        OBJECT_NAME(p.major_id) AS TableName,
        MAX(CASE WHEN p.permission_name = 'SELECT' THEN 1 ELSE 0 END) AS HasSelect,
        MAX(CASE WHEN p.permission_name = 'INSERT' THEN 1 ELSE 0 END) AS HasInsert,
        MAX(CASE WHEN p.permission_name = 'UPDATE' THEN 1 ELSE 0 END) AS HasUpdate,
        MAX(CASE WHEN p.permission_name = 'DELETE' THEN 1 ELSE 0 END) AS HasDelete
    FROM sys.database_permissions p
    INNER JOIN sys.database_principals u ON p.grantee_principal_id = u.principal_id
    WHERE u.name = @UserName
        AND p.major_id > 0
        AND p.class = 1 -- OBJECT class
        AND OBJECT_NAME(p.major_id) NOT LIKE 'sys%'
        AND OBJECT_NAME(p.major_id) != 'SESSION_GLOBAL'  -- 🚫 LOẠI BỎ SESSION_GLOBAL
    GROUP BY OBJECT_NAME(p.major_id)
    
    UNION ALL
    
    -- Thêm các bảng không có quyền (hiển thị 0)
    SELECT 
        t.name AS TableName,
        0 AS HasSelect,
        0 AS HasInsert, 
        0 AS HasUpdate,
        0 AS HasDelete
    FROM sys.tables t
    WHERE t.name NOT LIKE 'sys%'
        AND t.name != 'SESSION_GLOBAL'  -- 🚫 LOẠI BỎ SESSION_GLOBAL
        AND t.name NOT IN (
            SELECT OBJECT_NAME(p.major_id)
            FROM sys.database_permissions p
            INNER JOIN sys.database_principals u ON p.grantee_principal_id = u.principal_id
            WHERE u.name = @UserName
                AND p.major_id > 0
                AND p.class = 1
        )
    ORDER BY TableName;
END

-----Thu hồi 1 quyền
CREATE OR ALTER PROCEDURE sp_RevokePermissions
    @UserName NVARCHAR(50),
    @TableName NVARCHAR(128),
    @PermissionType NVARCHAR(20) -- 'SELECT', 'INSERT', 'UPDATE', 'DELETE'
AS
BEGIN
    DECLARE @Sql NVARCHAR(MAX);
    
    SET @Sql = N'REVOKE ' + @PermissionType + N' ON ' + @TableName + N' FROM [' + @UserName + N']';
    
    EXEC(@Sql);
    
    PRINT N'Đã thu hồi quyền ' + @PermissionType + N' trên bảng ' + @TableName + N' từ user ' + @UserName;
END
-----thu hồi nhiều quyền
CREATE OR ALTER PROCEDURE sp_RevokeMultiplePermissions
    @UserName NVARCHAR(50),
    @Permissions XML  -- Chứa danh sách quyền cần thu hồi
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        
        DECLARE @TableName NVARCHAR(128), @PermissionType NVARCHAR(20);
        
        -- Parse XML để lấy danh sách quyền cần thu hồi
        DECLARE permission_cursor CURSOR FOR
        SELECT 
            x.value('@TableName', 'NVARCHAR(128)'),
            x.value('@PermissionType', 'NVARCHAR(20)')
        FROM @Permissions.nodes('/Permissions/Permission') AS T(x);
        
        OPEN permission_cursor;
        FETCH NEXT FROM permission_cursor INTO @TableName, @PermissionType;
        
        WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Thu hồi từng quyền
            DECLARE @Sql NVARCHAR(MAX) = N'REVOKE ' + @PermissionType + N' ON ' + @TableName + N' FROM [' + @UserName + N']';
            EXEC(@Sql);
            
            FETCH NEXT FROM permission_cursor INTO @TableName, @PermissionType;
        END
        
        CLOSE permission_cursor;
        DEALLOCATE permission_cursor;
        
        COMMIT TRANSACTION;
        PRINT N'Đã thu hồi tất cả quyền đã chọn từ user ' + @UserName;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
-----CẤP QUYỀN đơn lẻ
CREATE OR ALTER PROCEDURE sp_GrantPermission
    @UserName NVARCHAR(50),
    @TableName NVARCHAR(128),
    @PermissionType NVARCHAR(20) -- 'SELECT', 'INSERT', 'UPDATE', 'DELETE'
AS
BEGIN
    BEGIN TRY
        DECLARE @Sql NVARCHAR(MAX);
        
        -- Kiểm tra bảng có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = @TableName)
        BEGIN
            RAISERROR(N'Bảng không tồn tại!', 16, 1);
            RETURN;
        END
        
        -- Kiểm tra user có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @UserName)
        BEGIN
            RAISERROR(N'User không tồn tại!', 16, 1);
            RETURN;
        END
        
        -- Cấp quyền
        SET @Sql = N'GRANT ' + @PermissionType + N' ON ' + @TableName + N' TO [' + @UserName + N']';
        EXEC(@Sql);
        
        PRINT N'Đã cấp quyền ' + @PermissionType + N' trên bảng ' + @TableName + N' cho user ' + @UserName;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
----Kiểm tra quyền
-- TẠO THỦ TỤC KIỂM TRA ROLE USER
ALTER PROCEDURE sp_GetCurrentUserInfo
    @UserName NVARCHAR(50) OUTPUT,
    @UserRole NVARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Lấy username
    SET @UserName = USER_NAME();
    
    -- Lấy role CAO NHẤT với ưu tiên: QuanLy > NhanVien > KhachHang
    SELECT TOP 1 @UserRole = r.name
    FROM sys.database_role_members drm
    JOIN sys.database_principals r ON drm.role_principal_id = r.principal_id
    JOIN sys.database_principals u ON drm.member_principal_id = u.principal_id
    WHERE u.name = @UserName
    ORDER BY CASE 
        WHEN r.name = 'QuanLy' THEN 1
        WHEN r.name = 'NhanVien' THEN 2
        ELSE 3
    END;
    
    -- Nếu không có role, gán mặc định
    IF @UserRole IS NULL
        SET @UserRole = 'KhachHang';
END
GO
----Tạo nhóm quyền
-- THỦ TỤC TẠO NHÓM QUYỀN MỚI
CREATE OR ALTER PROCEDURE sp_CreateRoleGroup
    @RoleName NVARCHAR(50),
    @ResultMessage NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- KIỂM TRA TÊN ROLE ĐÃ TỒN TẠI CHƯA
        IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
        BEGIN
            SET @ResultMessage = N'Lỗi: Nhóm quyền "' + @RoleName + N'" đã tồn tại!';
            RETURN;
        END

        -- KIỂM TRA TÊN ROLE HỢP LỆ
        IF @RoleName IS NULL OR LTRIM(RTRIM(@RoleName)) = ''
        BEGIN
            SET @ResultMessage = N'Lỗi: Tên nhóm quyền không được để trống!';
            RETURN;
        END

        -- TẠO ROLE MỚI
        DECLARE @Sql NVARCHAR(MAX) = N'CREATE ROLE [' + @RoleName + N']';
        EXEC(@Sql);

        SET @ResultMessage = N'Đã tạo nhóm quyền "' + @RoleName + N'" thành công!';
    END TRY
    BEGIN CATCH
        SET @ResultMessage = N'Lỗi khi tạo nhóm quyền: ' + ERROR_MESSAGE();
    END CATCH
END
GO
---Xóa nhóm quyền
-- THỦ TỤC XÓA NHÓM QUYỀN
CREATE OR ALTER PROCEDURE sp_DeleteRoleGroup
    @RoleName NVARCHAR(50),
    @ResultMessage NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- KIỂM TRA ROLE CÓ TỒN TẠI KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
        BEGIN
            SET @ResultMessage = N'Lỗi: Nhóm quyền "' + @RoleName + N'" không tồn tại!';
            RETURN;
        END

        -- KHÔNG CHO XÓA CÁC ROLE HỆ THỐNG QUAN TRỌNG
        IF @RoleName IN ('QuanLy', 'NhanVien')
        BEGIN
            SET @ResultMessage = N'Lỗi: Không được phép xóa nhóm quyền hệ thống (QuanLy, NhanVien)!';
            RETURN;
        END

        -- KIỂM TRA XEM ROLE CÓ USER NÀO KHÔNG
        IF EXISTS (SELECT 1 FROM sys.database_role_members rm 
                  JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                  WHERE r.name = @RoleName)
        BEGIN
            SET @ResultMessage = N'Lỗi: Không thể xóa nhóm quyền "' + @RoleName + N'" vì còn user trong nhóm!';
            RETURN;
        END

        -- XÓA ROLE
        DECLARE @Sql NVARCHAR(MAX) = N'DROP ROLE [' + @RoleName + N']';
        EXEC(@Sql);

        SET @ResultMessage = N'Đã xóa nhóm quyền "' + @RoleName + N'" thành công!';
    END TRY
    BEGIN CATCH
        SET @ResultMessage = N'Lỗi khi xóa nhóm quyền: ' + ERROR_MESSAGE();
    END CATCH
END
GO


---Thủ tục lấy user trong nhóm quyền

CREATE OR ALTER PROCEDURE sp_GetUsersInRole
    @RoleName NVARCHAR(50)
AS
BEGIN
    SELECT 
        u.name AS UserName,
        u.create_date AS CreatedDate,
        u.type_desc AS UserType
    FROM sys.database_principals u
    INNER JOIN sys.database_role_members rm ON u.principal_id = rm.member_principal_id
    INNER JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
    WHERE r.name = @RoleName
        AND u.type IN ('S', 'U')  -- SQL user và Windows user
    ORDER BY u.name;
END
GO
-- THỦ TỤC LẤY USER NGOÀI NHÓM QUYỀN
CREATE OR ALTER PROCEDURE sp_GetUsersNotInRole
    @RoleName NVARCHAR(50)
AS
BEGIN
    SELECT 
        u.name AS UserName,
        u.create_date AS CreatedDate,
        u.type_desc AS UserType
    FROM sys.database_principals u
    WHERE u.type IN ('S', 'U')  -- SQL user và Windows user
        AND u.name NOT LIKE '##%'  -- Loại bỏ system internal users
        AND u.name NOT IN ('dbo', 'guest', 'sa', 'public')
        AND u.principal_id > 4
        AND NOT EXISTS (
            SELECT 1 
            FROM sys.database_role_members rm 
            JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
            WHERE rm.member_principal_id = u.principal_id 
            AND r.name = @RoleName
        )
    ORDER BY u.name;
END
GO
-- THỦ TỤC THÊM USER VÀO NHÓM QUYỀN
CREATE OR ALTER PROCEDURE sp_AddUserToRole
    @UserName NVARCHAR(50),
    @RoleName NVARCHAR(50),
    @ResultMessage NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- KIỂM TRA USER CÓ TỒN TẠI KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @UserName AND type IN ('S', 'U'))
        BEGIN
            SET @ResultMessage = N'Lỗi: User "' + @UserName + N'" không tồn tại!';
            RETURN;
        END

        -- KIỂM TRA ROLE CÓ TỒN TẠI KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
        BEGIN
            SET @ResultMessage = N'Lỗi: Nhóm quyền "' + @RoleName + N'" không tồn tại!';
            RETURN;
        END

        -- KIỂM TRA USER ĐÃ TRONG ROLE CHƯA
        IF EXISTS (SELECT 1 FROM sys.database_role_members rm 
                  JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
                  JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                  WHERE u.name = @UserName AND r.name = @RoleName)
        BEGIN
            SET @ResultMessage = N'Lỗi: User "' + @UserName + N'" đã có trong nhóm "' + @RoleName + N'"!';
            RETURN;
        END

        -- THÊM USER VÀO ROLE
        DECLARE @Sql NVARCHAR(MAX) = N'ALTER ROLE [' + @RoleName + N'] ADD MEMBER [' + @UserName + N']';
        EXEC(@Sql);

        SET @ResultMessage = N'Đã thêm user "' + @UserName + N'" vào nhóm "' + @RoleName + N'" thành công!';
    END TRY
    BEGIN CATCH
        SET @ResultMessage = N'Lỗi khi thêm user vào nhóm: ' + ERROR_MESSAGE();
    END CATCH
END
GO
-- THỦ TỤC XÓA USER KHỎI NHÓM QUYỀN
CREATE OR ALTER PROCEDURE sp_RemoveUserFromRole
    @UserName NVARCHAR(50),
    @RoleName NVARCHAR(50),
    @ResultMessage NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        -- KIỂM TRA USER CÓ TỒN TẠI KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @UserName AND type IN ('S', 'U'))
        BEGIN
            SET @ResultMessage = N'Lỗi: User "' + @UserName + N'" không tồn tại!';
            RETURN;
        END

        -- KIỂM TRA ROLE CÓ TỒN TẠI KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = @RoleName AND type = 'R')
        BEGIN
            SET @ResultMessage = N'Lỗi: Nhóm quyền "' + @RoleName + N'" không tồn tại!';
            RETURN;
        END

        -- KIỂM TRA USER CÓ TRONG ROLE KHÔNG
        IF NOT EXISTS (SELECT 1 FROM sys.database_role_members rm 
                      JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
                      JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
                      WHERE u.name = @UserName AND r.name = @RoleName)
        BEGIN
            SET @ResultMessage = N'Lỗi: User "' + @UserName + N'" không có trong nhóm "' + @RoleName + N'"!';
            RETURN;
        END

        -- XÓA USER KHỎI ROLE
        DECLARE @Sql NVARCHAR(MAX) = N'ALTER ROLE [' + @RoleName + N'] DROP MEMBER [' + @UserName + N']';
        EXEC(@Sql);

        SET @ResultMessage = N'Đã xóa user "' + @UserName + N'" khỏi nhóm "' + @RoleName + N'" thành công!';
    END TRY
    BEGIN CATCH
        SET @ResultMessage = N'Lỗi khi xóa user khỏi nhóm: ' + ERROR_MESSAGE();
    END CATCH
END
GO
--- lấy các table
-- THỦ TỤC LẤY DANH SÁCH BẢNG (LOẠI TRỪ SESSION_GLOBAL)
CREATE OR ALTER PROCEDURE sp_GetTablesForPermissions
AS
BEGIN
    SELECT name AS TableName
    FROM sys.tables 
    WHERE name NOT LIKE '%_TMP%'
        AND name NOT LIKE 'sys%'
        AND name != 'SESSION_GLOBAL'
    ORDER BY name;
END
GO
-- THỦ TỤC KIỂM TRA USER THUỘC NHÓM NÀO
CREATE OR ALTER PROCEDURE sp_GetUserRoles
    @UserName NVARCHAR(50)
AS
BEGIN
    SELECT 
        r.name AS RoleName,
        r.type_desc AS RoleType
    FROM sys.database_role_members rm
    JOIN sys.database_principals r ON rm.role_principal_id = r.principal_id
    JOIN sys.database_principals u ON rm.member_principal_id = u.principal_id
    WHERE u.name = @UserName
    ORDER BY r.name;
END
GO
exec sp_GetUserRoles 'nv002'
----Lấy thông tin users





SELECT *FROM V_TonKho;
SELECT *FROM KHACHHANG;
SELECT *FROM NHANVIEN;
SELECT *FROM THENHANVIEN;    
SELECT *FROM SANPHAM;
SELECT *FROM HOADON;
SELECT *FROM CHITIETHOADON;


DELETE FROM KHACHHANG
DELETE FROM NHANVIEN
DELETE FROM THENHANVIEN
DELETE FROM SANPHAM
DELETE FROM HOADON
DELETE FROM CHITIETHOADON
DELETE FROM V_TonKho;

