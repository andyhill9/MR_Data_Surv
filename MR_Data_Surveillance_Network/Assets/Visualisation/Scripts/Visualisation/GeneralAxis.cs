using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAxis : MonoBehaviour, IChart
{
    [SerializeField]
    protected ViewAxis _viewAxisPrefab = null;
    protected ViewAxis _viewAxis = null;
    [SerializeField]
    protected PresenterAxis _axisPresenter = new PresenterAxis();
    [SerializeField]
    bool vertical = false;
    [SerializeField]
    bool categorical = false;
    public Presenter presenter { get; set; } = null;
    public void Reload() { SetupInitialAxisViews(); }

    public virtual void SetupInitialAxisViews()
    {
        DestroyAxisViews();
        // Generic X Axis
        _viewAxis = Instantiate(_viewAxisPrefab, transform, false);
        _viewAxis.AxisPresenter = _axisPresenter;
        if (vertical)
        {
            _viewAxis.transform.localRotation = Quaternion.Euler(0, 0, 90);
            _viewAxis.Swapped = true;
        }
        if (categorical) { 
        _viewAxis.RebuildAxis(_axisPresenter.GenerateFromDiscreteRange(0, presenter.dataDimensions[0].Count));
        }
        else { 
        _viewAxis.RebuildAxis(_axisPresenter.GenerateFromMinMaxValue(0, ((FloatDataDimension)presenter.dataDimensions[1]).Max()));
        }
    }
    protected virtual void DestroyAxisViews()
    {
        if (_viewAxis == null)
            return;
        if (_viewAxis != null)
            DestroyImmediate(_viewAxis.gameObject);
        _viewAxis = null;
    }

    public void Initialize()
    {
        SetupInitialAxisViews();
    }
}
