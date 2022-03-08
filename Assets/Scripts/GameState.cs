using System.Collections.Generic;
using UnityEngine;

namespace Sokobrain
{
    public class GameState {
        public bool ghostSwitchThing;
        public bool normalMove;
        public List<(Rigidbody2D, Vector2Int)> changesInThisMove;
        public List<(DimensionBox, DimensionBoxGoal)> completionsInMove; 

        public GameState() {
            changesInThisMove = new List<(Rigidbody2D, Vector2Int)>();
            completionsInMove = new List<(DimensionBox, DimensionBoxGoal)>();
        }
    }
}