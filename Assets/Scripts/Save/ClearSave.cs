using UnityEngine;
using UnityEngine.InputSystem; // NEW input system

public class ClearSaveHotkey : MonoBehaviour
{
    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            SaveManager.Clear();
            Debug.Log("💾 Save cleared (with f10)!");
        }
    }
}
