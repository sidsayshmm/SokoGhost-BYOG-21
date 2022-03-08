using System.Collections;
using System.Collections.Generic;
using Sokobrain;
using UnityEngine;

public class InputHandler : MonoBehaviour {

    public MenuController menuController;

    private float undoTime = 0.15f;
    private float undoTimeCounter;

    private bool playerInputBlocked;

    private void Update() {

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {

            if (menuController.isPaused) {
                menuController.KeySelect(Vector2Int.up);
            } else PlayerMove(Vector2Int.up);

        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {

            if (menuController.isPaused) {
                menuController.KeySelect(Vector2Int.right);
            } else PlayerMove(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {

            if (menuController.isPaused) {
                menuController.KeySelect(Vector2Int.left);
            } else PlayerMove(Vector2Int.left);
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {

            if (menuController.isPaused) {
                menuController.KeySelect(Vector2Int.down);
            } else PlayerMove(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            menuController.TogglePause();
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            menuController.SelectMenuItem();
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            //GameManager.Instance.IncrementMove();
            //GameManager.Instance.RegisterMove()
            GameManager.Instance.ToggleGhostOnPlayers();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            undoTimeCounter = 0;
            if (!playerInputBlocked)
                GameManager.Instance.UndoLastMove();

        } else if (Input.GetKey(KeyCode.R)) {

            if (undoTimeCounter < undoTime) {
                undoTimeCounter += 1f * Time.deltaTime;

            } else {
                undoTimeCounter = 0;
                if (!playerInputBlocked)
                    GameManager.Instance.UndoLastMove();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (!playerInputBlocked)
                GameManager.Instance.LoadNextLevel();
        }
    }

    public void BlockPlayerInput() {
        playerInputBlocked = true;
    }

    public void UnBlockPlayerInput() {
        playerInputBlocked = false;
    }

    private void PlayerMove(Vector2Int movement) {
        if (!playerInputBlocked) GameManager.Instance.MovePlayers(movement);
    }

}
