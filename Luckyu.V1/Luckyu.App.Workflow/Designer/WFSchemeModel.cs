﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.App.Workflow
{
    public class WFSchemeModel
    {
        public List<WFSchemeNodeModel> nodes { get; set; }
        public List<WFSchemeLineModel> lines { get; set; }
    }
}
