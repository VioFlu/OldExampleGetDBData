using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDBData
{
    public enum ParamSpecific { DBFilter, DBValue }
    public class DBParameter
    {
        public string paramName { get; set; }
        public string paramValue { get; set; }
        public string paramType { get; set; }
        public SqlDbType paramSQLType{get; set;}
        public ParamSpecific paramSpec { get; set; }
    }
}
