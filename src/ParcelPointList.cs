using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;
using System.Globalization;

namespace Boxtal
{
  ///<summary>Query to load a list of parcel point close to a given address.</summary>
  ///<remarks></remarks>
  public class ParcelPointList : Query
  {
    private string _type;
    private Person _address;
    private List<string> _carriers;

    ///<summary></summary>
    ///<remarks>See <see cref="ParcelPoint.ParcelPointType"/> for all possible values</remarks>
    ///<value></value>
    public string Type {get{return _type;} set{_type = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public Person Address {get{return _address;} set{_address = value;}}
    ///<summary>Carrier codes of the carrier associated with this parcel point.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<string> Carriers {get{return _carriers;} set{_carriers = value;}}

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="type"></param>
    ///<param name="address"></param>
    public ParcelPointList(string type, Person address)
    {
      Type = ParcelPoint.ParcelPointType.Dropoff;
      Address = address;
      Carriers = new List<string>();
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="type"></param>
    ///<param name="address"></param>
    ///<param name="carrierCodes"></param>
    public ParcelPointList(string type, Person address, List<string> carrierCodes)
    :this(type, address)
    {
      Carriers = carrierCodes;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="type"></param>
    ///<param name="address"></param>
    ///<param name="carrierCode"></param>
    public ParcelPointList(string type, Person address, string carrierCode)
    :this(type, address)
    {
      Carriers.Add(carrierCode);
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns></returns>
    override protected Dictionary<string, string> ToQueryParameters()
    {
      Dictionary<string, string> result = base.ToQueryParameters();

      int serviceCount = 0;
      foreach(string carrier in Carriers)
      {
        result["carriers["+serviceCount+"]"] = carrier.Insert(4, "_");
        serviceCount++;
      }
      result["collecte"] = (Type == ParcelPoint.ParcelPointType.Dropoff) ? "exp" : "dest";
      result["pays"] = Address.CountryIso;
      result["cp"] = Address.PostalCode;
      result["ville"] = Address.Town;

      return result;
    }

    ///<summary>Load a list of parcel point for each carrier code.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public Dictionary<string, List<ParcelPoint>> Get()
    {
      // parsing response
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/listpoints");

      Dictionary<string, List<ParcelPoint>> result = new Dictionary<string, List<ParcelPoint>>();
      try
      {
        XmlNodeList carrierNodes = xml.SelectNodes("carriers/carrier");
        foreach (XmlNode carrierNode in carrierNodes)
        {

          XmlNode operatorNode = carrierNode["operator"];
          XmlNode serviceNode = carrierNode["service"];

          string codeService = operatorNode.InnerText + serviceNode.InnerText;
          result[codeService] = new List<ParcelPoint>();

          XmlNodeList parcelNodes = carrierNode.SelectNodes("points/point");
          foreach (XmlNode parcelNode in parcelNodes)
          {
            XmlNode codeNode = parcelNode["code"];
            XmlNode nameNode = parcelNode["name"];
            XmlNode addressNode = parcelNode["address"];
            XmlNode countryNode = parcelNode["country"];
            XmlNode phoneNode = parcelNode["phone"];
            XmlNode descriptionNode = parcelNode["description"];
            XmlNode cityNode = parcelNode["city"];
            XmlNode zipcodeNode = parcelNode["zipcode"];
            XmlNode latitudeNode = parcelNode["latitude"];
            XmlNode longitudeNode = parcelNode["longitude"];
            XmlNodeList daysNodes = parcelNode["schedule"].SelectNodes("day");

            string code = String.Join("", codeNode.InnerText.Split('_'));
            string ope = code.Substring(0, 4);
            string name = nameNode.InnerText;
            string address = addressNode.InnerText;
            string country = countryNode.InnerText;
            string city = cityNode.InnerText;
            string zipcode = zipcodeNode.InnerText;
            string phone = phoneNode.InnerText;
            string description = descriptionNode.InnerText;
            float latitude = float.Parse(latitudeNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
            float longitude = float.Parse(longitudeNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
            Dictionary<string, ParcelPoint.ParcelPointSchedule> schedule = new Dictionary<string, ParcelPoint.ParcelPointSchedule>();

            foreach (XmlNode dayNode in daysNodes)
            {
              XmlNode weekDayNode = dayNode["weekday"];
              XmlNode openAmNode = dayNode["open_am"];
              XmlNode closeAmNode = dayNode["close_am"];
              XmlNode openPmNode = dayNode["open_pm"];
              XmlNode closePmNode = dayNode["close_pm"];

              if (!schedule.ContainsKey(weekDayNode.InnerText))
              {
                schedule.Add(weekDayNode.InnerText, new ParcelPoint.ParcelPointSchedule(
                  (openAmNode.InnerText == "") ? null : openAmNode.InnerText,
                  (closeAmNode.InnerText == "") ? null : closeAmNode.InnerText,
                  (openPmNode.InnerText == "") ? null : openPmNode.InnerText,
                  (closePmNode.InnerText == "") ? null : closePmNode.InnerText
                ));
              }
            }
            result[codeService].Add( new ParcelPoint(ope, code, name, address, country, city, zipcode, phone,
              description, latitude, longitude, schedule));
          }
        }
      }
      catch (Exception e)
      {
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Failed to parse parcel point list : " + e.Message);
      }

      return result;
    }
  }
}
