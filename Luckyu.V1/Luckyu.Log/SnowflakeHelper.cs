using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Luckyu.Log
{
    public class SnowflakeHelper
    {
        public static IdWorker worker;

        public static string NewCode()
        {
            if (worker == null)
            {
                worker = new IdWorker(1, 1);
            }
            long id = worker.NextId();
            return id.ToString();
        }
    }
}
