using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    [Header("reffers")]
    public MomMove      mom;
    public HealthBar    health;
    
    private BoxCollider2D col;
    private bool isRunning = false;
    public float speed = 3.0f;

    private Vector3 runTarget;

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

        if (isRunning)
             return;

        if (mom)
        {
            health.TakeDamage(1);
        }
    }

    public void Run(Vector3 target)
    {
        runTarget = target;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                runTarget,
                speed * Time.deltaTime
            );
        }
    }

}