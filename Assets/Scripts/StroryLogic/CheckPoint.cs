using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
     public GameObject [] objectsForDestroyBeforeStart;

    private void Start()
    {
        if (gameObject.transform.position == CharacterPosition.CharacterStartPosition)
        {
            foreach (var obj in objectsForDestroyBeforeStart)
            {
                Destroy(obj);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            CharacterPosition.CharacterStartPosition = gameObject.transform.position;
        }
    }
}
