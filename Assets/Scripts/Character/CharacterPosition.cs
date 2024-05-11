using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPosition : MonoBehaviour
{
    public static Vector3 CharacterStartPosition = new Vector3(-166f, -7.5f, 0f);

    [SerializeField] GameObject character;

    private void Awake()
    {
        Debug.Log("Awake");
        Instantiate(character, CharacterStartPosition, Quaternion.identity);
    }
}
