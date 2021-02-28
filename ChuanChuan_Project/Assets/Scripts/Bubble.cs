using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float raycastRange = 0.7f;

    public float raycastOffset = 0.51f;

    public float lowerLimit = -5f;

    public bool isFixed;

    public bool isConnected;

    public BubbleColor bubbleColor;

    void Update()
    {
        if (
            (gameObject.tag == "Bubble" || gameObject.tag == "Bomb") &&
            gameObject.transform.position.y < lowerLimit
        )
        {
            LevelManager.instance.GameOver();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fork")
        {
            Rigidbody2D rd = this.GetComponent<Rigidbody2D>();
            CircleCollider2D cc = this.GetComponent<CircleCollider2D>();
            Destroy (rd);
            Destroy (cc);
            LevelManager.instance.shootScript.UpdateCollection(this);
        }
    }

    public void Crash()
    {
        Destroy (gameObject);
    }

    public enum BubbleColor
    {
        BLUE,
        YELLOW,
        RED,
        GREEN
    }
}
