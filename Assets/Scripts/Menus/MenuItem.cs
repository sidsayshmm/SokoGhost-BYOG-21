using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuItem : MonoBehaviour {

    public GameObject highlightObject;
    public UnityEvent action;

    public void Highlight(bool setState) {
        highlightObject.SetActive(setState);
    }

    public void SelectItem() {
        action?.Invoke();
    }
}
