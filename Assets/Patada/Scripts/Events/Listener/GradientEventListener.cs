using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

[System.Serializable]
public class GradientEventListener : TEventListener<Gradient, SCOB_GradientEvent, GradientUnityEvent, GradientGameAction>{}