using System;
using System.Collections;
using UnityEngine;

namespace Sokobrain {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Player otherPlayer;
        [SerializeField] private MeshRenderer ghostCoverMeshRenderer;

        private Rigidbody2D rb2d;
        private GameManager gm;
        private SpriteRenderer spriteRenderer;
        public bool isGhost;

        public Sprite ghostSprite;
        public Sprite playerSprite;

        private void Start() {
            gm = GameManager.Instance;
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public bool MainMove(Vector2Int movement)
        {
            DoAnimationShit(movement);
            if (isGhost)
                return false;

            if (Move(movement)) {
                otherPlayer.Move(movement);
                return true;
            }
            else
            {
                CantMoveAnimationShit();
            }
            return false;
        }
        
        public bool Move(Vector2Int moveDir) {
            Debug.DrawLine(rb2d.position, rb2d.position + moveDir * gm.moveDistance, Color.red, 3f);
            RaycastHit2D hit2D = Physics2D.Raycast(rb2d.position, moveDir, gm.moveDistance, gm.WallOrObstacleMask);

            if (hit2D.collider != null && hit2D.collider.isTrigger == false) {

                DimensionBox dimensionBox = null;
                WallOrObstacle wob = hit2D.collider.GetComponent<WallOrObstacle>();

                if (wob != null) {
                    if (wob && !isGhost || wob.isOuterWall) {
                        return false;
                    }

                } else if (hit2D.collider.TryGetComponent(out dimensionBox)) {
                    if (!dimensionBox.CanMove(moveDir)) {
                        return false;
                    }

                    dimensionBox.Move(moveDir, true);
                }
            }

            rb2d.position += moveDir * gm.moveDistance;
            gm.RegisterMove(rb2d, moveDir);
            return true;
        }

        public void ToggleGhost()
        {
            isGhost = !isGhost;

            if (isGhost) {
                ghostCoverMeshRenderer.enabled = true;
                spriteRenderer.sprite = ghostSprite;
                spriteRenderer.color = new Color(1, 1, 1, 0.8f);

            } else {
                ghostCoverMeshRenderer.enabled = false;
                spriteRenderer.sprite = playerSprite;
                spriteRenderer.color = Color.white;
            }
        }

        public void DoAnimationShit(Vector2Int movDir)
        {
            spriteRenderer.flipX = movDir == Vector2Int.right;
        }
        public void CantMoveAnimationShit() {
            StartCoroutine(Bla());
        }

        private IEnumerator Bla()
        {
            spriteRenderer.color = new Color(1, 0.6f, 0.6f);
            yield return new WaitForSeconds(0.3f);
            spriteRenderer.color = Color.white;
        }
        
    }
}