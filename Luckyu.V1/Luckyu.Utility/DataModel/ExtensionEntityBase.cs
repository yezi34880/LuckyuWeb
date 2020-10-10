using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Luckyu.Utility
{
    public class ExtensionEntityBase
    {
        [NotMapped]
        public JObject ExtObject { get; set; }

        [NotMapped]
        public string ExtDataJson
        {
            get
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(ExtObject);
            }
            set
            {
                ExtObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(value);
            }
        }
    }
}
