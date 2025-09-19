using UnityEngine;

public class Shrooms : MonoBehaviour
{
    public ColorType color;
    public Transform bugHangPoint;

    private PuzzleController _puzzle;

    void Awake()
    {
        _puzzle = GetComponentInParent<PuzzleController>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.attachedRigidbody) return;

        var bug = other.GetComponent<Bugs>();
        if (bug == null) return;

        if (!bug.isCarried && !bug.isPlacedOnShroom && bug.color == color)
        {
            bug.LockToShroom(bugHangPoint ? bugHangPoint : transform);
            if (_puzzle) _puzzle.NotifyBugPlaced(bug);
        }
    }
}
