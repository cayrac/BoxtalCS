using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.Web;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Net.Security;
using System.Text.RegularExpressions;

namespace Boxtal
{
  ///<summary>Http query to communicate with Boxtal servers.</summary>
  ///<remarks></remarks>
  public class Query
  {
    ///<summary>User's login, can be set by default in <see cref="Credentials.Login"/>.</summary>
    ///<remarks></remarks>
    protected string _login = null;
    ///<summary>User's password, can be set by default in <see cref="Credentials.Password"/>.</summary>
    ///<remarks></remarks>
    protected string _password = null;
    ///<summary>User's API production key, can be set by default in <see cref="Credentials.KeyProd"/>.</summary>
    ///<remarks></remarks>
    protected string _keyProd = null;
    ///<summary>User's API test key, can be set by default in <see cref="Credentials.KeyTest"/>.</summary>
    ///<remarks></remarks>
    protected string _keyTest = null;
    ///<summary>API environment, can be set by default in <see cref="Global.Environment"/>. See <see cref="Environment"/> for possible values.</summary>
    ///<remarks></remarks>
    protected string _environment = null;
    ///<summary>Locale required, can be set by default in <see cref="Global.Locale"/>. See <see cref="Locale"/> for possible values.</summary>
    ///<remarks></remarks>
    protected string _locale = null;
    ///<summary>Query parameters.</summary>
    ///<remarks></remarks>
    protected Dictionary<string, string> _parameters = new Dictionary<string, string>();

    ///<summary>Get or set user's login.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Login {get{return _login;} set{_login = value;}}
    ///<summary>Get or set user's password.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Password {get{return _password;} set{_password = value;}}
    ///<summary>Get or set user's production key.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string KeyProd {get{return _keyProd;} set{_keyProd = value;}}
    ///<summary>Get or set user's test key.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string KeyTest {get{return _keyTest;} set{_keyTest = value;}}
    ///<summary>Get or set API environment.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Environment {get{return _environment;} set{_environment = value;}}
    ///<summary>Get or set Locale.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Locale {get{return _locale;} set{_locale = value;}}
    ///<summary>Get the actual host regarding to the query's environment.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Host {get{return Boxtal.Environment.Hosts[_environment];}}
    ///<summary>Get or set query parameters.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public Dictionary<string, string> Parameters {get{return _parameters;} set{_parameters = value;}}

    private static class SSLValidator
    {
      private static RemoteCertificateValidationCallback origin;

      private static bool OnValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors error)
      {
        if (certificate.GetCertHashString() == "EF926134FEEEDF0E1B65B7CA47BBF59816ED412F")
        {
            return true;
        }
        else
        {
           return error == SslPolicyErrors.None;
        }
      }

      public static void OverrideValidation()
      {
        origin = ServicePointManager.ServerCertificateValidationCallback;
        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnValidateCertificate);
        ServicePointManager.Expect100Continue = true;
      }

      public static void RestoreValidation()
      {
        ServicePointManager.ServerCertificateValidationCallback = origin;
      }
    }

    private static Dictionary<string, string> getDefaultParameters()
    {
      return new Dictionary<string, string> {
        {"platform", Global.Platform},
        {"platform_version", Global.PlatformVersion},
        {"module_version", Global.ModuleVersion},
        {"module_platform", Global.ModulePlatform}
      };
    }

    private static HttpWebResponse Request(string login, string password, string keyProd, string keyTest, string environment, string locale, string method, string url, Dictionary<string, string> parameters)
    {
      // building parameters list
      string urlParams = string.Join("&", parameters.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value)).ToArray());

      // is SSL required ?
      bool ssl = url.StartsWith("https://");

      // enable certificate validation
      if (ssl) SSLValidator.OverrideValidation();

      // calling
      HttpWebResponse response = null;
      try
      {
        // creating request
        Global.DebugPrint("Creating new Emc request...");
        var httpWebRequest = (HttpWebRequest)WebRequest.Create(url + (url.Contains("?") ? "&" : "?") + urlParams);
          //((method == WebRequestMethods.Http.Get)?((url.Contains("?") ? "&" : "?") + urlParams):""));
        httpWebRequest.ReadWriteTimeout = 15000;
        httpWebRequest.ContentType = "text/plain";
        httpWebRequest.Method = method;
        httpWebRequest.Headers["Accept-Language"] = locale;
        httpWebRequest.Headers["Api-Version"] = Global.ApiVersion;
        if (((environment == Boxtal.Environment.Test)?keyTest:keyProd) != null)
        {
          httpWebRequest.Headers["access_key"] = (environment == Boxtal.Environment.Test)?keyTest:keyProd;
        }
        if (login != null || password != null)
        {
          httpWebRequest.Headers["Authorization"] =  Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(login + ":" + password));
        }
        Global.DebugPrint("Path : " + url);
        Global.DebugPrint("Method : " + method);
        Global.DebugPrint("Parameters : " + urlParams);
        Global.DebugPrint("Header : ");
        foreach(string key in httpWebRequest.Headers)
        {
          Global.DebugPrint("  " + key + " : " + httpWebRequest.Headers[key]);
        }

        // sending request parameters
        if (httpWebRequest.Method == WebRequestMethods.Http.Post)
        {
          var postData = Encoding.ASCII.GetBytes(urlParams);
          Global.DebugPrint("Sending parameters...");
          httpWebRequest.ContentLength = postData.Length;
          using (var stream = httpWebRequest.GetRequestStream())
          {
              stream.Write(postData, 0, postData.Length);
          }
        }

        // waiting for response
        Global.DebugPrint("Waiting for response...");
        response = (HttpWebResponse)httpWebRequest.GetResponse();

        // disable ssl validation
        if (ssl) SSLValidator.RestoreValidation();
      }
      catch(WebException e)
      {
        Global.DebugPrint("Failed to send the request (WebException)");
        response = (HttpWebResponse)e.Response;
        if (response == null)
        {
          if (ssl) SSLValidator.RestoreValidation();
          throw new ApiException(ApiException.ErrorCode.Client, 0, "Failed to send request : " + e.Message);
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Failed to send the request (Exception)");
        if (ssl) SSLValidator.RestoreValidation();
        throw new ApiException(ApiException.ErrorCode.Client, 0, "Failed to send request : " + e.Message);
      }

      if (ssl) SSLValidator.RestoreValidation();

      if (response == null)
      {
        Global.DebugPrint("Server did not respond...");
        throw new ApiException(ApiException.ErrorCode.Access, 0, "Server did not respond");
      }

      Global.DebugPrint("Response status : " + response.StatusCode);

      return response;
    }

    ///<summary>Execute a custom request</summary>
    ///<remarks>Throws <see cref="ApiException"/></remarks>
    ///<param name="method">HTTP method used. See <see cref="WebRequestMethods.Http"/> for possible values.</param>
    ///<param name="action">Resource requested.</param>
    ///<param name="parameters">Request parameters, can be empty.</param>
    ///<param name="locale">Response locale, set by default as <see cref="Global.Locale"/>.</param>
    ///<returns></returns>
    public static HttpWebResponse Call(string method, string action, Dictionary<string, string> parameters = null, string locale = null)
    {
      if (locale == null)
      {
        locale = Global.Locale;
      }
      if (parameters == null)
      {
        parameters = new Dictionary<string, string>();
      }

      var defaultparameters = Query.getDefaultParameters();
      foreach (var item in defaultparameters)
      {
        parameters[item.Key] = item.Value;
      }
      return Query.Request(Credentials.Login, Credentials.Password, Credentials.KeyProd, Credentials.KeyTest, Global.Environment, locale, method, Boxtal.Environment.Hosts[Global.Environment] + action, parameters);
    }

    ///<summary>Constructor by copy.</summary>
    ///<remarks></remarks>
    ///<param name="query"></param>
    public Query(Query query)
    {
      _login = query.Login;
      _password = query.Password;
      _keyProd = query.KeyProd;
      _keyTest = query.KeyTest;
      _environment = query.Environment;
      _locale = query.Locale;
      foreach(KeyValuePair<string, string> param in query.Parameters)
      {
        _parameters.Add(param.Key, param.Value);
      }
    }

    ///<summary>Constructor with default parameters. See <see cref="Credentials"/> (Login, Password, KeyProd and KeyTest) and <see cref="Global"/> (Environment) for default values.</summary>
    ///<remarks></remarks>
    public Query()
    : this(
      Boxtal.Credentials.Login,
      Boxtal.Credentials.Password,
      Boxtal.Credentials.KeyProd,
      Boxtal.Credentials.KeyTest,
      Boxtal.Global.Environment)
    {
      _parameters = Query.getDefaultParameters();
    }

    ///<summary>Constructor with default parameters.</summary>
    ///<remarks></remarks>
    ///<param name="login"></param>
    ///<param name="password"></param>
    ///<param name="keyProd"></param>
    ///<param name="keyTest"></param>
    ///<param name="environment"></param>
    public Query(string login, string password, string keyProd, string keyTest, string environment)
    {
      _login = login;
      _password = password;
      _keyProd = keyProd;
      _keyTest = keyTest;
      _environment = environment;
      _locale = Global.Locale;
      _parameters = Query.getDefaultParameters();
    }

    ///<summary>Return a list of all parameters.</summary>
    ///<remarks>Contains the parameters in <see cref="_parameters"/> + dynamic parameters</remarks>
    ///<returns></returns>
    protected virtual Dictionary<string, string> ToQueryParameters()
    {
      return new Dictionary<string, string>(_parameters);
    }

    private static void checkResponseStatus(HttpStatusCode status, XmlDocument content)
    {
      // load error message (if it exists)
      var errors = content.SelectNodes("error/message");
      string errorMessage = "No error message";
      if (errors.Count > 0)
      {
        Regex loginPattern = new Regex("^\\[[a-zA-Z0-9]+\\]\\s");
        errorMessage = errors.Item(0).InnerText
        //.Replace("[" + login + "] ", "")
        .Replace("access_denied - ", "")
        .Replace("server_error - ", "")
        .Replace("bad_request - ", "");
        errorMessage = loginPattern.Replace(errorMessage, "");
      }

      switch(status)
      {
        case HttpStatusCode.OK :
        case HttpStatusCode.Created :
        case HttpStatusCode.Continue :
        case HttpStatusCode.Accepted :
          break;
        case HttpStatusCode.GatewayTimeout :
        case HttpStatusCode.BadGateway :
          throw new ApiException(ApiException.ErrorCode.Network, status, errorMessage);
        case HttpStatusCode.Forbidden :
        case HttpStatusCode.Unauthorized :
        case HttpStatusCode.NotFound :
          throw new ApiException(ApiException.ErrorCode.Access, status, errorMessage);
        case HttpStatusCode.BadRequest :
          throw new ApiException(ApiException.ErrorCode.Request, status, errorMessage);
        case HttpStatusCode.Conflict :
        case HttpStatusCode.InternalServerError :
        case HttpStatusCode.ServiceUnavailable :
        case HttpStatusCode.RequestTimeout :
          throw new ApiException(ApiException.ErrorCode.Server, status, errorMessage);
        default :
          throw new ApiException(ApiException.ErrorCode.Unknown, status, "Unexpected response code recieved");
      }
    }

    ///<summary>Execute a request to Boxtal's server, return the response as <see cref="HttpWebResponse"/>.</summary>
    ///<remarks>Throws <see cref="ApiException"/></remarks>
    ///<param name="method">HTTP method used. See <see cref="WebRequestMethods.Http"/> for possible values.</param>
    ///<param name="action">Resource requested</param>
    ///<returns></returns>
    protected HttpWebResponse ExecuteWeb(string method, string action)
    {
      return Query.Request(_login, _password, _keyProd, _keyTest, _environment, Global.Locale, method, Host + action, ToQueryParameters());
    }

    ///<summary>Execute a request to Boxtal's server, return the response as <see cref="XmlDocument"/>.</summary>
    ///<remarks>Throws <see cref="ApiException"/></remarks>
    ///<param name="method">HTTP method used. See <see cref="WebRequestMethods.Http"/> for possible values.</param>
    ///<param name="action">Resource requested</param>
    ///<returns></returns>
    protected XmlDocument ExecuteXml(string method, string action)
    {
      // calling request
      HttpWebResponse response = ExecuteWeb(method, action);

      // get content
      string content = new StreamReader(response.GetResponseStream()).ReadToEnd();
      Global.DebugPrint("Response content : \n" + content);
      if (Global.Debug) File.WriteAllText("return.xml", content);

      // parsing response
      XmlDocument xml = new XmlDocument();
      try
      {
        xml.LoadXml(content);
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse result...");
        throw new ApiException(ApiException.ErrorCode.Parsing, 0, "Unable to parse result : " + e.Message);
      }

      checkResponseStatus(response.StatusCode, xml);

      return xml;
    }
  }
}
