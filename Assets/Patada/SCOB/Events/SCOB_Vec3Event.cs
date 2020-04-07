using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Patada.Events {
    [CreateAssetMenu(fileName = "SCOBI_Vec3Event_Name", menuName = "Patada/Events/New<Vector3>", order = 1)]
    [System.Serializable]
    public class SCOB_Vec3Event : SCOB_TEvent<Vector3> {}
}