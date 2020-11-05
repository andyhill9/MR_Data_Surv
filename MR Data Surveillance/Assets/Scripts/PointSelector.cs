using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public class PointSelector : MonoBehaviour
{

    private GameObject ball1;
    private GameObject ball2;
    private GameObject ball3;
    private Material ball1PreviousMaterial;
    private Material ball2PreviousMaterial;
    private Material ball3PreviousMaterial;

    private GameObject ball4;
    private GameObject ball5;
    private GameObject ball6;
    private Material ball4PreviousMaterial;
    private Material ball5PreviousMaterial;
    private Material ball6PreviousMaterial;

    List<string> selectedBalls = new List<string>();

    private GameObject lastSelectedBall;

    Dictionary<string, Material> materialsPlot1 = new Dictionary<string, Material>();
    Dictionary<string, Material> materialsPlot2 = new Dictionary<string, Material>();
    Dictionary<string, Material> materialsPlot3 = new Dictionary<string, Material>();

    Dictionary<string, Material> materialsPlot4 = new Dictionary<string, Material>();
    Dictionary<string, Material> materialsPlot5 = new Dictionary<string, Material>();
    Dictionary<string, Material> materialsPlot6 = new Dictionary<string, Material>();


    public void LightUpBalls(string dataBallName)
    {
        Debug.Log("LIGHTUPBALL");
        Debug.Log(dataBallName);

        if (selectedBalls.Contains(dataBallName))
        {

            //change materials in all graphs
            GameObject.Find("Scatterplot1/GraphFrame/PointHolder/" + dataBallName).GetComponent<MeshRenderer>().material =
                materialsPlot1[dataBallName];
            GameObject.Find("Scatterplot2/GraphFrame/PointHolder/" + dataBallName).GetComponent<MeshRenderer>().material =
                materialsPlot2[dataBallName];
            GameObject.Find("Scatterplot3/GraphFrame/PointHolder/" + dataBallName).GetComponent<MeshRenderer>().material =
                materialsPlot3[dataBallName];

            GameObject parentObject = GameObject.Find("MixedRealityPlayspace");

            parentObject.transform.Find("2DScatterplot1/GraphFrame/PointHolder/" + dataBallName).gameObject.GetComponent<MeshRenderer>().material =
                materialsPlot4[dataBallName];
            parentObject.transform.Find("2DScatterplot2/GraphFrame/PointHolder/" + dataBallName).gameObject.GetComponent<MeshRenderer>().material =
                materialsPlot5[dataBallName];
            parentObject.transform.Find("2DScatterplot3/GraphFrame/PointHolder/" + dataBallName).gameObject.GetComponent<MeshRenderer>().material =
                materialsPlot6[dataBallName];

            GameObject.Find("Scatterplot1/GraphFrame/PointHolder/" + dataBallName).layer = 0;
            GameObject.Find("Scatterplot2/GraphFrame/PointHolder/" + dataBallName).layer = 0;
            GameObject.Find("Scatterplot3/GraphFrame/PointHolder/" + dataBallName).layer = 0;

            parentObject.transform.Find("2DScatterplot1/GraphFrame/PointHolder/" + dataBallName).gameObject.layer = 0;
            parentObject.transform.Find("2DScatterplot2/GraphFrame/PointHolder/" + dataBallName).gameObject.layer = 0;
            parentObject.transform.Find("2DScatterplot3/GraphFrame/PointHolder/" + dataBallName).gameObject.layer = 0;

            selectedBalls.Remove(dataBallName);

            materialsPlot1.Remove(dataBallName);
            materialsPlot2.Remove(dataBallName);
            materialsPlot3.Remove(dataBallName);

            materialsPlot4.Remove(dataBallName);
            materialsPlot5.Remove(dataBallName);
            materialsPlot6.Remove(dataBallName);

        }
        else
        {
            Material GlowMaterial = Resources.Load<Material>("Materials/GlowBall");

            ball1 = GameObject.Find("Scatterplot1/GraphFrame/PointHolder/" + dataBallName);
            ball2 = GameObject.Find("Scatterplot2/GraphFrame/PointHolder/" + dataBallName);
            ball3 = GameObject.Find("Scatterplot3/GraphFrame/PointHolder/" + dataBallName);

            GameObject parentObject = GameObject.Find("MixedRealityPlayspace");

            ball4 = parentObject.transform.Find("2DScatterplot1/GraphFrame/PointHolder/" + dataBallName).gameObject;
            ball5 = parentObject.transform.Find("2DScatterplot2/GraphFrame/PointHolder/" + dataBallName).gameObject;
            ball6 = parentObject.transform.Find("2DScatterplot3/GraphFrame/PointHolder/" + dataBallName).gameObject;

            selectedBalls.Add(dataBallName);

            materialsPlot1.Add(dataBallName, ball1.GetComponent<MeshRenderer>().material);
            materialsPlot2.Add(dataBallName, ball2.GetComponent<MeshRenderer>().material);
            materialsPlot3.Add(dataBallName, ball3.GetComponent<MeshRenderer>().material);

            materialsPlot4.Add(dataBallName, ball4.GetComponent<MeshRenderer>().material);
            materialsPlot5.Add(dataBallName, ball5.GetComponent<MeshRenderer>().material);
            materialsPlot6.Add(dataBallName, ball6.GetComponent<MeshRenderer>().material);

            ball1.GetComponent<MeshRenderer>().material = GlowMaterial;
            ball2.GetComponent<MeshRenderer>().material = GlowMaterial;
            ball3.GetComponent<MeshRenderer>().material = GlowMaterial;
            ball4.GetComponent<MeshRenderer>().material = GlowMaterial;
            ball5.GetComponent<MeshRenderer>().material = GlowMaterial;
            ball6.GetComponent<MeshRenderer>().material = GlowMaterial;

            ball1.layer = 8;
            ball2.layer = 8;
            ball3.layer = 8;
            ball4.layer = 8;
            ball5.layer = 8;
            ball6.layer = 8;
        }
        
    }

    public void ShowToolTip(GameObject dataPoint)
    {
        if (selectedBalls.Contains(dataPoint.name))
        {
            if (lastSelectedBall != null && lastSelectedBall.name == dataPoint.name)
            {
                lastSelectedBall.GetComponent<ToolTipSpawner>().MyHideToolTip();
                lastSelectedBall = null;
            }
            else
            {
                return;
            }
        }
        else
        {
            if (lastSelectedBall == null)
            {
                dataPoint.GetComponent<ToolTipSpawner>().MyShowToolTip();
                lastSelectedBall = dataPoint;
                ShowOnPanel(dataPoint);
            }
            else
            {
                lastSelectedBall.GetComponent<ToolTipSpawner>().MyHideToolTip();
                dataPoint.GetComponent<ToolTipSpawner>().MyShowToolTip();
                lastSelectedBall = dataPoint;
                ShowOnPanel(dataPoint);
            }
        } 
    }

    public void Reset()
    {
        for (int i = selectedBalls.Count - 1; i >= 0; i--)
        {
            Debug.Log(selectedBalls[i]);
            LightUpBalls(selectedBalls[i]);
        }

        if (lastSelectedBall != null)
        {
            lastSelectedBall.GetComponent<ToolTipSpawner>().MyHideToolTip();
        }
    }

    public void ShowOnPanel(GameObject dataPoint)
    {
        Debug.Log("clicked");
        // change the text of Slate
        Debug.Log("ddd");
        GameObject.Find("Slate/TitleBar/Title").GetComponent<TMP_Text>().text = "" + dataPoint.name;
        //works: Debug.Log(GameObject.Find("Slate/TitleBar/Title"));

        // change the r graphs for a last subject
        Debug.Log("last_graph");
        Debug.Log(GameObject.Find("RawImage_2").GetComponent<RawImage>().texture);
        UnityEngine.UI.RawImage UIicon = GameObject.Find("RawImage_2").GetComponent<RawImage>();

        Debug.Log("text" + "Assets/Resources/Patients/" + dataPoint.name + "_distribution.png");
        Texture2D othericon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Patients/" + dataPoint.name + "_distribution.png", typeof(Texture2D));
        Debug.Log(othericon);
        Debug.Log("before texture change");
        UIicon.texture = othericon;

        UnityEngine.UI.RawImage UIicon2 = GameObject.Find("RawImage_1").GetComponent<RawImage>();
        Debug.Log("text" + "Assets/Resources/Patients/" + dataPoint.name + "_distribution.png");
        Texture2D othericon3 = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Patients/" + dataPoint.name + "_contr.png", typeof(Texture2D));
        Debug.Log(othericon3);
        Debug.Log("before texture change");
        UIicon2.texture = othericon3;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit) && hit.transform.tag == "DataPoint")
        //    {
        //        string dataBallName = hit.transform.name;
        //        Debug.Log(dataBallName);
        //        //if (hit.transform.name == "Cube")
        //        //Behaviour h = (Behaviour)hit.transform.gameObject.GetComponent("Halo");


        //        //if (previousClickedBall != null)
        //        //{
        //        //    previousClickedBall.GetComponent<MeshRenderer>().material = previousMaterial;
        //        //    previousClickedBall.layer = 0;
        //        //}

        //        //previousClickedBall = hit.transform.name;
        //        //previousMaterial = hit.transform.gameObject.GetComponent<MeshRenderer>().material;

        //        Material GlowMaterial = Resources.Load<Material>("Materials/GlowBall");
        //        GameObject ball1 = GameObject.Find("Scatterplot1/GraphFrame/PointHolder/" + dataBallName);
        //        GameObject ball2 = GameObject.Find("Scatterplot2/GraphFrame/PointHolder/" + dataBallName);
        //        GameObject ball3 = GameObject.Find("Scatterplot3/GraphFrame/PointHolder/" + dataBallName);
                
        //        ball1.GetComponent<MeshRenderer>().material = GlowMaterial;
        //        ball2.GetComponent<MeshRenderer>().material = GlowMaterial;
        //        ball3.GetComponent<MeshRenderer>().material = GlowMaterial;
        //        ball1.layer = 8;
        //        ball2.layer = 8;
        //        ball3.layer = 8;

                //MeshRenderer meshRenderer = hit.transform.gameObject.GetComponent<MeshRenderer>();
                //meshRenderer.material = GlowMaterial;
                //hit.transform.gameObject.layer = 8;

        //    }
        //}
    }
}
