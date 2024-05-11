using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLogic : MonoBehaviour
{
    //[SerializeField] GameObject good;
    //[SerializeField] GameObject bad;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadSceneAsync(6);
            //collision.GetComponent<PlayerController>().enabled = false;
            //if(MushroomCounter.mushrooms > 2) { BadEnd(); }
            //else { GoodEnd(); }
        }
    }

    //void BadEnd() 
    //{
    //    bad.SetActive(true);
    //}   

    //void GoodEnd()
    //{
    //    good.SetActive(true);
    //}
}
