using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Handler : MonoBehaviour, IMixedRealityTouchHandler
{
    private PointSelector pointSelector;
    public float Cooldown = 0.5f;
    private float coolDownRemaining = 0;

    public void OnTouchCompleted(HandTrackingInputEventData eventData)
    {
        if (coolDownRemaining <= 0)
        {
            Debug.Log("TOUCHED");
            Debug.Log(gameObject.name);
            pointSelector.ShowToolTip(gameObject);
            pointSelector.LightUpBalls(gameObject.name);
            coolDownRemaining = Cooldown;
        }
    }

    //void ShowOnPanel()
    //{
    //    Debug.Log("clicked");
    //    // change the text of Slate
    //    Debug.Log("ddd");
    //    GameObject.Find("Slate/TitleBar/Title").GetComponent<TMP_Text>().text = "" + gameObject.name;
    //    //works: Debug.Log(GameObject.Find("Slate/TitleBar/Title"));

    //    // change the r graphs for a last subject
    //    Debug.Log("last_graph");
    //    Debug.Log(GameObject.Find("RawImage_2").GetComponent<RawImage>().texture);
    //    UnityEngine.UI.RawImage UIicon = GameObject.Find("RawImage_2").GetComponent<RawImage>();

    //    Debug.Log("text" + "Assets/Resources/Patients/" + gameObject.name + "_distribution.png");
    //    Texture2D othericon = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Patients/" + gameObject.name + "_distribution.png", typeof(Texture2D));
    //    Debug.Log(othericon);
    //    Debug.Log("before texture change");
    //    UIicon.texture = othericon;

    //    UnityEngine.UI.RawImage UIicon2 = GameObject.Find("RawImage_1").GetComponent<RawImage>();
    //    Debug.Log("text" + "Assets/Resources/Patients/" + gameObject.name + "_distribution.png");
    //    Texture2D othericon3 = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Patients/" + gameObject.name + "_contr.png", typeof(Texture2D));
    //    Debug.Log(othericon3);
    //    Debug.Log("before texture change");
    //    UIicon2.texture = othericon3;
    //}

    public void OnTouchStarted(HandTrackingInputEventData eventData)
    {
        //Debug.Log("AAAAAAAAA");
    }

    public void OnTouchUpdated(HandTrackingInputEventData eventData)
    {
        //Debug.Log("AAAAAAAAA");
    }

    // Start is called before the first frame update
    void Start()
    {
        pointSelector = GameObject.Find("Point Selector").GetComponent<PointSelector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownRemaining > 0)
        {
            coolDownRemaining -= Time.deltaTime;
        }
    }
}
