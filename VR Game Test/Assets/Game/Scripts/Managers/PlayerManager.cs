using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

namespace Augbox
{

    public class PlayerManager : MonoBehaviour 
    {
        [SerializeField] private GameObject MRToolKit;
        [SerializeField] private GameObject MRPlaySpace;
        [SerializeField] private GameObject MRSceneContent;

        private Camera cam;
        private CinemachineBrain cineBrain;
        private GameObject virtualCamObject;

        private void Awake() {
            if(PlayerEnvironment.build_environment == BUILD_ENVIRONMENT.DESKTOP){
                MRToolKit.SetActive(false);
                MRPlaySpace.SetActive(false);
                MRSceneContent.SetActive(false);

                // cam = gameObject.AddComponent<Camera>();
                // cam.tag = "MainCamera";

                // virtualCamObject = new GameObject("FollowCamera");
                // CinemachineVirtualCamera cv = virtualCamObject.AddComponent<CinemachineVirtualCamera>();

                // cineBrain = gameObject.AddComponent<CinemachineBrain>();

                // gameObject.AddComponent<ClientNetwork
            }
        }

        private void Start() {

        }
    }
}
