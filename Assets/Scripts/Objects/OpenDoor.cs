using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Animator anim;
    public bool isOpen;
    bool isReadyToOpen;
    private PlayerController playerController;
    public BoxCollider2D closeCollider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        GameObject targetGO = GameObject.FindGameObjectWithTag("Player");
        playerController = targetGO.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        OpentoDoor();
    }

    void OpentoDoor()
    {
        anim.SetBool("isOpen", isOpen);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3) //&& Input.GetKey(KeyCode.LeftShift))
        {
            isReadyToOpen = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision != null && collision.gameObject.layer == 3) && Input.GetKey(KeyCode.LeftShift) && isReadyToOpen){
            if (!isOpen)
            {
                isOpen = true;
                closeCollider.GetComponent<Collider2D>().enabled = false;
            }
            else { isOpen = false; }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
            //isOpen = false;
            isReadyToOpen = false;
            closeCollider.GetComponent<Collider2D>().enabled = true;
        }
    }
}
