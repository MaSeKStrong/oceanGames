using System.Collections;
using UnityEngine;

public class LegieClimb : MonoBehaviour
{
    [SerializeField] Transform transform1;
    [SerializeField] Transform transform2;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] bool isActiveFromTheLeft;
    private bool isMoving = false;
    PlayerController controller;

    private void Start()
    {
        controller = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !isMoving && isActiveFromTheLeft)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.x > 0)
            {
                isMoving = true;
                controller.enabled = false;
                Animator animator = collision.gameObject.GetComponent<Animator>();
                if (animator != null)
                    //collision.GetComponent<Animator>().SetTrigger("LedgeClimbing");
                    collision.GetComponent<Animator>().SetBool("canClimbe", true);

                StartCoroutine(MoveObject(collision.transform));
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !isMoving && !isActiveFromTheLeft)
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null && rb.velocity.x < 0)
            {
                isMoving = true;
                controller.enabled = false;
                Animator animator = collision.gameObject.GetComponent<Animator>();
                if (animator != null)
                    //collision.GetComponent<Animator>().SetTrigger("LedgeClimbing");
                    collision.GetComponent<Animator>().SetBool("canClimbe", true);

                StartCoroutine(MoveObject(collision.transform));
            }
        }
    }

    IEnumerator MoveObject(Transform objectToMove)
    {
        float journeyLength = Vector2.Distance(objectToMove.position, transform2.position);
        float startTime = Time.time;
        while (Vector2.Distance(objectToMove.position, transform2.position) > 0.1f)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            objectToMove.position = Vector2.Lerp(objectToMove.position, transform2.position, fractionOfJourney);
            yield return null;
        }
        isMoving = false;
        controller.enabled = true;
        //animator = GetComponent<Animator>().SetBool("canClimbe", false);
    }
}
