using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField]
    private Player controlledPlayer;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.name);
        if (other.gameObject.CompareTag("Player"))
        {
            
            try
            {
                Player enemyHealth = other.gameObject.GetComponent<Player>();

                if (enemyHealth.Health < 0)
                {
                    enemyHealth.Health = enemyHealth.Health - controlledPlayer.Damage;
                    //enemyHealth.refBar.UpdateValue();
                    Debug.Log($"Enemy damaged by -{controlledPlayer.Damage}\nHealth Remaining: {enemyHealth.Health}");
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
