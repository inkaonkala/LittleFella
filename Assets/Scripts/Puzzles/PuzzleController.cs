using UnityEngine;
using System.Linq;

public class PuzzleController : MonoBehaviour
{
    public LiftController lift;
    [Tooltip("Leave 0 to auto-detect from Shrooms under this Puzzle")]
    public int requiredKeys = 0;

    Bugs[] _bugs;
    bool _done;

    void Start()
    {
        // Only search under Puzzle1
        _bugs = GetComponentsInChildren<Bugs>(includeInactive: false);

        if (requiredKeys <= 0)
            requiredKeys = GetComponentsInChildren<Shrooms>(includeInactive: false).Length;

        Check();
    }

    public void NotifyBugPlaced(Bugs _)
    {
        Check();
    }

    void Check()
    {
        if (_done) return;

        int placed = _bugs.Count(b => b != null && b.isPlacedOnShroom);

        if (placed >= requiredKeys)
        {
            _done = true;
            if (lift) lift.Activate();
            Debug.Log($"Puzzle complete: {placed}/{requiredKeys}");
        }
        else
        {
            Debug.Log($"Puzzle progress: {placed}/{requiredKeys}");
        }
    }
}
