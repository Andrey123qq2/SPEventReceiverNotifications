﻿using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SPCustomHelpers;
using SPItemFieldHelpers;

namespace SPEventReceiverNotificationsLib.SendersHTMLBodyAndSubject
{
    class SubjectCreator
    {
        protected ERItemContext<List<ConfigItem>, ConfigItemGlobal> _context;
        private readonly Regex _subjetTemplateRegex = new Regex(@"(?<={)\w+(?=})");
        private readonly string _subjectTemplate;
        private readonly SPItemEventProperties _properties;
        public SubjectCreator(string subjectTemplate, ERItemContext<List<ConfigItem>, ConfigItemGlobal> context)
        {
            _context = context;
            _properties = _context.EventTypeAsync ? null : _context.EventProperties;
            _subjectTemplate = subjectTemplate;
        }
        public string GetSubject()
        {
            StringBuilder subjectBuilder = new StringBuilder(_subjectTemplate);
            _subjetTemplateRegex
                .Matches(_subjectTemplate)
                .Cast<Match>()
                .Select(m => m.ToString())
                .ToList()
                .ForEach(f => subjectBuilder
                        .Replace(
                            $"{{{f}}}", SPItemFieldWrapperFactory.Create(_context.CurrentItem, f, _properties).GetValueAfterFriendly())
                    );
            return subjectBuilder.ToString();
        }
    }
}
