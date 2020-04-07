using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

[System.Serializable]
public class SwipeEventListener : TEventListener<Swipe, SCOB_SwipeEvent, SwipeUnityEvent, SwipeGameAction> { }
