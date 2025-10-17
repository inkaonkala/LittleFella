using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    public Image[] healthPieces;

    public int maxHealth = 5;
    private int currentHealth;
    public int CurrentHealth => currentHealth;

    //add System.events for momma
    public System.Action<int, int> OnHealthChange;
    public System.Action OnZeroHealth;
    public System.Action OnHealthUp;


    void Start()
    {
        currentHealth = Mathf.Clamp(maxHealth, 0, maxHealth);
        updateHealthUI();
        OnHealthChange?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        int prev = currentHealth;
        //currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);

        updateHealthUI();
        
        if (currentHealth != prev)
        {
            OnHealthChange?.Invoke(currentHealth, maxHealth);
            if (prev > 0 && currentHealth == 0)
                OnZeroHealth?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        int prev = currentHealth;
        //currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        updateHealthUI();

        if (currentHealth != prev)
        {
            OnHealthChange?.Invoke(currentHealth, maxHealth);
            if (prev == 0 && currentHealth > 0)
                OnHealthUp?.Invoke();
        }
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
