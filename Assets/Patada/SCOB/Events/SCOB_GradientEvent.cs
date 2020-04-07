using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_GradientEvent_Name", menuName = "Patada/Events/New<Gradient>", order = 1)]
    [System.Serializable]
    public class SCOB_GradientEvent : SCOB_TEvent<Gradient> {}
}