using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sokobrain;
using System.Linq;
using System;

public class LevelTransitioner : MonoBehaviour {

    private List<Animator> innerWalls = new List<Animator>();
    private InputHandler inputHandler;

    private void Awake() {
        List<WallOrObstacle> allWalls = FindObjectsOfType<WallOrObstacle>().ToList();

        for (int i = 0; i < allWalls.Count; i++) {
            if (allWalls[i].isOuterWall) {
                continue;
            } else {
                innerWalls.Add(allWalls[i].GetComponent<Animator>());
                allWalls[i].gameObject.SetActive(false);
            }
        }
    }

    private void Start() {
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.BlockPlayerInput();
        StartCoroutine(UnblockPlayerInput());
        StartCoroutine(PlayOpeningTransition());
    }

    private IEnumerator UnblockPlayerInput() {
        yield return new WaitForSeconds(1);
        inputHandler.UnBlockPlayerInput();
    }

    public IEnumerator PlayOpeningTransition() {

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < innerWalls.Count; i++) {
            innerWalls[i].gameObject.SetActive(true);
            innerWalls[i].Play("DropWall");
            StartCoroutine(PlayImpactSound());
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator PlayImpactSound() {
        yield return new WaitForSeconds(0.25f);
        AudioManager.Instance.PlayNormalMove();
    }

    public IEnumerator PlayLiftSound() {
        yield return new WaitForSeconds(0.25f);
    }

    public void PlayClosingTransition() {
        int lastCompletedLevel = PlayerPrefs.GetInt("LastCompletedLevel") + 1;
        PlayerPrefs.SetInt("LastCompletedLevel", lastCompletedLevel);
        StartCoroutine(ClosingTransitionCoRoutine());
    }

    public IEnumerator ClosingTransitionCoRoutine() {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < innerWalls.Count; i++) {
            innerWalls[i].Play("LiftWall");
            AudioManager.Instance.PlayUndoMove();
            //StartCoroutine(PlayLiftSound());
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.LoadNextLevel();
    }
}
