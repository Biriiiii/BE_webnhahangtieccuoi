using BE_webnhahangtieccuoi.Data;
using BE_webnhahangtieccuoi.Models.Entities;
using BE_webnhahangtieccuoi.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace BE_webnhahangtieccuoi.Data;

// Tạo dữ liệu mẫu ban đầu:
// 5 nhóm dịch vụ chính:
// 1. Rạp Cưới
// 2. Sinh nhật
// 3. Sự kiện
// 4. Tân gia
// 5. Liên hoan
//
// Mỗi nhóm có 3 gói dịch vụ:
// - Gói Decor
// - Gói Âm thanh - Ánh sáng - DJ - MC - Dancer
// - Gói Combo Decor + Âm thanh - Ánh sáng trọn gói

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        // =========================================================
        // 1. TẠO TÀI KHOẢN ADMIN
        // =========================================================

        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "Quản trị viên",
                Email = "admin@trungtam.com",
                IsActive = true
            });
        }

        // =========================================================
        // 2. TẠO 5 NHÓM DỊCH VỤ
        // =========================================================

        if (!context.ServiceCategories.Any())
        {
            var rapCuoi = new ServiceCategory
            {
                Name = "Rạp Cưới",
                Slug = "rap-cuoi",
                Description = "Không gian rạp cưới sang trọng, rộng rãi với thiết kế hiện đại, được trang trí tinh tế và phù hợp cho các buổi tiệc cưới, lễ gia tiên và sự kiện đặc biệt.",
                ImageUrl = "https://res.cloudinary.com/dpiolhoq6/image/upload/v1789394010/wedding-center/services/tonjcm7s5psrjpmril4n.jpg",
                DisplayOrder = 1
            };

            var sinhNhat = new ServiceCategory
            {
                Name = "Sinh Nhật",
                Slug = "sinh-nhat",
                Description = "Không gian ấm cúng, trang trí theo chủ đề cùng thực đơn đa dạng, mang đến một bữa tiệc sinh nhật vui vẻ và đáng nhớ.",
                ImageUrl = "https://res.cloudinary.com/dpiolhoq6/image/upload/v1789394180/wedding-center/services/jwg8f9t3kgbpmygdbwik.jpg",
                DisplayOrder = 2
            };

            var suKien = new ServiceCategory
            {
                Name = "Sự kiện",
                Slug = "su-kien",
                Description = "Không gian sang trọng, linh hoạt, phù hợp tổ chức hội nghị, họp mặt, lễ kỷ niệm và các sự kiện đặc biệt. Nhà hàng hỗ trợ bố trí không gian, thực đơn và trang trí theo nhu cầu, mang đến một chương trình chuyên nghiệp và trọn vẹn.",
                ImageUrl = "https://res.cloudinary.com/dpiolhoq6/image/upload/v1789394330/wedding-center/services/wng2j13xog4awpseupul.jpg",
                DisplayOrder = 3
            };

            var tanGia = new ServiceCategory
            {
                Name = "Tân gia",
                Slug = "tan-gia",
                Description = "Không gian ấm cúng và thoải mái, thích hợp tổ chức tiệc tân gia cùng gia đình, bạn bè và người thân. Thực đơn đa dạng, phục vụ tận tình giúp gia chủ có một buổi tiệc vui vẻ và đáng nhớ.",
                ImageUrl = "https://res.cloudinary.com/dpiolhoq6/image/upload/v1789395509/wedding-center/services/px6k8ulmau7yyr3nvfza.jpg",
                DisplayOrder = 4
            };

            var lienHoan = new ServiceCategory
            {
                Name = "Liên hoan",
                Slug = "lien-hoan",
                Description = "Không gian rộng rãi, thoải mái, phù hợp cho các buổi liên hoan, gặp gỡ đồng nghiệp, bạn bè và gia đình. Với thực đơn phong phú cùng dịch vụ chu đáo, nhà hàng mang đến những khoảnh khắc sum họp vui vẻ và đáng nhớ.",
                ImageUrl = "https://res.cloudinary.com/dpiolhoq6/image/upload/v1789395592/wedding-center/services/xpe632qh5gwqenrokdzd.jpg",
                DisplayOrder = 5
            };

            context.ServiceCategories.AddRange(
                rapCuoi,
                sinhNhat,
                suKien,
                tanGia,
                lienHoan
            );

            // Lưu trước để lấy Id của các ServiceCategory
            context.SaveChanges();

            // =====================================================
            // 3. GÓI DỊCH VỤ - RẠP CƯỚI
            // =====================================================

            AddServicesForCategory(
                context,
                rapCuoi,
                "Rạp Cưới"
            );

            // =====================================================
            // 4. GÓI DỊCH VỤ - SINH NHẬT
            // =====================================================

            AddServicesForCategory(
                context,
                sinhNhat,
                "Sinh nhật"
            );

            // =====================================================
            // 5. GÓI DỊCH VỤ - SỰ KIỆN
            // =====================================================

            AddServicesForCategory(
                context,
                suKien,
                "Sự kiện"
            );

            // =====================================================
            // 6. GÓI DỊCH VỤ - TÂN GIA
            // =====================================================

            AddServicesForCategory(
                context,
                tanGia,
                "Tân gia"
            );

            // =====================================================
            // 7. GÓI DỊCH VỤ - LIÊN HOAN
            // =====================================================

            AddServicesForCategory(
                context,
                lienHoan,
                "Liên hoan"
            );
        }

        context.SaveChanges();
    }

    // =============================================================
    // HÀM TẠO 3 GÓI DỊCH VỤ CHO MỖI NHÓM
    // =============================================================

    private static void AddServicesForCategory(
        AppDbContext context,
        ServiceCategory category,
        string categoryName)
    {
        var slugPrefix = category.Slug;

        context.Services.AddRange(

            // =====================================================
            // GÓI 1 - DECOR
            // =====================================================

            new Service
            {
                ServiceCategoryId = category.Id,

                Name = $"Gói Decor {categoryName}",

                Slug = $"{slugPrefix}-decor",

                ShortDescription =
                    $"Trang trí không gian {categoryName.ToLower()}, " +
                    "bao gồm hoa, backdrop, cổng chào, bàn gallery, bàn ghế và các phụ kiện trang trí theo chủ đề.",

                DisplayOrder = 1,

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            },

            // =====================================================
            // GÓI 2 - ÂM THANH / ÁNH SÁNG / DJ / MC / DANCER
            // =====================================================

            new Service
            {
                ServiceCategoryId = category.Id,

                Name = $"Gói Âm thanh - Ánh sáng - DJ - MC - Dancer {categoryName}",

                Slug = $"{slugPrefix}-am-thanh-anh-sang-dj-mc-dancer",

                ShortDescription =
                    $"Cung cấp hệ thống âm thanh, ánh sáng sân khấu, " +
                    "DJ, MC và Dancer phục vụ chương trình " +
                    $"{categoryName.ToLower()} theo nhu cầu.",

                DisplayOrder = 2,

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            },

            // =====================================================
            // GÓI 3 - COMBO TRỌN GÓI
            // =====================================================

            new Service
            {
                ServiceCategoryId = category.Id,

                Name = $"Gói Combo {categoryName} Trọn Gói",

                Slug = $"{slugPrefix}-combo-tron-goi",

                ShortDescription =
                    $"Combo trọn gói {categoryName.ToLower()}, " +
                    "kết hợp Decor, hoa, backdrop cùng hệ thống âm thanh, " +
                    "ánh sáng, DJ, MC và Dancer.",

                DisplayOrder = 3,

                IsActive = true,

                CreatedAt = DateTime.UtcNow
            }
        );
    }
}