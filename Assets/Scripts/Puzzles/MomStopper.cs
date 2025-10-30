using UnityEngine;

public class MomStopper : MonoBehaviour
{
    private BoxCollider2D col;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = false;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (!collision.gameObject.CompareTag("Player"))
            Physics2D.IgnoreCollision(collision.collider, col, true);
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
       
        if (!collision.gameObject.CompareTag("Player"))
            Physics2D.IgnoreCollision(collision.collider, col, true);

    }
}
