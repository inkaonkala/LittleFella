using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    [SerializeField] private GameObject txtObj;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            txtObj.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            txtObj.SetActive(false);
    }
}
