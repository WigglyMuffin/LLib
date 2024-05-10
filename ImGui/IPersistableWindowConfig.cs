namespace LLib.ImGui;

public interface IPersistableWindowConfig
{
    WindowConfig? WindowConfig { get; }

    void SaveWindowConfig();
}
