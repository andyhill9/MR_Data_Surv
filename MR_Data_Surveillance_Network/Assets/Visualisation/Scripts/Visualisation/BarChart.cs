using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO add require stuff for every thing to make it as save as possible
public class BarChart : MonoBehaviour, IGraph
{
    [SerializeField]
    protected Mesh _dataItemMesh = null;
    [SerializeField]
    protected float _barThickness = 0.9f;
    [SerializeField]
    protected StringDataDimension xValues = null;
    [SerializeField]
    protected FloatDataDimension yValues = null;
    [SerializeField]
    protected Color baseColor = Color.blue;
    [SerializeField]
    protected Color highlightColor = Color.red;
    [SerializeField]
    protected MeshFilter meshFilter = null;
    public Presenter presenter { get; set; } = null;
    /*private void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        RebuildVisualization();
    }*/

    public void Reload() { RebuildVisualization(); }
    public void RebuildVisualization()
    {
        //TODO calc max and min and normalise the data based on them
        if (_dataItemMesh == null)
        {
            Debug.LogError("No DataMesh was set for this visualization.");
            return;
        }
        float max = yValues.Max();
        var iMesh = new u2vis.Utilities.IntermediateMesh();
        // temporary save the mesh data from the template for faster access
        var tVertices = _dataItemMesh.vertices;
        var tNromals = _dataItemMesh.normals;
        var tUVs = _dataItemMesh.uv;
        var tIndices = _dataItemMesh.triangles;

        int offset = 0;
        int length = xValues.Count;
        float posXStep = 1.0f / length;
        float posXOffset = posXStep * 0.5f;
        float uStep = 1.0f / length;
        float uOffset = uStep * 0.5f;
        for (int i = 0; i < length; i++)
        {
            int itemIndex = i + offset;
            float value = yValues[i];
            var pos = new Vector3(posXOffset + i * posXStep, 0, 0);
            var scale = new Vector3(posXStep * _barThickness, value/max, 1);
            var startIndex = iMesh.Vertices.Count;
            var color = baseColor;
            foreach (var v in tVertices)
            {
                iMesh.Vertices.Add(new Vector3(pos.x + v.x * scale.x, pos.y + v.y * scale.y, pos.z + v.z * scale.z));
                iMesh.Colors.Add(color);
            }
            iMesh.Normals.AddRange(tNromals);
            iMesh.TexCoords.AddRange(tUVs);
            foreach (var j in tIndices)
                iMesh.Indices.Add(startIndex + j);
        }
        meshFilter.sharedMesh = iMesh.GenerateMesh("BarChart2DMesh", MeshTopology.Triangles);
    }

    public void Initialize()
    {
        meshFilter = GetComponent<MeshFilter>();
        RebuildVisualization();
    }

    public void Rebuild()
    {
        RebuildVisualization();
    }
}
