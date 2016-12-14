using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;


#if UNITY_IOS
    public Text IOSscoreText;
    public Text IOSgameOverText;
#else
    public GUIText scoreText;
    public GUIText restartText;
    public GUIText gameOverText;
#endif
    public GameObject restartButton;

    private int score;
    private bool gameOver;
    private bool restart;

    


	// Use this for initialization
	void Start () {
        score = 0;
        gameOver = false;
        restart = false;

#if UNITY_IOS
#else
    restartText.text = "";
#endif

    restartButton.SetActive(false);
        gameOverText.text = "";

        UpdateScore();
        StartCoroutine (SpawnWaves());
	}
#if UNITY_IOS
    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }
#else
    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("Main");
            }
        }
    }
#endif


    IEnumerator SpawnWaves ()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
#if UNITY_IOS
#else
                restartText.text = "Press 'R' to restart";
#endif
                restartButton.SetActive(true);
                restart = true;
                break;
            }
        }
            
        
    }
    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        gameOver = true;
    }

}
