using SPCustomHelpers;
using SPCustomHelpers.SPCustomExtensions;
using SPEventReceiverNotificationsLib.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    public class BodyMacrosResolver : IBodyMacrosResolver
    {
        private readonly ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly Dictionary<string, Func<ERItemContext<List<ConfigItem>, ConfigItemGlobal>, string>> _mapMacrosToReplacer = 
        new Dictionary<string, Func<ERItemContext<List<ConfigItem>, ConfigItemGlobal>, string>>
        {
            {"{ITEMURL}", (context) => ReplaceITEMURL(context)},
            {"{ITEMBASEURL}", (context) => ReplaceITEMBASEURL(context)},
            {"{ATTACHURL}", (context) => ReplaceATTACHURL(context)},
            {"{ATTACHNAME}", (context) => ReplaceATTACHNAME(context)},
            {"{EDITOR}", (context) => ReplaceEDITOR(context)},
            {"{RELATEDITEMURL}", (context) => ReplaceRELATEDITEMURL(context)},
            {"{RELATEDITEMTITLE}", (context) => ReplaceRELATEDITEMTITLE(context)},
        };
        public BodyMacrosResolver(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            _context = context;
        }
        public Dictionary<string, string> GetMacrosToValues()
        {
            var mapReplacements = _mapMacrosToReplacer
                .ToDictionary(pair => pair.Key, pair => pair.Value(_context));
            return mapReplacements;
        }
        private static string ReplaceITEMURL(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return context.CurrentItem.GetFullUrl();
        }
        private static string ReplaceITEMBASEURL(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return context.CurrentItem.GetFullBaseUrl();
        }
        private static string ReplaceATTACHURL(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return context.AttachmentUrl;
        }
        private static string ReplaceATTACHNAME(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return Regex.Replace(context.AttachmentUrl, @"^.*\/", "");
        }
        private static string ReplaceEDITOR(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return context.EventProperties.UserDisplayName;
        }
        private static string ReplaceRELATEDITEMURL(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return context.RelatedItem?.GetFullUrl() ?? string.Empty;
        }
        private static string ReplaceRELATEDITEMTITLE(ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            return context.RelatedItem?.Title ?? string.Empty;
        }
    }
}