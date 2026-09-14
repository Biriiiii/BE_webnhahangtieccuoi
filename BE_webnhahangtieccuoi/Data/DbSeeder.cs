using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BE_webnhahangtieccuoi.Data;

// Tạo dữ liệu mẫu ban đầu
public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        // Áp dụng các migration chưa chạy
        context.Database.Migrate();

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "Quản trị viên",
                Email = "admin@trungtam.com",
                Role = UserRole.SuperAdmin,
                IsActive = true
            });
        }

        if (!context.ServiceCategories.Any())
        {
            context.ServiceCategories.AddRange(
                new ServiceCategory
                {
                    Name = "Tiệc cưới tại trung tâm",
                    Slug = "tiec-cuoi-tai-trung-tam",
                    Description = "Sảnh tiệc, thực đơn, các gói tiệc cưới trọn gói.",
                    DisplayOrder = 1
                },
                new ServiceCategory
                {
                    Name = "Rạp cưới - Decor",
                    Slug = "rap-cuoi-decor",
                    Description = "Dựng rạp, trang trí cho Cưới, Sinh nhật, Sự kiện, Tân gia, Liên hoan.",
                    DisplayOrder = 2
                },
                new ServiceCategory
                {
                    Name = "Nấu đám lưu động",
                    Slug = "nau-dam-luu-dong",
                    Description = "Nhận nấu tiệc lưu động tại nhà khách hàng, từ 5-10 mâm trở lên.",
                    DisplayOrder = 3
                },
                new ServiceCategory
                {
                    Name = "Âm thanh - Ánh sáng - DJ - MC - Vũ đoàn",
                    Slug = "am-thanh-anh-sang-dj-mc-vu-doan",
                    Description = "Cho thuê thiết bị âm thanh ánh sáng, MC, DJ, vũ đoàn cho sự kiện.",
                    DisplayOrder = 4
                }
            );
        }

        context.SaveChanges();
    }
}