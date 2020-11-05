using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayElements : MonoBehaviour
{
    public GameObject[] plots;

    public void Show2DPlots()
    {
        foreach (GameObject plot in plots)
        {
            plot.SetActive(true);
        }
    }

    public void Hide2DPlots()
    {
        foreach (GameObject plot in plots)
        {
            plot.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        plots = GameObject.FindGameObjectsWithTag("2Dscatterplot");
        //Hide2DPlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
