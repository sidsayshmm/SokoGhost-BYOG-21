using Sokobrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTriggerEvent : MonoBehaviour {

    public UnityEvent tutorialEvent;
    public bool hasWaited = false;

    private void Start() {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(0.5f);
        hasWaited = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.GetComponent<Player>() != null)
            tutorialEvent.Invoke();
    }
}
