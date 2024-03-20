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
    public static SeString? GetSeString<T>(this IDataManager dataManager, string key)
        where T : QuestDialogueText
    {
        ArgumentNullException.ThrowIfNull(dataManager);

        return dataManager.GetExcelSheet<T>()?
            .SingleOrDefault(x => x.Key == key)
            ?.Value;
    }

    public static string? GetString<T>(this IDataManager dataManager, string key, IPluginLog? pluginLog)
        where T : QuestDialogueText
    {
        string? text = GetSeString<T>(dataManager, key)?.ToString();

        pluginLog?.Verbose($"{typeof(T).Name}.{key} => {text}");
        return text;
    }

    public static Regex? GetRegex<T>(this IDataManager dataManager, string key, IPluginLog? pluginLog)
        where T : QuestDialogueText
    {
        SeString? text = GetSeString<T>(dataManager, key);
        if (text == null)
            return null;

        string regex = string.Join("", text.Payloads.Select(payload =>
        {
            if (payload is TextPayload)
                return Regex.Escape(payload.RawString);
            else
                return "(.*)";
        }));
        pluginLog?.Verbose($"{typeof(T).Name}.{key} => /{regex}/");
        return new Regex(regex);
    }

    public static SeString? GetSeString<T>(this IDataManager dataManager, uint rowId, Func<T, SeString?> mapper)
        where T : ExcelRow
    {
        ArgumentNullException.ThrowIfNull(dataManager);
        ArgumentNullException.ThrowIfNull(mapper);

        var row = dataManager.GetExcelSheet<T>()?.GetRow(rowId);
        if (row == null)
            return null;

        return mapper(row);
    }

    public static string? GetString<T>(this IDataManager dataManager, uint rowId, Func<T, SeString?> mapper,
        IPluginLog? pluginLog = null)
        where T : ExcelRow
    {
        string? text = GetSeString(dataManager, rowId, mapper)?.ToString();

        pluginLog?.Verbose($"{typeof(T).Name}.{rowId} => {text}");
        return text;
    }

    public static Regex? GetRegex<T>(this IDataManager dataManager, uint rowId, Func<T, SeString?> mapper,
        IPluginLog? pluginLog = null)
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
                return "(.*)";
        }));
        pluginLog?.Verbose($"{typeof(T).Name}.{rowId} => /{regex}/");
        return new Regex(regex);
    }

    public static Regex? GetRegex<T>(this T excelRow, Func<T, SeString?> mapper, IPluginLog? pluginLog)
        where T : ExcelRow
    {
        ArgumentNullException.ThrowIfNull(excelRow);
        ArgumentNullException.ThrowIfNull(mapper);
        SeString? text = mapper(excelRow);
        if (text == null)
            return null;

        string regex = string.Join("", text.Payloads.Select(payload =>
        {
            if (payload is TextPayload)
                return Regex.Escape(payload.RawString);
            else
                return "(.*)";
        }));
        pluginLog?.Verbose($"{typeof(T).Name}.regex => /{regex}/");
        return new Regex(regex);
    }
}
