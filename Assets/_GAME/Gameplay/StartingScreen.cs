using _GAME.Gameplay;
using UnityEngine;

public class StartingScreen : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManager gameManager = FindFirstObjectByType<GameManager>();
            gameManager.StartGame();
            Destroy(gameObject);
        }
    }

    public void OpenSettings()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        gameManager.ToggleSettingsMenu(true);
    }
}
