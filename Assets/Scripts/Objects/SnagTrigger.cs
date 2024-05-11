using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagTrigger : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        { rb.isKinematic = false; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        { 
            rb.isKinematic = true; 
        }
    }
}
