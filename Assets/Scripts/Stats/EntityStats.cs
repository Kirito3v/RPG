using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [Header("Major Stats")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;

    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critDamage;

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;


    [SerializeField] private int currentHealth;

    public System.Action OnHealthChanged;

    protected virtual void Start()
    {
        currentHealth = GetMaxHealthValue();
        critDamage.SetDefaultValue(150);
    }

    public virtual void DoDamage(EntityStats entity)
    {
        if (AvoidAttack(entity))
            return;

        int totaldamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
            totaldamage = CalculateCritDamage(totaldamage);

        totaldamage = CheckArmor(entity, totaldamage);
        entity.TakeDamage(totaldamage);

        Debug.Log(GetComponent<Entity>().name + $" Deal {totaldamage}");
    }

    public virtual void TakeDamage(int dmg)
    {
        DecreaseHealthBy(dmg);

        GetComponent<Entity>().fx.FlashFX().Forget();

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void DecreaseHealthBy(int dmg)
    {
        currentHealth -= dmg;

        if (OnHealthChanged != null)
            OnHealthChanged();
    }

    protected virtual void Die()
    {

    }
    
    private int CheckArmor(EntityStats entity, int totaldamage)
    {
        totaldamage -= entity.armor.GetValue();
        totaldamage = Mathf.Clamp(totaldamage, 0, int.MaxValue);
        return totaldamage;
    }
    
    private bool AvoidAttack(EntityStats entity)
    {
        int totalEvasion = entity.evasion.GetValue() + entity.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log(GetComponent<Entity>().name + " Attack Missed");
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritChance)
        {
            Debug.Log(GetComponent<Entity>().name + " Crit Hit");
            return true;
        }
        return false;
    }

    private int CalculateCritDamage(int dmg)
    {
        float totalCritDamage = (critDamage.GetValue() + strength.GetValue()) * 0.01f;
        float totalDamage = dmg * totalCritDamage;

        return Mathf.RoundToInt(totalDamage);
    }

    public int GetCurrentHealthValue() => currentHealth;

    public int GetMaxHealthValue() => maxHealth.GetValue() + vitality.GetValue() * 5;
}
