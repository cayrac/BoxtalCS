using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;

namespace Boxtal
{
  ///<summary>Loader for a document stored in Boxtal servers.</summary>
  ///<remarks></remarks>
  public class Document : Query
  {
    ///<summary>List of all document types.</summary>
    ///<remarks></remarks>
    public static class Type{
      ///<summary>Document required for the carrier in order to ship your package.</summary>
      ///<remarks></remarks>
      public const string Label = "bordereau";
      ///<summary>Special document required for POFR shipping orders.</summary>
      ///<remarks></remarks>
      public const string Remittance = "remise";
      ///<summary>Invoice describing the exact content of your parcel and it's value.</summary>
      ///<remarks></remarks>
      public const string Proforma = "proforma";
    }

    // list od production domain aliases
    private static Dictionary<string, string> aliases = new Dictionary<string, string>{
      {"documents.envoimoinscher.com", Boxtal.Environment.Hosts[Boxtal.Environment.Prod]},
      {"www.boxtal.es", Boxtal.Environment.Hosts[Boxtal.Environment.Prod]},
      {"www.boxtal.it", Boxtal.Environment.Hosts[Boxtal.Environment.Prod]},
      {"www.boxtal.fr", Boxtal.Environment.Hosts[Boxtal.Environment.Prod]},
      {"www.boxtal.co.uk", Boxtal.Environment.Hosts[Boxtal.Environment.Prod]},
      {"www.boxtal.com", Boxtal.Environment.Hosts[Boxtal.Environment.Prod]}
    };

    private string _resource = null;
    private string _content = null;

    ///<summary>Full uri to the document.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Uri {get{return Boxtal.Environment.Hosts[Global.Environment] + _resource;}}
    ///<summary>Content of the loaded document.</summary>
    ///<remarks>The document have to be loaded first.</remarks>
    ///<value></value>
    public string Content {get{return _content;}}

    ///<summary>Constructor by copy.</summary>
    ///<remarks></remarks>
    ///<param name="document"></param>
    ///<returns></returns>
    public Document(Document document)
    {
      _resource = document._resource;
      _content = document._content;
    }

    ///<summary>Create a new document from it's uri.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<param name="uri"></param>
    ///<returns></returns>
    public Document(string uri)
    {

      if (!uri.StartsWith("http://") && !uri.StartsWith("https://"))
      {
        uri = "http://" + uri;
      }

      if (System.Uri.IsWellFormedUriString(uri, UriKind.Absolute))
      {
        string hostParam = new Uri(uri).Host;
        string normalizedHostParam = hostParam;
        string hostEnv = new Uri(Boxtal.Environment.Hosts[Global.Environment]).Host;
        if (aliases.ContainsKey(normalizedHostParam))
        {
          normalizedHostParam = new Uri(aliases[normalizedHostParam]).Host;
        }
        if (normalizedHostParam != hostEnv)
        {
          throw new ApiException(ApiException.ErrorCode.Request, 0, "The uri given do not match the environment's domain (" + hostEnv + ") : " + hostParam);
        }

        string[] splited = uri.Split(new string[] { hostParam }, StringSplitOptions.None);
        _resource = splited[1];
      }
      else
      {
        throw new ApiException(ApiException.ErrorCode.Request, 0, "The uri given is not valid");
      }

      if (_resource.StartsWith("/"))
      {
        _resource = _resource.Substring(1, _resource.Length-1);
      }
    }

    ///<summary>Create a new Document for multiple orders.</summary>
    ///<remarks></remarks>
    ///<param name="type">Type of document to load.</param>
    ///<param name="references">Array of order references.</param>
    ///<returns></returns>
    public Document(string type, string[] references)
    :this(type, string.Join(",", references))
    {
    }

    ///<summary>Create a new Document for an order.</summary>
    ///<remarks></remarks>
    ///<param name="type">Type of document to load.</param>
    ///<param name="reference">Order reference.</param>
    ///<returns></returns>
    public Document(string type, string reference)
    {
      _resource = "documents?type=" + type + "&envoi=" + reference;
    }

    ///<summary>Load the document's content.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public void Load()
    {
      HttpWebResponse response = ExecuteWeb(WebRequestMethods.Http.Get, _resource);


      if (response.StatusCode == HttpStatusCode.NotFound)
      {
        throw new ApiException(ApiException.ErrorCode.Access, HttpStatusCode.NotFound, "The requested resource do not exists");
      }
      if (response.StatusCode != HttpStatusCode.OK)
      {
        throw new ApiException(ApiException.ErrorCode.Access, response.StatusCode, "Failed to load resource");
      }

      try
      {
        _content = new StreamReader(response.GetResponseStream()).ReadToEnd();
      }
      catch (Exception e)
      {
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Failed to load document : " + e.Message);
      }

      if (!_content.StartsWith("%PDF-"))
      {
        Console.WriteLine(_content.Substring(0,10));
        throw new ApiException(ApiException.ErrorCode.Access, response.StatusCode, "Resource is not of format PDF");
      }
    }

    ///<summary>Save the document to the disc.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<param name="filename">path to the file to create</param>
    ///<returns></returns>
    public void Save(string filename)
    {
      if (Content == "")
      {
        throw new ApiException(ApiException.ErrorCode.Client, 0, "The document you want to save is empty or has not been loaded yet");
      }
      try{
        File.WriteAllText(filename, Content);
      }
      catch(Exception e)
      {
        throw new ApiException(ApiException.ErrorCode.Client, 0, "Failed to save document : " + e.Message);
      }
    }
  }
}
