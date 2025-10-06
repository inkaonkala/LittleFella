using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavePoint : MonoBehaviour
{
    [Header("RespawnPoint")]
    public Transform respawnPoint;

    [Header("Links")]
    public Animator saveAnimation;
    public AudioSource saveSound;
    public TextMesh txt;
    public Renderer txtRenderer;

    [Header("Sounds")]
    public AudioClip save;        // normal save sound
    public AudioClip littleBunny; // first save sound

    [Header("FlowerId")]
    public string savePointId = "Flower_1";

    public float cooldown = 1f;
    float _lastTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("FellaTag"))
            return;
        if (Time.time - _lastTime < cooldown)
            return;
        _lastTime = Time.time;

        bool firstTimeHere = FirstTimeAtFlowa();

        if (!firstTimeHere)
        {
            if (saveAnimation)
                saveAnimation.SetTrigger("HowToBow");
            if (littleBunny && saveSound)
                saveSound.PlayOneShot(littleBunny);
            MarkFlowaUsed();
        }
        else
        {
            if (saveAnimation)
                saveAnimation.SetTrigger("Saving");
            if (save && saveSound)
                saveSound.PlayOneShot(save);
        }

        ShowSaveText("Game Saved");

        // SAVING!
        // SaveManager.SaveAt(savePointId, respawnPoint ? respawnPoint.position : transform.position);
        // ---- SAVE POSITION ----
        Vector3 pos = respawnPoint ? respawnPoint.position : transform.position;

        SaveManager.SaveAt(savePointId, pos);
    }


    bool FirstTimeAtFlowa()
    {
        // PlayerPrefs key per-flower
        string k = "flower_first_" + savePointId;
        return PlayerPrefs.GetInt(k, 0) == 0;
    }

    void MarkFlowaUsed()
    {
        string k = "flower_first_" + savePointId;
        PlayerPrefs.SetInt(k, 1);
        PlayerPrefs.Save();
    }

    private void ShowSaveText(string message)
    {
        if (!txt) return;

        txt.text = message;

        if (txtRenderer)
            txtRenderer.enabled = true;
        CancelInvoke(nameof(HideTxt));
        Invoke(nameof(HideTxt), 2f);
    }

    private void HideTxt()
    {
        if (txtRenderer)
            txtRenderer.enabled = false;
    }
}
