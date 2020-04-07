using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Patada.Events;

[System.Serializable]
public class MaterialEventListener : TEventListener<Material, SCOB_MaterialEvent, MaterialUnityEvent, MaterialGameAction>{}