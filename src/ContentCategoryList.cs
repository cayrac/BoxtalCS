using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;

namespace Boxtal
{
  ///<summary>Query to list all available content category.</summary>
  ///<remarks></remarks>
  public class ContentCategoryList : Query
  {

    ///<summary></summary>
    ///<remarks></remarks>
    public ContentCategoryList() {}

    ///<summary>Load the list of all content categories</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public List<ContentCategory> Get()
    {
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/content_categories");

      // parsing response
      Dictionary<string, string> contentCategories = new Dictionary<string, string>();
      try
      {
        XmlNodeList contentCategoryNodes = xml.SelectNodes("content_categories/content_category");
        foreach (XmlNode contentCategory in contentCategoryNodes)
        {
          XmlNode codeNode = contentCategory["code"];
          XmlNode labelNode = contentCategory["label"];

          string _code = codeNode.InnerText;
          string _label = labelNode.InnerText;

          contentCategories.Add(_code, _label);
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse content categories ...");
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Unable to parse content categories : " + e.Message);
      }

      xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/contents");

      // parsing response
      List<ContentCategory> result = new List<ContentCategory>();
      try
      {
        XmlNodeList contentNodes = xml.SelectNodes("contents/content");
        foreach (XmlNode contentNode in contentNodes)
        {
          XmlNode codeNode = contentNode["code"];
          XmlNode labelNode = contentNode["label"];
          XmlNode categoryNode = contentNode["category"];

          string _code = codeNode.InnerText;
          string _label = labelNode.InnerText;
          string _category = categoryNode.InnerText;

          if (_category != "0")
          {
            result.Add(new ContentCategory(_code, _label, _category, contentCategories[_category]));
          }
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse content categories ...");
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Unable to parse content categories : " + e.Message);
      }

      return result;
    }
  }
}
