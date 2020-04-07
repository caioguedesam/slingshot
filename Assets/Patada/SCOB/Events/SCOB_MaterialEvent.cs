using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_MaterialEvent_Name", menuName = "Patada/Events/New<Material>", order = 1)]
    [System.Serializable]
    public class SCOB_MaterialEvent : SCOB_TEvent<Material> {

    }
}