using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFixer : MonoBehaviour
{
    PlayerController controller;
    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //controller.isGrounded = false;
            controller.rb.velocity = new Vector2(controller.rb.velocity.x, -1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            controller.enabled = true;
        }
    }
}
