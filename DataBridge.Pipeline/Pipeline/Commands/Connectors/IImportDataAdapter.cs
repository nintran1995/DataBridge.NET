﻿using DataConnectors.Common.Model;

namespace DMF.Data.DataAdapters
{
    using System.Collections.Generic;
    using System.Data;

    public interface IImportDataAdapter
    {
        // ***********************Properties***********************

        FieldDefinitionList FieldDefinitions { get; }

        // ***********************Functions***********************

        bool Connect();

        bool Disconnect();

        IList<string> GetAvailableTables();

        IList<Field> GetAvailableColumns();

        IEnumerable<DataTable> ReadData();

        IEnumerable<DataTable> ReadData(int? blockSize);

        int GetCount();
    }
}