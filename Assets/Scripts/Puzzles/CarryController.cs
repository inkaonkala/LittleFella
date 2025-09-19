using UnityEngine;
using UnityEngine.InputSystem;

public class CarryController : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference pickNDrop;   // drag your action here

    [Header("Carry Settings")]
    public Transform carryPoint;
    public float pickupRange = 1.1f;
    public LayerMask bugLayer;

    private Bugs _carried;

    private void OnEnable()
    {
        if (pickNDrop != null)
        {
            pickNDrop.action.performed += OnPickNDrop;
            pickNDrop.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (pickNDrop != null)
        {
            pickNDrop.action.performed -= OnPickNDrop;
            pickNDrop.action.Disable();
        }
    }

    private void OnPickNDrop(InputAction.CallbackContext ctx)
    {
        // Drop if already carrying
        if (_carried != null)
        {
            _carried.Drop();
            _carried = null;
            return;
        }

        // Try to pick nearest bug
        Vector2 center = (carryPoint != null) ? (Vector2)carryPoint.position
                                              : (Vector2)transform.position;

        var hits = Physics2D.OverlapCircleAll(center, pickupRange, bugLayer);

        float best = float.MaxValue;
        Bugs bestBug = null;

        foreach (var h in hits)
        {
            var bu = h.GetComponent<Bugs>();
            if (bu == null || bu.isPlacedOnShroom || bu.isCarried) continue;

            float d = Vector2.Distance(center, bu.transform.position);
            if (d < best) { best = d; bestBug = bu; }
        }

        if (bestBug != null)
        {
            // NOTE: your Bugs script method is PickUP (capital UP)
            bestBug.PickUp(carryPoint != null ? carryPoint : transform);
            _carried = bestBug;
        }
    }

}
