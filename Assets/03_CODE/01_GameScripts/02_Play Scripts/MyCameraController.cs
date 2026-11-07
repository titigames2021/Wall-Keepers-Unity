using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    public Transform t_mainCamera;
    Vector3 v3_returnPos = new Vector3(0, 0, 0);
    Quaternion q_returnRot = new Quaternion(0,0,0,0);
    float f_timeToReturnToPos = 1f;
    // Update is called once per frame
    void LateUpdate()
    {
        // Check if camera has moved and if so move it back to pos 0,0,0
        if(t_mainCamera.localPosition != v3_returnPos)
        {
            t_mainCamera.rotation = q_returnRot;
            //t_mainCamera.localPosition = Vector3.MoveTowards(transform.position, v3_returnPos, f_timeToReturnToPos);
            t_mainCamera.localPosition = v3_returnPos;
        }
    }
}
