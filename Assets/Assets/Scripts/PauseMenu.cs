using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
	public static bool isPaused = false;
	
	[Header("References")]
	public CanvasGroup pauseScreen;
    public PlayerInput playerInput;

	private InputAction pauseAction;
	
	void Start() {
		pauseAction = playerInput.actions["Pause"];
	}
	
	void Update()
    {
        if (pauseAction.WasPressedThisFrame()) {
			TogglePause();
		}
    }
	
	public void TogglePause() {
		if (!PauseMenu.isPaused) {
			PauseMenu.isPaused = true;
			Time.timeScale = 0f;
			pauseScreen.alpha = 1f;
			pauseScreen.interactable = true;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		} else {
			PauseMenu.isPaused = false;
			Time.timeScale = 1f;
			pauseScreen.alpha = 0f;
			pauseScreen.interactable = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
	
	public void QuitGame()
    {
        Debug.Log("Quitting game");
		#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
		#else
        Application.Quit();
		#endif
    }
}