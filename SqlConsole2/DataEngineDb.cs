using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace SqlConsole2
{
    internal static class DataEngineDb
    {
        public static DataSet ExecuteDbCommand(string providerType, string connectionString, string commandText, int commandTimeout)
        {
            var commandBlocks = new List<string>();

            using (var stringReader = new StringReader(commandText))
            {
                var commandBlockBuilder = new StringBuilder();

                string commandBlockLine; while ((commandBlockLine = stringReader.ReadLine()) != null)
                {
                    var commandBlockLineTrimEnd = commandBlockLine.TrimEnd();

                    if (commandBlockLineTrimEnd.Equals("GO", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (commandBlockBuilder.Length > 0)
                        {
                            commandBlocks.Add(commandBlockBuilder.ToString());

                            commandBlockBuilder.Clear();
                        }
                    }
                    else if (!string.IsNullOrEmpty(commandBlockLineTrimEnd))
                    {
                        commandBlockBuilder.AppendLine(commandBlockLineTrimEnd);
                    }
                }

                if (commandBlockBuilder.Length > 0)
                {
                    commandBlocks.Add(commandBlockBuilder.ToString());
                }
            }

            var commandBlocksCount = commandBlocks.Count;

            if (commandBlocksCount > 0)
            {
                if (commandBlocksCount > 1)
                {
                    using (var connection = GetDbConnection(providerType, connectionString))
                    {
                        connection.Open();

                        foreach (var commandBlock in commandBlocks)
                        {
                            using (var command = connection.CreateCommand())
                            {
                                command.CommandTimeout = commandTimeout; command.CommandType = CommandType.Text; command.CommandText = commandBlock;

                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
                else
                {
                    var dataSet = new DataSet("Root");

                    using (var connection = GetDbConnection(providerType, connectionString))
                    {
                        connection.Open();

                        using (var command = connection.CreateCommand())
                        {
                            command.CommandTimeout = commandTimeout; command.CommandType = CommandType.Text; command.CommandText = commandBlocks[0];

                            var dataAdapter = GetDbDataAdapter(providerType, command); dataAdapter.Fill(dataSet);

                            return dataSet;
                        }
                    }
                }
            }

            return null;
        }

        private static IDbConnection GetDbConnection(string providerType, string connectionString)
        {
            switch (providerType)
            {
                case "SqlServer":
                    return new System.Data.SqlClient.SqlConnection(connectionString);

                case "SQLite":
                    return new System.Data.SQLite.SQLiteConnection(connectionString);

                case "MySql":
                    return new MySql.Data.MySqlClient.MySqlConnection(connectionString);

                case "OleDb":
                    return new System.Data.OleDb.OleDbConnection(connectionString);

                case "Odbc":
                    return new System.Data.Odbc.OdbcConnection(connectionString);

                default:
                    throw new ArgumentOutOfRangeException(providerType);
            }
        }

        private static IDataAdapter GetDbDataAdapter(string providerType, IDbCommand command)
        {
            switch (providerType)
            {
                case "SqlServer":
                    return new System.Data.SqlClient.SqlDataAdapter((System.Data.SqlClient.SqlCommand)command);

                case "SQLite":
                    return new System.Data.SQLite.SQLiteDataAdapter((System.Data.SQLite.SQLiteCommand)command);

                case "MySql":
                    return new MySql.Data.MySqlClient.MySqlDataAdapter((MySql.Data.MySqlClient.MySqlCommand)command);

                case "OleDb":
                    return new System.Data.OleDb.OleDbDataAdapter((System.Data.OleDb.OleDbCommand)command);

                case "Odbc":
                    return new System.Data.Odbc.OdbcDataAdapter((System.Data.Odbc.OdbcCommand)command);

                default:
                    throw new ArgumentOutOfRangeException(providerType);
            }
        }
    }
}