using System;

namespace Boxtal
{
  ///<summary>Storage for your default Boxtal credentials.</summary>
  ///<remarks>You can create new credentials by registering to www.boxtal.com or www.boxtal.es.</remarks>
  public static class Credentials
  {
    ///<summary>Boxtal account login.</summary>
    ///<remarks></remarks>
    public static string Login = "";
    ///<summary>Boxtal account password.</summary>
    ///<remarks></remarks>
    public static string Password = "";
    ///<summary>Boxtal production API key.</summary>
    ///<remarks></remarks>
    public static string KeyProd = "";
    ///<summary>Boxtal test API key.</summary>
    ///<remarks></remarks>
    public static string KeyTest = "";

    ///<summary>Set your default credentials.</summary>
    ///<remarks>Most Boxtal requests will use there credentials by default.</remarks>
    ///<param name="login"></param>
    ///<param name="password"></param>
    ///<param name="keyProd"></param>
    ///<param name="keyTest"></param>
    ///<returns></returns>
    public static void Set(string login, string password, string keyProd, string keyTest)
    {
      Login = login;
      Password = password;
      KeyProd = keyProd;
      KeyTest = keyTest;
    }
  }
}
