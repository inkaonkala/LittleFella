using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    [Header("reffers")]
    public SpiderEnemy spider;
    public MomMove  mom;
    public float hp = 1;

    private  BoxCollider2D col;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if (col)
            col.isTrigger = true;
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
