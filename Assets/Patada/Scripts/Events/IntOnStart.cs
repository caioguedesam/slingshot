using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada.Actions;

public class IntOnStart : OnStart {
    [SerializeField] private int intValue = 0;
    [SerializeField] private IntUnityEvent uActions = null;
    [SerializeField] private List<IntGameAction> actions = null;

    public override void Start() {
        uActions.Invoke(intValue);
        for(int i = 0 ; i < actions.Count ; i++) {
            actions[i].Do(intValue);
        }
    }
}