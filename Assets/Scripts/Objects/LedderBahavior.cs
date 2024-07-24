using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LedderBahavior : MonoBehaviour
{
    private PlayerController playerController;

    private void Start()
    {
        GameObject targetGO = GameObject.FindGameObjectWithTag("Player");
        playerController = targetGO.GetComponent<PlayerController>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) //&& Input.GetKey(KeyCode.LeftShift)) 
        {
            playerController.CanWeGoVertical = true;            
            playerController.animator.SetBool("IsOnTree", true);
            if (playerController.rb.velocity.y > 0.2)
            {
                Debug.Log("вверх");
                playerController.animator.SetBool("IsClimbingDown", false);
                playerController.animator.SetBool("IsClimbingUp", true);
            }
            else if (playerController.rb.velocity.y < - 0.2)
            {
                Debug.Log("вниз");
                playerController.animator.SetBool("IsClimbingUp", false);
                playerController.animator.SetBool("IsClimbingDown", true);
            }
            else if (playerController.rb.velocity.y == 0)
            {
                playerController.animator.SetBool("IsClimbingUp", false);
                playerController.animator.SetBool("IsClimbingDown", false); ;
            }
        }
        else
        {
            playerController.CanWeGoVertical = false;            
            playerController.animator.SetBool("IsOnTree", false);
            playerController.animator.SetBool("IsClimbingUp", false);
            playerController.animator.SetBool("IsClimbingDown", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && playerController != null)
        {
            playerController.CanWeGoVertical = false;
            playerController.animator.SetBool("IsOnTree", false);
            playerController.animator.SetBool("IsClimbingDown", false);
            playerController.animator.SetBool("IsClimbingUp", false);
        }
    }
}
