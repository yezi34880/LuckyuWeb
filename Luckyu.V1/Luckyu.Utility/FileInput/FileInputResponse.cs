using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.Utility
{
    public class FileInputResponse
    {
        public string error { get; set; }

        public List<string> initialPreview { get; set; } = new List<string>();

        public List<initialPreviewConfig> initialPreviewConfig { get; set; } = new List<initialPreviewConfig>();

        public bool append { get; set; }
    }
}
