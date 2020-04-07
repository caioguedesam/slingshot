using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada.Actions {
    [CreateAssetMenu(fileName = "SCOBI_Action_AddCurrency_[NAME]", menuName = "Patada/Actions/Base/Add Currency", order = 1)]
    public class SCOB_Action_AddMatchCurrency : BaseGameAction {
        
        [SerializeField] private SCOB_Match_Bonus bonus = null;
        [SerializeField] private SCOB_Shop_Currency currency = null;
        public override void Do() {
            currency.Add(bonus.CristalBonus);
        }
    }
}
