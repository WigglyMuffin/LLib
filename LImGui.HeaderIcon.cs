using System.Numerics;
using System.Runtime.InteropServices;
using Dalamud.Interface;
using Dalamud.Interface.Utility;
using Dalamud.Plugin;
using ImGuiNET;

namespace LLib;

/// <summary>
/// Originally part of ECommons by NightmareXIV.
///
/// https://github.com/NightmareXIV/ECommons/blob/master/ECommons/ImGuiMethods/ImGuiEx.cs
/// </summary>
partial class LImGui
{
   public sealed record HeaderIconOptions
    {
        public Vector2 Offset { get; init; } = Vector2.Zero;
        public ImGuiMouseButton MouseButton { get; init; } = ImGuiMouseButton.Left;
        public string Tooltip { get; init; } = string.Empty;
        public uint Color { get; init; } = 0xFFFFFFFF;
    }

    private static uint _headerLastWindowId = 0;
    private static ulong _headerLastFrame = 0;
    private static float _headerCurrentPos = 0;
    private static float _headerImGuiButtonWidth = 0;

    public static bool AddHeaderIcon(DalamudPluginInterface pluginInterface, string id, FontAwesomeIcon icon, HeaderIconOptions options = null)
    {
        if (ImGui.IsWindowCollapsed()) return false;

        var scale = ImGuiHelpers.GlobalScale;
        var currentID = ImGui.GetID(0);
        if (currentID != _headerLastWindowId || _headerLastFrame != pluginInterface.UiBuilder.FrameCount)
        {
            _headerLastWindowId = currentID;
            _headerLastFrame = pluginInterface.UiBuilder.FrameCount;
            _headerCurrentPos = 0.25f * ImGui.GetStyle().FramePadding.Length();
            if (!GetCurrentWindowFlags().HasFlag(ImGuiWindowFlags.NoTitleBar))
                _headerCurrentPos = 1;
            _headerImGuiButtonWidth = 0f;
            if (CurrentWindowHasCloseButton())
                _headerImGuiButtonWidth += 17 * scale;
            if (!GetCurrentWindowFlags().HasFlag(ImGuiWindowFlags.NoCollapse))
                _headerImGuiButtonWidth += 17 * scale;
        }

        options ??= new();
        var prevCursorPos = ImGui.GetCursorPos();
        var buttonSize = new Vector2(20 * scale);
        var buttonPos = new Vector2((ImGui.GetWindowWidth() - buttonSize.X - _headerImGuiButtonWidth * scale * _headerCurrentPos) - (ImGui.GetStyle().FramePadding.X * scale), ImGui.GetScrollY() + 1);
        ImGui.SetCursorPos(buttonPos);
        var drawList = ImGui.GetWindowDrawList();
        drawList.PushClipRectFullScreen();

        var pressed = false;
        ImGui.InvisibleButton(id, buttonSize);
        var itemMin = ImGui.GetItemRectMin();
        var itemMax = ImGui.GetItemRectMax();
        var halfSize = ImGui.GetItemRectSize() / 2;
        var center = itemMin + halfSize;
        if (ImGui.IsWindowHovered() && ImGui.IsMouseHoveringRect(itemMin, itemMax, false))
        {
            if (!string.IsNullOrEmpty(options.Tooltip))
                ImGui.SetTooltip(options.Tooltip);
            ImGui.GetWindowDrawList().AddCircleFilled(center, halfSize.X, ImGui.GetColorU32(ImGui.IsMouseDown(ImGuiMouseButton.Left) ? ImGuiCol.ButtonActive : ImGuiCol.ButtonHovered));
            if (ImGui.IsMouseReleased(options.MouseButton))
                pressed = true;
        }

        ImGui.SetCursorPos(buttonPos);
        ImGui.PushFont(UiBuilder.IconFont);
        var iconString = icon.ToIconString();
        drawList.AddText(UiBuilder.IconFont, ImGui.GetFontSize(), itemMin + halfSize - ImGui.CalcTextSize(iconString) / 2 + options.Offset, options.Color, iconString);
        ImGui.PopFont();

        ImGui.PopClipRect();
        ImGui.SetCursorPos(prevCursorPos);

        return pressed;
    }

    [LibraryImport("cimgui")]
    [UnmanagedCallConv(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
    private static partial nint igGetCurrentWindow();
    private static unsafe ImGuiWindow* GetCurrentWindow() => (ImGuiWindow*)igGetCurrentWindow();
    private static unsafe ImGuiWindowFlags GetCurrentWindowFlags() => GetCurrentWindow()->Flags;
    private static unsafe bool CurrentWindowHasCloseButton() => GetCurrentWindow()->HasCloseButton != 0;

    [StructLayout(LayoutKind.Explicit)]
    private struct ImGuiWindow
    {
        [FieldOffset(0xC)] public ImGuiWindowFlags Flags;

        [FieldOffset(0xD5)] public byte HasCloseButton;

        // 0x118 is the start of ImGuiWindowTempData
        [FieldOffset(0x130)] public Vector2 CursorMaxPos;
    }
}
