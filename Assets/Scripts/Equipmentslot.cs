using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Equipmentslot
{
    public Slots slot = Slots.LEFTHAND;
    public GameObject heldItem;
    public Transform PosAnchor;
}
