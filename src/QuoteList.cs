using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;
using System.Globalization;

namespace Boxtal
{
  ///<summary>Query to load a list of quotes for a shipping.</summary>
  ///<remarks></remarks>
  public class QuoteList : Shipping
  {
    private List<string> _carrier = new List<string>();

    ///<summary>List of carrier wanted for the quotation.</summary>
    ///<remarks>If the carriers list is empty, will load all available carriers. The query time will take more time proportionaly with the number of carriers.</remarks>
    ///<value></value>
    public List<string> Carriers {get{return _carrier;} set{_carrier = value;}}

    private static Quote.Parameter parseParameter(XmlNode node)
    {
      XmlNode codeNode = node["code"];
      XmlNode labelNode = node["label"];
      XmlNode typeNode = node["type"].FirstChild;

      string code = codeNode.InnerText;
      string label = labelNode.InnerText;

      return new Quote.Parameter(code, label, typeNode);
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="from"></param>
    ///<param name="to"></param>
    ///<param name="packageType"></param>
    public QuoteList(Person from, Person to, string packageType)
    : base(from, to, packageType)
    {}

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns></returns>
    override protected Dictionary<string, string> ToQueryParameters()
    {
      Dictionary<string, string> result = base.ToQueryParameters();

      int carrierCount = 0;
      foreach(string carrier in Carriers)
      {
        result.Add("offers["+carrierCount+"]", carrier);
        carrierCount++;
      }

      return result;
    }

    ///<summary>Load a list of quotes corresponding to the given parameters.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public List<Quote> Get()
    {
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/cotation");

      // parsing response
      List<Quote> result = new List<Quote>();
      try
      {
        XmlNodeList offerNodes = xml.SelectNodes("cotation/shipment/offer");
        foreach (XmlNode offerNode in offerNodes)
        {
          XmlNode operatorNode = offerNode["operator"];
          XmlNode operatorLabelNode = operatorNode["label"];
          XmlNode operatorCodeNode = operatorNode["code"];
          XmlNode serviceNode = offerNode["service"];
          XmlNode serviceLabelNode = serviceNode["label"];
          XmlNode serviceCodeNode = serviceNode["code"];
          XmlNode logoNode = operatorNode["logo"];
          XmlNode priceNode = offerNode["price"];
          XmlNode currencyNode = priceNode["currency"];
          XmlNode taxExclusiveNode = priceNode["tax-exclusive"];
          XmlNode taxInclusiveNode = priceNode["tax-inclusive"];
          XmlNodeList characteristicsNodes = offerNode.SelectNodes("characteristics/label");
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
          XmlNodeList mandatoryInformationNodes = offerNode.SelectNodes("mandatory_informations/parameter");
          XmlNodeList optionNodes = offerNode.SelectNodes("options/option");

          string operatorLabel = operatorLabelNode.InnerText;
          string operatorCode = operatorCodeNode.InnerText;
          string serviceCode = serviceCodeNode.InnerText;
          string serviceLabel = serviceLabelNode.InnerText;
          string logo = logoNode.InnerText;
          string currency = currencyNode.InnerText;
          float priceTaxExcl = float.Parse(taxExclusiveNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
          float priceTaxIncl = float.Parse(taxInclusiveNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
          List<string> characteristics = new List<string>();
          foreach(XmlNode characteristicNode in characteristicsNodes)
          {
            characteristics.Add(characteristicNode.InnerText);
          }
          string collectionCodeType = collectionCodeTypeNode.InnerText;
          string collectionLabelType = collectionLabelTypeNode.InnerText;
          DateTime collectionDate = DateTime.ParseExact(collectionDateNode.InnerText, Global.Dateformat, null);
          string collectionLabel = collectionLabelNode.InnerText;
          string deliveryCodeType = deliveryCodeTypeNode.InnerText;
          string deliveryLabelType = deliveryLabelTypeNode.InnerText;
          DateTime deliveryDate = DateTime.ParseExact(deliveryDateNode.InnerText, Global.Dateformat, null);
          string deliveryLabel = deliveryLabelNode.InnerText;

          List<Quote.Parameter> parameters = new List<Quote.Parameter>();
          foreach(XmlNode mandatoryInformationNode in mandatoryInformationNodes)
          {
            parameters.Add(parseParameter(mandatoryInformationNode));
          }

          List<Quote.Option> options = new List<Quote.Option>();
          foreach(XmlNode optionNode in optionNodes)
          {
            XmlNode codeNode = optionNode["code"];
            XmlNode nameNode = optionNode["name"];
            XmlNode optionDescriptionNode = optionNode["description"];
            XmlNodeList parametesrNode = optionNode.SelectNodes("parameter");
            XmlNode optionPriceNode = optionNode["price"];
            XmlNode optionPriceCurrencyNode = (optionPriceNode == null) ? null : optionPriceNode["currency"];
            XmlNode optionTaxExclusiveNode = (optionPriceNode == null) ? null : optionPriceNode["tax-exclusive"];
            XmlNode optionTaxInclusiveNode = (optionPriceNode == null) ? null : optionPriceNode["tax-inclusive"];

            string optionCode = codeNode.InnerText;
            string optionName = nameNode.InnerText;
            string optionDescription = (optionDescriptionNode==null) ? "" : optionDescriptionNode.InnerText;
            string optionCurrency = (optionPriceCurrencyNode==null) ? "" : optionPriceCurrencyNode.InnerText;
            float optionPriceTaxExcl = (optionTaxExclusiveNode == null) ? 0.0f :
              float.Parse(optionTaxExclusiveNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
            float optionPriceTaxIncl = (optionTaxInclusiveNode == null) ? 0.0f :
              float.Parse(optionTaxInclusiveNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
            List<Quote.Parameter> optionParameters = new List<Quote.Parameter>();
            foreach(XmlNode optionParameterNode in parametesrNode)
            {
              optionParameters.Add(parseParameter(optionParameterNode));
            }

            options.Add(new Quote.Option(optionCode, optionName, optionDescription, optionCurrency, optionPriceTaxExcl, optionPriceTaxIncl, optionParameters));
          }

          result.Add(new Quote(operatorCode, serviceCode, operatorLabel, serviceLabel, logo, currency, priceTaxIncl, priceTaxExcl,
            characteristics, collectionCodeType, collectionLabelType, collectionDate, collectionLabel, deliveryCodeType,
            deliveryLabelType, deliveryDate, deliveryLabel, parameters, options));
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse quotes ...");
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Unable to parse quotes : " + e.Message);
      }

      return result;
    }

    ///<summary>Create a new order query with the same parameters.</summary>
    ///<remarks></remarks>
    ///<param name="carrierCode">Carrier code chosen for the order.</param>
    ///<returns></returns>
    public Order ToOrder(string carrierCode)
    {
      return new Order(carrierCode, this);
    }
  }
}
