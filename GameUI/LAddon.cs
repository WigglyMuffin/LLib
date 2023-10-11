using System;
using System.Linq;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace LLib.GameUI;

public static class LAddon
{
    private const int UnitListCount = 18;
    public static unsafe AtkUnitBase* GetAddonById(uint id)
    {
        var unitManagers = &AtkStage.GetSingleton()->RaptureAtkUnitManager->AtkUnitManager.DepthLayerOneList;
        for (var i = 0; i < UnitListCount; i++)
        {
            var unitManager = &unitManagers[i];
            foreach (var j in Enumerable.Range(0, Math.Min(unitManager->Count, unitManager->EntriesSpan.Length)))
            {
                var unitBase = unitManager->EntriesSpan[j].Value;
                if (unitBase != null && unitBase->ID == id)
                {
                    return unitBase;
                }
            }
        }

        return null;
    }

    public static unsafe bool TryGetAddonByName<T>(this IGameGui gameGui, string addonName, out T* addonPtr)
        where T : unmanaged
    {
        var a = gameGui.GetAddonByName(addonName);
        if (a != IntPtr.Zero)
        {
            addonPtr = (T*)a;
            return true;
        }
        else
        {
            addonPtr = null;
            return false;
        }
    }

    public static unsafe bool IsAddonReady(AtkUnitBase* addon)
    {
        return addon->IsVisible && addon->UldManager.LoadedState == AtkLoadState.Loaded;
    }
}
