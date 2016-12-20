using System;

namespace Boxtal
{
  ///<summary>Contains all informations necessary in order to use insurance with your orders.</summary>
  ///<remarks></remarks>
  public class Insurance
  {
    ///<summary>List of all wrapping values.</summary>
    ///<remarks></remarks>
    public static class WrappingValue
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Box = "Boîte";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Crate = "Caisse";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Container = "Bac";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string InsulatedPackaging = "Emballage isotherme";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string CarryingCase = "Etui";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Trunk = "Malle";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Bag = "Sac";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Tube = "Tube";
    }

    ///<summary>List of all package material values.</summary>
    ///<remarks></remarks>
    public static class PackageMaterialValue
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string CardboardBox = "Carton";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Wood = "Bois";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string ReinforcedCardboard = "Carton blindé";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string OpaqueFilm = "Film opaque";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string TransparentFilm = "Film Transparent";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Metal = "Métal";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Paper = "Papier";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string ReinforcedPaper = "Papier armé";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string PlasticAndCardboard = "Plastique et carton";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Plastic = "Plastic";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string OpaquePlastic = "Opaque plastic";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string TransparentPlastic = "Transparent plastic";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Polystyrene = "Polystyrene";
    }

    ///<summary>List of all interior protection values.</summary>
    ///<remarks></remarks>
    public static class InteriorProtectionValue
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string None = "Sans protection particulière";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string PaperCushioning = "Calage papier";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string PlasticBubbleWrap = "Bulles plastiques";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string ImpactResistantCardboard = "Carton antichoc";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string AirCushion = "Coussin air";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string FoamCushion = "Coussin mousse";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string CardboardBottleProtector = "Manchon carton (bouteille)";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string FoamBottleProtector = "Manchon mousse (bouteille)";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Padding = "Matelassage";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string FoamPad = "Plaque mousse";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Polystyrene = "Polystyrène";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string CushioningMaterial = "Coussin de calage";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string PaddedMailer = "Sachet bulles";
    }

    ///<summary>List of all fastener values.</summary>
    ///<remarks></remarks>
    public static class FastenerValue
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string SelfAdhesiveFastener = "Fermeture autocollante";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string AdhesiveTape = "Ruban adhésif";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Staples = "Agrafes";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Nails = "Clous";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string Binding = "Collage";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string StrappingTape = "Ruban de cerclage";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string StrapOrCoiledTrip = "Sangle ou feuillard";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string NailsAndFastening = "Clous et cerclage";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string String = "Ficelles";
    }

    ///<summary>List of all fixture values.</summary>
    ///<remarks></remarks>
    public static class FixtureValue
    {
      ///<summary></summary>
      ///<remarks></remarks>
      public const string None = "Sans système de stabilisation";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string TransparentFilm = "Film transparent";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string OpaqueFilm = "Film opaque";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string OpaqueFilmAndStraps = "Film transparent et sangles";
      ///<summary></summary>
      ///<remarks></remarks>
      public const string StrapsOrStrips = "Sangle ou feuillard uniquement";
    }

    private string _wrapping;
    private string _packageMaterial;
    private string _interiorProtection;
    private string _fastener;
    private string _fixture;
    private int _quantity;

    ///<summary>Type of wrapping used for your package.</summary>
    ///<remarks>See <see cref="WrappingValue"/> for all possible values</remarks>
    ///<value></value>
    public string Wrapping {get{return _wrapping;} set{_wrapping = value;}}
    ///<summary>Material used for your package.</summary>
    ///<remarks>See <see cref="PackageMaterialValue"/> for all possible values</remarks>
    ///<value></value>
    public string PackageMaterial {get{return _packageMaterial;} set{_packageMaterial = value;}}
    ///<summary>Interior protection for your package.</summary>
    ///<remarks>See <see cref="InteriorProtectionValue"/> for all possible values</remarks>
    ///<value></value>
    public string InteriorProtection {get{return _interiorProtection;} set{_interiorProtection = value;}}
    ///<summary>Fastener used for your package.</summary>
    ///<remarks>See <see cref="FastenerValue"/> for all possible values</remarks>
    ///<value></value>
    public string Fastener {get{return _fastener;} set{_fastener = value;}}
    ///<summary>Fixture used for your packages (necessary for pallet shipping).</summary>
    ///<remarks>See <see cref="FixtureValue"/> for all possible values</remarks>
    ///<value></value>
    public string Fixture {get{return _fixture;} set{_fixture = value;}}
    ///<summary>Number of packages (necessary for pallet shipping)</summary>
    ///<remarks></remarks>
    ///<value></value>
    public int Quantity {get{return _quantity;} set{_quantity = value;}}

    ///<summary>Constructor by copy.</summary>
    ///<param name="insurance"></param>
    ///<remarks></remarks>
    ///<value></value>
    public Insurance(Insurance insurance)
    {
      _wrapping = insurance.Wrapping;
      _packageMaterial = insurance.PackageMaterial;
      _interiorProtection = insurance.InteriorProtection;
      _fastener = insurance.Fastener;
      _fixture = insurance.Fixture;
      _quantity = insurance.Quantity;
    }

    ///<summary>Constructor for a base insurance.</summary>
    ///<param name="wrapping"></param>
    ///<param name="packageMaterial"></param>
    ///<param name="interiorProtection"></param>
    ///<param name="fastener"></param>
    ///<remarks></remarks>
    ///<value></value>
    public Insurance(string wrapping, string packageMaterial, string interiorProtection, string fastener)
    {
      _wrapping = wrapping;
      _packageMaterial = packageMaterial;
      _interiorProtection = interiorProtection;
      _fastener = fastener;
      _fixture = FixtureValue.None;
      _quantity = 1;
    }

    ///<summary>Constructor for a pallet insurance</summary>
    ///<param name="wrapping"></param>
    ///<param name="packageMaterial"></param>
    ///<param name="interiorProtection"></param>
    ///<param name="fastener"></param>
    ///<param name="fixture"></param>
    ///<param name="quantity"></param>
    ///<remarks></remarks>
    ///<value></value>
    public Insurance(string wrapping, string packageMaterial, string interiorProtection, string fastener, string fixture, int quantity)
    {
      _wrapping = wrapping;
      _packageMaterial = packageMaterial;
      _interiorProtection = interiorProtection;
      _fastener = fastener;
      _fixture = fixture;
      _quantity = quantity;
    }
  }
}
