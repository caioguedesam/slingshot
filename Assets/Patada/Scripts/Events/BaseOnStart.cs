using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Patada.Actions;

public class BaseOnStart : OnStart {
    [SerializeField] private UnityEvent uActions = null;
    [SerializeField] private List<GameAction> actions = null;

    public override void Start() {
        uActions.Invoke();
        for(int i = 0 ; i < actions.Count ; i++) {
            actions[i].Do();
        }
    }
}