using UnityEngine;

namespace Sokobrain {

    [RequireComponent(typeof(Rigidbody2D))]
    public class WallOrObstacle : MonoBehaviour {

        public bool IsMovable;
        public bool isOuterWall;
        private GameManager gm;

        private Rigidbody2D rb2d;
        private void Start() {
            gm = GameManager.Instance;
            rb2d = GetComponent<Rigidbody2D>();
            GetColor();
        }

        public void ToggleStatus() {
            IsMovable = !IsMovable;
            GetColor();
        }

        private void GetColor() {
            //Color colorToSet;

            ////if (IsMovable) {
            ////    colorToSet = gm.movableColor;
            ////} else colorToSet = gm.nonMovableColor;
            //GetComponent<SpriteRenderer>().color = colorToSet;
        }

        public void Move(Vector2Int moveDir) {
            if (!CanMove(moveDir)) return;

            rb2d.position += moveDir * gm.moveDistance;
        }

        public bool CanMove(Vector2Int moveDir) {
            RaycastHit2D hit2D = Physics2D.Raycast(rb2d.position, moveDir, gm.moveDistance, gm.WallOrObstacleMask);
            if (hit2D.collider != null) {
                return false;
            }

            return true;
        }
    }
}