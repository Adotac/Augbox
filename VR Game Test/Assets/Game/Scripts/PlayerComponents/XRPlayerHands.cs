// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.Netcode;
// using Microsoft.MixedReality.Toolkit;
// using Microsoft.MixedReality.Toolkit.Input;
// using Microsoft.MixedReality.Toolkit.Utilities;

// namespace Augbox{
// public class XRPlayerHands : NetworkBehaviour, IMixedRealitySourcePoseHandler
// {

//     private MixedRealityControllerVisualizer mrtkcv;
//     public event IMixedRealityEventHandler<SourcePoseEventData<MixedRealityPose> > s;
//     private void Start() {
//         mrtkcv = GetComponent<MixedRealityControllerVisualizer>();
//         // mrtkcv.OnSourcePoseChanged();
//     }   

//     public override void OnNetworkSpawn()
//     {
//         base.OnNetworkSpawn();
//         OnSourcePoseChanged();

//     }

//     // Implement the other interface functions with empty/default implementations
//     public void OnSourceDetected(SourceStateEventData eventData) { }
//     public void OnSourceLost(SourceStateEventData eventData) { }
//     public void OnSourcePoseChanged(SourcePoseEventData<TrackingState> eventData){}
//     public void OnSourcePoseChanged(SourcePoseEventData<Vector2> eventData){}
//     public void OnSourcePoseChanged(SourcePoseEventData<Vector3> eventData){}
//     public void OnSourcePoseChanged(SourcePoseEventData<Quaternion> eventData){}
//     public void OnSourcePoseChanged(SourcePoseEventData<MixedRealityPose> eventData){
//         if(!IsOwner) return;
//         mrtkcv.OnSourcePoseChanged(eventData);
//     }
// }
// }