using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
    public class GameOver : MonoBehaviour
    {
        public Canvas GameOverScreen;

        void Awake()
        {
            GameOverScreen.gameObject.SetActive(false);
        }

        public void ShowScreen()
        {
            GameOverScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void PlayAgain()
        {
            Time.timeScale = 1;
			Scene currentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(currentScene.name);
        }
    }
}