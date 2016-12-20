using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;

namespace Boxtal
{
  ///<summary>Query for a passed order status.</summary>
  ///<remarks></remarks>
  public class Status : Query
  {
    private string _reference;
    private string _state;
    private string _carrierReference;
    private bool _orderAccepted;
    private Dictionary<string, Document> _documents;

    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Reference {get{return _reference;}}
    ///<summary></summary>
    ///<remarks>Use <see cref="Status.Load"/> to load this value.</remarks>
    ///<value></value>
    public string State {get{return _state;}}
    ///<summary>Reference used by the carrier (and not Boxtal) for tracking your order.</summary>
    ///<remarks>Use <see cref="Status.Load"/> to load this value.</remarks>
    ///<value></value>
    public string CarrierReference {get{return _carrierReference;}}
    ///<summary></summary>
    ///<remarks>Use <see cref="Status.Load"/> to load this value.</remarks>
    ///<value>true if shipping label has been generated.</value>
    public bool OrderAccepted {get{return _orderAccepted;}}
    ///<summary></summary>
    ///<remarks>Use <see cref="Status.Load"/> to load this value.</remarks>
    ///<value></value>
    public Dictionary<string, Document> Documents {get{return _documents;}}

    ///<summary>Create a new status request with the given order reference.</summary>
    ///<remarks></remarks>
    ///<param name="reference"></param>
    public Status(string reference)
    {
      _documents = new Dictionary<string, Document>();
      _reference = reference;
    }

    ///<summary>Load the order's status.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public void Load()
    {
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/order_status/" + Reference + "/informations");

      try
      {
        XmlNode orderNode = xml["order"];
        XmlNode stateNode = orderNode["state"];
        XmlNode carrierReferenceNode = orderNode["carrier_reference"];
        XmlNode labelAvailableNode = orderNode["label_available"];
        XmlNode labelUrlNode = orderNode["label_url"];
        XmlNodeList labelNodes = orderNode.SelectNodes("labels/*");

        _state = stateNode.InnerText;
        _carrierReference = carrierReferenceNode.InnerText;
        _orderAccepted = labelAvailableNode.InnerText == "1";

        _documents.Add("label", new Document(labelUrlNode.InnerText));
        foreach(XmlNode labelNode in labelNodes)
        {
          if (labelNode.Name != "label")
          {
            _documents.Add(labelNode.Name, new Document(labelNode.InnerText));
          }
        }
      }
      catch (NullReferenceException e)
      {
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Failed to parse order status : " + e.Message);
      }
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the order status.</returns>
    override public string ToString()
    {
      string result = "Status of order " + Reference;
      result += "\n  State : " + State;
      result += "\n  Order accepted : " + (OrderAccepted?"yes":"no");
      result += "\n  Carrier tracking reference : " + CarrierReference;
      result += "\n  Documents available : ";
      foreach(KeyValuePair<string, Document> pair in Documents)
      {
        result += "\n    " + pair.Key;
      }
      return result;
    }
  }
}
