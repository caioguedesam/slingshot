using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_ShopItemEvent_Name", menuName = "Patada/Events/New<ShopItem>", order = 1)]
    [System.Serializable]
    public class SCOB_ShopItemEvent : SCOB_TEvent<SCOB_Shop_Item> {

    }
}