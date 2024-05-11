using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hint;
    [SerializeField] private float DelayForShowHint;
    [SerializeField] Image beresta;
    private int _counter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_counter == 0)
        {
            if (collision.gameObject.layer == 3)
            {
                beresta.gameObject.SetActive(true);
                Color textColor = hint.color;
                textColor.a = 1f;
                hint.color = textColor;
                StartCoroutine(TurnOffAfterDelay(DelayForShowHint));
                _counter++;
            }
        }
    }

    private IEnumerator TurnOffAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Color textColor = hint.color;
        textColor.a = 0f;
        beresta.gameObject.SetActive(false);
        hint.color = textColor;
    }
}
