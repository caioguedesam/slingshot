using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patada;

[CreateAssetMenu(fileName = "SCOBI_Match_Bonus_", menuName = "Patada/Match/Bonus", order = 1)]
public class SCOB_Match_Bonus : ScriptableObject {
    [SerializeField] private SafeInt cristalBonus = null;
    public int CristalBonus {
        get{
            return cristalBonus.Value;
        }
    }
}
