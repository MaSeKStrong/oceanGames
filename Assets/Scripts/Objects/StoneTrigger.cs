using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTrigger : MonoBehaviour
{
    [SerializeField] float delay = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stone"))
        {
            collision.gameObject.layer = 0;
            StartCoroutine(MakeStaticAfterDelay(collision.gameObject, delay));
        }
    }

    private IEnumerator MakeStaticAfterDelay(GameObject stone, float delay)
    {
        yield return new WaitForSeconds(delay);

        Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            rb.freezeRotation = true;
        }
        else
        {
            Debug.LogWarning("Rigidbody2D component not found on stone object.");
        }
    }
}
