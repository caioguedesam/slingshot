using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada;
using Patada.Events;

[CreateAssetMenu(fileName = "SCOBI_Shop_Currency", menuName = "Patada/Shop/Currency", order = 1)]
[System.Serializable]
public class SCOB_Shop_Currency : SCOB_TextVariable {
    [SerializeField] private string currencyName = "";
    [Space]
    [SerializeField] private SCOB_BaseEvent success = null;
    [SerializeField] private SCOB_BaseEvent failed = null;

    public string Name{
        get{
            return currencyName;
        }
    }

    [SerializeField] private SafeInt quantity = null;
    public int Value{
        get{
            if(quantity == null || quantity.Value == -1) {
                quantity = new SafeInt(Save.Instance.GetInt(this.Name +"_quantity", 0));
            }

            return quantity.Value;
        }
    }

    public override string StringValue(string key) {
        return Value.ToString();
    }

    public void Add(int amount){
        quantity = new SafeInt(amount+Value);
        Save.Instance.SetInt(this.Name +"_quantity", Value);
    }

    public void Remove(int amount) {
        if(Value >= amount) {
            quantity = new SafeInt(Value-amount);
            Save.Instance.SetInt(this.Name +"_quantity", Value);
        }
    }
}
