using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float Health {get; set;} = 100;
    [SerializeField]
    public float Damage {get;set;} = 20;

    public ProgressBar refBar;

    // Start is called before the first frame update
    void Start()
    {
        refBar.BarValue = Health;
    }

    // Update is called once per frame
    void Update()
    {
        refBar.UpdateValue(Health);
    }
        
    

}
