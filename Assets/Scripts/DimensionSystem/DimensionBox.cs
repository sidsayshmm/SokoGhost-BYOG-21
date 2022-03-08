using UnityEngine;

namespace Sokobrain {

    [RequireComponent(typeof(Rigidbody2D))]
    public class DimensionBox : MonoBehaviour {

        private GameManager gm;
        private Rigidbody2D rb2d;
        private Animator animator;

        public DimensionBox linkedBox;
        public bool hasMetGoal;

        private void Start() {
            gm = GameManager.Instance;
            rb2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        public void MetGoal() {
            animator.SetBool("ReachedGoal", true);
            hasMetGoal = true;
        }

        public void Unmeet()
        {
            animator.SetBool("ReachedGoal", false);
            hasMetGoal = false;
        }

        public void Move(Vector2Int moveDir, bool isOutsideSender) {
            if (!CanMove(moveDir)) return;

            rb2d.position += rb2d.GetRelativeVector(moveDir) * gm.moveDistance;
            gm.RegisterMove(rb2d, moveDir);

            if (isOutsideSender) {
                linkedBox.Move(moveDir, false);
            }
        }

        public bool CanMove(Vector2Int moveDir) {
            RaycastHit2D hit2D = Physics2D.Raycast(rb2d.position, moveDir, gm.moveDistance, gm.WallOrObstacleMask);
            if (hit2D.collider != null && hit2D.collider.isTrigger == false) {
                return false;
            }

            return true;
        }
    }
}