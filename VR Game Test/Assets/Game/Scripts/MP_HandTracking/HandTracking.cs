using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
using Stopwatch = System.Diagnostics.Stopwatch;

using Unity.Netcode;

namespace Augbox
{
    public class HandTracking : NetworkBehaviour
    {
        [SerializeField] private float MaxPunchDistance = 5;

        public GameObject LeftHand;
        public GameObject RightHand;

        protected virtual void Start() {
            if(!IsOwner)return;
            HandTrackingSolution handSolutionEvents = GetComponent<HandTrackingSolution>();
            handSolutionEvents.OnHandLandmarkPos += HandSolutionEvents_OnHandLandmarkPos;
        }

        private void HandSolutionEvents_OnHandLandmarkPos(object sender, HandTrackingSolution.OnHandLandmarkPosEventArgs e){
            if(!IsOwner)return;

            // this.hand_landmark_pos = e.hand_landmark_pos;
            // this.handedness = e.handedness;
            print(e.handedness[0].Classification[0].Index); // if 0 its left; if 1 its right
            if(e.hand_landmark_pos != null){
                MoveHand(0, e.handedness[0].Classification[0].Index, e);
                if(e.hand_landmark_pos.Count > 1 && e.hand_landmark_pos[1] != null){
                    MoveHand(1, e.handedness[1].Classification[0].Index, e);    
                }
            }
        }

        private void MoveHand(int idx, int handIdx, HandTrackingSolution.OnHandLandmarkPosEventArgs e){
            int trackPoint = 9;
            float xOffset = 0.5f;
            float yOffset = -0.5f;

            float x1 = e.hand_landmark_pos[idx].Landmark[5].X;
            float y1 =  e.hand_landmark_pos[idx].Landmark[5].Y; 
            float x2 = e.hand_landmark_pos[idx].Landmark[17].X;
            float y2 =  e.hand_landmark_pos[idx].Landmark[17].Y; 

            float nDistance = CalculateDistance(x1, y1, x2, y2);
            if(nDistance <= 2){
                x1 = e.hand_landmark_pos[idx].Landmark[17].X;
                y1 =  e.hand_landmark_pos[idx].Landmark[17].Y; 
                x2 = e.hand_landmark_pos[idx].Landmark[18].X;
                y2 =  e.hand_landmark_pos[idx].Landmark[18].Y; 

                nDistance = CalculateDistance(x1, y1, x2, y2);
                trackPoint = 17;
            }

            float ztemp = (MaxPunchDistance - zDepth(nDistance));

            if(ztemp < 0)
                ztemp = 0;

            switch(handIdx){
                case 0: 
                    LeftHand.transform.localPosition = new Vector3(e.hand_landmark_pos[idx].Landmark[trackPoint].X - xOffset, 
                                                                -e.hand_landmark_pos[idx].Landmark[trackPoint].Y - yOffset, 
                                                                ztemp);
                break;
                case 1:
                    RightHand.transform.localPosition = new Vector3(e.hand_landmark_pos[idx].Landmark[trackPoint].X - xOffset, 
                                                                -e.hand_landmark_pos[idx].Landmark[trackPoint].Y - yOffset, 
                                                                ztemp);
                break;
                
            }
        }

        static float CalculateDistance(float x1, float y1, float x2, float y2)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
            return distance;
        }
        private float zDepth(float z){
            float temp1 = 0.1f;
            float tempwidth = MaxPunchDistance;
            float focal = (tempwidth * temp1) / 3; // manual cam calibration
            return distanceToCamera(3, focal, z);
        }
        private float distanceToCamera(float knownWidth, float focalLength, float perWidth){
            return (knownWidth * focalLength) / perWidth;
        }

    }
}