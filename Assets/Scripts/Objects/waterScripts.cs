using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterScripts : MonoBehaviour
{
    private PlayerConditions playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerConditions>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            playerController.WaterDeath();
        }
    }
}
