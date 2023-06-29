using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

namespace Augbox{
// [RequireComponent(typeof(Rigidbody))]
public class ControlledPlayer : NetworkBehaviour
{
    [SerializeField] protected float startingHealth {get;set;} = 100f;
    [SerializeField] public float damage {get;set;} = 20f;
    private float health;

    private Rigidbody rb;

    public ProgressBar refBar;
    public event Action OnDie;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [SerializeField]private GameObject _mainCam;
    private CinemachineVirtualCamera _cineVCam;
    public Vector3 offset;


    private void Awake() {
        health = startingHealth;

        // if(_mainCam == null){
        //     _mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        // }
        // if(_cineVCam == null){
        //     _cineVCam = FindObjectOfType<CinemachineVirtualCamera>();
        // }
    }
    
    protected virtual void Update(){
        if(!IsOwner)return;

        // refBar.UpdateValue(health);
        Move();

        _mainCam.transform.position = this.transform.position + offset; 
        _mainCam.transform.LookAt(this.transform.position + this.transform.forward * 30);
        _mainCam.transform.parent = this.transform;
    }

    private void Start() {
        if(!IsOwner){
            _mainCam.GetComponent<Camera>().enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody>();
        refBar.BarValue = startingHealth;

    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // if(IsClient && IsOwner){
        //     CinemachineVirtualCamera vcam = FindAnyObjectByType<CinemachineVirtualCamera>();
        //     vcam.m_Follow = this.gameObject.transform;
        //     vcam.transform.position = this.gameObject.transform.position;
        //     vcam.transform.rotation = this.gameObject.transform.rotation;
        // }
    }
    

    public void Move(){
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void TakeDamage(float val){
        health -= val;
        if(health <= 0)
            Die();
    }

    public float getHealth(){
        return health;
    }

    private void Die(){
        if(OnDie != null)
            OnDie();

        //Destroy object here or similar
    }
}
}