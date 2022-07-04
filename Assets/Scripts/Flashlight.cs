using UnityEngine;

public class Flashlight : MonoBehaviour, IEquipable
{
    [SerializeField] private new Light light;
    private bool _lightIsOn = false;
    private bool _itemIsEquipped = false;
    
    private Equipment _equippedBy;
    private Slots _currentSlot = Slots.NONE;
    private IEquipable _equipableImplementation;

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
        _lightIsOn = !_lightIsOn;
        light.enabled = _lightIsOn;
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
