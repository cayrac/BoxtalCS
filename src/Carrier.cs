using System;
using System.Collections.Generic;

namespace Boxtal
{
  ///<summary>Shipping carrier descriptions and restrictions.</summary>
  ///<remarks>See <see cref="CarrierList"/> to load a list of available carriers.</remarks>
  public class Carrier
  {
    private string _operatorCode;
    private string _operatorName;
    private string _serviceCode;
    private string _serviceName;
    private string _description;
    private string _descriptionShort;
    private string _delai;
    private string _collectionType;
    private string _deliveryType;
    private string _pickupName;
    private string _dropoffName;
    private CarrierFamily _family;
    private bool _zoneFR;
    private bool _zoneES;
    private bool _zoneEU;
    private bool _zoneINT;
    private List<string> _restrictions;
    private List<string> _details;

    ///<summary>Carrier code, used for Order, QuoteList or CarrierList requests.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CarrierCode {get{return _operatorCode + _serviceCode;}}
    ///<summary>Operator code of the carrier.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string OperatorCode {get{return _operatorCode;}}
    ///<summary>Public operator's name.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string OperatorName {get{return _operatorName;}}
    ///<summary>Service code of the carrier.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string ServiceCode {get{return _serviceCode;}}
    ///<summary>Public service's name.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string ServiceName {get{return _serviceName;}}
    ///<summary>Full description of the carrier.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Description {get{return _description;}}
    ///<summary>Quick description of the carrier.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DescriptionShort {get{return _descriptionShort;}}
    ///<summary>Estimated delai in days for this shipping.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Delai {get{return _delai;}}
    ///<summary>Type of collection. See <see cref="Shipping.CollectionType"/> for possible collection values.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CollectionType {get{return _collectionType;}}
    ///<summary>Type of delivery. See <see cref="Shipping.DeliveryType"/> for possible delivery values.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DeliveryType {get{return _deliveryType;}}
    ///<summary>Pickup name for parcel point collection.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string PickupName {get{return _pickupName;}}
    ///<summary>Dropoff name for parcel point delivery.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DropoffName {get{return _dropoffName;}}
    ///<summary>Type of carrier. See <see cref="Carrier.CarrierFamily"/> for possible carrier family values.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public CarrierFamily Family {get{return _family;}}
    ///<summary>True if the carrier deliver to France.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool ZoneFR {get{return _zoneFR;}}
    ///<summary>True if the carrier deliver to Spain.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool ZoneES {get{return _zoneES;}}
    ///<summary>True if the carrier deliver to Europe.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool ZoneEU {get{return _zoneEU;}}
    ///<summary>True if the carrier deliver to the international.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool ZoneINT {get{return _zoneINT;}}
    ///<summary>Delivery country restrictions.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<string> CountryRestrictions {get{return _restrictions;}}
    ///<summary>Additional details about carrier or shipping conditions.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<string> Details {get{return _details;}}

    ///<summary>List of carrier's family.</summary>
    ///<remarks></remarks>
    public enum CarrierFamily
    {
      ///<summary>An economic carrier is suitable if the price is more important than the delay.</summary>
      Economic = 1,
      ///<summary>An express carrier offer better delay for a price higher than economic carriers.</summary>
      Express = 1
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="operatorCode"></param>
    ///<param name="operatorName"></param>
    ///<param name="serviceCode"></param>
    ///<param name="serviceName"></param>
    ///<param name="description"></param>
    ///<param name="descriptionShort"></param>
    ///<param name="delai"></param>
    ///<param name="collectionType"></param>
    ///<param name="deliveryType"></param>
    ///<param name="pickupName"></param>
    ///<param name="dropoffName"></param>
    ///<param name="family"></param>
    ///<param name="zoneFR"></param>
    ///<param name="zoneES"></param>
    ///<param name="zoneEU"></param>
    ///<param name="zoneINT"></param>
    ///<param name="restrictions"></param>
    ///<param name="details"></param>
    public Carrier(string operatorCode, string operatorName, string serviceCode, string serviceName, string description,
      string descriptionShort, string delai, string collectionType, string deliveryType, string pickupName, string dropoffName,
      CarrierFamily family, bool zoneFR, bool zoneES, bool zoneEU, bool zoneINT, List<string> restrictions, List<string> details)
    {
      _operatorCode = operatorCode;
      _operatorName = operatorName;
      _serviceCode = serviceCode;
      _serviceName = serviceName;
      _description = description;
      _descriptionShort = descriptionShort;
      _delai = delai;
      _collectionType = collectionType;
      _deliveryType = deliveryType;
      _pickupName = pickupName;
      _dropoffName = dropoffName;
      _family = family;
      _zoneFR = zoneFR;
      _zoneES = zoneES;
      _zoneEU = zoneEU;
      _zoneINT = zoneINT;
      _restrictions = restrictions;
      _details = details;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the Carrier.</returns>
    public override string ToString()
    {
      string result = "Carrier " + CarrierCode + " :\n";
      result += "Operator          : " + OperatorName + " [" + OperatorCode + "]\n";
      result += "Service           : " + ServiceName + " [" + ServiceCode + "]\n";
      result += "Description       : " + Description + "\n";
      result += "Description (short) : " + DescriptionShort + "\n";
      result += "Delai             : " + Delai + "\n";
      result += "Collection        : " + CollectionType + "\n";
      if (CollectionType == Shipping.CollectionType.ParcelPoint)
      {
        result += "Pickup name       : " + PickupName + "\n";
      }
      result += "Delivery          : " + DeliveryType + "\n";
      if (DeliveryType == Shipping.DeliveryType.ParcelPoint)
      {
        result += "Dropoff name      : " + DropoffName + "\n";
      }
      result += "family            : " + Family + "\n";
      result += "Deliver to France : " + (ZoneFR?"yes":"no") + "\n";
      result += "Deliver to Spain  : " + (ZoneES?"yes":"no") + "\n";
      result += "Deliver to Euro   : " + (ZoneEU?"yes":"no") + "\n";
      result += "Deliver to International : " + (ZoneINT?"yes":"no");
      if (CountryRestrictions.Count > 0) result += "\nCountry restrictions : " + string.Join(", ", CountryRestrictions.ToArray());
      if (Details.Count > 0) result += "\nDetails : \n  " + string.Join("\n  ", Details.ToArray());
      return result;
    }
  }
}
