using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


namespace Augbox{
[RequireComponent(typeof(Rigidbody))]
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

    private void Awake() {
        health = startingHealth;
    }
    
    protected virtual void Update(){
        if(!IsOwner)return;

        refBar.UpdateValue(health);
        Move();
    }

    private void Start() {
        rb = GetComponent<Rigidbody>();
        refBar.BarValue = startingHealth;
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