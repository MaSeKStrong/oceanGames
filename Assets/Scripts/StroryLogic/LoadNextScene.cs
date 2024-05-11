using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] int nextSceneIndex;
    private AsyncOperation loadingOperation;

    public async void LoadNext()
    {
        CharacterPosition.CharacterStartPosition = new Vector3(-166f, -7.5f, 0f);
        loadingOperation = SceneManager.LoadSceneAsync(nextSceneIndex);
        loadingOperation.allowSceneActivation = false;

        await WaitUntilSceneLoaded();
        loadingOperation.allowSceneActivation = true;
    }

    private async Task WaitUntilSceneLoaded()
    {
        while (loadingOperation.progress < 0.75f)
        {
            await Task.Yield();
        }
    }
}

