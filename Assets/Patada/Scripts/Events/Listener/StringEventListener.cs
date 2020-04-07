using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

[System.Serializable]
public class StringEventListener : TEventListener<string, SCOB_StringEvent, StringUnityEvent, StringGameAction>{}