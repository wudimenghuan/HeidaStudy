using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

/// <summary>
/// Test 的摘要说明
/// </summary>

public class HTMLHelper
{
    /// <summary>
    /// 获取CooKie
    /// </summary>
    /// <param name="loginUrl"></param>
    /// <param name="postdata"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static CookieContainer GetCooKie(string loginUrl, string postdata, HttpHeader header, out HttpStatusCode code)
    {
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        try
        {
            CookieContainer cc = new CookieContainer();
            request = (HttpWebRequest)WebRequest.Create(loginUrl);
            request.Method = header.method;
            request.ContentType = header.contentType;
            byte[] postdatabyte = Encoding.Default.GetBytes(postdata);
            request.ContentLength = postdatabyte.Length;
            request.AllowAutoRedirect = false;
            request.CookieContainer = cc;
            request.KeepAlive = true;

            //提交请求
            Stream stream;
            stream = request.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            //接收响应
            response = (HttpWebResponse)request.GetResponse();
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);


            code = response.StatusCode;

            //Cookie字符串格式
            string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);

            return cc;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            response.Close();
        }
    }

    /// <summary>
    /// 获取CooKie
    /// </summary>
    /// <param name="loginUrl"></param>
    /// <param name="postdata"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static CookieContainer GetCooKie(string loginUrl, string postdata, HttpHeader header)
    {
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        try
        {
            CookieContainer cc = new CookieContainer();
            request = (HttpWebRequest)WebRequest.Create(loginUrl);
            request.Method = header.method;
            request.ContentType = header.contentType;
            byte[] postdatabyte = Encoding.Default.GetBytes(postdata);
            request.ContentLength = postdatabyte.Length;
            request.AllowAutoRedirect = false;
            request.CookieContainer = cc;
            request.KeepAlive = true;

            //提交请求
            Stream stream;
            stream = request.GetRequestStream();
            stream.Write(postdatabyte, 0, postdatabyte.Length);
            stream.Close();

            //接收响应
            response = (HttpWebResponse)request.GetResponse();
            response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);

            //Cookie字符串格式
            string strcrook = request.CookieContainer.GetCookieHeader(request.RequestUri);

            return cc;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            response.Close();
        }
    }

    /// <summary>
    /// 获取html
    /// </summary>
    /// <param name="getUrl"></param>
    /// <param name="cookieContainer"></param>
    /// <param name="header"></param>
    /// <returns></returns>
    public static string GetHtml(string getUrl, CookieContainer cookieContainer, HttpHeader header)
    {
        Thread.Sleep(1000);
        HttpWebRequest httpWebRequest = null;
        HttpWebResponse httpWebResponse = null;
        try
        {
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(getUrl);
            httpWebRequest.CookieContainer = cookieContainer;
            httpWebRequest.ContentType = header.contentType;
            httpWebRequest.ServicePoint.ConnectionLimit = header.maxTry;
            httpWebRequest.Referer = getUrl;
            httpWebRequest.Accept = header.accept;
            httpWebRequest.UserAgent = header.userAgent;
            httpWebRequest.Method = "GET";
            httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.Default);
            string html = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();
            return html;
        }
        catch (Exception e)
        {
            if (httpWebRequest != null) httpWebRequest.Abort();
            if (httpWebResponse != null) httpWebResponse.Close();
            return string.Empty;
        }
    }
}

public class HttpHeader
{
    public string contentType { get; set; }

    public string accept { get; set; }

    public string userAgent { get; set; }

    public string method { get; set; }

    public int maxTry { get; set; }
}