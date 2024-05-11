using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownBehavior : MonoBehaviour
{
    [SerializeField] GameObject thisGameObject;
    PlayerController playerController;
    private GameObject mainCharacter;
    private bool canWeTakeThisObj;
    private bool isWeTakeThisObj;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        mainCharacter = playerController.gameObject;

        playerController.onTakeKeyPressed.AddListener(Take);
        playerController.onThrowKeyPressed.AddListener(Throw);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && playerController != null)
        {
            Debug.Log("вход");
            playerController.CanWeTakeAnObject = true;
            canWeTakeThisObj = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && playerController != null)
        {
            Debug.Log("выход");
            playerController.CanWeTakeAnObject = false;
            canWeTakeThisObj = false;
        }
    }

    private void Take()
    {
        if (playerController.CanWeTakeAnObject && canWeTakeThisObj)
        {
            if (thisGameObject.CompareTag("Mushroom"))
            {
                MushroomCounter.mushrooms++;
                thisGameObject.GetComponent<Animator>().SetTrigger("MushAnimTrigger");
                Invoke("Destroy", 2.5f);
            }
            else
            {
                thisGameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                MakeChild(mainCharacter.transform, thisGameObject.transform);
                thisGameObject.transform.localPosition = new Vector3(-0.6f, 2.6f, 0f);
                isWeTakeThisObj = true;
                Invoke("SetBoolsBeforeThrow", 0.1f);
            }
        }
    }

    private void SetBoolsBeforeThrow()
    {
        playerController.IsWeHoldAnObject = true;
        playerController.CanWeTakeAnObject = false;
    }

    private void Throw()
    {
        if (isWeTakeThisObj && playerController.IsWeHoldAnObject)
        {
            thisGameObject.transform.SetParent(null);
            thisGameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            Vector2 throwForce;
            if (playerController.facingRight)
            {
                throwForce = new Vector2(10f, 10f);
                thisGameObject.GetComponent<Rigidbody2D>().velocity = throwForce;
            }
            else
            {
                throwForce = new Vector2(-10f, 10f);
                thisGameObject.GetComponent<Rigidbody2D>().velocity = throwForce;
            }
            playerController.IsWeHoldAnObject = false;
            isWeTakeThisObj = false;
        }
    }

    private void MakeChild(Transform parentTransform, Transform childTransform)
    {
        childTransform.parent = parentTransform;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
