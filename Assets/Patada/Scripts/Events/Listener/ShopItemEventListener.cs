using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada.Events;

[System.Serializable]
public class ShopItemEventListener : TEventListener<SCOB_Shop_Item, SCOB_ShopItemEvent, ShopItemUnityEvent, ShopItemGameAction> { }