using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sokobrain {

    public class DimensionBoxGoal : MonoBehaviour {

        [SerializeField] private DimensionBoxGoal otherGoal;
        public bool isTriggered;

        private LevelTransitioner levelTransitioner;

        private void Start() {
            levelTransitioner = FindObjectOfType<LevelTransitioner>();
        }

        private void OnTriggerEnter2D(Collider2D collision) {

            if (isTriggered) return;

            if (collision.TryGetComponent(out DimensionBox dimensionBox)) {
                
                GameManager.Instance.RegisterMetGoal(dimensionBox, this);
                dimensionBox.MetGoal();
                TriggerGoal();
            }
        }

        private void TriggerGoal() {
            isTriggered = true;

            if (otherGoal.isTriggered) {
                AudioManager.Instance.PlayGoalSound2();
                levelTransitioner.PlayClosingTransition();
            } else AudioManager.Instance.PlayGoalSound1();
        }

        public void UnTriggerGoal() {
            isTriggered = false;
        }
    } 
}
