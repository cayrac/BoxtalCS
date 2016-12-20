using System;
using System.Xml;
using System.Collections.Generic;

namespace Boxtal
{
  ///<summary>Describe a quote for a single carrier service.</summary>
  ///<remarks></remarks>
  public class Quote
  {
    ///<summary>Describe a quote parameter required for an order.</summary>
    ///<remarks></remarks>
    public struct Parameter
    {
      private static Dictionary<string, string> propertyNames = new Dictionary<string, string> {
        {"depot.pointrelais", "DropParcelPoint"},
        {"retrait.pointrelais", "PickupParcelPoint"},
        {"disponibilite.HDE", "DisponibilityMin"},
        {"disponibilite.HLE", "DisponibilityMax"},
        {"code_contenu", "ContentType"},
        {"collecte", "PickupDate"},
        {"pli.description", "Description"},
        {"colis.description", "Description"},
        {"encombrant.description", "Description"},
        {"palette.description", "Description"},
        {"pli.valeur", "Value"},
        {"colis.valeur", "Value"},
        {"encombrant.valeur", "Value"},
        {"palette.valeur", "Value"},
        {"envoi.raison", "Raison"},
        {"proforma.description_fr", "Proforma"},
        {"proforma.description_en", "Proforma"},
        {"proforma.nombre", "Proforma"},
        {"proforma.poids", "Proforma"},
        {"proforma.valeur", "Proforma"},
        {"proforma.origine", "Proforma"}
      };

      private string _code;
      private string _label;
      private string _propertyName;
      private XmlNode _type;

      ///<summary>Parameter code as expected in <see cref="Query.Parameters"/>.</summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Code {get{return _code;}}
      ///<summary>Message describing the parameter</summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Label {get{return _label;}}
      ///<summary>return the property name if there is one.</summary>
      ///<remarks></remarks>
      ///<value></value>
      public string PropertyName {get{return _propertyName;}}
      ///<summary>THe XML node describing the type expected for this parameter.</summary>
      ///<remarks>May be a primitive type or enumeration of values.</remarks>
      ///<value></value>
      public XmlNode Type {get{return _type;}}

      ///<summary></summary>
      ///<remarks></remarks>
      ///<param name="code"></param>
      ///<param name="label"></param>
      ///<param name="type"></param>
      public Parameter(string code, string label, XmlNode type)
      {
        _code = code;
        _label = label;
        _type = type;

        _propertyName = propertyNames.ContainsKey(code) ? propertyNames[code] : null;
      }

      ///<summary></summary>
      ///<remarks></remarks>
      ///<returns>String representation of the parameter.</returns>
      public override string ToString()
      {
        return ToString(0);
      }

      ///<summary></summary>
      ///<remarks></remarks>
      ///<returns></returns>
      public string ToString(int level)
      {
        string tab = "";
        for (uint i = 0; i < level; i++)
        {
          tab += "  ";
        }
        // special cases
        string type = "";
        if (Code == "depot.pointrelais" || Code == "retrait.pointrelais")
        {
          type = "Parcel point code";
        }
        else
        {
          type = Type.Name;
        }

        string result = tab + "Parameter " + Code + " : \n";
        result += tab + "  Label : " + Label + "\n";
        if (PropertyName != null)
        {
          result += tab + "  Property name : " + PropertyName + "\n";
        }
        result += tab + "  Type : " + type;
        return result;
      }
    }

    ///<summary>Describe an option available for this shipping.</summary>
    ///<remarks></remarks>
    public struct Option
    {
      private string _code;
      private string _name;
      private string _description;
      private string _currency;
      private float _priceTaxExcl;
      private float _priceTaxIncl;
      private List<Parameter> _parameters;

      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Code {get{return _code;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Name {get{return _name;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Description {get{return _description;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public string Currency {get{return _currency;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public float PriceTaxExcl {get{return _priceTaxExcl;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public float PriceTaxIncl {get{return _priceTaxIncl;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value></value>
      public List<Parameter> Parameters {get{return _parameters;}}

      ///<summary></summary>
      ///<remarks></remarks>
      ///<param name="code"></param>
      ///<param name="name"></param>
      ///<param name="description"></param>
      ///<param name="currency"></param>
      ///<param name="priceTaxExcl"></param>
      ///<param name="priceTaxIncl"></param>
      ///<param name="parameters"></param>
      public Option(string code, string name, string description, string currency, float priceTaxExcl, float priceTaxIncl, List<Parameter> parameters)
      {
        _code = code;
        _name = name;
        _description = description;
        _currency = currency;
        _priceTaxExcl = priceTaxExcl;
        _priceTaxIncl = priceTaxIncl;
        _parameters = parameters;
      }

      ///<summary></summary>
      ///<remarks></remarks>
      ///<returns>String representation of the option.</returns>
      public override string ToString()
      {
        return ToString(0);
      }

      ///<summary></summary>
      ///<remarks></remarks>
      ///<returns></returns>
      public string ToString(int level)
      {
        string tab = "";
        for (uint i = 0; i < level; i++)
        {
          tab += "  ";
        }
        string result = tab + "Option " + Name + " : \n";
        result += tab + "  Code : " + Code + "\n";
        result += tab + "  Description : " + Description + "\n";
        result += tab + "  Price : " + PriceTaxIncl + " " + _currency + " TE, " + PriceTaxExcl + " " + _currency + " TI";
        foreach(Parameter param in Parameters)
        {
          result += "\n" + param.ToString(level);
        }
        return result;
      }
    }

    private string _opeCode;
    private string _serviceCode;
    private string _ope;
    private string _service;
    private string _logo;
    private string _currency;
    private float _priceTaxIncl;
    private float _priceTaxExcl;
    private List<string> _characteristics;
    private string _collectionCodeType;
    private string _collectionLabelType;
    private DateTime _collectionDate;
    private string _collectionLabel;
    private string _deliveryCodeType;
    private string _deliveryLabelType;
    private DateTime _deliveryDate;
    private string _deliveryLabel;
    private List<Parameter> _parameters;
    private List<Option> _options;

    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string OpeCode {get{return _opeCode;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string ServiceCode {get{return _serviceCode;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CarrierCode {get{return OpeCode + ServiceCode;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Ope {get{return _ope;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Service {get{return _service;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Logo {get{return _logo;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Currency {get{return _currency;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float PriceTaxIncl {get{return _priceTaxIncl;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float PriceTaxExcl {get{return _priceTaxExcl;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<string> Characteristics {get{return _characteristics;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CollectionCodeType {get{return _collectionCodeType;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CollectionLabelType {get{return _collectionLabelType;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public DateTime CollectionDate {get{return _collectionDate;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CollectionLabel {get{return _collectionLabel;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DeliveryCodeType {get{return _deliveryCodeType;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DeliveryLabelType {get{return _deliveryLabelType;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public DateTime DeliveryDate {get{return _deliveryDate;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DeliveryLabel {get{return _deliveryLabel;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<Parameter> Parameters {get{return _parameters;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<Option> Options {get{return _options;}}

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="opeCode"></param>
    ///<param name="serviceCode"></param>
    ///<param name="ope"></param>
    ///<param name="service"></param>
    ///<param name="logo"></param>
    ///<param name="currency"></param>
    ///<param name="priceTaxIncl"></param>
    ///<param name="priceTaxExcl"></param>
    ///<param name="characteristics"></param>
    ///<param name="collectionCodeType"></param>
    ///<param name="collectionLabelType"></param>
    ///<param name="collectionDate"></param>
    ///<param name="collectionLabel"></param>
    ///<param name="deliveryCodeType"></param>
    ///<param name="deliveryLabelType"></param>
    ///<param name="deliveryDate"></param>
    ///<param name="deliveryLabel"></param>
    ///<param name="parameters"></param>
    ///<param name="options"></param>
    public Quote(string opeCode, string serviceCode, string ope, string service, string logo, string currency, float priceTaxIncl, float priceTaxExcl,
      List<string> characteristics, string collectionCodeType, string collectionLabelType, DateTime collectionDate, string collectionLabel,
      string deliveryCodeType, string deliveryLabelType, DateTime deliveryDate, string deliveryLabel, List<Parameter> parameters, List<Option> options)
    {
      _opeCode = opeCode;
      _serviceCode = serviceCode;
      _ope = ope;
      _service = service;
      _logo = logo;
      _currency = currency;
      _priceTaxIncl = priceTaxIncl;
      _priceTaxExcl = priceTaxExcl;
      _characteristics = characteristics;
      _collectionCodeType = collectionCodeType;
      _collectionLabelType = collectionLabelType;
      _collectionDate = collectionDate;
      _collectionLabel = collectionLabel;
      _deliveryCodeType = deliveryCodeType;
      _deliveryLabelType = deliveryLabelType;
      _deliveryDate = deliveryDate;
      _deliveryLabel = deliveryLabel;
      _parameters = parameters;
      _options = options;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the quote.</returns>
    public override string ToString()
    {
      string result = "Quote " + CarrierCode + "\n";
      result += "  Logo : " + Logo + "\n";
      result += "  Price : " + PriceTaxExcl + " " + Currency + " TE, " + PriceTaxIncl + " " + Currency + " TI\n";
      result += "  Characteristics : " + "\n";
      foreach(string characteristic in Characteristics)
      {
        result += "    " + characteristic + "\n";
      }
      result += "  Collection : " + "\n";
      result += "    Type : " + CollectionLabelType + " (" + CollectionCodeType + ")" + "\n";
      result += "    Date : " + CollectionDate.ToString(Global.Dateformat) + "\n";
      result += "    Details : " + CollectionLabel + "\n";
      result += "  Delivery : " + "\n";
      result += "    Type : " + DeliveryLabelType + " (" + DeliveryCodeType + ")" + "\n";
      result += "    Date : " + DeliveryDate.ToString(Global.Dateformat) + "\n";
      result += "    Details : " + DeliveryLabel;
      foreach(Option option in Options)
      {
        result += "\n" + option.ToString(1);
      }
      foreach(Parameter parameter in Parameters)
      {
        result += "\n"+parameter.ToString(1);
      }
      return result;
    }
  }
}
