using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Data;
using System.Linq;
using System;

public class StringDataDimension : SyncDataDimension
{
    protected SyncList<string> values = new SyncList<string>();
    //protected SyncDataTable dataTable;
    public override int Count{get{return values.Count;}}

    //TODO max min would be probably usefule
    private void OnEnable()
    {
        dataType = typeof(string);
    }
    public string this[int key]
    {
        get => values[key];
        set => values[key] = value;
    }

    // SyncList should not be replaced with a new object instead use add, addRange, clear, remove, removeRange or override previous values
    public void Add(string value) => values.Add(value);
    public void AddRange(string[] value) => values.AddRange(value);
    public void Clear() => values.Clear();
    public void Remove(string value) => values.Remove(value); 
    public void RemoveAt(int key) => values.RemoveAt(key); 
    public override IEnumerator GetEnumerator() => values.GetEnumerator();

    [Server]
    public void ServerInitialize()
    {
        values.AddRange(SyncDataTable.Instance.data[tableName].AsEnumerable().Select(s => s.Field<string>(dimensionName)).ToList<string>());
    }
    public override void Initialize()
    {
        ServerInitialize();
        values.Callback += OnDataChanged;
    }

    void OnDataChanged(SyncList<string>.Operation op, int index, string oldItem, string newItem)
    {
        base.OnDataChanged();
    }

}
