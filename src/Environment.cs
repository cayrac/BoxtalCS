using System;
using System.Collections.Generic;

namespace Boxtal
{
  ///<summary>List of all environments.</summary>
  ///<remarks>You can set your default requests environment in <see cref="Global.Environment"/>.</remarks>
  public static class Environment
  {
    ///<summary></summary>
    ///<remarks>In test environment, all orders passed are sent to the test environement of the carrier.</remarks>
    public const string Test = "test";
    ///<summary></summary>
    ///<remarks>In production environement, orders are real. You need to be in differed payment in order to use it.</remarks>
    public const string Prod = "prod";

    ///<summary>Contains all server domains for each environmeent</summary>
    ///<remarks></remarks>
    public static Dictionary<string, string> Hosts = new Dictionary<string, string> {
      {"test", "https://test.envoimoinscher.com/"},
      {"prod", "https://www.envoimoinscher.com/"}
    };
  }
}
