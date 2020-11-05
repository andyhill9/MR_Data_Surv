using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Microsoft.MixedReality.Toolkit.Input;
/// Include the name space for TextMesh Pro
using TMPro;
using UnityEngine.UI;

public class BallSelection : MonoBehaviour, IMixedRealityPointerHandler
{
    private bool clicked;
    public float DoubleClickTime = 1;
    private float timeSinceClick;

    private PointSelector pointSelector;

    public void OnPointerClicked(MixedRealityPointerEventData pointer)
    {
        if (clicked)
        {
            OnDoubleClick(pointer);
            clicked = false;
        }
        else
        {
            clicked = true;
            timeSinceClick = 0;
        }
    }

    public void OnPointerDown(MixedRealityPointerEventData pointer)
    {
        //Debug.Log("down");
    }

    public void OnPointerUp(MixedRealityPointerEventData pointer)
    {
        //Debug.Log("up");
    }

    public void OnPointerDragged(MixedRealityPointerEventData pointer)
    {
        //Debug.Log("dragged");
    }

    //public void Selection()
    //{
    //    Debug.Log("Selection");
    //    pointSelector.LightUpBalls(gameObject.name);
    //}

    public void OnDoubleClick(MixedRealityPointerEventData pointer)
    {
        Debug.Log("double clicked");
        //this allows to select ball from a far pointer double click
        //pointSelector.LightUpBalls(gameObject.name);
    }

    void Start()
    {
        pointSelector = GameObject.Find("Point Selector").GetComponent<PointSelector>();
    }

    void Update()
    {
        if (clicked)
        {
            timeSinceClick += Time.deltaTime;
            if (timeSinceClick >= DoubleClickTime)
            {
                clicked = false;
            }
        }
    }

}
