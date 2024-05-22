using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerConditions : MonoBehaviour
{
    [SerializeField] private int lifePoints = 1;
    [SerializeField] private UnityEvent OnCharacterDeath;
    [SerializeField] private PlayerController playerController;
    public bool IsRunning;

    public void CheckIsWeAlived() 
    {        
        if (lifePoints == 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        playerController.animator.SetTrigger("Dead");
        playerController.enabled = false;
        lifePoints = 0;
        OnCharacterDeath.Invoke();
        Debug.Log("youdied");
        Restart.ReloadSceneWithProgressCheck();
        Invoke("ReloadSceneWithDelay", 2f);
    }
}
