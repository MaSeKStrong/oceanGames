using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBreaker : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
  //  [SerializeField] EdgeCollider2D edge;
    bool onBranches;

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (collision.gameObject.layer == 3 && onBranches || collision.gameObject.layer == 1 )
        {            
            rb.bodyType = RigidbodyType2D.Dynamic;
           // edge.GetComponent<EdgeCollider2D>().enabled = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.layer == 1 && !onBranches)
        {
            onBranches = true;            
            //rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}