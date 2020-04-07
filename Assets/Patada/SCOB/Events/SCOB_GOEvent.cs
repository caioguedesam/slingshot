using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_GOEvent_Name", menuName = "Patada/Events/New<Game Object>", order = 1)]
    [System.Serializable]
    public class SCOB_GOEvent : SCOB_TEvent<GameObject> {}
}