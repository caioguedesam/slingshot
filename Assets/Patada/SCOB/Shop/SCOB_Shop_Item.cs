using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SCOB_Shop_Item : SCOB_TextVariable {
    [Space]
    [SerializeField] private SCOB_Item item;
    public SCOB_Item Item {
        get{
            return item;
        }
    }

    [Space]
    [Tooltip("Which tab it will be shown in shop")] [SerializeField] protected int shopTab;
    public int ShopTab{
        get{
            return shopTab;
        }
    }

    [Tooltip("The position this item will appear in the list")] [SerializeField] protected int orderInShop;
    public int OrderInShop{
        get{
            return orderInShop;
        }
    }
    
    [Space]
    [SerializeField] protected int cost;
    public int Cost {
        get {
            return cost;
        }
    }

    [Space]
    [SerializeField] protected bool allowRebuy;
    [SerializeField] protected int boughtCount;

    [Space]
    [SerializeField] protected SCOB_Shop_Currency currency;
    public SCOB_Shop_Currency Currency {
        get {
            return currency;
        }
    }

    public bool CanRebuy {
        get {
            return allowRebuy;
        }
    }

    public bool AlreadyBought {
        get {
            return Save.Instance.GetBool(this.item.Name+"_bought", boughtCount != 0);
        }
    }

    public Sprite GetIcon(Icon.Size size) {
        return item.GetIcon(size);
    }

    public virtual void Bought() {
        Save.Instance.SetBool(this.item.Name+"_bought", true);
    }

    public override string StringValue(string key) {
        if (key.ToLower().Equals("cost")) {
            return cost.ToString();
        } else {
            return item.StringValue(key);
        }
    }
}
