using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;
using System.Linq;

namespace Boxtal
{
  ///<summary>Query to list all available carriers.</summary>
  ///<remarks></remarks>
  public class CarrierList : Query
  {
    ///<summary>Channel used for the carrier list.</summary>
    ///<remarks>A channel for your own application can be created, contact api@boxtal.com for more informations.</remarks>
    ///<value>Default channel is library.</value>
    public string Channel
    {
      get {return _parameters["channel"];}
      set {_parameters["channel"] = value;}
    }
    ///<summary>Channel's version which is the version of your application (or the library if you use library channel).</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Version
    {
      get {return _parameters["version"];}
      set {_parameters["version"] = value;}
    }

    ///<summary>Initialize a new carrier's list.</summary>
    ///<remarks></remarks>
    ///<param name="channel"></param>
    ///<param name="version"></param>
    public CarrierList(string channel, string version)
    {
      Channel = channel;
      Version = version;
    }

    ///<summary>Initialize a new carrier's list with global parameters.</summary>
    ///<remarks>Channel and versions are initialized with respectively <see cref="Global.Platform"/> and <see cref="Global.PlatformVersion"/></remarks>
    public CarrierList()
    :this(Global.Platform, Global.PlatformVersion)
    {
    }

    ///<summary>Load the list of all available carriers</summary>
    ///<remarks>Throws <see cref="ApiException"/></remarks>
    ///<returns></returns>
    public List<Carrier> Get()
    {
      XmlDocument xml = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/carriers_list");

      List<Carrier> result = new List<Carrier>();
      try
      {
        XmlNodeList operators = xml.SelectNodes("operators/operator");
        foreach (XmlNode ope in operators)
        {
          XmlNode nodeNameOperator = ope["name"];
          XmlNode nodeCodeOperator = ope["code"];
          XmlNodeList services = ope.SelectNodes("services/service");
          foreach (XmlNode service in services)
          {
            XmlNode nodeNameService = service["label"];
            XmlNode nodeCodeService = service["code"];
            XmlNode nodePickupParcelPoint = service["parcel_pickup_point"];
            XmlNode nodeDropoffParcelPoint = service["parcel_dropoff_point"];
            XmlNode nodeDelai = service["delivery_due_time"];
            XmlNode nodeFamily = service["family"];
            XmlNode nodeZoneFR = service["zone_fr"];
            XmlNode nodeZoneES = service["zone_es"];
            XmlNode nodeZoneEU = service["zone_eu"];
            XmlNode nodeZoneINT = service["zone_int"];

            string operatorName = nodeNameOperator.InnerText;
            string operatorCode = nodeCodeOperator.InnerText;
            string serviceName = nodeNameService.InnerText;
            string serviceCode = nodeCodeService.InnerText;
            string collectionType = (nodePickupParcelPoint.InnerText == "1") ? Shipping.CollectionType.ParcelPoint :
                                     (nodePickupParcelPoint.InnerText == "2") ? Shipping.CollectionType.PostOffice :
                                     Shipping.CollectionType.Home;
            string deliveryType = (nodeDropoffParcelPoint.InnerText == "1") ? Shipping.DeliveryType.ParcelPoint :
                                     (nodeDropoffParcelPoint.InnerText == "2") ? Shipping.DeliveryType.PostOffice :
                                     Shipping.DeliveryType.Home;
            string delai = nodeDelai.InnerText;
            Carrier.CarrierFamily family = (Carrier.CarrierFamily)Int32.Parse(nodeFamily.InnerText);
            bool zoneFR = nodeZoneFR.InnerText == "1";
            bool zoneES = nodeZoneES.InnerText == "1";
            bool zoneEU = nodeZoneEU.InnerText == "1";
            bool zoneINT = nodeZoneINT.InnerText == "1";

            XmlNode localization = service;
            XmlNodeList translations = service.SelectNodes("translations/translation");
            foreach (XmlNode translation in translations)
            {
              if (translation["locale"].InnerText == Locale)
              {
                localization = translation;
              }
            }

            XmlNode descriptionNode = localization["label_store"];
            XmlNode descriptionShortNode = localization["description"];
            XmlNode restrictionsNode = localization["zone_restriction"];
            XmlNodeList detailsNodes = localization.SelectNodes("details/detail");
            XmlNode pickupNameNode = localization["pickup_place"];
            XmlNode dropoffNameNode = localization["dropoff_place"];

            string description = descriptionNode.InnerText;
            string descriptionShort = descriptionShortNode.InnerText;
            string contentRestriction = restrictionsNode.InnerText.Trim();
            List<string> restrictions = new List<string>();
            if (contentRestriction.Length > 1) restrictions = restrictionsNode.InnerText.Trim().Split(new string[] {", "}, StringSplitOptions.None).ToList();
            List<string> details = new List<string>();
            foreach (XmlNode detailsNode in detailsNodes)
            {
              details.Add(detailsNode.InnerText);
            }
            string pickupName = pickupNameNode.InnerText;
            string dropoffName = dropoffNameNode.InnerText;

            result.Add(new Carrier(operatorCode, operatorName, serviceCode, serviceName, description, descriptionShort,
              delai, collectionType, deliveryType, pickupName, dropoffName, family, zoneFR, zoneES, zoneEU,
              zoneINT, restrictions, details));
          }
        }
      }
      catch(Exception e)
      {
        Global.DebugPrint("Unable to parse carriers ...");
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Unable to parse carriers : " + e.Message);
      }
      return result;
    }
  }
}
