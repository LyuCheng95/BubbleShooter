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

    public GameObject bombPrefab;

    public float waitTime;

    public int batchBubbles;

    public Text scoreText;

    public Text gameOverScoreText;

    public Canvas gameOverBoard;

    public Canvas gameStartBoard;

    public Vector2 fallSpeed;

    public Vector2 fallSpeedAccelerator;

    public int score;

    public bool isGameOver;

    public Shooter shootScript;

    public Button startButton;

    public Button playAgainButton;

    private IEnumerator coroutine;

    public float bombRandomThreshold;

    private void Start()
    {
        gameStartBoard.enabled = true;
        gameOverBoard.enabled = false;
        startButton.onClick.AddListener(this.handleStartButtonClick);
        playAgainButton.onClick.AddListener(this.handlePlayAgainButtonClick);
    }

    IEnumerator StartGame()
    {
        gameOverBoard.enabled = false;
        yield return new WaitForSeconds(1);
        coroutine = GenerateBatchBubble();
        StartCoroutine (coroutine);
        shootScript.Initiate();
        scoreText.text = score.ToString();
        isGameOver = false;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (
                shootScript.canShoot &&
                Input.GetMouseButtonUp(0) &&
                //only can shoot upwards
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y >
                shootScript.transform.position.y
            )
            {
                shootScript.canShoot = false;
                shootScript.Shoot();
            }
        }
    }

    public IEnumerator GenerateBatchBubble()
    {
        while (true)
        {
            GenerateRandomBubblesForOneBatch (batchBubbles, bubblesPrefabs);
            waitTime = 10f / -fallSpeed.y;
            yield return new WaitForSeconds(waitTime);
            fallSpeed += fallSpeedAccelerator;
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
            float y = Random.Range(33f, 43f);
            Vector2 randomPosition = new Vector2(x, y);

            //instantiate ONE bubble randomly from list
            var bubble =
                Instantiate(bubbles[(
                int
                )(Random.Range(0, bubbles.Count * 1000000f) / 1000000f)]);
            bubble.transform.position = randomPosition;

            //add velocity
            Rigidbody2D rb =
                bubble.GetComponent(typeof (Rigidbody2D)) as Rigidbody2D;
            rb.velocity = fallSpeed;
        }
        float bombRandomIndicator = Random.Range(0f, 100f);
        if (bombRandomIndicator < bombRandomThreshold)
        {
            float x = Random.Range(-23f, 23f);
            float y = Random.Range(33f, 43f);
            Vector2 randomPosition = new Vector2(x, y);
            var bomb = Instantiate(bombPrefab);
            bomb.transform.position = randomPosition;

            //add velocity
            Rigidbody2D rb =
                bomb.GetComponent(typeof (Rigidbody2D)) as Rigidbody2D;
            rb.velocity = fallSpeed;
        }
    }

    public void AddScore(int x)
    {
        score = score + x;
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverBoard.enabled = true;
        gameOverScoreText.text = score.ToString();

        var allBubbles = GameObject.FindGameObjectsWithTag("Bubble");
        for (int i = 0; i < allBubbles.Length; i++)
        {
            Destroy(allBubbles[i]);
        }

        var allCollections = GameObject.FindGameObjectsWithTag("Collection");
        for (int i = 0; i < allCollections.Length; i++)
        {
            Destroy(allCollections[i]);
        }

        var allBombs = GameObject.FindGameObjectsWithTag("Bomb");
        for (int i = 0; i < allBombs.Length; i++)
        {
            Destroy(allBombs[i]);
        }
        StopCoroutine (coroutine);
    }

    public void handleStartButtonClick()
    {
        gameStartBoard.enabled = false;
        StartCoroutine(StartGame());
    }

    public void handlePlayAgainButtonClick()
    {
        gameOverBoard.enabled = false;
        gameStartBoard.enabled = true;
    }
}
