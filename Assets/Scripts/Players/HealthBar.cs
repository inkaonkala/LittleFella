using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    public Image healthFill;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        updateHealthUI();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); //keeps the value between min and max (0 - 100)
     
       updateHealthUI();
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        updateHealthUI();
    }

    private void updateHealthUI()
    {
        if (healthFill != null)
        {
            healthFill.fillAmount = currentHealth / maxHealth;
        }
    }

    void Update()
    {
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            TakeDamage(40f);
        }
    }

}
