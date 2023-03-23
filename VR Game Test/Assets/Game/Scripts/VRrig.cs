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
    public VRmap HandL;
    public VRmap HandR;
    public Transform headConstraint;
    public Vector3 headBodyOffset;

    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized;
    
        Head.Map();
        HandR.Map();
        HandL.Map();
    }
}
