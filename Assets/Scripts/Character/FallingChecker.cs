using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingChecker : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerConditions playerConditions;
    [SerializeField] float dangerHeight = -18.4f;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController.rb.velocity.y < dangerHeight)
        {
            //playerConditions.Dead();
            playerConditions.PreDeath();
        }
    }

    
}
