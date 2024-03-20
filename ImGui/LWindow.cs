using System.Diagnostics;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using IG = ImGuiNET.ImGui;

namespace LLib.ImGui;

public abstract class LWindow : Window
{
    protected bool ClickedHeaderLastFrame { get; private set; }
    protected bool ClickedHeaderCurrentFrame { get; private set; }

    protected LWindow(string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false)
        : base(name, flags, forceMainWindow)
    {
        TitleBarButtons.Add(new TitleBarButton
        {
            Icon = FontAwesomeIcon.Heart,
            ShowTooltip = () =>
            {
                IG.BeginTooltip();
                IG.Text("Go to patreon.com/lizac");
                IG.EndTooltip();
            },
            Priority = int.MinValue,
            IconOffset = new Vector2(1.5f, 1),
            Click = _ =>
            {
                // when you make a window click-through, `Click` is triggered on each individual frame the mouse button is held down.
                ClickedHeaderCurrentFrame = true;
                if (ClickedHeaderLastFrame)
                    return;

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
            },
            AvailableClickthrough = true,
        });
    }

    public override void PreDraw()
    {
        base.PreDraw();

        ClickedHeaderLastFrame = ClickedHeaderCurrentFrame;
        ClickedHeaderCurrentFrame = false;
    }
}
