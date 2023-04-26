using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRmap{
    public Transform vrtarget;
    public Transform rigtarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map(){
        rigtarget.position = vrtarget.TransformPoint(trackingPositionOffset);
        rigtarget.rotation = vrtarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }

}

public class VRrig : MonoBehaviour
{
    public VRmap Head;
    public int turnSmooth = 1;

    public VRmap HandL;
    public VRmap HandR;
    public Transform headConstraint;
    public Vector3 headBodyOffset;

    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    void LateUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;

        transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;

        // transform.forward = Vector3.Lerp(
        //                         transform.forward,
        //                         Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, 
        //                         Time.deltaTime * turnSmooth
        //                     );

        
        Head.Map();

        if(HandR != null) HandR.Map();
        if(HandL != null) HandL.Map();
    }
}
