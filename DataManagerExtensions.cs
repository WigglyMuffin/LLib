using System;
using System.Linq;
using System.Text.RegularExpressions;
using Dalamud.Plugin.Services;
using Lumina.Excel;
using Lumina.Excel.CustomSheets;
using Lumina.Text;
using Lumina.Text.Payloads;

namespace LLib;

public static class DataManagerExtensions
{
    public static string GetDialogue<T>(IDataManager dataManager, string key, IPluginLog? pluginLog)
        where T : QuestDialogueText
    {
        string result = dataManager.GetExcelSheet<T>()!
            .Single(x => x.Key == key)
            .Value
            .ToString();
        pluginLog?.Verbose($"{typeof(T).Name}.{key} => {result}");
        return result;
    }

    public static SeString? GetSeString<T>(this IDataManager dataManager, uint rowId, Func<T, SeString?> mapper)
        where T : ExcelRow
    {
        var row = dataManager.GetExcelSheet<T>()?.GetRow(rowId);
        if (row == null)
            return null;

        return mapper(row);
    }

    public static  string? GetString<T>(this IDataManager dataManager, uint rowId, Func<T, SeString?> mapper, IPluginLog? pluginLog = null)
        where T : ExcelRow
    {
        string? text = GetSeString(dataManager, rowId, mapper)?.ToString();

        pluginLog?.Verbose($"{typeof(T).Name}.{rowId} => {text}");
        return text;
    }

    public static  Regex? GetRegex<T>(this IDataManager dataManager, uint rowId, Func<T, SeString?> mapper, IPluginLog? pluginLog = null)
        where T : ExcelRow
    {
        SeString? text = GetSeString(dataManager, rowId, mapper);
        if (text == null)
            return null;

        string regex = string.Join("", text.Payloads.Select(payload =>
        {
            if (payload is TextPayload)
                return Regex.Escape(payload.RawString);
            else
                return ".*";
        }));
        pluginLog?.Verbose($"{typeof(T).Name}.{rowId} => /{regex}/");
        return new Regex(regex);
    }
}
