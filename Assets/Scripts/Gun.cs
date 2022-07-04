using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour, IEquipable
{
    [SerializeField] private InputActionAsset playerInput;
    
    private bool _itemIsEquipped = false;
    [SerializeField] private float fireCooldown = 1f;
    [SerializeField] private int magazineMax = 8;
    private int _ammo;

    private Equipment _equippedBy;
    private Slots _currentSlot = Slots.NONE;
    private UseMode _firemode;

    private void Start()
    {
        _ammo = magazineMax;
        _firemode = UseMode.SINGLE;
    }

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
        if (_ammo > 0) Shoot();
    }

    public UseMode GetUseMode()
    {
        return _firemode;
    }

    public float GetUseModeCooldown()
    {
        return fireCooldown;
    }

    public void Drop()
    {
        _equippedBy = null;
        _currentSlot = Slots.NONE;
        _itemIsEquipped = false;
    }

    public void Shoot()
    {
        _ammo -= 1;
        Debug.Log("You have " + _ammo + " ammo left");
    }

    public int Reload(int ammo)
    {
        if (ammo <= 0) return 0;
        _ammo += ammo;
        if (_ammo > magazineMax) _ammo = magazineMax;
        return _ammo - magazineMax;
    }

    public void SwitchUseMode()
    {
        if (_firemode == UseMode.SINGLE)
        {
            _firemode = UseMode.CONTINIOUS;
        }
        else
        {
            _firemode = UseMode.SINGLE;
        }
        Debug.Log("Switched firemode to " + _firemode);
    }
}
