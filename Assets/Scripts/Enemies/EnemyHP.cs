using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    [Header("reffers")]
    public SpiderEnemy spider;
    public MomMove  mom;
    public Transform runStopper;
    
    public float hp = 1.0f;

    private  BoxCollider2D col;
    private bool isDead = false;
    public AudioSource fleeSound;
    
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

        if (isDead)
            return;

        if (other.CompareTag("StrawHitBox"))
        {
                Debug.Log("TAKE DAMA");
                TakeDamage(1);
        }
    }

    void TakeDamage(float i)
    {
        hp -= i;
        
        if (hp <= 0)
            RunAndDie();
    }

    void RunAndDie()
    {
        isDead = true;

        col.enabled = false;
        if (fleeSound)
            fleeSound.Play();

        spider.Run(runStopper.position);
    }
}


