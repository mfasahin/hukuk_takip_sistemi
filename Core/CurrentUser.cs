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

        public static bool IsLoggedIn => UserId.HasValue;
    }
}