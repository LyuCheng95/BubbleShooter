using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
#endregion


    public Shooter shootScript;

    private IEnumerator coroutine;

    void Start()
    {
        coroutine = LevelManager.instance.GenerateLevel();
        StartCoroutine (coroutine);
        shootScript.Initiate();
    }

    void Update()
    {
        if (!LevelManager.instance.isGameOver)
        {
            if (
                shootScript.canShoot &&
                Input.GetMouseButtonUp(0) &&
                (
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y >
                shootScript.transform.position.y
                )
            )
            {
                shootScript.canShoot = false;
                shootScript.Shoot();
            }
        }
    }
}
