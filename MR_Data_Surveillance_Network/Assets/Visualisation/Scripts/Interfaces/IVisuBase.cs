using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisuBase
{
    void Initialize();
    Presenter presenter { get; set; }
    void Reload();
    /*delegate void OnDataChangedHandler();
    event OnDataChangedHandler DataChanged;*/
}
