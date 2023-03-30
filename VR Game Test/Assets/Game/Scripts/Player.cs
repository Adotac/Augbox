using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float Health {get; set;} = 100;
    [SerializeField]
    private float Damage {get;set;} = 20;

    public ProgressBar refBar;
    public Collider objCollider;

    // Start is called before the first frame update
    void Start()
    {
        refBar.BarValue = Health;
    }

    // Update is called once per frame
    void Update()
    {
        // if(TryGetComponent<MeshCollider>(out MeshCollider mc)){
        //     mc.
        // }
        
    }

    private void OnCollisionEnter(Collision other) {
        try{
            Player enemyHealth = other.gameObject.GetComponent<Player>();
            enemyHealth.refBar.UpdateValue(-Damage);
            Debug.Log($"Enemy damaged by -{Damage}");
        }
        catch(UnityException e){
            Debug.Log($"Collision Error\n{e}");
        }

        
    }

}
