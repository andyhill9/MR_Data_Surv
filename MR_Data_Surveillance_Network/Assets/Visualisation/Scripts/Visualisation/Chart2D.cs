using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chart2D : MonoBehaviour, IChart
{

    [SerializeField]
    protected ViewAxis _viewAxisPrefab = null;
    //[SerializeField]
    protected List<ViewAxis> _axisViews = null;
    [SerializeField]
    protected PresenterAxis[] _axisPresenter = new PresenterAxis[2];
    public Presenter presenter { get; set; } = null;

    public void Reload() { SetupInitialAxisViews(); }

    private void Start()
    {
    }


    /// <summary>
    /// Instantiates and transforms the default Axis Views for this visualization.
    /// The BaseVisualizationView defaults to a 2D visualization with an X and Y axis.
    /// If a derived visualization needs different axes, override this method.
    /// </summary>
    public virtual void SetupInitialAxisViews()
    {
        DestroyAxisViews();
        _axisViews = new List<ViewAxis>();
        // Generic X Axis
        var vX = Instantiate(_viewAxisPrefab, transform, false);
        vX.GetComponent<ViewAxis>().AxisPresenter = _axisPresenter[0];
        _axisViews.Add(vX);
        vX.GetComponent<ViewAxis>().RebuildAxis(_axisPresenter[0].GenerateFromDiscreteRange(0,presenter.dataDimensions[0].Count));
        // Generic Y Axis
        var vY = Instantiate(_viewAxisPrefab, transform, false);
        vY.GetComponent<ViewAxis>().AxisPresenter = _axisPresenter[1];
        vY.transform.localRotation = Quaternion.Euler(0, 0, 90);
        vY.Swapped = true;
        _axisViews.Add(vY);
        vY.GetComponent<ViewAxis>().RebuildAxis( _axisPresenter[1].GenerateFromMinMaxValue(0, ((FloatDataDimension)presenter.dataDimensions[1]).Max()));
    }
    protected virtual void DestroyAxisViews()
    {
        if (_axisViews == null)
            return;
        foreach (var axisView in _axisViews)
            if (axisView != null)
                DestroyImmediate(axisView.gameObject);
        _axisViews = null;
    }

    public void Initialize()
    {
        SetupInitialAxisViews();
    }
}
