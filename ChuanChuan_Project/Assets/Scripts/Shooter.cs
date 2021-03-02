using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject gunSprite;

    public bool canShoot;

    public bool isShooting;

    public float forkSpeed;

    public Transform nextBubblePosition;

    public Transform collectionPosition;

    public List<Bubble> collection;

    public Vector2 collectionOffset;

    private Vector2 collectionNextPosition;

    private Vector2 staticVector = new Vector2(0, 0);

    private Vector2 lookDirection;

    private float lookAngle;

    public int maxCollection;

    public AudioSource bombaudio;

    public AudioSource matchaudio;


    Vector3 forkStartPos;

    void Start()
    {
        forkStartPos = this.transform.position;
    }

    public void Initiate()
    {
        collection.Clear();
        canShoot = true;
        collectionNextPosition =
            new Vector2(collectionPosition.position.x,
                collectionPosition.position.y);
    }

    public void Update()
    {
        if (LevelManager.instance.isGameOver)
        {
            Rigidbody2D rb2d = gunSprite.GetComponent<Rigidbody2D>();
            rb2d.angularVelocity = 0;
        }
        else if (!isShooting)
        {
            updateShooterBearing();
        }
    }

    //must be attached to sprite
    // must be rb & collision + same to the collided sprite
    private void OnCollisionEnter2D(Collision2D col)
    {
        this.transform.position = forkStartPos;
        isShooting = false;
        canShoot = true;
        gunSprite.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void updateShooterBearing()
    {
        lookDirection =
            Camera.main.ScreenToWorldPoint(Input.mousePosition) -
            transform.position;
        lookAngle =
            Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        gunSprite.transform.rotation =
            Quaternion.Euler(0f, 0f, lookAngle - 90f);
    }

    public void Shoot()
    {
        isShooting = true;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);
        gunSprite.transform.rotation = transform.rotation;
        Rigidbody2D rb2d = gunSprite.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
        rb2d.AddForce(gunSprite.transform.up * forkSpeed, ForceMode2D.Impulse);

        //to prevent rotation
        rb2d.angularVelocity = 0;
    }

    public void UpdateCollection(Bubble newBubble)
    {
        if (newBubble.tag == "Bomb")
        {
            bombaudio.Play();
            newBubble.Crash();
            LevelManager.instance.AddScore(collection.Count);
            collection.Clear();
            collectionNextPosition = collectionPosition.position;
            var allCollections =
                GameObject.FindGameObjectsWithTag("Collection");
            for (int i = 0; i < allCollections.Length; i++)
            {
                Destroy(allCollections[i]);
            }
        }
        else
        {
            newBubble.tag = "Collection";
            collection.Add (newBubble);

            //location of collected bubbles
            newBubble.transform.position = collectionNextPosition;
            collectionNextPosition += collectionOffset;

            //mesh three
            //TODO: animation maybe
            if (collection.Count >= 3)
            {
                Bubble lastItem = collection[collection.Count - 1];
                Bubble lastItem2 = collection[collection.Count - 2];
                Bubble lastItem3 = collection[collection.Count - 3];
                if (
                    lastItem.bubbleColor.Equals(lastItem2.bubbleColor) &&
                    lastItem.bubbleColor.Equals(lastItem3.bubbleColor)
                )
                {
                    matchaudio.Play();
                    collection.Remove (lastItem);
                    collection.Remove (lastItem2);
                    collection.Remove (lastItem3);

                    //reposition of collected bubbles after mesh three
                    collectionNextPosition -= collectionOffset * 3;

                    //destrory lastitem is not working jwj??
                    lastItem.Crash();
                    lastItem2.Crash();
                    lastItem3.Crash();

                    LevelManager.instance.AddScore(3);
                }
            }
            if (collection.Count > maxCollection)
            {
                LevelManager.instance.GameOver();
            }
        }
    }
}
