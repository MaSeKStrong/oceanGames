using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    Animator anim;
    public bool isOpen;
    bool isReadyToOpen;
    bool isReadyToClose;
    public BoxCollider2D closeCollider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
     
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
        if (collision != null && collision.gameObject.layer == 3) 
        {
            if (!isOpen)
            {
                isReadyToOpen = true;
            }
            else { isReadyToClose = true; }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision != null && collision.gameObject.layer == 3) && Input.GetKey(KeyCode.LeftShift) && isReadyToOpen){
                isOpen = true;
                closeCollider.GetComponent<Collider2D>().enabled = false;
                      
         }
        if ((collision != null && collision.gameObject.layer == 3) && Input.GetKey(KeyCode.LeftShift) && isReadyToClose)
        {
            isOpen = false;
            closeCollider.GetComponent<Collider2D>().enabled = true;
        }
    }   
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.layer == 3)
        {
           
            isReadyToOpen = false;
            isReadyToClose = false;
           
            
        }
    }
}
