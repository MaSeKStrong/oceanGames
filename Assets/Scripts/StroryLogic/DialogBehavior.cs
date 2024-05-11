using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogBehavior : MonoBehaviour
{
    PlayerController playerController;
    private void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        if (playerController != null ) { playerController.StopCharacter(); }
    }

    public void CloseDialog()
    {
        if (playerController != null)
            playerController.CharacterCanGo();
        Destroy(gameObject);
    }
}
