using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultipleTargetButton : Button {
    [SerializeField] private List<Graphic> otherGraphics = new List<Graphic>();

    protected override void DoStateTransition(SelectionState state, bool instant) {
        base.DoStateTransition(state, instant);

        Color tintColor;
        Sprite transitionSprite;
        string triggerName;

        switch (state) {
            case SelectionState.Normal:
                tintColor = colors.normalColor;
                transitionSprite = null;
                triggerName = animationTriggers.normalTrigger;
                break;
            case SelectionState.Highlighted:
                tintColor = colors.highlightedColor;
                transitionSprite = spriteState.highlightedSprite;
                triggerName = animationTriggers.highlightedTrigger;
                break;
            case SelectionState.Pressed:
                tintColor = colors.pressedColor;
                transitionSprite = spriteState.pressedSprite;
                triggerName = animationTriggers.pressedTrigger;
                break;
            case SelectionState.Disabled:
                tintColor = colors.disabledColor;
                transitionSprite = spriteState.disabledSprite; ;
                triggerName = animationTriggers.disabledTrigger;
                break;
            default:
                tintColor = Color.black;
                transitionSprite = null;
                triggerName = string.Empty;
                break;
        }

        if (gameObject.activeInHierarchy) {
            StartColorTween(tintColor * colors.colorMultiplier, instant);
        }
    }

    void StartColorTween(Color targetColor, bool instant) {
        if (otherGraphics == null)
            return;

        foreach (var graphic in otherGraphics) {
            graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }
    }
}
