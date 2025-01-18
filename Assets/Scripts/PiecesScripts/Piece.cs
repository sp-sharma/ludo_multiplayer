using System;
using System.Collections;
using DG.Tweening;
using PlayerScripts;
using UnityEngine;

namespace PiecesScripts
{
    public class Piece : MonoBehaviour
    {
        public Player myPlayer;
        public int currentIndex;
        public bool isOut;
        public GameObject myHighlighter;

        void OnMouseDown()
        {
            if (myPlayer.myDice.diceValue == 6 && !isOut)
            {
                MoveFirstStep();
            }
            else if (isOut)
            {
                StartCoroutine(MoveRegularStep());
            }
        }
        void MoveFirstStep()
        {
            //set movement
            isOut = true;
            transform.position = myPlayer.piecePaths[0].transform.position;

            //reset dice
            myPlayer.myDice.diceValue = 0;
            myPlayer.myDice.canRollDice = true;
        }

        IEnumerator MoveRegularStep()
        {
            int targetIndex = currentIndex + myPlayer.myDice.diceValue;
            for (int i = currentIndex; i <= targetIndex; i++)
            {
                if (i >= myPlayer.piecePaths.Length)break; 
                transform.DOMove(myPlayer.piecePaths[i].gameObject.transform.position, 0.01f);
                yield return new WaitForSeconds(0.2f);
            }
            currentIndex = targetIndex;
        }
    }
}
