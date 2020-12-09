using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Data;
using System.Linq;

public class BoolDataDimension : SyncDataDimension
{
    protected SyncList<bool> values = new SyncList<bool>();
    //protected SyncDataTable dataTable;
    private void OnEnable()
    {
        dataType = typeof(bool);
    }
    public override int Count { get { return values.Count; } }

    //TODO max min would be probably usefule

    public bool this[int key]
    {
        get => values[key];
        set => values[key] = value;
    }

    // SyncList should not be replaced with a new object instead use add, addRange, clear, remove, removeRange or override previous values
    public void Add(bool value) => values.Add(value); 
    public void AddRange(bool[] value) => values.AddRange(value); 
    public void Clear() => values.Clear(); 
    public void Remove(bool value) => values.Remove(value); 
    public void RemoveAt(int key) => values.RemoveAt(key); 
    public override IEnumerator GetEnumerator() => values.GetEnumerator(); 
    //public BoolDataDimension(string name) : base(name, typeof(bool)) { }
    [Server]
    public void ServerInitialize()
    {
        values.AddRange(SyncDataTable.Instance.data[tableName].AsEnumerable().Select(s => s.Field<string>(dimensionName)).ToList<string>().Select(s => bool.Parse(s)).ToList<bool>());
    }
    public override void Initialize()
    {
        ServerInitialize();
        values.Callback += OnDataChanged;
    }

    void OnDataChanged(SyncList<bool>.Operation op, int index, bool oldItem, bool newItem)
    {
        base.OnDataChanged();
    }
}
