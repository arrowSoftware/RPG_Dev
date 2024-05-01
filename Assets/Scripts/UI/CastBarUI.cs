using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastBarUI : MonoBehaviour
{
    Slider castBarSliderUI;
    float castBarMaxTime = 0;
    float castBarCurrentTime = 0;
    bool startCast = false;

    public void StartCastbar(float maxTime) {
        castBarMaxTime = maxTime;
        castBarSliderUI.value = 0;
        castBarCurrentTime = 0;
        startCast = true;
   }

    public void StopCastbar() {
        castBarMaxTime = 0.0f;
        castBarSliderUI.value = 0;
        startCast = false;
    }

    void Start() {
        castBarSliderUI = transform.GetChild(0).GetComponent<Slider>();
    }

    void Update() {
        if (startCast) {
            float sliderValue = castBarCurrentTime / castBarMaxTime;
            castBarCurrentTime += Time.deltaTime;
            if (castBarCurrentTime > castBarMaxTime) {
                StopCastbar();
            } else {
                castBarSliderUI.value = sliderValue;
            }
        }
    }
}
