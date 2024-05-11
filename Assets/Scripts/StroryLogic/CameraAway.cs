using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAway : MonoBehaviour
{
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] Vector3 newMinCamerabounds;
    [SerializeField] Vector3 newMaxCamerabounds;
    [SerializeField] float transitionDuration = 1f; // ¬рем€, за которое происходит плавное перемещение
    [SerializeField] bool multipleTrigger;
    private Vector3 startMinCamerabounds;
    private Vector3 startMaxCamerabounds;
    private bool isTriggerWorked;
    private float transitionStartTime;

    private void Start()
    {
        startMinCamerabounds = cameraFollow.minCamerabounds;
        startMaxCamerabounds = cameraFollow.maxCamerabounds;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && collision.gameObject.activeSelf)
        {
            if (!isTriggerWorked)
            {
                StartCoroutine(TransitionCamera(newMinCamerabounds, newMaxCamerabounds));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && collision.gameObject.activeSelf)
        {
            StartCoroutine(TransitionCamera(startMinCamerabounds, startMaxCamerabounds));
            if (!multipleTrigger) { isTriggerWorked = true; }
        }
    }


    private IEnumerator TransitionCamera(Vector3 targetMinBounds, Vector3 targetMaxBounds)
    {
        float elapsedTime = 0f;
        Vector3 initialMinBounds = cameraFollow.minCamerabounds;
        Vector3 initialMaxBounds = cameraFollow.maxCamerabounds;

        while (elapsedTime < transitionDuration)
        {
            cameraFollow.minCamerabounds = Vector3.Lerp(initialMinBounds, targetMinBounds, elapsedTime / transitionDuration);
            cameraFollow.maxCamerabounds = Vector3.Lerp(initialMaxBounds, targetMaxBounds, elapsedTime / transitionDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cameraFollow.minCamerabounds = targetMinBounds;
        cameraFollow.maxCamerabounds = targetMaxBounds;
    }
}
