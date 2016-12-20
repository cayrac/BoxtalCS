using System;

namespace Boxtal
{
  ///<summary>Represent a parcel content category.</summary>
  ///<remarks>See <see cref="ContentCategoryList"/> for the full content category list.</remarks>
  public class ContentCategory
  {
    private string _familyId;
    private string _family;
    private string _name;
    private string _id;

    ///<summary>Content family id.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string FamilyId {get{return _familyId;}}
    ///<summary>Content family.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Family {get{return _family;}}
    ///<summary>Name describing the content of the parcel.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Name {get{return _name;}}
    ///<summary>Id of the content category.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Id {get{return _id;}}

    ///<summary>Content category constructor.</summary>
    ///<remarks>Throws <see cref="ApiException"/></remarks>
    ///<param name="id"></param>
    ///<param name="name"></param>
    ///<param name="familyId"></param>
    ///<param name="family"></param>
    ///<returns></returns>
    public ContentCategory(string id, string name, string familyId, string family)
    {
      _id = id;
      _name = name;
      _familyId = familyId;
      _family = family;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the content category.</returns>
    public override string ToString()
    {
      return "[" + FamilyId + "] " + Family + " -> [" + Id + "] " + Name;
    }
  }
}
