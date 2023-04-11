using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public enum Finger{
    Pinky = 0,
    Ring = 1,
    Middle = 2,
    Index = 3,
    Thumb = 4,
}

public class HandTrackingController : MonoBehaviour
{

    [Header("Left Hand Joints")]
    public GameObject L_pinkyObj;
    public GameObject L_ringObj;
    public GameObject L_middleObj;
    public GameObject L_indexObj;
    public GameObject L_thumbObj;

    [Header("Right Hand Joints")]
    public GameObject R_pinkyObj;
    public GameObject R_ringObj;
    public GameObject R_middleObj;
    public GameObject R_indexObj;
    public GameObject R_thumbObj;

    [Header("Offsets")]
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    private IMixedRealityHand detectLhand;
    private IMixedRealityHand detectRhand;

    private MixedRealityPose pose;

    //------------Left hand children---------------//
    private List<List<Transform>> LHand_Children;
    // private List<Transform> L_Children_pinkyObj;
    // private List<Transform> L_Children_ringObj;
    // private List<Transform> L_Children_middleObj;
    // private List<Transform> L_Children_indexObj;
    // private List<Transform> L_Children_thumbObj;
    //------------Right hand children---------------//
    private List<List<Transform>> RHand_Children;
    // private List<Transform> R_Children_pinkyObj;
    // private List<Transform> R_Children_ringObj;
    // private List<Transform> R_Children_middleObj;
    // private List<Transform> R_Children_indexObj;
    // private List<Transform> R_Children_thumbObj;


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

        LHand_Children.Add(L_pinkyObj.GetComponentsInChildren<Transform>().ToList());
        LHand_Children.Add(L_ringObj.GetComponentsInChildren<Transform>().ToList()); 
        LHand_Children.Add(L_middleObj.GetComponentsInChildren<Transform>().ToList());
        LHand_Children.Add(L_indexObj.GetComponentsInChildren<Transform>().ToList());
        LHand_Children.Add(L_thumbObj.GetComponentsInChildren<Transform>().ToList());

        RHand_Children.Add(R_pinkyObj.GetComponentsInChildren<Transform>().ToList());
        RHand_Children.Add(R_ringObj.GetComponentsInChildren<Transform>().ToList());
        RHand_Children.Add(R_middleObj.GetComponentsInChildren<Transform>().ToList());
        RHand_Children.Add(R_indexObj.GetComponentsInChildren<Transform>().ToList());
        RHand_Children.Add(R_thumbObj.GetComponentsInChildren<Transform>().ToList());
        // L_Children_pinkyObj = L_pinkyObj.GetComponentsInChildren<Transform>().ToList();
        // L_Children_ringObj = L_ringObj.GetComponentsInChildren<Transform>().ToList();
        // L_Children_middleObj = L_middleObj.GetComponentsInChildren<Transform>().ToList();
        // L_Children_indexObj = L_indexObj.GetComponentsInChildren<Transform>().ToList();
        // L_Children_thumbObj = L_thumbObj.GetComponentsInChildren<Transform>().ToList();

        // R_Children_pinkyObj = R_pinkyObj.GetComponentsInChildren<Transform>().ToList();
        // R_Children_ringObj = R_ringObj.GetComponentsInChildren<Transform>().ToList();
        // R_Children_middleObj = R_middleObj.GetComponentsInChildren<Transform>().ToList();
        // R_Children_indexObj = R_indexObj.GetComponentsInChildren<Transform>().ToList();
        // R_Children_thumbObj = R_thumbObj.GetComponentsInChildren<Transform>().ToList();

        
        // Debug.Log(L_Children_indexObj.Count);
    }

    // Update is called once per frame
    void Update()
    {

        // For hololens Hand Tracking

        detectLhand = HandJointUtils.FindHand(Handedness.Left);
        detectRhand = HandJointUtils.FindHand(Handedness.Right);

        Debug.Log("Left: " + detectLhand.TrackingState);
        Debug.Log("Right: " + detectRhand.TrackingState);

        if(detectLhand.TrackingState == TrackingState.Tracked){
 
            
        }

        // if(detectRhand.TrackingState == TrackingState.Tracked){
        //     if(HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyDistalJoint, Handedness.Right, out pose)){
        //         R_Children_pinkyObj.ElementAt<Transform>(R_Children_pinkyObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
        //     }
        //     if(HandJointUtils.TryGetJointPose(TrackedHandJoint.RingDistalJoint, Handedness.Right, out pose)){
        //         R_Children_ringObj.ElementAt<Transform>(R_Children_ringObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
        //     }
        //     if(HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleDistalJoint, Handedness.Right, out pose)){
        //         R_Children_middleObj.ElementAt<Transform>(R_Children_middleObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
        //     }
        //     if(HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexDistalJoint, Handedness.Right, out pose)){
        //         R_Children_indexObj.ElementAt<Transform>(R_Children_indexObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
        //     }
        //     if(HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbDistalJoint, Handedness.Right, out pose)){
        //         R_Children_thumbObj.ElementAt<Transform>(R_Children_thumbObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
        //     }
            
        // }
        ////------------------------------------------///
    }

    private void HoloHandsRig(List<List<Transform>> objarr, Handedness h){

        for(Finger fin = 0; fin <= Finger.Thumb; fin++){
            for(int i = 0, j = (int)TrackedHandJoint.PinkyKnuckle; 
                i < objarr.ElementAt<List<Transform>>((int)fin).Count; 
                i++, j++){

                if(HandJointUtils.TryGetJointPose((TrackedHandJoint) j, h, out pose)){
                    objarr.ElementAt<List<Transform>>((int)fin).ElementAt<Transform>(i).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
                }
            }
        }
        //Pinky
        for(int i = 0, j = (int)TrackedHandJoint.PinkyKnuckle; 
            i < objarr.ElementAt<List<Transform>>((int)Finger.Pinky).Count; 
            i++, j++){

            if(HandJointUtils.TryGetJointPose((TrackedHandJoint) j, h, out pose)){
                objarr.ElementAt<List<Transform>>((int)Finger.Pinky).ElementAt<Transform>(i).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            }
        }

        //Ring
        for(int i = 0, f = (int)TrackedHandJoint.RingKnuckle; 
            i < objarr.ElementAt<List<Transform>>((int)Finger.Pinky).Count; 
            i++, f++){

            if(HandJointUtils.TryGetJointPose((TrackedHandJoint)f, h, out pose)){
                objarr.ElementAt<List<Transform>>((int)Finger.Pinky).ElementAt<Transform>(i).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            }
        }

            // //Pinky
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyKnuckle, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Pinky).ElementAt<Transform>(objarr.Count-4).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyMiddleJoint, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Pinky).ElementAt<Transform>(objarr.Count-3).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyDistalJoint, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Pinky).ElementAt<Transform>(objarr.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.PinkyTip, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Pinky).ElementAt<Transform>(objarr.Count-1).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }


            // //Ring
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.RingKnuckle, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Ring).ElementAt<Transform>(objarr.Count-4).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.RingMiddleJoint, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Ring).ElementAt<Transform>(objarr.Count-3).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.RingDistalJoint, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Ring).ElementAt<Transform>(objarr.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.RingTip, Handedness.Left, out pose)){
            //     objarr.ElementAt<List<Transform>>((int)Finger.Ring).ElementAt<Transform>(objarr.Count-1).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }

            // //Middle
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleDistalJoint, Handedness.Left, out pose)){
            //     L_Children_middleObj.ElementAt<Transform>(L_Children_middleObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexDistalJoint, Handedness.Left, out pose)){
            //     L_Children_indexObj.ElementAt<Transform>(L_Children_indexObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
            // if(HandJointUtils.TryGetJointPose(TrackedHandJoint.ThumbDistalJoint, Handedness.Left, out pose)){
            //     L_Children_thumbObj.ElementAt<Transform>(L_Children_thumbObj.Count-2).transform.SetPositionAndRotation(pose.Position, pose.Rotation);
            // }
    }

}
