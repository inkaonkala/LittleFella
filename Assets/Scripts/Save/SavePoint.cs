using UnityEngine;
using UnityEngine.UI;

public class SavePoint : MonoBehaviour
{
    public Transform respawnPoint;

    [Header("Links")]
    public Animator saveAnimation;
    public AudioSource saveSound;
    public Text txt;

    [Header("Sounds")]
    public AudioClip save;        // normal save sound
    public AudioClip littleBunny; // first save sound

    [Header("FlowerId")]
    public string savePointId = "Flower_1";

    private bool hasSavedOnce = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) // <- change tag if you really want "FellaTag"
            return;

        if (!hasSavedOnce)
        {
            saveAnimation.SetTrigger("HowToBow");
            if (littleBunny) saveSound.PlayOneShot(littleBunny);
            hasSavedOnce = true;
        }
        else
        {
            saveAnimation.SetTrigger("Saving");
            if (save) saveSound.PlayOneShot(save);
        }

        ShowSaveText("Game Saved");

        // SAVING!
        SaveManager.SaveAt(savePointId, respawnPoint ? respawnPoint.position : transform.position);
    }

    private void ShowSaveText(string message)
    {
        if (!txt) return;

        txt.text = message;
        txt.enabled = true;
        CancelInvoke(nameof(HideTxt));
        Invoke(nameof(HideTxt), 2f);
    }

    private void HideTxt()
    {
        if (txt) txt.enabled = false;
    }
}
