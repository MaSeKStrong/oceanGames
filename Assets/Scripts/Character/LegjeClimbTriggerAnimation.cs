using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LegjeClimbTriggerAnimation : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Animator animator = collision.gameObject.GetComponent<Animator>();
            if (animator != null)
            collision.GetComponent<Animator>().SetTrigger("LedgeClimbing");
        }
    }
}
