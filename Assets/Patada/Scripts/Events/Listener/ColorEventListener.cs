using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada.Events;

[System.Serializable]
public class ColorEventListener : TEventListener<Color, SCOB_ColorEvent, ColorUnityEvent, ColorGameAction> { }