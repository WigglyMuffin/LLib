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
                Tooltip = "Open Liza's page on Patreon",
                Color = 0xFF000FF,
                Offset = Vector2.Zero,
                MouseButton = ImGuiMouseButton.Left
            }))
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "http://patreon.com/lizac",
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
