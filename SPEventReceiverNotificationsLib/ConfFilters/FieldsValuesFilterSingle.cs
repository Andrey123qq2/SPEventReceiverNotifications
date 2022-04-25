using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPEventReceiverNotificationsLib.ConfFilters
{
    public class FieldsValuesFilterSingle
    {
        [JsonProperty(Required = Required.Always)]
        public string FieldName;
        [JsonProperty(Required = Required.Always)]
        public string Value;
        [JsonProperty(Required = Required.Always)]
        public bool AndMode;
    }
}