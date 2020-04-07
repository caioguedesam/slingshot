using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

[System.Serializable]
public class GOEventListener : TEventListener<GameObject, SCOB_GOEvent, GOUnityEvent, GOGameAction>{}