using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDBData
{
    internal static class DBQueryStr
    {
        // WILL BE USET FOR TABLE AND VIEWS
        internal static string DBScriptUpdate = " UPDATE @TABLE SET @PARAMTERS ";
        internal static string DBScriptSelect = " SELECT * FROM  @TABLE ";
        internal static string DBScriptDelete = " DELETE FROM @TABLE ";
        internal static string DBScriptInsert = " INSERT INTO @TABLE(@PARAMETERS) VALUES (@VALUES) ";
        internal static string DBScriptWhere = " WHERE ";
        internal static string paramFilterSuffix = "_FILTER";
        internal static string paramValueSuffix = "_VALUE";
        internal static string dbTable = "@TABLE";
        internal static string dbParameter = "@PARAMETER";
    }
}
