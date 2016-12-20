using System;

namespace Boxtal
{
  ///<summary>A simple Boxtal news.</summary>
  ///<remarks>See <see cref="NewsList"/> to list all available news.</remarks>
  public class News
  {
    private string _type;
    private string _title;
    private string _message;

    ///<summary>Type of news.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Type{get{return _type;}}
    ///<summary>News's title.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Title{get{return _title;}}
    ///<summary>News's message.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Message{get{return _message;}}

    ///<summary>Create a new news.</summary>
    ///<remarks></remarks>
    ///<param name="type"></param>
    ///<param name="title"></param>
    ///<param name="message"></param>
    public News(string type, string title, string message)
    {
      _type = type;
      _title = title;
      _message = message;
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the news.</returns>
    public override string ToString()
    {
      string result = Type + " : \n";
      result += "Title : " + Title + "\n";
      result += "Message : " + Message;
      return result;
    }
  }
}
