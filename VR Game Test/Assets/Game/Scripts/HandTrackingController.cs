using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public enum Finger{
    THUMB = 0,
    INDEX = 1,
    MIDDLE = 2,
    RING = 3,
    PINKY = 4,
}

public class HandTrackingController : MonoBehaviour
{

    [Header("Left Hand Joints")]
    public GameObject L_pinkyObj;
    public GameObject L_ringObj;
    public GameObject L_middleObj;
    public GameObject L_indexObj;
    public GameObject L_thumbObj;
    [Header("Left Hand Palm")]
    public GameObject L_Hand;

    [Header("Right Hand Joints")]
    public GameObject R_pinkyObj;
    public GameObject R_ringObj;
    public GameObject R_middleObj;
    public GameObject R_indexObj;
    public GameObject R_thumbObj;
    [Header("Right Hand Palm")]
    public GameObject R_Hand;

    [Header("Offsets")]
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    private MixedRealityPose pose;

    //------------Left hand children---------------//
    private IDictionary<Finger, Transform[]> LHand_Children;
    //------------Right hand children---------------//
    private IDictionary<Finger, Transform[]> RHand_Children;

    private void Awake() {
        LHand_Children = new Dictionary<Finger, Transform[]>();
        RHand_Children = new Dictionary<Finger, Transform[]>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /* == Fingers ==
            Pinky = 0
            Ring = 1
            Middle = 2
            Index = 3
            Thumb = 4
        */
        LHand_Children.Add(Finger.THUMB, L_thumbObj.GetComponentsInChildren<Transform>());
        LHand_Children.Add(Finger.INDEX, L_indexObj.GetComponentsInChildren<Transform>());
        LHand_Children.Add(Finger.MIDDLE, L_middleObj.GetComponentsInChildren<Transform>());
        LHand_Children.Add(Finger.RING, L_ringObj.GetComponentsInChildren<Transform>()); 
        LHand_Children.Add(Finger.PINKY, L_pinkyObj.GetComponentsInChildren<Transform>());

        RHand_Children.Add(Finger.THUMB, R_thumbObj.GetComponentsInChildren<Transform>());
        RHand_Children.Add(Finger.INDEX, R_indexObj.GetComponentsInChildren<Transform>());
        RHand_Children.Add(Finger.MIDDLE, R_middleObj.GetComponentsInChildren<Transform>());
        RHand_Children.Add(Finger.RING, R_ringObj.GetComponentsInChildren<Transform>()); 
        RHand_Children.Add(Finger.PINKY, R_pinkyObj.GetComponentsInChildren<Transform>());

        // Debug.Log(L_Children_indexObj.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if(HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Left, out pose)){
            Debug.Log("Left Hand");
            HoloHandsRig(LHand_Children, Handedness.Left);
        }

        if(HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out pose)){
            Debug.Log("Right Hand");
            HoloHandsRig(RHand_Children, Handedness.Right);
        }
    }

    private void HoloHandsRig(IDictionary<Finger, Transform[]> objarr, Handedness h){
        if(HandJointUtils.TryGetJointPose(TrackedHandJoint.Palm, h, out pose)){
            if(h == Handedness.Left) L_Hand.transform.SetLocalPositionAndRotation(pose.Position, pose.Rotation);
            else R_Hand.transform.SetPositionAndRotation(pose.Position, pose.Rotation);
        }

        Finger fin = Finger.THUMB;
        TrackedHandJoint handJoint = TrackedHandJoint.None;
        while(fin <= Finger.PINKY){
            for(int i = 0; i < objarr[fin].Length;){ // Assuming to assign on 4 distinct bone joints
                // Debug.Log("Joint: " + handJoint );
                switch(handJoint){
                    case TrackedHandJoint.None: case TrackedHandJoint.Wrist: case TrackedHandJoint.Palm: // None, Palm and Wrist
                    case TrackedHandJoint.IndexMetacarpal: case TrackedHandJoint.MiddleMetacarpal: 
                    case TrackedHandJoint.RingMetacarpal: case TrackedHandJoint.PinkyMetacarpal: // Metacarpal values except for the thumb
                        handJoint++;
                    continue; // skip bone joint

                    default:
                    if(HandJointUtils.TryGetJointPose(handJoint, h, out pose)){
                        objarr[fin].ElementAt<Transform>(i).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
                        i++;
                        handJoint++;
                    }
                    break;
                }
            }

            fin++;
        }

    }

}
