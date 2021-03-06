﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada.Actions {
    [CreateAssetMenu(fileName = "SCOBI_IntAction_AddCurrency_[NAME]", menuName = "Patada/Actions/Int/Add Currency", order = 1)]
    public class SCOB_IntAction_AddCurrency : IntGameAction {
        [SerializeField] private SCOB_Shop_Currency currency = null;
        public override void Do(int value) {
            currency.Add(value);
        }
    }
}
