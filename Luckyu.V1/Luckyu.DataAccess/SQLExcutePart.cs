using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    public class SQLExcutePart
    {
        public string Sql { get; set; }

        public List<SugarParameter> SqlParams { get; set; }

    }
}
