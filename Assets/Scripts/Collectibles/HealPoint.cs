using UnityEngine;

public class HealPoint : MonoBehaviour
{
    [Header("reffers")]
    public FellaMove      fella;
    public BurbBar burb;
    public SpriteRenderer hPpoint;
    
    private BoxCollider2D col;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if (col)
            col.isTrigger = true;        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("FellaTag"))
            return;

        if (fella)
        {
            hPpoint.enabled = false;
            burb.SetBurbs(1);
        }
    }

}
