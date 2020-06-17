using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace ValorantTrainer.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (ValorantTrainer.Properties.Resources.resourceMan == null)
          ValorantTrainer.Properties.Resources.resourceMan = new ResourceManager("ValorantTrainer.Properties.Resources", typeof (ValorantTrainer.Properties.Resources).Assembly);
        return ValorantTrainer.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return ValorantTrainer.Properties.Resources.resourceCulture;
      }
      set
      {
        ValorantTrainer.Properties.Resources.resourceCulture = value;
      }
    }
  }
}
