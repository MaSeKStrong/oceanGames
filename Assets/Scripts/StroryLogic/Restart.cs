using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public static bool isCrest = false;
    public static void ReloadSceneWithProgressCheck()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Single).completed += OnSceneReloaded;
        isCrest = true;
    }

    private static void OnSceneReloaded(AsyncOperation operation)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        while (!operation.isDone || operation.progress < 0.9f)
        {
            return;
            isCrest = false;
        }
       
        SceneManager.LoadSceneAsync(currentSceneIndex, LoadSceneMode.Single);
     
    }

    public static void LoadThisScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}