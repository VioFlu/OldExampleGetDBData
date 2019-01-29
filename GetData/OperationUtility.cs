using Common.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDBData
{ 
    internal static class OperationUtility
    {
        private static ILog log = LogManager.GetCurrentClassLogger();
        internal static void IgnoreError(System.Action action){
            try{
                action.Invoke();
            }
            catch{
            }
        }
        internal static string createSQLCmdExe(DBQueryType sqlActType, string tblName, List<DBParameter> sqlParamValues, List<DBParameter> sqlParamFilters){
            string sqlCMDExe = "";
            try{
                switch (sqlActType)
                {
                    case DBQueryType.QDelete:
                        //get the standard delete script
                        sqlCMDExe = DBQueryStr.DBScriptDelete.Replace(DBQueryStr.dbTable, tblName);
                        // add the where criteria
                        sqlCMDExe = sqlCMDExe + assignFilterParam(sqlParamFilters);
                        break;
                    case DBQueryType.QUpdate:
                        sqlCMDExe = DBQueryStr.DBScriptUpdate.Replace(DBQueryStr.dbTable, tblName);
                        sqlCMDExe = sqlCMDExe.Replace(DBQueryStr.dbParameter, assignValueParam(sqlParamValues)) +
                                    DBQueryStr.DBScriptWhere + assignFilterParam(sqlParamFilters);
                        break;
                    case DBQueryType.QSelect:
                        sqlCMDExe = DBQueryStr.DBScriptSelect.Replace(DBQueryStr.dbTable, tblName);
                        break;
                    case DBQueryType.QInsert:
                        sqlCMDExe = DBQueryStr.DBScriptInsert.Replace(DBQueryStr.dbTable, tblName);
                        break;
                }

                return sqlCMDExe;
            }catch (Exception ex){
                log.Trace("GetDBData.OperationUtility.createSQLCMDExe" + ex.Message);
                return sqlCMDExe;
            }
        }
        internal static string assignFilterParam(List<DBParameter> sqlFilters){
            string assignOperation = "  ";
            try{
                foreach (DBParameter parm in sqlFilters)
                {
                    assignOperation = assignOperation + parm.paramName + " = " + "@" + parm.paramName + DBQueryStr.paramFilterSuffix + ", ";
                }
                return assignOperation.Substring(1, (assignOperation.Length - 2));
            }
            catch (Exception ex){
                log.Trace("GetDBData.OperationUtility.assignFilterParam" + ex.Message);
                return assignOperation;
            }
        }
        internal static string assignValueParam(List<DBParameter> sqlValues){
            string assignOperation = "  ";
            try{
                foreach (DBParameter parm in sqlValues)
                {
                    assignOperation = assignOperation + parm.paramName + " = " + "@" + parm.paramName + DBQueryStr.paramValueSuffix + ", ";
                }
                return assignOperation.Substring(1, (assignOperation.Length - 2));
            }catch (Exception ex)
            {
                log.Trace("GetDBData.OperationUtility.assignValueParam" + ex.Message);
                return assignOperation;
            }
        }

        // assign 
        internal static List<SqlParameter> createSQLParameter(List<DBParameter> dbParams){
            
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            try
            {
                foreach (DBParameter dbParam in dbParams)
                {
                    switch (dbParam.paramSpec)
                    {
                        case ParamSpecific.DBFilter:
                            sqlParams.Add(new SqlParameter() { ParameterName = dbParam.paramName + DBQueryStr.paramFilterSuffix, SqlDbType = dbParam.paramSQLType, SqlValue = dbParam.paramValue });
                            break;
                        case ParamSpecific.DBValue:
                            sqlParams.Add(new SqlParameter() { ParameterName = dbParam.paramName + DBQueryStr.paramValueSuffix, SqlDbType = dbParam.paramSQLType, SqlValue = dbParam.paramValue });
                            break;
                        default:
                            sqlParams.Add(new SqlParameter() { ParameterName = dbParam.paramName, SqlDbType = dbParam.paramSQLType, SqlValue = dbParam.paramValue });
                            break;
                    }


                }
                return sqlParams;
            }
            catch (Exception ex) {
                log.Trace("GetDBData.OperationUtility.createSQLParameter " + ex.Message);
                return sqlParams;
            }
            // some parameters are give as filters some are give as values
        }
    }
}
