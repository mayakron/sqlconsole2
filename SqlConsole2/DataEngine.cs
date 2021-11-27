using System;
using System.Data;

namespace SqlConsole2
{
    internal static class DataEngine
    {
        public static DataSet ExecuteCommand(string providerType, string connectionString, string commandText, int commandTimeout)
        {
            switch (providerType)
            {
                case "SqlServer":
                case "SQLite":
                case "MySql":
                case "OleDb":
                case "Odbc":
                    return DataEngineDb.ExecuteDbCommand(providerType, connectionString, commandText, commandTimeout);

                case "Wmi":
                    return DataEngineWmi.ExecuteWmiCommand(providerType, connectionString, commandText, commandTimeout);

                default:
                    throw new ArgumentOutOfRangeException(providerType);
            }
        }
    }
}