using System;
using System.Web;

namespace Core
{
    public static class CurrentUser
    {
        public static Guid? UserId
        {
            get
            {
                var session = HttpContext.Current?.Session;
                if (session == null || session["KullaniciId"] == null)
                    return null;

                return (Guid)session["KullaniciId"];
            }
        }

        // Kullanıcı Adı Bilgisi (Örn: "admin")
        public static string KullaniciAd
        {
            get
            {
                var session = HttpContext.Current?.Session;
                if (session != null && session["KullaniciAd"] != null)
                    return session["KullaniciAd"].ToString();

                return HttpContext.Current?.User?.Identity?.Name;
            }
        }

        public static bool IsLoggedIn => UserId.HasValue;
    }
}