using UnityEngine;

public class Exit : MonoBehaviour
{
    // Ваш существующий код

    // Метод для выхода из игры
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
