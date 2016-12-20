using System;
using System.Collections.Generic;
using System.Globalization;

namespace Boxtal
{
  ///<summary>Base for shipping requests.</summary>
  ///<remarks></remarks>
  public abstract class Shipping : Query
  {
    ///<summary>List of all shipping reasons.</summary>
    ///<remarks>Required for international shipping.</remarks>
    public static class ReasonType
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Sale = "sale";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Repair = "repr";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Return = "rtrn";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Sample = "smpl";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Personal = "prsu";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string InterCompany = "sale";
    }

    ///<summary>List of all shipping collection type.</summary>
    ///<remarks></remarks>
    public static class CollectionType
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Home = "HOME";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string PostOffice = "POST_OFFICE";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string ParcelPoint = "RELAY_POINT";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Company = "COMPANY";
    }

    ///<summary>List of all shipping delivery type.</summary>
    ///<remarks></remarks>
    public static class DeliveryType
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Home = "HOME";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string PostOffice = "POST_OFFICE";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string ParcelPoint = "PICKUP_POINT";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Company = "COMPANY";
    }

    ///<summary>List of all package types.</summary>
    ///<remarks></remarks>
    public static class PackageTypeValue
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Envelope = "pli";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Parcel = "colis";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Bulky = "encombrant";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Pallet = "palette";
    }

    ///<summary>List of all quotes filter type.</summary>
    ///<remarks></remarks>
    public static class FilterType
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public static string Price = "aucun";
      ///<summary></summary>
      ///<remarks></remarks>
      public static string Speed = "minimum";
      ///<summary></summary>
      ///<remarks></remarks>
      public static string SameDay = "course";
    }

    private Person _personFrom;
    private Person _personTo;
    private string _packageType;
    private List<Package> _packages = new List<Package>();
    private List<ProformaDescription> _proforma = new List<ProformaDescription>();
    private string _description;
    private float _value;
    private Insurance _insurance = null;
    private float _collectOnDelivery = 0.0f;

    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public Person PersonFrom {get{return _personFrom;} set{_personFrom = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public Person PersonTo {get{return _personTo;} set{_personTo = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string PackageType {get{return _packageType;} set{_packageType = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<Package> Packages {get{return _packages;} set{_packages = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public List<ProformaDescription> Proforma {get{return _proforma;} set{_proforma = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Description {get{return _description;} set{_description = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public Insurance Insurance {get{return _insurance;} set{_insurance = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float CollectOnDelivery {get{return _collectOnDelivery;} set{_collectOnDelivery = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public DateTime PickupDate
    {
      get{return DateTime.ParseExact(_parameters["collecte"], Global.Dateformat, null);}
      set{_parameters["collecte"] = value.ToString(Global.Dateformat);}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Filter
    {
      get{return _parameters["delai"];}
      set{_parameters["delai"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string ContentType
    {
      get{return _parameters["code_contenu"];}
      set{_parameters["code_contenu"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Operator
    {
      get{return _parameters["operateur"];}
      set{_parameters["operateur"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool SaturdayDelivery
    {
      get{return _parameters["saturdaydelivery.selection"] == "1";}
      set{_parameters["saturdaydelivery.selection"] = value?"1":"0";}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Service
    {
      get{return _parameters["service"];}
      set{_parameters["service"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DisponibilityMin
    {
      get{return _parameters["disponibilite.HDE"];}
      set{_parameters["disponibilite.HDE"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DisponibilityMax
    {
      get{return _parameters["disponibilite.HLE"];}
      set{_parameters["disponibilite.HLE"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DropParcelPoint
    {
      get{return _parameters["depot.pointrelais"];}
      set{_parameters["depot.pointrelais"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string PickupParcelPoint
    {
      get{return _parameters["retrait.pointrelais"];}
      set{_parameters["retrait.pointrelais"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string PushUrl
    {
      get{return _parameters["url_push"];}
      set{_parameters["url_push"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Reason
    {
      get{return _parameters["raison"];}
      set{_parameters["raison"] = value;}
    }
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Value
    {
      get{return _value;}
      set{_value = value;}
    }

    ///<summary>Constructor by copy.</summary>
    ///<remarks></remarks>
    ///<param name="shipping"></param>
    protected Shipping(Shipping shipping)
    :base(shipping)
    {
      if (shipping.PersonFrom != null)
      {
        _personFrom = new Person(shipping.PersonFrom); // clone
      }
      if (shipping.PersonTo != null)
      {
        _personTo = new Person(shipping.PersonTo); // clone
      }
      _packageType = shipping._packageType;
      foreach(Package package in shipping.Packages)
      {
        _packages.Add(new Package(package));
      }
      foreach(ProformaDescription proforma in shipping.Proforma)
      {
        _proforma.Add(new ProformaDescription(proforma));
      }
      _description = shipping._description;
      _value = shipping._value;
      if (shipping.Insurance != null)
      {
        _insurance = new Insurance(
          shipping.Insurance.Wrapping,
          shipping.Insurance.PackageMaterial,
          shipping.Insurance.InteriorProtection,
          shipping.Insurance.Fastener,
          shipping.Insurance.Fixture,
          shipping.Insurance.Quantity);
      }
      _collectOnDelivery = shipping._collectOnDelivery;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="from"></param>
    ///<param name="to"></param>
    ///<param name="packageType"></param>
    public Shipping(Person from, Person to, string packageType)
    {
      PersonFrom = from;
      PersonTo = to;
      PackageType = packageType;

      // add default params
      PickupDate = DateTime.Now;
      Filter = FilterType.Price;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns></returns>
    override protected Dictionary<string, string> ToQueryParameters()
    {
      Dictionary<string, string> result = base.ToQueryParameters();

      // add from addres
      if (PersonFrom != null)
      {
        if (PersonFrom.CountryIso != null)
        {
          result.Add("shipper.pays", PersonFrom.CountryIso);
        }
        if (PersonFrom.PostalCode != null)
        {
          result.Add("shipper.code_postal", PersonFrom.PostalCode);
        }
        if (PersonFrom.Town != null)
        {
          result.Add("shipper.ville", PersonFrom.Town);
        }
        if (PersonFrom.Type != null)
        {
          result.Add("shipper.type", PersonFrom.Type);
        }
        if (PersonFrom.Address != null)
        {
          result.Add("shipper.adresse", PersonFrom.Address);
        }
        if (PersonFrom.Company != null)
        {
          result.Add("shipper.societe", PersonFrom.Company);
        }
        if (PersonFrom.Civility != null)
        {
          result.Add("shipper.civilite", PersonFrom.Civility);
        }
        if (PersonFrom.Name != null)
        {
          result.Add("shipper.nom", PersonFrom.Name);
        }
        if (PersonFrom.Surname != null)
        {
          result.Add("shipper.prenom", PersonFrom.Surname);
        }
        if (PersonFrom.Email != null)
        {
          result.Add("shipper.email", PersonFrom.Email);
        }
        if (PersonFrom.Phone != null)
        {
          result.Add("shipper.tel", PersonFrom.Phone);
        }
      }

      // add to addres
      if (PersonTo != null)
      {
        if (PersonTo.CountryIso != null)
        {
          result.Add("recipient.pays", PersonTo.CountryIso);
        }
        if (PersonTo.PostalCode != null)
        {
          result.Add("recipient.code_postal", PersonTo.PostalCode);
        }
        if (PersonTo.Town != null)
        {
          result.Add("recipient.ville", PersonTo.Town);
        }
        if (PersonTo.Type != null)
        {
          result.Add("recipient.type", PersonTo.Type);
        }
        if (PersonTo.Address != null)
        {
          result.Add("recipient.adresse", PersonTo.Address);
        }
        if (PersonTo.Company != null)
        {
          result.Add("recipient.societe", PersonTo.Company);
        }
        if (PersonTo.Civility != null)
        {
          result.Add("recipient.civilite", PersonTo.Civility);
        }
        if (PersonTo.Name != null)
        {
          result.Add("recipient.nom", PersonTo.Name);
        }
        if (PersonTo.Surname != null)
        {
          result.Add("recipient.prenom", PersonTo.Surname);
        }
        if (PersonTo.Email != null)
        {
          result.Add("recipient.email", PersonTo.Email);
        }
        if (PersonTo.Phone != null)
        {
          result.Add("recipient.tel", PersonTo.Phone);
        }
      }

      // add description
      result.Add(PackageType + ".description", _description);

      // add value
      result.Add(PackageType + ".valeur", _value.ToString());

      // add packages
      int packageCount = 1;
      foreach(Package package in Packages)
      {
        result.Add(PackageType + "_" + packageCount + ".poids", package.Weight.ToString());
        result.Add(PackageType + "_" + packageCount + ".longueur", package.Length.ToString());
        result.Add(PackageType + "_" + packageCount + ".largeur", package.Width.ToString());
        result.Add(PackageType + "_" + packageCount + ".hauteur", package.Height.ToString());
        packageCount++;
      }

      int proformaCount = 1;
      foreach(ProformaDescription proforma in Proforma)
      {
        result.Add("proforma_" + proformaCount + ".description_en", proforma.Description);
        result.Add("proforma_" + proformaCount + ".description_fr", proforma.Description);
        result.Add("proforma_" + proformaCount + ".origine", proforma.CountryOrigin);
        result.Add("proforma_" + proformaCount + ".number", proforma.Quantity.ToString());
        result.Add("proforma_" + proformaCount + ".value", proforma.Value.ToString());
        result.Add("proforma_" + proformaCount + ".poids", proforma.Weight.ToString());
        proformaCount++;
      }

      // add insurance
      if (Insurance != null)
      {
        result.Add("assurance.selection", "true");
        result.Add("assurance.emballage", Insurance.Wrapping);
        result.Add("assurance.fermeture", Insurance.Fastener);
        result.Add("assurance.fixation", Insurance.Fixture);
        result.Add("assurance.materiau", Insurance.PackageMaterial);
        result.Add("assurance.protection", Insurance.InteriorProtection);
        result.Add("assurance.nombre", Insurance.Quantity.ToString());
      }
      else
      {
        result.Add("assurance.selection", "false");
      }

      // add collect on delivery
      if (CollectOnDelivery > 0.0)
      {
        result.Add("contre-remboursement.selection", "true");
        result.Add("contre-remboursement.valeur", CollectOnDelivery.ToString());
      }
      else
      {
        result.Add("contre-remboursement.selection", "false");
      }

      return result;
    }
  }
}
