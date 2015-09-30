using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace HeidaStudy
{
    public class User
    {
        public static string userName;
        public static string password;
        public static string course;

        public static bool Login()
        {
            HttpHeader header;
            CookieContainer cookieContainer;
            HttpStatusCode code = HttpStatusCode.Forbidden;
            header = new HttpHeader();
            header.accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
            header.contentType = "application/x-www-form-urlencoded";
            header.method = "POST";
            header.userAgent = "Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.12 (KHTML, like Gecko) Maxthon/3.0 Chrome/18.0.966.0 Safari/535.12";
            header.maxTry = 300;
            string loginUrl = "http://210.46.97.78/eol/homepage/common/login.jsp";
            try
            {
                cookieContainer = HTMLHelper.GetCooKie(loginUrl, "IPT_LOGINUSERNAME=" + userName + "&IPT_LOGINPASSWORD=" + password, header, out code);
            }
            catch (Exception)
            {
                return false;
            }

            if (code == HttpStatusCode.Found)
            {
                return true;
            }

            return false;
        }
    }
}
