using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class CameraController : NetworkBehaviour {
    
    private GameObject cam;
    public Vector3 offset;

    private void Awake() {
    }
    private void Start() {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        
    }

    private void Update() {
        // if(IsLocalPlayer) print("I AM LOCALL!!!");

        if(IsOwner){
            cam.transform.position = this.transform.position + offset; 
            cam.transform.LookAt(this.transform.position + this.transform.forward * 30);
            cam.transform.parent = this.transform;
        }
    }

    public override void OnNetworkSpawn()
    {


        // camHolder.SetActive(IsOwner);
        base.OnNetworkSpawn();

    }
}
