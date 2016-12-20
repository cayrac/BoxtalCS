using System;

namespace Boxtal
{
  ///<summary>Describe package content.</summary>
  ///<remarks></remarks>
  public class ProformaDescription
  {
    private string _description;
    private string _countryOrigin;
    private int _quantity;
    private float _value;
    private float _weight;

    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Description {get{return _description;} set{_description = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public string CountryOrigin {get{return _countryOrigin;} set{_countryOrigin = value;}}
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public int Quantity {get{return _quantity;} set{_quantity = value;}}
    ///<summary>Value of a single object.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Value {get{return _value;} set{_value = value;}}
    ///<summary>Weight of a single object.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Weight {get{return _weight;} set{_weight = value;}}

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="description"></param>
    ///<param name="countryOrigin"></param>
    ///<param name="quantity"></param>
    ///<param name="value"></param>
    ///<param name="weight"></param>
    public ProformaDescription(string description, string countryOrigin, int quantity, float value, float weight)
    {
      _description = description;
      _countryOrigin = countryOrigin;
      _quantity = quantity;
      _value = value;
      _weight = weight;
    }

    ///<summary>Constructor by copy.</summary>
    ///<remarks></remarks>
    ///<param name="proforma"></param>
    public ProformaDescription(ProformaDescription proforma)
    {
      _description = proforma.Description;
      _countryOrigin = proforma.CountryOrigin;
      _quantity = proforma.Quantity;
      _value = proforma.Value;
      _weight = proforma.Weight;
    }
  }
}
