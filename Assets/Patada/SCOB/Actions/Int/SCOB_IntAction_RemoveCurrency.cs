using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada.Actions {
    [CreateAssetMenu(fileName = "SCOBI_IntAction_RemoveCurrency_[NAME]", menuName = "Patada/Actions/Int/Remove Currency", order = 2)]
    public class SCOB_IntAction_RemoveCurrency : IntGameAction {
        [SerializeField] private SCOB_Shop_Currency currency = null;
        public override void Do(int value) {
            currency.Remove(value);
        }
    }
}