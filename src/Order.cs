using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace Boxtal
{
  ///<summary>Query to create an order.</summary>
  ///<remarks></remarks>
  public class Order : Shipping
  {
    private string _reference;
    private List<Document> _documents = new List<Document>();
    private string _currency;
    private float _priceTaxIncl;
    private float _priceTaxExcl;
    private List<PriceDetail> _priceDetails = new List<PriceDetail>();
    private string _collectionCodeType;
    private string _collectionLabelType;
    private DateTime _collectionDate;
    private string _collectionLabel;
    private string _deliveryCodeType;
    private string _deliveryLabelType;
    private DateTime _deliveryDate;
    private string _deliveryLabel;
    private string _alert;
    private List<string> _characteristics = new List<string>();

    ///<summary>Unique carrier code linked to the order.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CarrierCode {get{return Operator + Service;}}
    ///<summary>Unique reference of the created order.</summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string Reference {get{return _reference;}}
    ///<summary>List of all documents associated with the order.</summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public List<Document> Documents {get{return _documents;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string Currency {get{return _currency;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public float PriceTaxIncl {get{return _priceTaxIncl;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public float PriceTaxExcl {get{return _priceTaxExcl;}}
    ///<summary>Detailed version of the price.</summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public List<PriceDetail> PriceDetails {get{return _priceDetails;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>. See <see cref="Shipping.CollectionType"/> for a list of all collection types.</remarks>
    ///<value></value>
    public string CollectionCodeType {get{return _collectionCodeType;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string CollectionLabelType {get{return _collectionLabelType;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public DateTime CollectionDate {get{return _collectionDate;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string CollectionLabel {get{return _collectionLabel;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>. See <see cref="Shipping.DeliveryType"/> for a list of all delivery types.</remarks>
    ///<value></value>
    public string DeliveryCodeType {get{return _deliveryCodeType;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string DeliveryLabelType {get{return _deliveryLabelType;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.The delivery date is estimated and may vary.</remarks>
    ///<value></value>
    public DateTime DeliveryDate {get{return _deliveryDate;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string DeliveryLabel {get{return _deliveryLabel;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public string Alert {get{return _alert;}}
    ///<summary></summary>
    ///<remarks>You must create your order first with <see cref="Order.Create"/>.</remarks>
    ///<value></value>
    public List<string> Characteristics {get{return _characteristics;}}

    ///<summary>Represent a part of the order's price.</summary>
    ///<remarks></remarks>
    public struct PriceDetail
    {
      private string _label;
      private string _currency;
      private float _priceTaxIncl;
      private float _priceTaxExcl;

      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Label {get{return _label;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Currency{get{return _currency;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public float PriceTaxIncl{get{return _priceTaxIncl;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public float PriceTaxExcl{get{return _priceTaxExcl;}}

      ///<summary></summary>
      ///<remarks></remarks>
      ///<param name="label"></param>
      ///<param name="currency"></param>
      ///<param name="priceTaxIncl"></param>
      ///<param name="priceTaxExcl"></param>
      public PriceDetail(string label, string currency, float priceTaxIncl, float priceTaxExcl)
      {
        _label = label;
        _currency = currency;
        _priceTaxIncl = priceTaxIncl;
        _priceTaxExcl = priceTaxExcl;
      }
    }

    ///<summary>Create a new order with a carrier and a quotation.</summary>
    ///<remarks>All parameters from the created quotation are used for your new order. Throws <see cref="ApiException"/>.</remarks>
    ///<param name="carrierCode"></param>
    ///<param name="quotation"></param>
    public Order(string carrierCode, QuoteList quotation)
    :base(quotation)
    {
      setCarrierCode(carrierCode);
    }

    ///<summary></summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<param name="carrierCode"></param>
    ///<param name="from"></param>
    ///<param name="to"></param>
    ///<param name="packageType"></param>
    public Order(string carrierCode, Person from, Person to, string packageType)
    : base(from, to, packageType)
    {
      setCarrierCode(carrierCode);
    }

    private void setCarrierCode(string carrierCode)
    {
      if (carrierCode.Length <= 4)
      {
        throw new ApiException(ApiException.ErrorCode.Request, 0, "Invalid carrier code given : " + carrierCode);
      }

      Operator = carrierCode.Substring(0, 4);
      Service = carrierCode.Substring(4, carrierCode.Length - 4);
    }

    ///<summary>Return a list of missing parameters for your order.</summary>
    ///<remarks>Create a quotation request. Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public List<Quote.Parameter> getMissingParameters()
    {
      var quotation = new QuoteList(PersonFrom, PersonTo, PackageType);
      quotation.Packages = Packages;
      quotation.Carriers.Add(Operator + "_" + Service);
      quotation.Parameters = Parameters;

      var quotes = quotation.Get();

      if (quotes.Count == 0)
      {
        throw new ApiException(ApiException.ErrorCode.Request, HttpStatusCode.OK, "No carriers available for this cotation");
      }

      List<Quote.Parameter> result = new List<Quote.Parameter>();
      Dictionary<string, string> parameters = ToQueryParameters();

      foreach(Quote.Parameter param in quotes[0].Parameters)
      {
        if (param.PropertyName != null)
        {
          string code = param.Code
            .Replace("colis.", PackageType + ".")
            .Replace("envoi.raison", "raison");
          if (param.PropertyName == "Proforma")
          {
            if (Proforma.Count == 0)
            {
              result.Add(param);
            }
          }
          else if (!parameters.ContainsKey(code) || string.IsNullOrEmpty(parameters[code]))
          {
            result.Add(param);
          }
        }
      }

      return result;
    }

    ///<summary>Create a new order.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public string Create()
    {
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Post, "api/v1/order");

      // parsing response
      _documents.Clear();
      _priceDetails.Clear();
      _characteristics.Clear();
      try
      {
        XmlNode shipmentNode = xml["order"]["shipment"];
        XmlNode offerNode = shipmentNode["offer"];
        XmlNode referenceNode = shipmentNode["reference"];
        XmlNodeList labelNodes = shipmentNode.SelectNodes("labels/label");
        XmlNode priceNode = offerNode["price"];
        XmlNode currencyNode = priceNode["currency"];
        XmlNode taxExclusiveNode = priceNode["tax-exclusive"];
        XmlNode taxInclusiveNode = priceNode["tax-inclusive"];
        XmlNodeList priceDetailNodes = priceNode.SelectNodes("detail/line");
        XmlNode collectionNode = offerNode["collection"];
        XmlNode collectionTypeNode = collectionNode["type"];
        XmlNode collectionCodeTypeNode = collectionTypeNode["code"];
        XmlNode collectionLabelTypeNode = collectionTypeNode["label"];
        XmlNode collectionDateNode = collectionNode["date"];
        XmlNode collectionLabelNode = collectionNode["label"];
        XmlNode deliveryNode = offerNode["delivery"];
        XmlNode deliveryTypeNode = deliveryNode["type"];
        XmlNode deliveryCodeTypeNode = deliveryTypeNode["code"];
        XmlNode deliveryLabelTypeNode = deliveryTypeNode["label"];
        XmlNode deliveryDateNode = deliveryNode["date"];
        XmlNode deliveryLabelNode = deliveryNode["label"];
        XmlNode alertNode = offerNode["alert"];
        XmlNodeList characteristicNodes = offerNode.SelectNodes("characteristics/label");

        _reference = referenceNode.InnerText;
        _currency = currencyNode.InnerText;
        _priceTaxExcl = float.Parse(taxExclusiveNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
        _priceTaxIncl = float.Parse(taxInclusiveNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
        _collectionCodeType = collectionCodeTypeNode.InnerText;
        _collectionLabelType = collectionLabelTypeNode.InnerText;
        _collectionDate = DateTime.ParseExact(collectionDateNode.InnerText, Global.Dateformat, null);
        _collectionLabel = collectionLabelNode.InnerText;
        _deliveryCodeType = deliveryCodeTypeNode.InnerText;
        _deliveryLabelType = deliveryLabelTypeNode.InnerText;
        _deliveryDate = DateTime.ParseExact(deliveryDateNode.InnerText, Global.Dateformat, null);
        _deliveryLabel = deliveryLabelNode.InnerText;
        _alert = (alertNode != null) ? alertNode.InnerText.Trim() : null;
        foreach(XmlNode labelNode in labelNodes)
        {
          _documents.Add(new Document(labelNode.InnerText));
        }
        foreach(XmlNode priceDetailNode in priceDetailNodes)
        {
          XmlNode priceDetailLabelNode = priceDetailNode["label"];
          XmlNode priceDetailCurrencyNode = priceDetailNode["currency"];
          XmlNode priceDetailPriceTaxExclNode = priceDetailNode["tax-exclusive"];
          XmlNode priceDetailPriceTaxInclNode = priceDetailNode["tax-inclusive"];

          string priceDetailLabel = priceDetailLabelNode.InnerText;
          string priceDetailCurrency = priceDetailCurrencyNode.InnerText;
          float priceDetailPriceTaxExcl = float.Parse(priceDetailPriceTaxExclNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
          float priceDetailPriceTaxIncl = float.Parse(priceDetailPriceTaxInclNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);

          _priceDetails.Add(new PriceDetail(priceDetailLabel, priceDetailCurrency, priceDetailPriceTaxExcl, priceDetailPriceTaxIncl));
        }
        foreach(XmlNode characteristicNode in characteristicNodes)
        {
          _characteristics.Add(characteristicNode.InnerText.Trim());
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse order response ...");
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Unable to parse order response : " + e.Message);
      }

      return Reference;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the order.</returns>
    public override string ToString()
    {
      string result = "Order " + Reference;
      result += "\n  Carrier code : " + CarrierCode;
      result += "\n  Price : " + PriceTaxExcl + " " + Currency + " TE, " + PriceTaxIncl + " " + Currency + " TI";
      result += "\n  Price details :";
      foreach(PriceDetail detail in PriceDetails)
      {
        result += "\n    " + detail.Label + " : " + detail.PriceTaxExcl + " " + detail.Currency + " TE, "
          + detail.PriceTaxIncl + " " + detail.Currency + " TI";
      }
      result += "\n  Collection : ";
      result += "\n    Type : " + CollectionLabelType + " (" + CollectionCodeType + ")";
      result += "\n    Date : " + CollectionDate.ToString(Global.Dateformat);
      result += "\n    Details : " + CollectionLabel;
      result += "\n  Delivery : ";
      result += "\n    Type : " + DeliveryLabelType + " (" + DeliveryCodeType + ")";
      result += "\n    Date : " + DeliveryDate.ToString(Global.Dateformat);
      result += "\n    Details : " + DeliveryLabel;
      if (Alert != null)
      {
        result += "\n  Alert : ";
        result += "\n  " + Alert;
      }
      result += "\n  Characteristics :";
      foreach(string characteristic in Characteristics)
      {
        result += "\n    " + characteristic;
      }
      return result;
    }
  }
}
