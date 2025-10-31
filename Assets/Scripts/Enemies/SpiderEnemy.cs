using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [Header("reffers")]
    public MomMove      mom;
    public HealthBar    health;
    
    private BoxCollider2D col;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if (col)
            col.isTrigger = true;        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (mom)
        {
            health.TakeDamage(1);
        }
    }

}