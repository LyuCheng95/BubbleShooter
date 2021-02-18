using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float raycastRange = 0.7f;
    public float raycastOffset = 0.51f;
    public bool isFixed;
    public bool isConnected;
    public BubbleColor bubbleColor;
    void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Fork") {
            GameManager.instance.shootScript.UpdateCollection(this);
            GameManager.instance.ProcessTurn(transform);

        }
    }

    private void HasCollided(Bubble collisionObject)
    {
        // LevelManager.instance.SetAsBubbleAreaChild(transform);
    }

    public List<Transform> GetNeighbors()
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        List<Transform> neighbors = new List<Transform>();

        hits.Add(Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset, transform.position.y), Vector3.left, raycastRange));
        hits.Add(Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset, transform.position.y), Vector3.right, raycastRange));
        hits.Add(Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset, transform.position.y + raycastOffset), new Vector2(-1f, 1f), raycastRange));
        hits.Add(Physics2D.Raycast(new Vector2(transform.position.x - raycastOffset, transform.position.y - raycastOffset), new Vector2(-1f, -1f), raycastRange));
        hits.Add(Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset, transform.position.y + raycastOffset), new Vector2(1f, 1f), raycastRange));
        hits.Add(Physics2D.Raycast(new Vector2(transform.position.x + raycastOffset, transform.position.y - raycastOffset), new Vector2(1f, -1f), raycastRange));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.transform.tag.Equals("Bubble"))
            {
                neighbors.Add(hit.transform);
            }
        }

        return neighbors;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Crash()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }

    public enum BubbleColor
    {
        BLUE, YELLOW, RED, GREEN
    }
}
