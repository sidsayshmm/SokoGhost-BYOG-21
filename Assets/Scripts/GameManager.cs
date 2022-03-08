using System.Collections;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Sokobrain {
    public class GameManager : MonoBehaviour {

        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public int moveDistance = 1;
        public LayerMask WallOrObstacleMask;
        public Player Player1;
        public Player Player2;

        private MenuController menuController;
        private int currentIdx = -1;
        private List<DimensionBox> dimensionBoxes;
        private readonly List<GameState> moveList = new List<GameState>();

        private void Awake() {
            
            DontDestroyOnLoad(transform.root.gameObject);
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            } else {
                _instance = this;
            }

            menuController = transform.parent.GetComponentInChildren<MenuController>();
            dimensionBoxes = FindObjectsOfType<DimensionBox>().ToList();
        }

        public void ToggleGhostOnPlayers() {
            AudioManager.Instance.PlayGhostSwitchAudio();
            IncrementMove();
            Player1.ToggleGhost();
            Player2.ToggleGhost();
            RegisterSwitch();
        }

        public void MovePlayers(Vector2Int movement) {
            IncrementMove();
            bool didMove = Player1.MainMove(movement);
            bool didMoveOther = Player2.MainMove(movement);

            if (didMove || didMoveOther) {
                AudioManager.Instance.PlayNormalMove();
            } 
        }

        private void RegisterSwitch() {
            moveList[currentIdx].ghostSwitchThing = true;
        }

        public void RegisterMove(Rigidbody2D rb2d, Vector2Int dirMoved) {
            moveList[currentIdx].changesInThisMove.Add((rb2d, dirMoved));
        }

        public void RegisterMetGoal(DimensionBox dBox, DimensionBoxGoal dBoxGoal) {
            moveList[currentIdx].completionsInMove.Add((dBox, dBoxGoal));
        }

        private void IncrementMove() {
            moveList.Add(new GameState());
            currentIdx++;
        }

        public void UndoLastMove() {
            if (moveList.Count == 0)
                return;

            AudioManager.Instance.PlayUndoMove();

            var gm = GameManager.Instance;
            var lastMove = moveList[moveList.Count - 1];

            if (lastMove.ghostSwitchThing) {
                Player1.ToggleGhost();
                Player2.ToggleGhost();
            } else {
                foreach (var tuple in lastMove.changesInThisMove) {
                    tuple.Item1.position -= gm.moveDistance * tuple.Item2;
                }

                foreach (var tuple in lastMove.completionsInMove) {
                    tuple.Item1.Unmeet();
                    tuple.Item2.UnTriggerGoal();
                }
            }

            currentIdx--;
            moveList.Remove(lastMove);
        }

        public void QuitGame() {
            Application.Quit();

#if  UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }


        private int lastCompletedLevel = 0;
        public void PlayGame() {
            LoadNextLevel();
        }

        public void LoadNextLevel() {
            lastCompletedLevel = PlayerPrefs.GetInt("LastCompletedLevel", 0);
            StartCoroutine(LoadAndSetupScene($"Level-{lastCompletedLevel}"));
        }
        
        public void ForceLoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }

        public IEnumerator LoadAndSetupScene(string sceneName)
        {
            menuController.isMainMenu = false;
            menuController.UnPause();

            SceneManager.LoadScene(sceneName);
            yield return null;
            var players = FindObjectsOfType<Player>();
            Player1 = players[0];
            Player2 = players[1];
        }

        public void LoadLevel(int plsShowUp)
        {
            StartCoroutine(LoadAndSetupScene($"Level-{plsShowUp}"));

        }
    }
}