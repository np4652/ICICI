using System.IO;

namespace ICICI.AppCode.Helper
{
    public class FileDirectories
    {
       // public static string CourseMaterial = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/attachments/Course/{0}/");
        public static string ProductImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product/");
        public static string BannerImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Banner/");
        public static string Thumbnail = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/thumbnail/");
    }
}
