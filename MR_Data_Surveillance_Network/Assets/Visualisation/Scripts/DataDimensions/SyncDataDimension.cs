using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Data;
/// <summary>
/// basic class for all DataDimensions which will be synced over the network 
/// each derived class is expected to contain a SyncList<float> values 
/// this can't be included in this class since Unity neither supports template Monobehaviours 
/// nor default implementation of interface methods
/// changing this to an interface wouldn't allow it to be used in the editor
/// </summary>
public abstract class SyncDataDimension: NetworkBehaviour, IEnumerable
{
    [SerializeField]
    public string dimensionName = null;
    [SerializeField]
    public string tableName = "sample";
    public Type dataType = typeof(string);
    public abstract int Count { get; }
    //public abstract int getCount();
    public abstract IEnumerator GetEnumerator();
    public abstract void Initialize();

    //public Action OnDataChangedHandler();
    public event Action DataChanged;

    protected virtual void OnDataChanged()
    {
        DataChanged?.Invoke();
    }
}

