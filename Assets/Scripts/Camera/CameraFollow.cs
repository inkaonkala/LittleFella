using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                 // assign Fella in Inspector
    [Header("Offset from target (world units)")]
    public Vector2 followOffsetFromTarget = Vector2.zero;

    [Header("Dead-Zone (half extents)")]
    public Vector2 deadZone = new Vector2(2f, 1f);

    [Header("Smoothing")]
    public float smoothTime = 0.2f;

    [Header("Zoom")]
    public float defaultOrthoSize = 5f;

    // runtime
    private Vector3 _vel;
    private Camera _cam;

    // active (can be overridden by zones)
    private Vector2 _activeDeadZone;
    private float _activeSmooth;
    private float _activeOrtho;

    // defaults for reset
    private Transform _defaultTarget;
    private Vector2 _defaultOffset;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _activeDeadZone = deadZone;
        _activeSmooth = smoothTime;
        _activeOrtho = defaultOrthoSize;
        _cam.orthographicSize = _activeOrtho;

        _defaultTarget = target;
        _defaultOffset = followOffsetFromTarget;
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 camPos = transform.position;

        // apply offset so we can "aim higher"
        Vector3 tPos = target.position + (Vector3)followOffsetFromTarget;

        // dead-zone follow
        if (tPos.x < camPos.x - _activeDeadZone.x) camPos.x = tPos.x + _activeDeadZone.x;
        else if (tPos.x > camPos.x + _activeDeadZone.x) camPos.x = tPos.x - _activeDeadZone.x;

        if (tPos.y < camPos.y - _activeDeadZone.y) camPos.y = tPos.y + _activeDeadZone.y;
        else if (tPos.y > camPos.y + _activeDeadZone.y) camPos.y = tPos.y - _activeDeadZone.y;

        // smooth
        Vector3 targetCamPos = new Vector3(camPos.x, camPos.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetCamPos, ref _vel, _activeSmooth);
    }

    // ---------- Public API used by CameraTrigger ----------
    public void SetProfile(Vector2 newDeadZone, float newOrtho, float newSmooth, float blendTime)
    {
        StopAllCoroutines();
        StartCoroutine(BlendProfile(newDeadZone, newOrtho, newSmooth, blendTime));
    }

    public void ResetProfile(float blendTime)
    {
        SetProfile(deadZone, defaultOrthoSize, smoothTime, blendTime);
    }

    private IEnumerator BlendProfile(Vector2 dz, float ortho, float sm, float time)
    {
        Vector2 dz0 = _activeDeadZone;
        float o0 = _cam.orthographicSize;
        float s0 = _activeSmooth;
        float tt = 0f;

        if (time <= 0f)
        {
            _activeDeadZone = dz;
            _activeSmooth = sm;
            _cam.orthographicSize = ortho;
            yield break;
        }

        while (tt < time)
        {
            float k = tt / time;
            _activeDeadZone = Vector2.Lerp(dz0, dz, k);
            _activeSmooth   = Mathf.Lerp(s0, sm, k);
            _cam.orthographicSize = Mathf.Lerp(o0, ortho, k);
            tt += Time.deltaTime;
            yield return null;
        }

        _activeDeadZone = dz;
        _activeSmooth = sm;
        _cam.orthographicSize = ortho;
    }

    public void SetTarget(Transform newTarget) { target = newTarget; }
    public void ResetTarget() { target = _defaultTarget; }

    public void SetOffset(Vector2 newOffset) { followOffsetFromTarget = newOffset; }
    public void ResetOffset() { followOffsetFromTarget = _defaultOffset; }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!_cam) _cam = GetComponent<Camera>();
        Gizmos.color = new Color(1, 0.6f, 0.1f, 0.7f);
        Vector3 c = transform.position;
        Gizmos.DrawWireCube(new Vector3(c.x, c.y, 0), new Vector3(deadZone.x * 2f, deadZone.y * 2f, 0));
    }
#endif
}
