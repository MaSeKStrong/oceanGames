using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public GameObject outHouse;
    public GameObject inHouse;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.gameObject.layer == 3)
        {
            outHouse.gameObject.SetActive(false);
            inHouse.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        outHouse.gameObject.SetActive(true);
        inHouse.gameObject.SetActive(false);
    }
}
