using System.Diagnostics;
using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace LLib;

public static class LImGui
{
    public abstract class LWindow : Window
    {
        protected bool ClickedHeaderLastFrame;
        protected bool ClickedHeaderCurrentFrame;

        protected LWindow(string name, ImGuiWindowFlags flags = ImGuiWindowFlags.None, bool forceMainWindow = false)
            : base(name, flags, forceMainWindow)
        {
            TitleBarButtons.Add(new TitleBarButton
            {
                Icon = FontAwesomeIcon.Heart,
                ShowTooltip = () =>
                {
                    ImGui.BeginTooltip();
                    ImGui.Text("Go to patreon.com/lizac");
                    ImGui.EndTooltip();
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
}
