using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

[System.Serializable]
public class Vec3EventListener : TEventListener<Vector3, SCOB_Vec3Event, Vec3UnityEvent, Vec3GameAction>{}