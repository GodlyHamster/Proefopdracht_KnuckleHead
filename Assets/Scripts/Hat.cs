using UnityEngine;

public class Hat : MonoBehaviour, IEquipable
{
    private bool _itemIsEquipped = false;
    
    private Equipment _equippedBy;
    private Slots _currentSlot = Slots.NONE;

    public bool isEquipped()
    {
        return _itemIsEquipped;
    }

    public GameObject Pickup(Equipment equippedBy, Slots slot)
    {
        _equippedBy = equippedBy;
        _currentSlot = slot;
        _itemIsEquipped = true;
        return this.gameObject;
    }

    public void Use()
    {
        _equippedBy.EquipFromToSlot(_currentSlot, Slots.HEAD);
    }

    public UseMode GetUseMode()
    {
        return UseMode.SINGLE;
    }

    public float GetUseModeCooldown()
    {
        return 1f;
    }

    public void Drop()
    {
        _equippedBy = null;
        _currentSlot = Slots.NONE;
        _itemIsEquipped = false;
    }
}
