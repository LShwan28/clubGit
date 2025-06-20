using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 1f;
    public float maxStamina = 1f;
    public float staminaUsePerAction = 0.2f; //일단 기본적으로 행동할때 갈리는 값인데 나중에 값 세분화할때 따로따로할예정이라 임시

    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }

    void Awake()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    public bool UseStamina()
    {
        if (currentStamina >= staminaUsePerAction)
        {
            currentStamina -= staminaUsePerAction;
            CanvasManager.Instance.UpdateStaminaUI(currentStamina, maxStamina);
            return true;
        }
        return false;
    }

    public void RecoverStamina(float amount)
    {
        currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        CanvasManager.Instance.UpdateStaminaUI(currentStamina, maxStamina);
    }
}
