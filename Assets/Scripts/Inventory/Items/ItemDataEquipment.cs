using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Major Stats")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critDamage;

    [Header("Defensive Stats")]
    public int maxHealth;
    public int armor;
    public int evasion;

    public void AddModifiers()
    {
        PlayerStats stats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        stats.strength.AddModifier(strength);
        stats.agility.AddModifier(agility);
        stats.intelligence.AddModifier(intelligence);
        stats.vitality.AddModifier(vitality);
        
        stats.damage.AddModifier(damage);
        stats.critChance.AddModifier(critChance);
        stats.critDamage.AddModifier(critDamage);
        
        stats.maxHealth.AddModifier(maxHealth);
        stats.armor.AddModifier(armor);
        stats.evasion.AddModifier(evasion);
    }

    public void RemoveModifiers()
    {
        PlayerStats stats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        stats.strength.RemoveModifier(strength);
        stats.agility.RemoveModifier(agility);
        stats.intelligence.RemoveModifier(intelligence);
        stats.vitality.RemoveModifier(vitality);

        stats.damage.RemoveModifier(damage);
        stats.critChance.RemoveModifier(critChance);
        stats.critDamage.RemoveModifier(critDamage);

        stats.maxHealth.RemoveModifier(maxHealth);
        stats.armor.RemoveModifier(armor);
        stats.evasion.RemoveModifier(evasion);
    }
}
