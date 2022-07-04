using UnityEngine;

public class Ammoclip : MonoBehaviour, IEquipable
{
    private bool _itemIsEquipped = false;
    [SerializeField] private int _clipAmmo = 8;

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
        foreach (var item in _equippedBy.GetEquipment())
        {
            if (item.heldItem.GetComponent(typeof(Gun)))
            {
                _clipAmmo = item.heldItem.GetComponent<Gun>().Reload(_clipAmmo);
                Debug.Log("clip has " + _clipAmmo + " ammo left");
                break;
            }
        }
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
