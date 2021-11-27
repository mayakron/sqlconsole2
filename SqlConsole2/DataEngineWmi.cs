using System;
using System.Data;
using System.Management;
using System.Text.RegularExpressions;

namespace SqlConsole2
{
    internal static class DataEngineWmi
    {
        public static DataSet ExecuteWmiCommand(string providerType, string connectionString, string commandText, int commandTimeout)
        {
            var connectionStringMatch = Regex.Match(connectionString, "^Path=([^;]*);Domain=([^;]*);UserName=([^;]*);Password=(.*)$", RegexOptions.None);

            if (!connectionStringMatch.Success)
            {
                throw new ArgumentException("Invalid Wmi connection string.");
            }

            var path = connectionStringMatch.Groups[1].Captures[0].Value;
            var domain = connectionStringMatch.Groups[2].Captures[0].Value;
            var userName = connectionStringMatch.Groups[3].Captures[0].Value;
            var password = connectionStringMatch.Groups[4].Captures[0].Value;

            var dataSet = new DataSet("Root");

            if (!string.IsNullOrEmpty(domain) && !string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                dataSet.Tables.Add(GetWmiDataTable(new ManagementObjectSearcher(new ManagementScope(path, new ConnectionOptions() { Authority = "ntlmdomain:" + domain, Username = userName, Password = password }), new ObjectQuery(commandText)).Get()));
            }
            else
            {
                dataSet.Tables.Add(GetWmiDataTable(new ManagementObjectSearcher(new ManagementScope(path), new ObjectQuery(commandText)).Get()));
            }

            return dataSet;
        }

        private static DataTable GetWmiDataTable(ManagementObjectCollection managementObjectCollection)
        {
            if ((managementObjectCollection != null) && (managementObjectCollection.Count > 0))
            {
                var dataTable = new DataTable();

                foreach (ManagementObject managementObject in managementObjectCollection)
                {
                    foreach (PropertyData propertyData in managementObject.Properties)
                    {
                        dataTable.Columns.Add(propertyData.Name, typeof(string));
                    }

                    break;
                }

                foreach (ManagementObject managementObject in managementObjectCollection)
                {
                    var dataRow = dataTable.NewRow();

                    foreach (PropertyData propertyData in managementObject.Properties)
                    {
                        dataRow[propertyData.Name] = propertyData.Value;
                    }

                    dataTable.Rows.Add(dataRow);
                }

                return dataTable;
            }
            else
            {
                return null;
            }
        }
    }
}