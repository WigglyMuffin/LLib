using Dalamud.Memory;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace LLib.GameUI;

public static class LAtkValue
{
    public static unsafe string? ReadAtkString(this AtkValue atkValue)
    {
        if (atkValue.Type == ValueType.Undefined)
            return null;
        if (atkValue.String.HasValue)
            return MemoryHelper.ReadSeStringNullTerminated(new nint(atkValue.String)).ToString();
        return null;
    }
}
