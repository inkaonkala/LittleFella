using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    public Image[] healthPieces;

    public int maxHealth = 5;
    private int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
        updateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
     
        updateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        updateHealthUI();
    }

    private void updateHealthUI()
    {
        for (int i = 0; i < healthPieces.Length; i++)
        {
            if (healthPieces[i] != null)
                healthPieces[i].enabled = i < currentHealth;
        }
    }

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(1);
        }
    }

}
