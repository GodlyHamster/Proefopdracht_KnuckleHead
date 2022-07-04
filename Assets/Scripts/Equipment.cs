using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private InputActionAsset playerInput;
    [SerializeField] private Equipmentslot[] _slots;

    private float dropForce = 5;

    private IEquipable _currentEquipable;

    private void Awake()
    {
        playerInput.Enable();
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        
        var useLeftItem = playerInput["UseLeftHand"];
        useLeftItem.performed += PerformLeftUse;
        var useRightItem = playerInput["UseRightHand"];
        useRightItem.performed += PerformRightUse;
        var drop = playerInput["DropItem"];
        drop.performed += DropItem;
        var interact = playerInput["Interact"];
        interact.performed += DoEquip;
        var mode = playerInput["SwitchFiremode"];
        mode.performed += SwitchMode;
    }

    private void Update()
    {
        GameObject hitObject;
        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit))
        {
            hitObject = hit.transform.gameObject;

            if (hitObject.GetComponent(typeof(IEquipable)))
            {
                _currentEquipable = hitObject.GetComponent<IEquipable>();
            }
            else
            {
                _currentEquipable = null;
            }
        }
    }

    private void PerformLeftUse(InputAction.CallbackContext obj)
    {
        if (_slots[0].heldItem == null) return;
        
        if (playerInput["UseLeftHand"].IsPressed() && _slots[0].heldItem.GetComponent<IEquipable>().GetUseMode() == UseMode.CONTINIOUS)
        {
            StartCoroutine(HoldLeft());
        }
        else
        {
            UseItem(Slots.LEFTHAND);
        }
    }

    private IEnumerator HoldLeft()
    {
        if (_slots[0].heldItem == null) yield return new WaitForSeconds(0.1f);
        UseItem(Slots.LEFTHAND);
        
        float itemCooldown = _slots[0].heldItem.GetComponent<IEquipable>().GetUseModeCooldown();
        
        yield return new WaitForSeconds(itemCooldown);
        if (playerInput["UseLeftHand"].IsPressed())
        {
            StartCoroutine(HoldLeft());
        }
    }
    
    private void PerformRightUse(InputAction.CallbackContext obj)
    {
        if (_slots[1].heldItem == null) return;
        if (playerInput["UseRightHand"].IsPressed() && _slots[1].heldItem.GetComponent<IEquipable>().GetUseMode() == UseMode.CONTINIOUS)
        {
            StartCoroutine(HoldRight());
        }
        else
        {
            UseItem(Slots.RIGHTHAND);
        }
    }
    
    private IEnumerator HoldRight()
    {
        if (_slots[1].heldItem == null) yield return new WaitForSeconds(0.1f);
        UseItem(Slots.RIGHTHAND);
        
        float itemCooldown = _slots[1].heldItem.GetComponent<IEquipable>().GetUseModeCooldown();
        
        yield return new WaitForSeconds(itemCooldown);
        if (playerInput["UseRightHand"].IsPressed())
        {
            StartCoroutine(HoldRight());
        }
    }

    private void UseItem(Slots slot)
    {
        _slots[(int)slot].heldItem.GetComponent<IEquipable>().Use();
    }
    
    private void DropItem(InputAction.CallbackContext obj)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].heldItem != null)
            {
                GameObject handItem = _slots[i].heldItem;
                _slots[i].heldItem = null;
                
                handItem.gameObject.transform.parent = null;
                if (handItem.GetComponent<Rigidbody>())
                {
                    Rigidbody rb = handItem.GetComponent<Rigidbody>();
                    rb.isKinematic = false;
                }
                handItem.GetComponent<IEquipable>().Drop();

                _currentEquipable = null;
                break;
            }
        }
    }

    private void DoEquip(InputAction.CallbackContext obj)
    {
        if (_currentEquipable == null || _currentEquipable.isEquipped()) return;

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].heldItem == null)
            {
                _slots[i].heldItem = _currentEquipable.Pickup(GetComponent<Equipment>(), _slots[i].slot);
                GameObject handItem = _slots[i].heldItem;
                
                if (handItem.GetComponent<Rigidbody>())
                    handItem.GetComponent<Rigidbody>().isKinematic = true;
                handItem.transform.parent = _slots[i].PosAnchor;
                handItem.transform.localPosition = new Vector3(0, 0, 0);
                handItem.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            }
        }
    }
    
    private void SwitchMode(InputAction.CallbackContext obj)
    {
        foreach (var item in _slots)
        {
            if (item.heldItem == null) continue;
            if (!item.heldItem.GetComponent(typeof(Gun))) continue;
            
            item.heldItem.GetComponent<Gun>().SwitchUseMode();
        }
    }

    public void EquipFromToSlot(Slots fromSlot, Slots toSlot)
    {
        if (_slots[(int) toSlot].heldItem != null) return;
        
        _slots[(int) toSlot].heldItem = _slots[(int) fromSlot].heldItem;
        _slots[(int) fromSlot].heldItem = null;
        
        GameObject toSlotObj = _slots[(int) toSlot].heldItem;
        
        toSlotObj.transform.parent = _slots[(int) toSlot].PosAnchor;
        toSlotObj.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void DropSlot(Slots slot)
    {
        GameObject itemToDrop = _slots[(int)slot].heldItem;
        _slots[(int)slot].heldItem = null;
                
        itemToDrop.gameObject.transform.parent = null;
        if (itemToDrop.GetComponent<Rigidbody>())
        {
            Rigidbody rb = itemToDrop.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
        itemToDrop.GetComponent<IEquipable>().Drop();
    }

    public Equipmentslot[] GetEquipment()
    {
        return _slots;
    }
}
