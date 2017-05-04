﻿using System.Collections.Generic;
using System.Data;
using System.Xml.Serialization;
using DataPipeline.Helper;
using DataPipeline.Services;

namespace DataPipeline.Commands
{
    public class OracleTableReader : PipelineCommand
    {
        private int maxRowsToRead = -1;
        private DbAdapter dbAdapter = new DbAdapter();

        public OracleTableReader()
        {
            this.Parameters.Add(new CommandParameter() { Name = "User" });
            this.Parameters.Add(new CommandParameter() { Name = "TableName" });
            this.Parameters.Add(new CommandParameter() { Name = "Database" });
            this.Parameters.Add(new CommandParameter() { Name = "Password" });
            this.Parameters.Add(new CommandParameter() { Name = "Data", Direction = "Out" });

            dbAdapter.ConnectionString = "";
            dbAdapter.DataTypeMappings = new Dictionary<string, string>()
                        {
                            { "System.String", "VARCHAR2()" },
                            { "System.DateTime", "DATE" },
                            { "System.Single", "NUMBER" },
                            { "System.Int32", "NUMBER" }
                        };
        }

        [XmlIgnore]
        public string User
        {
            get { return this.Parameters.GetValue<string>("User"); }
            set { this.Parameters.SetOrAddValue("User", value); }
        }

        [XmlIgnore]
        public string TableName
        {
            get { return this.Parameters.GetValue<string>("TableName"); }
            set { this.Parameters.SetOrAddValue("TableName", value); }
        }

        [XmlIgnore]
        public string Database
        {
            get { return this.Parameters.GetValue<string>("Database"); }
            set { this.Parameters.SetOrAddValue("Database", value); }
        }

        [XmlIgnore]
        public string Password
        {
            get { return this.Parameters.GetValue<string>("Password"); }
            set { this.Parameters.SetOrAddValue("Password", value); }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override IEnumerable<CommandParameters> Execute(CommandParameters inParameters)
        {
            //inParameters = GetCurrentInParameters();
            string userName = inParameters.GetValue<string>("User");
            string tableName = inParameters.GetValue<string>("TableName");
            string database = inParameters.GetValue<string>("Database");
            string password = inParameters.GetValue<string>("Password");

            dbAdapter.UserName = userName;
            dbAdapter.TableName = tableName;

            dbAdapter.ConnectionString = "Provider=OraOLEDB.Oracle;"
                                         + "Data Source=" + database + ";"
                                         + "User Id=" + userName + ";"
                                         + "Password=" + password + ";"
                                         + "OLEDB.NET=true;";

            dbAdapter.Connect();

            bool cancel = false;
            while (!cancel)
            {
                DataTable table = dbAdapter.ReadTableData(maxRowsToRead);

                if (table.Rows.Count == 0)
                {
                    cancel = true;
                }

                var outParameters = GetCurrentOutParameters();
                outParameters.SetOrAddValue("Data", table);
                outParameters.SetOrAddValue("DataName", table.TableName);
                yield return TransferOutParameters(outParameters);

                DataTableHelper.DisposeTable(table);
            }

            dbAdapter.Disconnect();
        }
    }
}