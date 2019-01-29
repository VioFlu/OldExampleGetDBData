using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDBData
{
    public enum DBQueryType { QSelect, QUpdate, QInsert, QDelete }; 
   
    public class DBUserTable : DBSQLCommand
    {
        DBQueryType operationType;
        List<DBParameter> colFilters;
        List<DBParameter> colValues;
        List<DBParameter> colParams;


        public DBUserTable(string stringConnection, string sqlType, string sqlAction,
                            string sqlObject, DBQueryType operationType, List<DBParameter> colParams)
            : base(stringConnection, sqlType, sqlAction, sqlObject)
        {
            this.operationType = operationType;
            this.ColParams = colParams;
           // SplitParameters();
        }
        public DBUserTable(string stringConnection, string sqlType,
                            string sqlAction, string sqlObject, DBQueryType operationType)
            : base(stringConnection, sqlType, sqlAction, sqlObject)
        {
            this.operationType = operationType;
        }
        public List<DBParameter> ColParams{
            set
            {
                colParams = value;
                SplitParameters();
            }
        }
        private void SplitParameters(){
            foreach(DBParameter splitParam in colParams){
                switch (splitParam.paramSpec){
                    case ParamSpecific.DBFilter:
                        colFilters.Add(new DBParameter()
                                                            {
                                                                paramName = splitParam.paramName,
                                                                paramSpec = splitParam.paramSpec,
                                                                paramSQLType = splitParam.paramSQLType,
                                                                paramValue = splitParam.paramValue
                                                            });
                        break;
                    case ParamSpecific.DBValue:
                        colValues.Add(new DBParameter(){
                                                        paramName = splitParam.paramName,
                                                        paramSpec = splitParam.paramSpec,
                                                        paramSQLType = splitParam.paramSQLType,
                                                        paramValue = splitParam.paramValue});
                        break;
                }
            }
        }

        public override void ExecuteSQlCommand()
        {
            if (colFilters != null){
                //map columns to filters and add them 
                base.SQLAction = OperationUtility.createSQLCmdExe(operationType, base.SQLObject, colValues, colFilters);
            }
            base.ExecuteSQlCommand();
            //if we do have filters
        }


    }
}
