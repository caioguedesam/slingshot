using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SCOB_EquipItem : SCOB_Item {
    [Space]
    [SerializeField] private bool equipped = false;
    public bool Equipped{
        get{
            return equipped;
        }
    }

    [Space]
    [SerializeField] private Patada.Events.SCOB_IntEvent equipEvent = null;
    public Patada.Events.SCOB_IntEvent EquipEvent {
        get{
            return equipEvent;
        }
    }

    public void Equip(){
        Debug.Log("Equip " + this.name);
        equipped = true;
    }

    public void Unequip(){
        Debug.Log("Unequip " + this.name);
        equipped = false;
    }
}
