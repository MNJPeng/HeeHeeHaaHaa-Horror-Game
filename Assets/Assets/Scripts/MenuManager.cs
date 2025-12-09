using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    public void StartGameButtonClicked()
    {
        SceneManager.LoadScene("HeeHeeHaaHaaScene");
    }
    public void QuitGameButtonClicked()
    {
        Debug.Log("Quitting game");
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
