namespace GetDBData
{
    public class DBSQLCommand
    {
        string stringConnection;
        string sqlType;
        string sqlAction;
        string sqlObject;

        public DBSQLCommand(string stringConnection, string sqlType, string sqlAction, string sqlObject)
        {

            this.sqlType = sqlType;
            this.sqlObject = sqlObject;
            this.sqlAction = sqlAction;
        }
        public string SQLAction
        {
            set { this.sqlAction = value; }
            get { return sqlAction; }
        }
        public string SQLObject
        {
            set { this.sqlObject = value; }
            get { return sqlObject; }
        }

        public virtual void ExecuteSQlCommand()
        {

            DBDataHandler.RunSQLCommand(stringConnection, sqlAction);
        }

    }
}
