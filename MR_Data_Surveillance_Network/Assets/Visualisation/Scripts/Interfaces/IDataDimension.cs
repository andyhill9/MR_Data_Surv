using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataDimension<T>
{

    System.Type dataType { get;}
    int Count { get; }
    IEnumerator GetEnumerator();
    void Initialize();
    
}
