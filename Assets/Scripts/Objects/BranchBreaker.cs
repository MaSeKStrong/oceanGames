using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBreaker : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 1)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}