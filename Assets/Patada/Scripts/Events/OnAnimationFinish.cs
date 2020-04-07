using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnAnimationFinish : MonoBehaviour {
    [SerializeField] private Animator targetAnimator = null;
    [SerializeField] private float animationTime = 1f;
    [SerializeField] private int animationLayer = 0;
    [SerializeField] private string animationStateName = "";
    [Space]
    [SerializeField] private UnityEvent events = null;
    [SerializeField] private List<BaseGameAction> actions = null;
    private bool finished = false;

    void OnEnable() {
        finished = false;
    }

    void Update() {
        if (!finished && targetAnimator.GetCurrentAnimatorStateInfo(animationLayer).IsName(animationStateName) && targetAnimator.GetCurrentAnimatorStateInfo(animationLayer).normalizedTime >= animationTime) {
            finished = true;
            events.Invoke();
            foreach(var action in actions){
                action.Do();
            }
        }
    }
}
