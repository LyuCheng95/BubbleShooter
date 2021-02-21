using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public List<GameObject> bubblesPrefabs;

    public float waitTime;

    public int batchBubbles;

    public Text scoreText;

    public Text gameOverScoreText;

    public Canvas gameOverBoard;

    public Vector2 fallSpeed;

    public int score;

    public bool isGameOver;

    private void Start()
    {
        scoreText.text = score.ToString();
        gameOverBoard.enabled = false;
        isGameOver = false;
    }

    public IEnumerator GenerateLevel()
    {
        while (true)
        {
            GenerateRandomBubblesForOneBatch (batchBubbles, bubblesPrefabs);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void GenerateRandomBubblesForOneBatch(
        int count,
        List<GameObject> bubbles
    )
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(-23f, 23f);
            float y = Random.Range(30f, 40.0f);
            Vector2 randomPosition = new Vector2(x, y);
            var bubble =
                Instantiate(bubbles[(
                int
                )(Random.Range(0, bubbles.Count * 1000000f) / 1000000f)]);
            bubble.transform.position = randomPosition;
            Rigidbody2D rb =
                bubble.GetComponent(typeof (Rigidbody2D)) as Rigidbody2D;
            rb.velocity = fallSpeed;
        }
    }

    public void AddScore(int s)
    {
        score = score + s;
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true; 
        gameOverBoard.enabled = true;
        gameOverScoreText.text = score.ToString();
    }
}
