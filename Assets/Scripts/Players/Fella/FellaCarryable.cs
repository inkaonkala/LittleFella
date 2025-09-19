using UnityEngine;

public class FellaCarryable : MonoBehaviour
{
    public bool IsCarried { get; private set; }

    [Tooltip("Disable these while carried (e.g., FellaMove, Animator if you want).")]
    public MonoBehaviour[] movementScriptsToDisable;

    Rigidbody2D _rb;
    Collider2D  _col;
    Transform   _originalParent;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _originalParent = transform.parent;
    }

    public void BeginCarry(Transform parent)
    {
        if (IsCarried) return;
        IsCarried = true;

        // freeze physics & input while carried
        if (_rb) _rb.simulated = false;
        if (_col) _col.enabled = false;
        foreach (var m in movementScriptsToDisable)
            if (m) m.enabled = false;

        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }

    public void EndCarry(Vector2 inheritVelocity)
    {
        if (!IsCarried) return;
        IsCarried = false;

        transform.SetParent(_originalParent);

        if (_rb)
        {
            _rb.simulated = true;
            _rb.linearVelocity = inheritVelocity; // gives a nice hand-off when dropping
        }
        if (_col) _col.enabled = true;

        foreach (var m in movementScriptsToDisable)
            if (m) m.enabled = true;
    }
}


