using UnityEngine;

//fella
public class AudioScript : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip jumpClip;
    public AudioClip burbClip;
    public AudioClip climbClip;
    public AudioClip hurtClip;
    public AudioClip hitClip;

    public void PlayJump()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayBurb()
    {
        audioSource.PlayOneShot(burbClip);
    }

    public void PlayClimb()
    {
        audioSource.PlayOneShot(climbClip);
    }

    public void PlayHurt()
    {
        audioSource.PlayOneShot(hurtClip);
    }

        public void PlayHit()
    {
        audioSource.PlayOneShot(hitClip);
    }
}
