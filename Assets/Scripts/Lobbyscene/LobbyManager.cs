using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobbyscene
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        Action hello;
        public static LobbyManager Instance;
        public int turn = 0; // Add a turn counter here.

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void EndTurn()
        {
            // Increment turn and send it to all players using RPC
            turn++;
            if (turn >= PhotonNetwork.PlayerList.Length)
            {
                turn = 0; // Reset to the first player if the turn counter exceeds number of players.
            }

            // Update the turn to all players
            photonView.RPC("SyncTurn", RpcTarget.All, turn);
        }

        [PunRPC]
        public void SyncTurn(int newTurn)
        {
            turn = newTurn;
        }
    }
}
