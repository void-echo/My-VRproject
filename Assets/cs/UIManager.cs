using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Text restartText;
    public static UIManager myself = null;
    public static bool isGameOver = false;
    public static int total_score = 0;

    // Start is called before the first frame update
    void Start()
    {
        myself = this;
        //Disables panel if active
        gameOverPanel.SetActive(false);
        restartText.gameObject.SetActive(false);
    }

    public static void AddScore(int score)
    {
        total_score += score;
    }


    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            this.StartCoroutine(GameOverSequence());
        }
    }

    //controls game over canvas and there's a brief delay between main Game Over text and option to restart/quit text
    public IEnumerator GameOverSequence()
    {
        gameOverPanel.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        restartText.gameObject.SetActive(true);
    }
}