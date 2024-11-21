using System.Collections.Generic;
using System.Linq;

namespace LLib.Gear;

public sealed record EquipmentStats(Dictionary<EBaseParam, StatInfo> Stats, byte MateriaCount)
{
    public short Get(EBaseParam param)
    {
        return (short)(GetEquipment(param) + GetMateria(param));
    }

    public short GetEquipment(EBaseParam param)
    {
        Stats.TryGetValue(param, out StatInfo? v);
        return v?.EquipmentValue ?? 0;
    }

    public short GetMateria(EBaseParam param)
    {
        Stats.TryGetValue(param, out StatInfo? v);
        return v?.MateriaValue ?? 0;
    }

    public bool IsOvercapped(EBaseParam param)
    {
        Stats.TryGetValue(param, out StatInfo? v);
        return v?.Overcapped ?? false;
    }

    public bool Has(EBaseParam substat) => Stats.ContainsKey(substat);
    public bool HasMateria() => Stats.Values.Any(x => x.MateriaValue > 0);
}
