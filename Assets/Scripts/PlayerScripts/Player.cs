using DiceScripts;
using PiecesScripts;
using UnityEngine;

namespace PlayerScripts
{
    public class Player : MonoBehaviour
    {
        public Piece[] myPieces;  // All the pieces for the player
        public PiecePath[] piecePaths;  // The paths that the pieces follow
        public Dice myDice;  // The player's dice component
        public int myTurn;  // Indicates if it is this player's turn (used for turn management)
    }
}
