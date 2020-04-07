using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patada.Events;

namespace Patada.Actions {
    [CreateAssetMenu(fileName = "SCOBI_BaseAction_PlayMusic", menuName = "Patada/Actions/Base/Play Music", order = 1)]
    public class SCOB_BaseAction_PlayMusic : BaseGameAction {
        [SerializeField] private string value;

        public override void Do() {
            AudioManager.Instance.PlayNewMusic(value);
        }
    }
}