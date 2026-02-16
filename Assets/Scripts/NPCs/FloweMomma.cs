using UnityEngine;

public class FloweMomma : MonoBehaviour
{
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FellaTag"))
            audioSource.Play();
    }
}

/*

CHAT GPT CODE :OO

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FloweMomma : MonoBehaviour
{
    [Header("Volume by distance (inside trigger)")]
    [Range(0f, 1f)] public float maxVolume = 1f;     // volume when very close
    [Range(0f, 1f)] public float minVolume = 0.05f;  // volume at edge of distance
    public float maxDistance = 6f;                   // beyond this -> minVolume

    [Header("Fade")]
    public float fadeSpeed = 2.5f; // higher = faster fade

    private AudioSource audioSource;
    private Transform fella;
    private bool fellaInside = false;
    private float targetVolume = 0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        // audioSource.loop = true; // enable if your singing clip should loop
        audioSource.volume = 0f;   // start silent, we fade in
    }

    void Update()
    {
        // If inside, compute target volume from distance.
        if (fellaInside && fella != null)
        {
            float d = Vector2.Distance(transform.position, fella.position);

            // 0 when far, 1 when close
            float t = 1f - Mathf.Clamp01(d / maxDistance);

            targetVolume = Mathf.Lerp(minVolume, maxVolume, t);

            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            // Outside trigger -> fade to silence
            targetVolume = 0f;
        }

        // Smoothly move current volume toward target volume
        audioSource.volume = Mathf.MoveTowards(
            audioSource.volume,
            targetVolume,
            fadeSpeed * Time.deltaTime
        );

        // When faded out, actually stop (prevents “ghost playing”)
        if (!fellaInside && audioSource.isPlaying && audioSource.volume <= 0.001f)
            audioSource.Stop();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("FellaTag"))
        {
            fella = other.transform;
            fellaInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("FellaTag"))
        {
            fellaInside = false;
            fella = null;
        }
    }
}


*/
