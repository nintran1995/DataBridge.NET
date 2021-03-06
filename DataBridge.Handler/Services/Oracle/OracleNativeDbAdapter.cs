﻿using DataConnectors.Adapter.DbAdapter;
using DataConnectors.Adapter.DbAdapter.ConnectionInfos;

namespace DataBridge.Services
{
    public class OracleNativeDbAdapter : DbAdapter
    {
        public OracleNativeDbAdapter()
        {
            this.ConnectionInfo = new OracleNativeDbConnectionInfo();
        }
    }
}