using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            FindObjectOfType<PlayerConditions>().Dead();
            //Invoke("Restarter", 1.5f);
        }

    }

    void Restarter()
    {
        Restart.ReloadSceneWithProgressCheck();
    }
}
