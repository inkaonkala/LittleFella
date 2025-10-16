using UnityEngine;

public class PlatformBounce : MonoBehaviour
{
    public float bounceDownMom = 0.2f;
    public float bounceDownFella = 0.1f;
    public float bounceTime = 3f;
    public float squashAmont = 0.15f;


    private float targetOffset = 0f;
    private float targetSquash = 0f;

    private Vector3 startPos;
    private Vector3 startScale;

    private bool isBouncing = false;


    void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;        
    }

    void Update()
    {
        if (isBouncing)
        {
            //move
            Vector3 targetPos = startPos + Vector3.down * targetOffset;
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * bounceTime);

            //squish
            Vector3 targetScale = startScale + new Vector3(0, -targetSquash, 0);
            targetScale.x += targetSquash * 0.5f;
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * bounceTime);

            //return
            if (Mathf.Abs(transform.localPosition.y - startPos.y) < 0.0f && targetOffset == 0f)
            {
                transform.localPosition = startPos;
                transform.localScale = startScale;
                isBouncing = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            StartBounce(bounceDownMom, squashAmont);
        else if (collision.gameObject.CompareTag("FellaTag"))
            StartBounce(bounceDownFella, squashAmont * 0.5f);
    }

    private void StartBounce(float depth, float squash)
    {
        targetOffset = depth;
        targetSquash = squash;
        isBouncing = true;
        StopAllCoroutines();
        StartCoroutine(BounceBack());
    }
    
    private System.Collections.IEnumerator BounceBack()
    {
        yield return new WaitForSeconds(0.01f);
        targetOffset = 0f;
        targetSquash = 0f;
    }

}
