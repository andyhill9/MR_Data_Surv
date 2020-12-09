using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeData : MonoBehaviour
{
    [SerializeField]
    FloatDataDimension data;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("update data from " + data[0] + " to " + (data[0] + 5));
            data[0] += 5;
        }
    }
}
