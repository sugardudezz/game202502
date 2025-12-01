using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static event Action<string> OnSceneChange;

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene("Scenes/" + sceneName);
        OnSceneChange?.Invoke(sceneName);
    }
}