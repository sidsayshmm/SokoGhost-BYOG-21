using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderItem : LRCompatibleMenuItem {

    private Slider slider;

    private void Awake() {
        slider = GetComponent<Slider>();
        slider.wholeNumbers = true;
    }

    public override void SendLRInput(Vector2Int direction) {
        slider.value += direction.x;
    }

}
