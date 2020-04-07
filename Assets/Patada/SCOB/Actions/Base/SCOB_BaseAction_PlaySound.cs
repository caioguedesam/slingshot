using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patada.Events;

namespace Patada.Actions {
    [CreateAssetMenu(fileName = "SCOBI_BaseAction_PlaySound", menuName = "Patada/Actions/Base/Play SFX", order = 1)]
    public class SCOB_BaseAction_PlaySound : BaseGameAction {
        [SerializeField] private string value;
        public override void Do() {
            AudioManager.Instance.PlaySound(value);
        }
    }
}