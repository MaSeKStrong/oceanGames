using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartBehevior : MonoBehaviour
{
    PlayerController playerController;
    private const int PlayerLayerIndex = 3;
    private const int IgnorePlayerIndex = 9;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void EnableLayerCollision()
    {
        Physics2D.IgnoreLayerCollision(PlayerLayerIndex, IgnorePlayerIndex, false);
    }

    private void DisableLayerCollision()
    {
        Physics2D.IgnoreLayerCollision(PlayerLayerIndex, IgnorePlayerIndex, true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            if (playerController != null && playerController.isPushing)
            {
                EnableLayerCollision();
                playerController.MoveSpeed = 5f;
                if (playerController.animator != null)
                {
                    playerController.animator.SetBool("IsPushing", true);
                }
                if (playerController.rb != null)
                {
                    playerController.rb.velocity = new Vector2(playerController.rb.velocity.x, -1);
                }
            }
            else
            {
                DisableLayerCollision();
                if (playerController != null && playerController.animator != null)
                {
                    playerController.animator.SetBool("IsPushing", false);
                }
                if (playerController != null)
                {
                    playerController.MoveSpeed = 10f;
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            DisableLayerCollision();
            if (playerController != null && playerController.animator != null)
            {
                playerController.animator.SetBool("IsPushing", false);
            }
            if (playerController != null)
            {
                playerController.MoveSpeed = 10f;
            }
        }
    }
}
