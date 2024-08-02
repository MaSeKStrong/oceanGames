using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax : MonoBehaviour
{
    private float startPos, lenght;
    public GameObject cam;
    public float paralaxEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * paralaxEffect;
        float movement = cam.transform.position.x * (1 - paralaxEffect);
        
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if(movement > startPos + lenght)
        {
            startPos += lenght;
        }else if(movement< startPos - lenght)
        {
            startPos -= lenght;
        }
    }
}