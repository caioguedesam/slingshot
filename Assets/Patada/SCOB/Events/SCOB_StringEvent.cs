using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_StringEvent_Name", menuName = "Patada/Events/New<string>", order = 1)]
    [System.Serializable]
    public class SCOB_StringEvent : SCOB_TEvent<string> {}
}