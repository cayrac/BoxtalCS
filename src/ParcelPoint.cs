using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;
using System.Globalization;

namespace Boxtal
{
  ///<summary>Describe a parcel point with it's availabilities.</summary>
  ///<remarks>See <see cref="ParcelPointList"/> to list all available parcel point around an address.</remarks>
  public class ParcelPoint : Query
  {
    ///<summary>List of week days.</summary>
    ///<remarks></remarks>
    public static Dictionary<string, string> WeekDays = new Dictionary<string, string> {
      {"1", "Monday"},
      {"2", "Tuesday"},
      {"3", "Wednesday"},
      {"4", "Thursday"},
      {"5", "Friday"},
      {"6", "Saturday"},
      {"7", "Sunday"},
    };

    ///<summary>Describe aparcel point opening hours for a day.</summary>
    ///<remarks></remarks>
    public struct ParcelPointSchedule
    {
      private string _openAM;
      private string _closeAM;
      private string _openPM;
      private string _closePM;

      ///<summary></summary>
      ///<remarks></remarks>
      ///<value>format HH:MM.</value>
      public string OpenAM {get{return _openAM;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value>format HH:MM.</value>
      public string CloseAM {get{return _closeAM;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value>format HH:MM.</value>
      public string OpenPM {get{return _openPM;}}
      ///<summary></summary>
      ///<remarks></remarks>
      ///<value>format HH:MM.</value>
      public string ClosePM {get{return _closePM;}}

      ///<summary></summary>
      ///<remarks></remarks>
      ///<param name="openAM"></param>
      ///<param name="closeAM"></param>
      ///<param name="openPM"></param>
      ///<param name="closePM"></param>
      public ParcelPointSchedule(string openAM, string closeAM, string openPM, string closePM)
      {
        _openAM = openAM;
        _closeAM = closeAM;
        _openPM = openPM;
        _closePM = closePM;
      }
    }

    ///<summary>List of parcel point types.</summary>
    ///<remarks></remarks>
    public static class ParcelPointType
    {
      ///<summary>Parcel point where your package will be collected.</summary>
      ///<remarks></remarks>
      public static string Dropoff = "dropoff_point";
      ///<summary>Parcel point where your package will be delivered.</summary>
      ///<remarks></remarks>
      public static string Pickup = "pickup_point";
    }

    private string _ope;
    private string _code;
    private string _type;
    private string _country;
    private string _name;
    private string _address;
    private string _city;
    private string _zipcode;
    private float _latitude;
    private float _longitude;
    private string _phone;
    private string _description;
    private Dictionary<string, ParcelPointSchedule> _schedule;

    ///<summary>Operator code of the carrier associated with this parcel point.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Ope {get{return _ope;}}
    ///<summary>Carrier code of the carrier associated with this parcel point.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CarrierCode {get{return _code;}}
    ///<summary>Type of parcel point.</summary>
    ///<remarks>See <see cref="ParcelPoint.ParcelPointType"/> for all possible values</remarks>
    ///<value></value>
    public string Type {get{return _type;}}
    ///<summary>Country iso code.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Country {get{return _country;}}
    ///<summary>Parcel point name.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Name {get{return _name;}}
    ///<summary>Parcel point address.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Address {get{return _address;}}
    ///<summary>Parcel point city.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string City {get{return _city;}}
    ///<summary>Parcel point zipcode.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Zipcode {get{return _zipcode;}}
    ///<summary>Parcel point latitude localization.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Latitude {get{return _latitude;}}
    ///<summary>Parcel point longitude localization.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Longitude {get{return _longitude;}}
    ///<summary>Parcel point phone number.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Phone {get{return _phone;}}
    ///<summary>Parcel point description.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Description {get{return _description;}}
    ///<summary>Parcel point opening hours.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public Dictionary<string, ParcelPointSchedule> Schedule {get{return _schedule;}}

    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    private ParcelPoint(){}

    ///<summary>Build a parcel point by it's code and location.</summary>
    ///<remarks>Use <see cref="ParcelPoint.Load"/> to load it's informations.</remarks>
    ///<param name="type"></param>
    ///<param name="code"></param>
    ///<param name="country"></param>
    public ParcelPoint(string type, string code, string country)
    {
      _type = type;
      _code = code;
      _country = country;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="ope"></param>
    ///<param name="code"></param>
    ///<param name="name"></param>
    ///<param name="address"></param>
    ///<param name="country"></param>
    ///<param name="city"></param>
    ///<param name="zipcode"></param>
    ///<param name="phone"></param>
    ///<param name="description"></param>
    ///<param name="latitude"></param>
    ///<param name="longitude"></param>
    ///<param name="schedule"></param>
    public ParcelPoint(string ope, string code, string name, string address, string country, string city,
      string zipcode, string phone, string description, float latitude, float longitude, Dictionary<string, ParcelPointSchedule> schedule)
    {
      _ope = ope;
      _code = code;
      _name = name;
      _address = address;
      _country = country;
      _city = city;
      _zipcode = zipcode;
      _phone = phone;
      _description = description;
      _latitude = latitude;
      _longitude = longitude;
      _schedule = schedule;
    }

    ///<summary>Load all parcel point informations</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    public void Load()
    {
      XmlDocument data = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/" + Type + "/" + CarrierCode.Insert(4, "_") + "/" + Country + "/informations");

      try
      {
        XmlNode nameNode = data["name"];
        XmlNode codeNode = data["code"];
        XmlNode addressNode = data["address"];
        XmlNode countryNode = data["country"];
        XmlNode phoneNode = data["phone"];
        XmlNode descriptionNode = data["description"];
        XmlNode cityNode = data["city"];
        XmlNode zipcodeNode = data["zipcode"];
        XmlNode latitudeNode = data["latitude"];
        XmlNode longitudeNode = data["longitude"];
        XmlNodeList daysNodes = data["schedule"].SelectNodes("day");

        _code = String.Join("", codeNode.InnerText.Split('_'));
        _ope = _code.Substring(0, 4);
        _name = nameNode.InnerText;
        _address = addressNode.InnerText;
        _country = countryNode.InnerText;
        _city = cityNode.InnerText;
        _zipcode = zipcodeNode.InnerText;
        _phone = phoneNode.InnerText;
        _description = descriptionNode.InnerText;
        _latitude = float.Parse(latitudeNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
        _longitude = float.Parse(longitudeNode.InnerText , CultureInfo.InvariantCulture.NumberFormat);
        _schedule = new Dictionary<string, ParcelPointSchedule>();

        foreach (XmlNode dayNode in daysNodes)
        {
          XmlNode weekDayNode = dayNode["weekday"];
          XmlNode openAmNode = dayNode["open_am"];
          XmlNode closeAmNode = dayNode["close_am"];
          XmlNode openPmNode = dayNode["open_pm"];
          XmlNode closePmNode = dayNode["close_pm"];

          if (!Schedule.ContainsKey(weekDayNode.InnerText))
          {
            Schedule.Add(weekDayNode.InnerText, new ParcelPointSchedule(
              (openAmNode.InnerText == "") ? null : openAmNode.InnerText,
              (closeAmNode.InnerText == "") ? null : closeAmNode.InnerText,
              (openPmNode.InnerText == "") ? null : openPmNode.InnerText,
              (closePmNode.InnerText == "") ? null : closePmNode.InnerText
            ));
          }
        }
      }
      catch (NullReferenceException e)
      {
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Failed to parse parcel point infos : " + e.Message);
      }
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the parcel point.</returns>
    override public string ToString()
    {
      string result = "Parcel " + CarrierCode + " :\n";
      result += "Name    : " + Name + "\n";
      result += "Country : " + Country + "\n";
      result += "Town    : " + City + "\n";
      result += "Zipcode : " + Zipcode + "\n";
      result += "Address : " + Address + "\n";
      result += "Phone   : " + Phone + "\n";
      result += "Description : " + Description + "\n";
      result += "Geoloc  : " + Latitude + ", " + Longitude + "\n";
      result += "Schedule :";
      foreach(KeyValuePair<string, ParcelPointSchedule> entry in Schedule)
      {
        bool openAM = entry.Value.OpenAM != null && entry.Value.CloseAM != null;
        bool openPM = entry.Value.OpenPM != null && entry.Value.ClosePM != null;

        result += "\n  " + WeekDays[entry.Key] + " : ";
        if (openAM)
        {
          result += "\n    From " + entry.Value.OpenAM + " to " + entry.Value.CloseAM;
          if (openPM)
          {
            result += "\n    And  " + entry.Value.OpenPM + " to " + entry.Value.ClosePM;
          }
        }
        else
        {
          result += "\n    Closed";
        }
      }
      return result;
    }
  }
}
