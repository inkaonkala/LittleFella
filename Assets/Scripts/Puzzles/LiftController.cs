using UnityEngine;
using System.Collections;

public class LiftController : MonoBehaviour
{
    [Header("Stops (set in Inspector)")]
    public Transform firstStop;
    public Transform secondStop;
	public Transform thirdStop;

    [Header("Movement")]
    public float moveTime = 2f;     // time per leg
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0, 1,1);

    [Header("Boarding detection")]
    public LiftBoardZone boardZone; // drag the child trigger here

    [Header("Return")]
    public bool autoReturnToBottom = false;
    public float returnDelay = 1.0f;

    Vector3 _bottomPos;
    bool _running;

    void Awake()
    {
        _bottomPos = transform.position;
    }

    public void Activate()
    {
        if (_running) return;
        StartCoroutine(RunSequence());
    }

    IEnumerator RunSequence()
    {
        _running = true;

        // 1) Go to first stop, wait for Fella
        if (firstStop) yield return MoveTo(firstStop.position);
        if (boardZone != null)
            yield return new WaitUntil(() => boardZone.HasFella);

        // 2) Go to second stop, wait for BOTH
        if (secondStop) yield return MoveTo(secondStop.position);
        if (boardZone != null)
            yield return new WaitUntil(() => boardZone.BothOnboard);

        // 3) Go to third stop
        if (thirdStop) yield return MoveTo(thirdStop.position);

        // 4) Optional return
        if (autoReturnToBottom)
        {
            yield return new WaitForSeconds(returnDelay);
            yield return MoveTo(_bottomPos);
        }

        _running = false;
    }

    public void LowerToBottom()
    {
        if (_running) return;
        StartCoroutine(MoveTo(_bottomPos));
    }

    IEnumerator MoveTo(Vector3 target)
    {
        Vector3 a = transform.position;
        Vector3 b = target;
        float t = 0f;

        while (t < moveTime)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / moveTime);
            float e = ease.Evaluate(u);
            transform.position = Vector3.LerpUnclamped(a, b, e);
            yield return null;
        }
        transform.position = b;
    }
}
