using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ColorChanger : MonoBehaviour {
    [Space]
    [SerializeField] private Color defaultColor = Color.white;

    [Space]
    [SerializeField] private float fadeTime = .2f;
    [SerializeField] private EasingFunction.Ease fadeEasing = EasingFunction.Ease.EaseInOutSine;

    private SpriteRenderer spriteRendTarget = null;
    private Image imageTarget = null;
    private TextMeshProUGUI textMeshProUGUI = null;
    private TextMeshPro textMeshPro = null;

    private Coroutine fadeCoroutine = null;

    void Awake() {
        spriteRendTarget = GetComponent<SpriteRenderer>();
        imageTarget = GetComponent<Image>();
    }

    public void FadeToDefault() {
        FadeToColor(defaultColor, fadeTime, fadeEasing);
    }
    
    public void FadeToColor(string data) {
        var tokens = data.Split(',');
        if(tokens.Length == 1) {
            FadeToColor(HexToColor(tokens[0]), fadeTime, fadeEasing);
        } else if(tokens.Length == 2) {
            FadeToColor(HexToColor(tokens[0]), float.Parse(tokens[1]), fadeEasing);
        } else if(tokens.Length == 3){
            Debug.Log(tokens[2] + " " + this.name);
            var ease = (EasingFunction.Ease)(System.Enum.Parse(typeof(EasingFunction.Ease), tokens[2]));
            FadeToColor(HexToColor(tokens[0]), float.Parse(tokens[1]), ease);
        }
    }

    public void FadeToColor(Color c, float duration, EasingFunction.Ease easing) {    
        if(fadeCoroutine != null) {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToColorHandler(c, duration, easing));
    }

    private IEnumerator FadeToColorHandler(Color targetColor, float duration, EasingFunction.Ease easing) {
        var easingFunc = EasingFunction.GetEasingFunction(easing);
        var currentColor = GetCurrentColor();
        var elapsedTime = 0f;
        
        Debug.Log(targetColor + " " + duration + " " + easing);
        while(elapsedTime < duration){
            var percentage = elapsedTime / duration;

            var tempColor = new Color(
                easingFunc.Invoke(currentColor.r, targetColor.r, percentage),
                easingFunc.Invoke(currentColor.g, targetColor.g, percentage),
                easingFunc.Invoke(currentColor.b, targetColor.b, percentage),
                easingFunc.Invoke(currentColor.a, targetColor.a, percentage)
            );

            SetColor(tempColor);

            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
        }

        SetColor(targetColor);
    }

    public void SetColor(Color c) {
        if(spriteRendTarget != null) {
            spriteRendTarget.color = c;
        }
        if(imageTarget != null) {
            imageTarget.color = c;
        }
        if(textMeshProUGUI != null) {
            textMeshProUGUI.color = c;
        }
        if(textMeshPro != null) {
            textMeshPro.color = c;
        }
    }

    public void SetDefaultColor() {
        SetColor(defaultColor);
    }

    public void SetColor(string hexValue) {
        var colorValue = Color.white;

        if(!hexValue[0].Equals('#')) {
            hexValue = "#" + hexValue;
        }

        var colorHex = hexValue.Substring(0,Mathf.Min(7,hexValue.Length));

        if(!ColorUtility.TryParseHtmlString(colorHex, out colorValue)){
            colorValue = Color.white;
        }

        if(hexValue.Length == 8) {
            colorValue.a = int.Parse(hexValue.Substring(6,2), System.Globalization.NumberStyles.HexNumber) / 255f;
        }

        SetColor(colorValue);
    }

    private Color GetCurrentColor() {
        if(spriteRendTarget != null) {
            return spriteRendTarget.color;
        }
        if(imageTarget != null) {
            return imageTarget.color;
        }
        if(textMeshProUGUI != null) {
            return textMeshProUGUI.color;
        }
        if(textMeshPro != null) {
            return textMeshPro.color;
        }

        return Color.white;
    }

    

    public static Color HexToColor(string hexValue) {
        var colorValue = Color.white;

        if(!hexValue[0].Equals('#')){
            hexValue = hexValue.Insert(0,"#");
        }

        if(!ColorUtility.TryParseHtmlString(hexValue.ToLower().Substring(0,7), out colorValue)){
            colorValue = Color.white;
        }
        

        if(hexValue.Length == 8) {
            colorValue.a = int.Parse(hexValue.Substring(hexValue.Length-2,2), System.Globalization.NumberStyles.HexNumber) / 255f;
        }

        return colorValue;
    }

}
