using System;

namespace Boxtal
{
  ///<summary>Global and default variables.</summary>
  ///<remarks></remarks>
  public static class Global
  {
    ///<summary>Default api date format.</summary>
    ///<remarks></remarks>
    public const string Dateformat = "yyyy-MM-dd";
    ///<summary>API platform name.</summary>
    ///<remarks></remarks>
    public const string Platform = "library";
    ///<summary>API platform version.</summary>
    ///<remarks></remarks>
    public const string PlatformVersion = "1.0.0.0";
    ///<summary>API version the platform is built for.</summary>
    ///<remarks></remarks>
    public const string ApiVersion = "1.3.2";
    ///<summary>Name of the module using the platform.</summary>
    ///<remarks></remarks>
    public static string ModulePlatform = null;
    ///<summary>Version of the module using the platform.</summary>
    ///<remarks></remarks>
    public static string ModuleVersion = null;
    ///<summary>Define the language of Boxtal request responses.</summary>
    ///<remarks>See <see cref="Locale"/> for all available locales.</remarks>
    public static string Locale = Boxtal.Locale.EN;
    ///<summary>Enable debug mode.</summary>
    ///<remarks>In debug mode, all requests are printed on the console</remarks>
    public static bool Debug = false;
    ///<summary>Set the default API environment.</summary>
    ///<remarks>See <see cref="Environment"/> for all available environments.</remarks>
    public static string Environment = Boxtal.Environment.Test;

    ///<summary>Print in console the message if debug mode is activated.</summary>
    ///<param name="message"></param>
    ///<remarks></remarks>
    ///<returns></returns>
    public static void DebugPrint(string message)
    {
      if (Debug) Console.WriteLine(message);
    }
  }
}
