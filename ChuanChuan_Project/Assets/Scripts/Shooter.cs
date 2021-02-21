using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject gunSprite;
    public bool canShoot;
    public bool isShooting;
    public float forkSpeed;

    public Transform nextBubblePosition;
    public GameObject currentBubble;
    public GameObject nextBubble;

    public Transform collectionPosition;
    public List<Bubble> collection;
    public Vector2 collectionOffset;
    private Vector2 collectionNextPosition;

    private Vector2 staticVector = new Vector2(0,0);

    private Vector2 lookDirection;
    private float lookAngle;
    public bool isSwaping = false;
    public float time = 0.02f;

    
    Vector3 startPos;
    void Start()
    {
        startPos = this.transform.position;

    }
    public void Initiate() {
        canShoot = true;
        collectionNextPosition = new Vector2(collectionPosition.position.x, collectionPosition.position.y);
    }

    public void Update()
    {
        if (!isShooting) {
            updateShooterBearing();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
        this.transform.position = startPos;
        isShooting = false;
        canShoot = true;
        gunSprite.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

    }

    private void updateShooterBearing() {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        gunSprite.transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);
    }

    public void Shoot()
    {
        isShooting = true;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle - 90f);
        gunSprite.transform.rotation = transform.rotation;
        Rigidbody2D rb2d = gunSprite.GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
        rb2d.AddForce(gunSprite.transform.up * forkSpeed, ForceMode2D.Impulse);
    }

    [ContextMenu("SwapBubbles")]
    public void SwapBubbles()
    {
        currentBubble.GetComponent<Collider2D>().enabled = false;
        nextBubble.GetComponent<Collider2D>().enabled = false;
        isSwaping = true;
    }

    [ContextMenu("CreateNextBubble")]
    public void CreateNextBubble()
    {
        List<GameObject> bubblesInScene = LevelManager.instance.bubblesInScene;
        List<string> colors = LevelManager.instance.colorsInScene;

        if (nextBubble == null)
        {
            nextBubble = InstantiateNewBubble(bubblesInScene);
        }
        else
        {
            if(!colors.Contains(nextBubble.GetComponent<Bubble>().bubbleColor.ToString()))
            {
                Destroy(nextBubble);
                nextBubble = InstantiateNewBubble(bubblesInScene);
            }
        }

        if(currentBubble == null)
        {
            currentBubble = nextBubble;
            currentBubble.transform.position = new Vector2(transform.position.x, transform.position.y);
        }
    }

    private GameObject InstantiateNewBubble(List<GameObject> bubblesInScene)
    {
        return null;
        GameObject newBubble = Instantiate(bubblesInScene[Random.Range(0, bubblesInScene.Count)]);
        newBubble.transform.position = new Vector2(nextBubblePosition.position.x, nextBubblePosition.position.y);
        newBubble.GetComponent<Bubble>().isFixed = false;
        Rigidbody2D rb2d = newBubble.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        rb2d.gravityScale = 0f;

        return newBubble;
    }

    public void UpdateCollection(Bubble newBubble) {
        collection.Add(newBubble);
        newBubble.transform.position = collectionNextPosition;
        collectionNextPosition += collectionOffset;
        newBubble.GetComponent<Rigidbody2D>().velocity = staticVector;

//mesh three

        //TODO: animation maybe

        if (collection.Count >= 3) {
            Bubble lastItem = collection[collection.Count-1];
            Bubble lastItem2 = collection[collection.Count-2];
            Bubble lastItem3 = collection[collection.Count-3];
            if (lastItem.bubbleColor.Equals(lastItem2.bubbleColor) && lastItem.bubbleColor.Equals(lastItem3.bubbleColor)) {
                collection.Remove(lastItem);
                collection.Remove(lastItem2);
                collection.Remove(lastItem3);
                collectionNextPosition -= collectionOffset * 3;
                lastItem.Crash();
                lastItem2.Crash();
                lastItem3.Crash();
            }
        }
    }
}
