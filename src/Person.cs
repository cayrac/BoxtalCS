using System;
using System.Collections.Generic;

namespace Boxtal
{
  ///<summary>Describe a person and it's address.</summary>
  ///<remarks></remarks>
  public class Person
  {

    ///<summary>List of all person types.</summary>
    ///<remarks></remarks>
    public static class PersonType
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public static string Company = "entreprise";
      ///<summary></summary>
      ///<remarks></remarks>
      public static string Individual = "particulier";
    }

    ///<summary>List of all supported civilities.</summary>
    ///<remarks></remarks>
    public static class CivilityType
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string M = "M";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Mme = "Mme";
    }

    private string _countryIso = null;
    private string _postalCode = null;
    private string _town = null;
    private string _type = null;
    private string _address = null;
    private string _company = null;
    private string _civility = null;
    private string _name = null;
    private string _surname = null;
    private string _email = null;
    private string _phone = null;

    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CountryIso {get{return _countryIso;} set{_countryIso = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string PostalCode {get{return _postalCode;} set{_postalCode = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Town {get{return _town;} set{_town = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Type {get{return _type;} set{_type = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Address {get{return _address;} set{_address = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Company {get{return _company;} set{_company = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Civility {get{return _civility;} set{_civility = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Name {get{return _name;} set{_name = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Surname {get{return _surname;} set{_surname = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Email {get{return _email;} set{_email = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Phone {get{return _phone;} set{_phone = value;}}

    ///<summary>Constructor by copy.</summary>
    ///<remarks></remarks>
    ///<param name="person"></param>
    public Person(Person person)
    {
      CountryIso = person.CountryIso;
      PostalCode = person.PostalCode;
      Town = person.Town;
      Type = person.Type;
      Address = person.Address;
      Company = person.Company;
      Civility = person.Civility;
      Name = person.Name;
      Surname = person.Surname;
      Email = person.Email;
      Phone = person.Phone;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="countryIso"></param>
    ///<param name="postalCode"></param>
    ///<param name="town"></param>
    ///<param name="address"></param>
    ///<param name="type"></param>
    public Person(string countryIso, string postalCode, string town, string address, string type)
    {
      _countryIso = countryIso;
      _postalCode = postalCode;
      _town = town;
      _address = address;
      _type = type;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="countryIso"></param>
    ///<param name="postalCode"></param>
    ///<param name="town"></param>
    ///<param name="address"></param>
    ///<param name="type"></param>
    ///<param name="company"></param>
    ///<param name="civility"></param>
    ///<param name="name"></param>
    ///<param name="surname"></param>
    ///<param name="email"></param>
    ///<param name="phone"></param>
    public Person(string countryIso, string postalCode, string town, string address, string type,
                   string company, string civility, string name, string surname, string email, string phone)
    : this(countryIso, postalCode, town, address, type)
    {
      _company = company;
      _civility = civility;
      _name = name;
      _surname = surname;
      _email = email;
      _phone = phone;
    }
  }
}
