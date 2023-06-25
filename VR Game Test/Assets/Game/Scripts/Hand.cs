using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace Augbox{
public class Hand : MonoBehaviour
{

    [SerializeField]
    private ControlledPlayer controlledPlayer;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            
            try
            {
                ControlledPlayer enemyHealth = other.gameObject.GetComponent<ControlledPlayer>();

                if (enemyHealth.getHealth() > 0)
                {
                    enemyHealth.TakeDamage(controlledPlayer.damage);
                    //enemyHealth.refBar.UpdateValue();
                    Debug.Log($"Enemy damaged by -{controlledPlayer.damage}\nHealth Remaining: {enemyHealth.getHealth()}");
                }

            }
            catch (UnityException e)
            {
                Debug.Log($"Collision Error\n{e}");
            }
        }

        
    }

    private void OnCollisionEnter(Collision other)
    {
        //controlledPlayer.OnCollisionEnter(collision);
        //Debug.Log("Hand on collision");

        
    }
}
}