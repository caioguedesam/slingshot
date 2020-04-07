using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_ColorEvent_Name", menuName = "Patada/Events/New<Color>", order = 1)]
    [System.Serializable]
    public class SCOB_ColorEvent : SCOB_TEvent<Color> {}
}