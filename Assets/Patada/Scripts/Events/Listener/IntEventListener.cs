using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada.Events;

[System.Serializable]
public class IntEventListener : TEventListener<int, SCOB_IntEvent, IntUnityEvent, IntGameAction> { }