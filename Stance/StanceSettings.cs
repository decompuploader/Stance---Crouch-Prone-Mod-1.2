using GTA;
using System.Windows.Forms;

namespace Stance
{
  public class StanceSettings : Script
  {
    private ScriptSettings iniFile;
    public static Keys stanceKey;
    public static bool overrideStealth;

    public StanceSettings()
    {
      this.iniFile = ScriptSettings.Load("scripts//Stance.ini");
      StanceSettings.overrideStealth = (bool) this.iniFile.GetValue<bool>("Settings", "overrideStealthButton", (M0) 1);
      StanceSettings.stanceKey = (Keys) this.iniFile.GetValue<Keys>("Keys", nameof (stanceKey), (M0) 74);
    }
  }
}
