using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialPause : MonoBehaviour {

    public GameObject container;
    public UnityEvent turnOffPauseEvent;

    private InputHandler inputHandler;
    public bool isTutorialPauseOn;

    private void Awake() {
        inputHandler = FindObjectOfType<InputHandler>();
    }

    private void Update() {

        if (isTutorialPauseOn && Input.anyKeyDown) {
            TurnOffTutorialPause();
        }
    }

    public void TurnOnTutorialPause() {
        isTutorialPauseOn = true;
        inputHandler.BlockPlayerInput();
        container.SetActive(true);
    }

    public void TurnOffTutorialPause() {
        inputHandler.UnBlockPlayerInput();
        container.SetActive(false);
        turnOffPauseEvent?.Invoke();
    }
}
