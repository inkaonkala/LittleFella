using UnityEngine;

public class TheStrawStart : MonoBehaviour
{
    [Header("reffers")]
    public MomMove mom;
    public SpriteRenderer pic;
    public Canvas txt;
    private BoxCollider2D col;

    void Awake()
    {
        col = GetComponent<BoxCollider2D>();
        if (col)
            col.isTrigger = true;        
    }

    void Start()
    {
        if (pic)
            pic.enabled = true;
        if (txt)
            txt.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (pic)
            pic.enabled = false;
        if (txt)
            txt.gameObject.SetActive(true);

        if (mom)
        {
            mom.hasWeapon = true;
            mom.RefreshSmackBind();
        }
        if (col)
            col.enabled = false;

    }
}
