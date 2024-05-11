using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] GameObject dialog; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (dialog != null)
            dialog.SetActive(true);
            PlayerController playerController = FindObjectOfType<PlayerController>();
            playerController.gameObject.GetComponent<PlayerController>().enabled = false;
            playerController.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void ShowCharacter() 
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        playerController.gameObject.GetComponent<PlayerController>().enabled = true;
        playerController.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
}
