using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Data;
using System;
using System.Text;

public class SyncDataTable// : NetworkBehaviour
{
    
    public Dictionary<string, DataTable> data = new Dictionary<string, DataTable>();

    private SyncDataTable()
    {
        LoadDataServer(Application.streamingAssetsPath + @"\RandomSamples3.csv", "sample", ';');
    }
    private static SyncDataTable instance = null;
    public static SyncDataTable Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SyncDataTable();
            }
            return instance;
        }
    }
    /// <summary>
    /// loads a simple csv from file all as string the DataDimension need to handle the conversion of data types (in order to extract rows they need the same type)
    /// </summary>
    /// <param name="path">csv path</param>
    /// <param name="tableName">the name for the dict in wich the data is stored</param>
    /// <param name="delimiter">seperation between cells</param>
    /// <param name="firstRowHeader">if the first row contains the headers</param>
    private void LoadDataServer(string path,string tableName, char delimiter, bool firstRowHeader=true) {

        GenericParsing.GenericParserAdapter parser = new GenericParsing.GenericParserAdapter(path)
        {
            ColumnDelimiter = delimiter,
            FirstRowHasHeader = firstRowHeader
        };
        data[tableName] = parser.GetDataTable();
    }
    /// <summary>
    /// Basic function to dump some dataTable in Debug.Log
    /// </summary>
    /// <param name="data">The dataTable</param>
    /// <param name="maxRow">Max printed rows</param>
    /// <param name="maxCol">Max printed cols</param>
    private void DebugLogDataTable(DataTable data, int maxRow=10, int maxCol=10)
    {
        Debug.Log(ToStringDataTable(data, maxRow, maxCol));
    }
    /// <summary>
    /// Basic function which transforms a DataTable in a string
    /// </summary>
    /// <param name="data">The dataTable</param>
    /// <param name="maxRow">Max printed rows</param>
    /// <param name="maxCol">Max printed cols</param>
    /// <returns></returns>
    private string ToStringDataTable(DataTable data, int maxRow = 10, int maxCol = 10)
    {
        if (data.Rows.Count < maxRow) maxRow = data.Rows.Count;
        if (data.Columns.Count < maxCol) maxCol = data.Columns.Count;
        StringBuilder sb = new StringBuilder();
        if (null != data && null != data.Rows)
        {
            int rowIndex = 0;
            foreach (DataColumn column in data.Columns)
            {
                if (rowIndex > maxRow) break;
                sb.Append(column.ColumnName);
                sb.Append(',');
                rowIndex++;
            }
            sb.AppendLine();
            rowIndex = 0;
            foreach (DataRow dataRow in data.Rows)
            {
                if (rowIndex > maxRow) break;
                int colIndex = 0;
                foreach (var item in dataRow.ItemArray)
                {
                    if (colIndex > maxCol) break;
                    sb.Append(item);
                    sb.Append(',');
                    colIndex++;
                }
                sb.AppendLine();
                rowIndex++;
            }
            return sb.ToString();
        }
        return "";
    }
    private DataTable ConvertDataTableTypes(DataTable dataInput)
    {
        System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US"); // needed to parse floats correctly on german PC's since . != ,
        DataTable data = dataInput.Clone();
        for(int i=0; i<dataInput.Columns.Count; i++)
        {
            var col = dataInput.Rows[0][i];
            if (int.TryParse(col.ToString(), out _))
            {
                data.Columns[i].DataType = typeof(int);
            }
            else if (float.TryParse(col.ToString(), out _))
            {
                data.Columns[i].DataType = typeof(float);
            }
            else if (bool.TryParse(col.ToString(), out _))
            {
                data.Columns[i].DataType = typeof(bool);
            }
        }
        foreach (DataRow row in dataInput.Rows)
        {
            data.ImportRow(row);
        }
        return data;
    }
}
