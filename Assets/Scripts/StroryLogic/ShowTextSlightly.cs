using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTextSlightly : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] string TextForShow;
    [SerializeField] string TextForShow2;

    bool skip = false;

    void Start()
    {
        ShowText();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            skip = true;
        }
    }
    public void ShowText() 
    {
        StartCoroutine(c_Output(TextForShow, 0.04f));
    }
    public void ShowText2()
    {
        StopAllCoroutines();
        txt.text = " ";
        StartCoroutine(c_Output(TextForShow2, 0.04f));
    }

    IEnumerator c_Output(string str, float delay)
    {
        foreach (var sym in str)
        {
            //print(sym);

            txt.text += sym;

            if (!skip)
                yield return new WaitForSeconds(delay);
        }

        skip = false;
    }
}
