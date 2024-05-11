using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChanger : MonoBehaviour
{
    [SerializeField] GameObject gameObjectForChange;
    [SerializeField] int indexOfNewLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            gameObjectForChange.layer = indexOfNewLayer;
        }
    }
}
