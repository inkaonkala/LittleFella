using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BurbBar : MonoBehaviour
{
    public Image[] burbPieces;
    public FellaMove fella;

    void Update()
    {
        if (!fella)
            return;

        SetBurbs(fella.burbs);
    }

    public void SetBurbs(int count)
    {
        count = Mathf.Clamp(count, 0, burbPieces.Length);
        for(int i = 0; i < burbPieces.Length; i++)
        {
            if (burbPieces[i])
                burbPieces[i].enabled = i < count;
        }
    }

}
