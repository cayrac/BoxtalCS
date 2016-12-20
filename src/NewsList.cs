using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;

namespace Boxtal
{
  ///<summary>Query to list all available Boxtal news.</summary>
  ///<remarks></remarks>
  public class NewsList : Query
  {
    ///<summary>Channel used to load news.</summary>
    ///<remarks>Channel is initialized by default with <see cref="Global.ModulePlatform"/>.</remarks>
    ///<value></value>
    public string Channel
    {
        get{return Parameters["channel"];}
        set{Parameters["channel"] = value;}
    }
    ///<summary>Channel version.</summary>
    ///<remarks>Channel version is initialized by default with <see cref="Global.ModuleVersion"/>.</remarks>
    ///<value></value>
    public string Version
    {
        get{return Parameters["version"];}
        set{Parameters["version"] = value;}
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="channel"></param>
    ///<param name="version"></param>
    public NewsList(string channel, string version)
    {
      Channel = channel;
      Version = version;
    }

    ///<summary>Initialise a news list with default parameters.</summary>
    ///<remarks></remarks>
    ///<returns></returns>
    public NewsList()
    :this(Global.ModulePlatform, Global.ModuleVersion)
    {}

    ///<summary>Load a list of all available news.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public List<News> Get()
    {
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/news");

      // parsing response
      List<News> result = new List<News>();
      try
      {
        XmlNodeList newsNodes = xml.SelectNodes("flux/news");
        foreach (XmlNode newsNode in newsNodes)
        {
          XmlNode typeNode = newsNode["type"];
          XmlNode titleNode = newsNode["message_short"];
          XmlNode messageNode = newsNode["message"];

          string _type = typeNode.InnerText;
          string _title = titleNode.InnerText;
          string _message = messageNode.InnerText;

          _message = _message.Replace("[[","<").Replace("]]",">");

          result.Add(new News(_type, _title, _message));
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse news ...");
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Unable to parse news : " + e.Message);
      }

      return result;
    }
  }
}
