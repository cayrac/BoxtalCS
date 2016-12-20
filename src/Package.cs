using System;
using System.Collections.Generic;

namespace Boxtal
{
  ///<summary>Describe a package containing your goods.</summary>
  ///<remarks></remarks>
  public class Package
  {
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Weight;
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Height;
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Width;
    ///<summary></summary>
    ///<remarks></remarks>
    ///<value></value>
    public float Length;

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="_weight"></param>
    ///<param name="_height"></param>
    ///<param name="_width"></param>
    ///<param name="_length"></param>
    public Package(float _weight, float _height, float _width, float _length)
    {
      Weight = _weight;
      Height = _height;
      Width = _width;
      Length = _length;
    }

    ///<summary>Constructor by copy.</summary>
    ///<remarks></remarks>
    ///<param name="package"></param>
    public Package(Package package)
    {
      Weight = package.Weight;
      Height = package.Height;
      Width = package.Width;
      Length = package.Length;
    }
  }
}
