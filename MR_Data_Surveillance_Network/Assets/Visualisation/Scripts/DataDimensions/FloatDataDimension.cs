using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Data;
using System.Linq;

public class FloatDataDimension : SyncDataDimension
{
    protected SyncList<float> values = new SyncList<float>();
    //protected SyncDataTable dataTable;
    
    public override int Count { get { return values.Count; } }
    private void OnEnable()
    {
        dataType = typeof(float);
    }
    //TODO max min would be probably usefule

    public float this[int key]
    {
        get => values[key];
        set => values[key] = value;
    }
    public float Max()
    {
        return values.Max();
    }
    public float Min()
    {
        return values.Min();
    }
    // SyncList should not be replaced with a new object instead use add, addRange, clear, remove, removeRange or override previous values
    public void Add(float value) => values.Add(value); 
    public void AddRange(float[] value) => values.AddRange(value); 
    public void Clear() => values.Clear(); 
    public void Remove(float value) => values.Remove(value); 
    public void RemoveAt(int key) => values.RemoveAt(key); 
    public override IEnumerator GetEnumerator() => values.GetEnumerator();

    public override void Initialize()
    {
        ServerInitialize();
        values.Callback += OnDataChanged;
    }
    [Server]
    public void ServerInitialize()
    {
        System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-US"); // needed to parse floats correctly on german PC's since . != ,
        values.AddRange(SyncDataTable.Instance.data[tableName].AsEnumerable().Select(s => s.Field<string>(dimensionName)).ToList<string>().Select(s => float.Parse(s)).ToList<float>());   
    }
    void OnDataChanged(SyncList<float>.Operation op, int index, float oldItem, float newItem)
    {
        base.OnDataChanged();
    }
}
