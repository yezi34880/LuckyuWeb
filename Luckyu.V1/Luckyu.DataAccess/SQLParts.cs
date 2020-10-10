using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luckyu.DataAccess
{
    public struct SQLParts
    {
        /// <summary>
        ///     The SQL.
        /// </summary>
        public string Sql;

        /// <summary>
        ///     The SQL Order By
        /// </summary>
        public string SqlOrderBy;

        /// <summary>
        ///     The SQL Form
        /// </summary>
        public string SqlForm;

        /// <summary>
        ///     The SQL Select Column
        /// </summary>
        public string SqlSelectColumn;

        /// <summary>
        ///     The SQL Select Order Removed
        /// </summary>
        public string SqlSelectOrderRemoved;
    }
}
