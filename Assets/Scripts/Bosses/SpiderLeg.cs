using UnityEngine;
using System.Collections;

public class SpiderLeg : MonoBehaviour
{
    [Header("Setup")]
    public Transform tip;          // last bone of IK chain
    public Transform tipVisual;    // child with sprite + collider
    public Transform target;       // IK target
    public Transform home;         // rest position for target
    public Transform mom;          // victim
    public Collider2D killZone;

    [Header("Timing")]
    public float tipAimTime = 0.15f;
    public float tipForwardAngle = -90f;   // which way is “pointing” on the sprite
    public float windupHeight = 1f;
    public float windupTime = 0.3f;
    public float slamTime = 0.1f;
    public float recoverTime = 0.4f;
    public float pauseTime = 1f;

    [Header("Control")]
    public bool controlledByBoss = true;
    public bool IsBusy => busy;

    bool busy;
    float _maxReach;
    Transform _root;

    void Start()
    {
        _root = transform;

        // Auto-create home if missing
        if (home == null && target != null)
        {
            var h = new GameObject(name + "_home");
            h.transform.position = target.position;
            h.transform.SetParent(target.parent, true);
            home = h.transform;
        }

        // Compute how far the leg can reach in rest pose
        if (tip != null)
            _maxReach = Vector3.Distance(_root.position, tip.position);
        else if (home != null)
            _maxReach = Vector3.Distance(_root.position, home.position);

        if (killZone != null)
            killZone.enabled = false;

        if (!controlledByBoss)
            StartCoroutine(AttackLoop());
    }

    // ---------------- Loops ----------------

    IEnumerator AttackLoop()
    {
        while (true)
        {
            if (!busy && mom != null)
                yield return AttackMom();

            yield return new WaitForSeconds(pauseTime);
        }
    }

    // ---------------- Helpers ----------------

    IEnumerator MoveTarget(Vector3 from, Vector3 to, float duration)
    {
        if (target == null)
            yield break;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / duration);
            target.position = Vector3.Lerp(from, to, k);
            yield return null;
        }
        target.position = to;
    }

    Vector3 ClampToLegReach(Vector3 worldPos)
    {
        if (_maxReach <= 0f)
            return worldPos;

        Vector3 origin = _root.position;
        Vector3 offset = worldPos - origin;

        if (offset.sqrMagnitude <= _maxReach * _maxReach)
            return worldPos;

        return origin + offset.normalized * _maxReach;
    }

    IEnumerator AimTipAt(Vector3 worldPos)
    {
        if (tip == null || tipVisual == null)
            yield break;

        Vector3 dir = (worldPos - tip.position);
        if (dir.sqrMagnitude < 0.0001f)
            yield break;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float targetZ = angle + tipForwardAngle;

        Quaternion start = tipVisual.rotation;
        Quaternion end   = Quaternion.Euler(0f, 0f, targetZ);

        float t = 0f;
        while (t < tipAimTime)
        {
            t += Time.deltaTime;
            float k = Mathf.SmoothStep(0f, 1f, t / tipAimTime);
            tipVisual.rotation = Quaternion.Slerp(start, end, k);
            yield return null;
        }

        tipVisual.rotation = end;
    }

    // ---------------- Attacks ----------------

    IEnumerator AttackMom()
    {
        if (mom == null) yield break;
        yield return AttackOnce(mom.position);
    }

    public IEnumerator AttackOnce(Vector3 worldPos)
    {
        if (busy || target == null || home == null)
            yield break;

        busy = true;

        // 1) Only the claw turns to look at the target
        yield return AimTipAt(worldPos);

        // 2) Leg windup
        Vector3 startPos = target.position;
        Vector3 liftPos  = startPos + Vector3.up * windupHeight;
        yield return MoveTarget(startPos, liftPos, windupTime);

        // 3) Slam – clamped so leg doesn’t over-extend
        Vector3 slamPos = ClampToLegReach(worldPos);
        if (killZone != null) killZone.enabled = true;
        yield return MoveTarget(liftPos, slamPos, slamTime);
        if (killZone != null) killZone.enabled = false;

        // 4) Recover back home
        yield return MoveTarget(target.position, home.position, recoverTime);

        busy = false;
    }
}
