using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class ControlComponents : MonoBehaviour
{
    public GameObject scatterplot1;
    public GameObject scatterplot2;
    public GameObject scatterplot3;

    public GameObject dim2scatterplot1;
    public GameObject dim2scatterplot2;
    public GameObject dim2scatterplot3;

    //List of components to manipulate
    public bool BoxColliderComponent;
    public bool BoundingBoxComponent;
    public bool ManipulationHandlerComponent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BoxColliderComponent = scatterplot1.GetComponent<BoxCollider>().enabled;


        BoundingBoxComponent = scatterplot1.GetComponent<BoundingBox>().enabled;
        ManipulationHandlerComponent = scatterplot1.GetComponent<ManipulationHandler>().enabled;

        switch (BoxColliderComponent)
        {
            case true:
                scatterplot2.GetComponent<BoxCollider>().enabled = true;
                scatterplot3.GetComponent<BoxCollider>().enabled = true;

                dim2scatterplot1.GetComponent<BoxCollider>().enabled = true;
                dim2scatterplot2.GetComponent<BoxCollider>().enabled = true;
                dim2scatterplot3.GetComponent<BoxCollider>().enabled = true;
                break;

            case false:
                scatterplot2.GetComponent<BoxCollider>().enabled = false;
                scatterplot3.GetComponent<BoxCollider>().enabled = false;

                dim2scatterplot1.GetComponent<BoxCollider>().enabled = false;
                dim2scatterplot2.GetComponent<BoxCollider>().enabled = false;
                dim2scatterplot3.GetComponent<BoxCollider>().enabled = false;
                break;
        }

        switch (BoundingBoxComponent)
        {
            case true:
                scatterplot2.GetComponent<BoundingBox>().enabled = true;
                scatterplot3.GetComponent<BoundingBox>().enabled = true;

                dim2scatterplot1.GetComponent<BoundingBox>().enabled = true;
                dim2scatterplot2.GetComponent<BoundingBox>().enabled = true;
                dim2scatterplot3.GetComponent<BoundingBox>().enabled = true;
                break;

            case false:
                scatterplot2.GetComponent<BoundingBox>().enabled = false;
                scatterplot3.GetComponent<BoundingBox>().enabled = false;

                dim2scatterplot1.GetComponent<BoundingBox>().enabled = false;
                dim2scatterplot2.GetComponent<BoundingBox>().enabled = false;
                dim2scatterplot3.GetComponent<BoundingBox>().enabled = false;
                break;
        }
        switch (ManipulationHandlerComponent)
        {
            case true:
                scatterplot2.GetComponent<ManipulationHandler>().enabled = true;
                scatterplot3.GetComponent<ManipulationHandler>().enabled = true;

                dim2scatterplot1.GetComponent<ManipulationHandler>().enabled = true;
                dim2scatterplot2.GetComponent<ManipulationHandler>().enabled = true;
                dim2scatterplot3.GetComponent<ManipulationHandler>().enabled = true;
                break;

            case false:
                scatterplot2.GetComponent<ManipulationHandler>().enabled = false;
                scatterplot3.GetComponent<ManipulationHandler>().enabled = false;

                dim2scatterplot1.GetComponent<ManipulationHandler>().enabled = false;
                dim2scatterplot2.GetComponent<ManipulationHandler>().enabled = false;
                dim2scatterplot3.GetComponent<ManipulationHandler>().enabled = false;
                break;
        }

    }
}
