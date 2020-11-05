using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Threading.Tasks;
using UnityEngine;
using System;

class DataForPlotting
{
    public string xName;
    public string yName;
    public string zName;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;

    public DataForPlotting(string xname, string yname, string zname)
    {
        xName = xname;
        yName = yname;
        zName = zname;
        xMin = 0;
        xMax = 0;
        yMin = 0;
        yMax = 0;
        zMin = 0;
        zMax = 0;
    }
}


public class DataPlotter : MonoBehaviour
{
       //********Public Variables********

    // Bools for editor options

    public bool renderPrefabsWithColor = true;

    // The prefab for the data points to be instantiated
    public GameObject PointPrefab;

    // The prefab for the data points that will be instantiated
    public GameObject PointHolder1;
    public GameObject PointHolder2;
    public GameObject PointHolder3;

    // Name of the input file, no extension
    public string inputfile;

    // Indices for columns to be assigned
        // Plot 1
    public int plot1columnX = 2;
    public int plot1columnY = 3;
    public int plot1columnZ = 4;
        // Plot 2
    public int plot2columnX = 7;
    public int plot2columnY = 8;
    public int plot2columnZ = 9;
        // Plot 3
    public int plot3columnX = 12;
    public int plot3columnY = 13;
    public int plot3columnZ = 14;

    //columns with status
    public int plot1status = 6;
    public int plot2status = 11;
    public int plot3status = 16;
    

    // Indices for ToolTipText and sphereName columns to be assigned
    public int SUBJID_column = 1;

    public int plot1SUBJ_distance = 5;
    public int plot1SUBJ_status = 6;

    public int plot2SUBJ_distance = 10;
    public int plot2SUBJ_status = 11;

    public int plot3SUBJ_distance = 15;
    public int plot3SUBJ_status = 16;

    public float plotScale = 5;

    // Scale of the prefab particlePoints
    [Range(0.0f, 0.5f)]
    public float pointScale = 0.5f;

    // Changes size of particles generated
    [Range(0.0f, 2.0f)]
    public float particleScale = 5.0f;


    //********Private Variables********

    private bool renderPointPrefabs = true;
    // NEED to fix particle positioning --- but no need so far:using only pointsPrefabs, no particles
    private bool renderParticles = false;

    // Data for plotting 
    private DataForPlotting plot1data;
    private DataForPlotting plot2data;
    private DataForPlotting plot3data;

    // Columns for tooltipText
    private string USUBJID_name;
    private string plot1USUBJID_distance;
    private string plot1USUBJID_status;
    private string plot2USUBJID_distance;
    private string plot2USUBJID_status;
    private string plot3USUBJID_distance;
    private string plot3USUBJID_status;


    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;

    // Number of rows
    private int rowCount;

    // Particle system for holding point particles
    private ParticleSystem.Particle[] particlePoints;

    //Lists to store points by status
    public List<GameObject> withinRange = new List<GameObject>();
    public List<GameObject> moderateInlier = new List<GameObject>();
    public List<GameObject> severeInlier = new List<GameObject>();
    public List<GameObject> moderateOutlier = new List<GameObject>();
    public List<GameObject> severeOutlier = new List<GameObject>();

    void Awake()
    {
        //Run CSV Reader. Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);

        //Log to console
        Debug.Log(pointList);
    }

    void SetDataForPlotting(out DataForPlotting data, string xname, string yname, string zname)
    {
        data = new DataForPlotting(xname, yname, zname);

        // Get maxes of each axis
        data.xMax = FindMaxValue(data.xName);
        data.yMax = FindMaxValue(data.yName);
        data.zMax = FindMaxValue(data.zName);

        // Get minimums of each axis
        data.xMin = FindMinValue(data.xName);
        data.yMin = FindMinValue(data.yName);
        data.zMin = FindMinValue(data.zName);
    }

    // Start is called before the first frame update
    void Start()
    {

        // Declare list of strings, fill with keys (column names)
        List<string> columnList = new List<string>(pointList[1].Keys);

        // Print number of keys (using .count)
        Debug.Log("There are " + columnList.Count + " columns in CSV");

        foreach (string key in columnList)
            Debug.Log("Column name is " + key);

        Debug.Log("Column name is " + plot1columnX);
        // Set data for plotting
        SetDataForPlotting(out plot1data, columnList[plot1columnX], columnList[plot1columnY], columnList[plot1columnZ]);
        SetDataForPlotting(out plot2data, columnList[plot2columnX], columnList[plot2columnY], columnList[plot2columnZ]);
        SetDataForPlotting(out plot3data, columnList[plot3columnX], columnList[plot3columnY], columnList[plot3columnZ]);

        // Get columns where variables for tooltip text and dataPointName
        USUBJID_name = columnList[SUBJID_column];

        plot1USUBJID_distance = columnList[plot1SUBJ_distance];
        plot1USUBJID_status = columnList[plot1SUBJ_status];

        plot2USUBJID_distance = columnList[plot2SUBJ_distance];
        plot2USUBJID_status = columnList[plot2SUBJ_status];

        plot3USUBJID_distance = columnList[plot3SUBJ_distance];
        plot3USUBJID_status = columnList[plot3SUBJ_status];

        string status1 = columnList[plot1status];
        string status2 = columnList[plot2status];
        string status3 = columnList[plot3status];

        if (renderPointPrefabs == true)
        {
            // Call PlacePoint methods defined below
            PlacePrefabPoints(PointHolder1, plot1data, "PCA", status1);
            PlacePrefabPoints(PointHolder2, plot2data, "MDS", status2);
            PlacePrefabPoints(PointHolder3, plot3data, "LLE", status3);
            AssignLabels(1, plot1data, "PCA");
            AssignLabels(2, plot2data, "MDS");
            AssignLabels(3, plot3data, "LLE");

        }

        // If statement to turn particles on and off
        //if (renderParticles == true)
        //{
        //    // Call CreateParticles() for particle system
        //    CreateParticles();

        //    // Set particle system, for point glow- depends on CreateParticles()
        //    GetComponent<ParticleSystem>().SetParticles(particlePoints, particlePoints.Length);
        //}


    }

    private float FindMaxValue(string columnName)
    {
        //set initial value to first value
        float maxValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }

        //Spit out the max value
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {
        //set initial value to first value
        float minValue = Convert.ToSingle(pointList[0][columnName]);

        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Places the prefabs according to values read in
    private void PlacePrefabPoints(GameObject PointHolder, DataForPlotting data, string method_name, string status)
    {

        // Get count (number of rows in table)
        rowCount = pointList.Count;


        for (var i = 0; i < pointList.Count; i++)
        {

            // Set x/y/z, standardized to between 0-1
            float x = (Convert.ToSingle(pointList[i][data.xName]) - data.xMin) / (data.xMax - data.xMin);
            float y = (Convert.ToSingle(pointList[i][data.yName]) - data.yMin) / (data.yMax - data.yMin);
            float z = (Convert.ToSingle(pointList[i][data.zName]) - data.zMin) / (data.zMax - data.zMin);

            // Create vector 3 for positioning particlePoints
            Vector3 position = new Vector3(x, y, z) * plotScale;

            //instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(PointPrefab, Vector3.zero, Quaternion.identity);

            //dataPoint.GetComponent<Microsoft.MixedReality.Toolkit.UI.Interactable>().Profiles[0].Target = dataPoint;

            // Make dataPoint child of PointHolder object , to keep Points within container in hiearchy
            dataPoint.transform.parent = PointHolder.transform;

            // Position point at relative to parent
            dataPoint.transform.localPosition = position;

            dataPoint.transform.localScale = new Vector3(pointScale, pointScale, pointScale);

            // Assigns original values to dataPointName
            //string dataPointName = "PCA_" + pointList[i][USUBJID_name];
            string dataPointName = "" + pointList[i][USUBJID_name];

            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;

            // Assigns original text to Tooltip text
            string dist_col;
            string stat_col;
            if (method_name == "PCA")
            {
                dist_col = plot1USUBJID_distance;
                stat_col = plot1USUBJID_status;
            } else if (method_name == "MDS")
            {
                dist_col = plot2USUBJID_distance;
                stat_col = plot2USUBJID_status;
            } else
            {
                dist_col = plot3USUBJID_distance;
                stat_col = plot3USUBJID_status;
            }


            //string dataPointTooltipText = method_name + " " +
            //pointList[i][USUBJID_name] + System.Environment.NewLine + "Distance: " + pointList[i][dist_col] + System.Environment.NewLine + "Status: " + pointList[i][stat_col];

            string dataPointTooltipText = method_name + System.Environment.NewLine + pointList[i][USUBJID_name] + System.Environment.NewLine +
                "PCA: " + pointList[i][plot1USUBJID_status] + System.Environment.NewLine +
                "MDS: " + pointList[i][plot2USUBJID_status] + System.Environment.NewLine +
                "LLE: " + pointList[i][plot3USUBJID_status];

            dataPoint.GetComponent<Microsoft.MixedReality.Toolkit.UI.ToolTipSpawner>().toolTipText = dataPointTooltipText;


            if (renderPrefabsWithColor == true)
            {

                // Sets color according to x/y/z value
                //dataPoint.GetComponent<Renderer>().material.color = new Color(x, y, z, 1.0f);

                //// Activate emission color keyword so we can modify emission color
                dataPoint.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

                dataPoint.GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0.0f, 0.0f, 0.0f, 1.0f));


                string s = pointList[i][status] as string;
                if (s == "Severe Outlier")
                {
                    dataPoint.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                } else if (s == "Moderate Outlier")
                {
                    dataPoint.GetComponent<Renderer>().material.color = new Color(0.5f, 0.0f, 0.5f, 1.0f);
                } else if (s == "Moderate Inlier")
                {
                    dataPoint.GetComponent<Renderer>().material.color = new Color(0.0f, 1.0f, 0.3f, 1.0f);
                } else if (s == "Severe Inlier")
                {
                    dataPoint.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
                } else
                {
                    dataPoint.GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                }


            }

            //Form arrays by status in PCA (used to filter points by status)
            if (method_name == "PCA")
            {
                string st = pointList[i][status] as string;
                if (st == "Severe Outlier")
                {
                    severeOutlier.Add(dataPoint);
                }
                else if (st == "Moderate Outlier")
                {
                    moderateOutlier.Add(dataPoint);
                }
                else if (st == "Moderate Inlier")
                {
                    moderateInlier.Add(dataPoint);
                }
                else if (st == "Severe Inlier")
                {
                    severeInlier.Add(dataPoint);
                }
                else
                {
                    withinRange.Add(dataPoint);
                }
            }

            //AssignLabels();

        }

    }

    public void ShowAll()
    {
        foreach (GameObject point in severeInlier)
        {
            point.SetActive(true);
        }
        foreach (GameObject point in moderateInlier)
        {
            point.SetActive(true);
        }
        foreach (GameObject point in withinRange)
        {
            point.SetActive(true);
        }
    }

    public void ShowOnlyOutliers()
    {
        foreach (GameObject point in severeInlier)
        {
            point.SetActive(false);
        }
        foreach (GameObject point in moderateInlier)
        {
            point.SetActive(false);
        }
        foreach (GameObject point in withinRange)
        {
            point.SetActive(false);
        }
    }


    // creates particlePoints in the Particle System game object
    // 
    // 
    //private void CreateParticles()
    //{


    //    rowCount = pointList.Count;

    //    particlePoints = new ParticleSystem.Particle[rowCount];

    //    for (int i = 0; i < pointList.Count; i++)
    //    {
    //        // Convert object from list into float
    //        float x = (Convert.ToSingle(pointList[i][xName]) - xMin) / (xMax - xMin);
    //        float y = (Convert.ToSingle(pointList[i][yName]) - yMin) / (yMax - yMin);
    //        float z = (Convert.ToSingle(pointList[i][zName]) - zMin) / (zMax - zMin);


    //        // Set point location
    //        particlePoints[i].position = new Vector3(x, y, z) * plotScale;

    //        //GlowColor = 
    //        // Set point color
    //        particlePoints[i].startColor = new Color(x, y, z, 1.0f);
    //        particlePoints[i].startSize = particleScale;
    //    }

    //}

    //Finds labels named in scene, assigns values to their text meshes
    // WARNING: game objects need to be named within scene
    private void AssignLabels(int number, DataForPlotting data, string method_name)
    {
        string postfix = number.ToString("0");

        // Update point counter
        GameObject.Find("Count_Description" + postfix).GetComponent<TextMesh>().text = "Number of subjects: " + pointList.Count.ToString("0");

        // Update title according to dimensionality reduction method
        GameObject.Find("Dataset_Label" + postfix).GetComponent<TextMesh>().text = method_name;

        // Update axis titles to ColumnNames
        GameObject.Find("X_Title" + postfix).GetComponent<TextMesh>().text = data.xName;
        GameObject.Find("Y_Title" + postfix).GetComponent<TextMesh>().text = data.yName;
        GameObject.Find("Z_Title" + postfix).GetComponent<TextMesh>().text = data.zName;

        // Set x Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        GameObject.Find("X_Min_Lab" + postfix).GetComponent<TextMesh>().text = data.xMin.ToString("0.0");
        GameObject.Find("X_Mid_Lab" + postfix).GetComponent<TextMesh>().text = (data.xMin + (data.xMax - data.xMin) / 2f).ToString("0.0");
        GameObject.Find("X_Max_Lab" + postfix).GetComponent<TextMesh>().text = data.xMax.ToString("0.0");

        // Set y Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        GameObject.Find("Y_Min_Lab" + postfix).GetComponent<TextMesh>().text = data.yMin.ToString("0.0");
        GameObject.Find("Y_Mid_Lab" + postfix).GetComponent<TextMesh>().text = (data.yMin + (data.yMax - data.yMin) / 2f).ToString("0.0");
        GameObject.Find("Y_Max_Lab" + postfix).GetComponent<TextMesh>().text = data.yMax.ToString("0.0");

        // Set z Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        GameObject.Find("Z_Min_Lab" + postfix).GetComponent<TextMesh>().text = data.zMin.ToString("0.0");
        GameObject.Find("Z_Mid_Lab" + postfix).GetComponent<TextMesh>().text = (data.zMin + (data.zMax - data.zMin) / 2f).ToString("0.0");
        GameObject.Find("Z_Max_Lab" + postfix).GetComponent<TextMesh>().text = data.zMax.ToString("0.0");

    }
}
