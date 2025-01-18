using System.Collections;
using Lobbyscene;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Player = PlayerScripts.Player;

namespace DiceScripts
{
    public class Dice : MonoBehaviourPun
    {
        public int diceValue;
        public bool canRollDice;
        public Player player;

        [SerializeField] Vector3 defaultdicePosition;
        [SerializeField] Sprite defaultSprite;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] GameObject diceAnimation;
        [SerializeField] Sprite[] diceSprites;
        [SerializeField] Animator animationOfDice;
        [SerializeField] Transform diceTransform;

        void Start()
        {
            defaultdicePosition = spriteRenderer.gameObject.transform.position;
            spriteRenderer.sprite = defaultSprite;
            animationOfDice.enabled = true;
        }

        void OnMouseDown()
        {
            if (!canRollDice || !IsMyTurn())
            {
                return;
            }

            canRollDice = false;

            // Roll the dice and synchronize the result
            photonView.RPC("RollDiceRPC", RpcTarget.All, Random.Range(1, 7));
        }

        [PunRPC]
        void RollDiceRPC(int rolledValue)
        {
            StartCoroutine(RollingDice(rolledValue));
        }

        IEnumerator RollingDice(int rolledValue)
        {
            diceValue = rolledValue;

            spriteRenderer.gameObject.SetActive(false);
            diceAnimation.SetActive(true);

            yield return new WaitForSeconds(0.4f);

            diceAnimation.SetActive(false);
            spriteRenderer.sprite = diceSprites[diceValue - 1];
            spriteRenderer.gameObject.SetActive(true);

            animationOfDice.enabled = false;
            diceTransform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            diceTransform.position = defaultdicePosition;

            // Handle turn logic if the rolled value is not 6
            if (diceValue != 6)
            {
                canRollDice = true;
                EndTurn();
            }
        }

        bool IsMyTurn()
        {
            // Check if it's this player's turn
            return player.myTurn == LobbyManager.Instance.turn;
        }

        void EndTurn()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                LobbyManager.Instance.EndTurn();
            }
        }
    }
}
