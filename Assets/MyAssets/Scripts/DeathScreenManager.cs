using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    public void RestartLevel()
    {
        // Despausa o jogo antes de recarregar a cena.
        Time.timeScale = 1f;

        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}