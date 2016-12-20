using System;
using System.Collections.Generic;
using System.Xml;
using System.Net;
using System.IO;

namespace Boxtal
{
  ///<summary>Query for user data.</summary>
  ///<remarks></remarks>
  public class User : Query
  {
    private string _partnership;
    private string _defaultCountry;

    ///<summary>User account's partnership</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string Partnership {get{return _partnership;}}
    ///<summary>If set to tru, you will recieve an email with the shipping label with each created order.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool LabelMail {get{return Parameters["label"] == "true";} set{Parameters["label"] = value ? "true" : "";}}
    ///<summary>If set to true, your recipient will recieve a Boxtal notification when an order is created.</summary>
    ///<remarks>This option is not necessary the carrier you have chosen already send a notification.</remarks>
    ///<value></value>
    public bool NotificationMail {get{return Parameters["notification"] == "true";} set{Parameters["notification"] = value ? "true" : "";}}
    ///<summary>If set to true, you will recieve your bills by email.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public bool BillMail {get{return Parameters["bill"] == "true";} set{Parameters["bill"] = value ? "true" : "";}}
    ///<summary>Default user account's country.</summary>
    ///<remarks></remarks>
    ///<value></value>
    public string DefaultCountry {get{return _defaultCountry;}}

    ///<summary></summary>
    ///<remarks></remarks>
    ///<param name="login"></param>
    ///<param name="password"></param>
    public User(string login, string password)
    {
      Login = login;
      Password = password;
    }

    ///<summary>Constructor with Credentials login and password</summary>
    ///<remarks></remarks>
    public User()
    :this(Credentials.Login, Credentials.Password)
    {
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns></returns>
    override protected Dictionary<string, string> ToQueryParameters()
    {
      Dictionary<string, string> result = base.ToQueryParameters();

      return result;
    }

    ///<summary>Load all user informations.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public void Load()
    {
      LoadDetails();
      LoadPartnership();
    }

    ///<summary>Load all user credentials, default country and email configuration.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public void LoadDetails()
    {
      XmlDocument data = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/user_details");

      try
      {
        XmlNode userNode = data["user"];
        XmlNode mailsNode = userNode["mails"];
        XmlNode nodeApiKeyTest = userNode["api_key_test"];
        XmlNode nodeApiKeyProd = userNode["api_key_prod"];
        XmlNode nodeLabel = mailsNode["label"];
        XmlNode nodeNotification = mailsNode["notification"];
        XmlNode nodeBill = mailsNode["bill"];
        XmlNode nodeDefaultShippingCountry = userNode["default_shipping_country"];

        KeyTest = nodeApiKeyTest.InnerText;
        KeyProd = nodeApiKeyProd.InnerText;
        LabelMail = nodeLabel.InnerText == "true";
        NotificationMail = nodeNotification.InnerText == "true";
        BillMail = nodeBill.InnerText == "true";
        _defaultCountry = nodeDefaultShippingCountry.InnerText;
      }
      catch (NullReferenceException e)
      {
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Failed to parse user details : " + e.Message);
      }
    }

    ///<summary>Load user partnership.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public void LoadPartnership()
    {
      XmlDocument data = base.ExecuteXml(WebRequestMethods.Http.Get, "api/v1/partnership");

      try
      {
        XmlNode partnershipNode = data["user"]["partnership"];

        _partnership = partnershipNode.InnerText;
      }
      catch (NullReferenceException e)
      {
        throw new ApiException(ApiException.ErrorCode.Parsing, HttpStatusCode.OK, "Failed to parse user partnership : " + e.Message);
      }
    }

    ///<summary>Update user informations.</summary>
    ///<remarks>Throws <see cref="ApiException"/>.</remarks>
    ///<returns></returns>
    public void Update()
    {
      base.ExecuteXml(WebRequestMethods.Http.Post, "api/v1/emails_configuration");
    }

    ///<summary></summary>
    ///<remarks></remarks>
    ///<returns>String representation of the user.</returns>
    override public string ToString()
    {
      string result = "User " + Login + "\n";
      result += "  Key : " + KeyProd + " (prod) - " + KeyTest + "(test)\n";
      result += "  Default country : " + DefaultCountry + "\n";
      result += "  Partnership : " + Partnership + "\n";
      result += "  Send label email to sender on order : " + (LabelMail?"yes":"no") + "\n";
      result += "  Send notification email to recipient on order : " + (NotificationMail?"yes":"no") + "\n";
      result += "  Send bill email to sender on order : " + (BillMail?"yes":"no") + "\n";
      return result;
    }
  }

}
