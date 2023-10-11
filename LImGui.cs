using System.Diagnostics;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Plugin;
using ImGuiNET;

namespace LLib;

public static partial class LImGui
{
    public static void AddPatreonIcon(DalamudPluginInterface pluginInterface)
    {
        if (AddHeaderIcon(pluginInterface, "##Patreon", FontAwesomeIcon.Heart, new HeaderIconOptions
            {
                Tooltip = "Go to patreon.com/lizac",
                Color = 0xFF3030D0,
            }))
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.patreon.com/lizac",
                    UseShellExecute = true,
                    Verb = string.Empty,
                });
            }
            catch
            {
                // not sure what to do anyway
            }
        }
    }
}
