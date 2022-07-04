using UnityEngine;

public interface IEquipable
{
    bool isEquipped();
    GameObject Pickup(Equipment equippedBy, Slots slot);
    void Use();
    UseMode GetUseMode();
    float GetUseModeCooldown();
    void Drop();
}
