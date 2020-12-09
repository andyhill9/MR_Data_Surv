using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using u2vis;
using Mirror;
[RequireComponent(typeof(IGraph))]
public class Presenter : NetworkBehaviour
{
    // Holds the data dimensions of this presenter as a list of indices to the data dimensions of the data provider.
    // Yes, it would be better and more efficient to simply hold the references to the data dimensions directly,
    // but have fun to try to get that to work with the unity editor in a stable way.
    [SerializeField]
    public SyncDataDimension[] dataDimensions = null;
    [SerializeField]
    protected bool[] highlightedItems = null;
    //Represented the reference of Axis which also is showed in the editor script
    [SerializeField]
    protected AxisPresenter[] _axisPresenters = null;
    [SerializeField]
    protected GeneralAxis[] chart = null;
    protected IGraph graph = null;
    protected bool _isInitialized = false;
    public int NumberOfDimensions => dataDimensions.Length;
    public AxisPresenter[] AxisPresenters => _axisPresenters;
    public void Initialize()
    {
        // in case the provider was set in the editor
        if (!_isInitialized)
        {
            //chart = gameObject.GetComponent<IChart>();
            graph = gameObject.GetComponent<IGraph>();
            //_dataProvider.DataChanged += Provider_DataUpdated;
            //highlightedItems = new bool[TotalItemCount];
        }
        foreach (var x in dataDimensions)
        {
            x.Initialize();
            x.DataChanged += Reload;
        }
        foreach (var x in chart)
        {
            x.presenter = this;
            x.Initialize();
        }
        /*chart.presenter = this;
        chart.Initialize();*/
        graph.presenter = this;
        graph.Initialize();
    }
    public virtual void Reload()
    {
        foreach (var x in chart)
        {
            x.Reload();
        }
        graph.Reload();
    }
    protected virtual void Start()
    {

        Initialize();

    }
}