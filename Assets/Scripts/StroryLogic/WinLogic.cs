using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLogic : MonoBehaviour
{
    [SerializeField] GameObject winDialog;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cart"))
        {
            FindObjectOfType<PlayerController>().enabled = false;
            winDialog.gameObject.SetActive(true);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().enabled = false;
            FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        }
    }
}
