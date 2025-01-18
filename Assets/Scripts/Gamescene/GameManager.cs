using Photon.Pun;
using UnityEngine;

namespace Gamescene
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public PhotonView photonviewer;
        [SerializeField] GameObject[] playerObjects;
        void Start()
        {
            photonviewer = GetComponent<PhotonView>();
            if (PhotonNetwork.IsMasterClient)
            {
                // Only the Master Client triggers the RPC
                photonviewer.RPC(nameof(InitializeScene), RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        void InitializeScene()
        {
            Debug.Log($"Number of players: {PhotonNetwork.CountOfPlayersInRooms}");

            for (int i = 0; i < PhotonNetwork.CountOfPlayersInRooms; i++)
            {
                if (i < playerObjects.Length)
                {
                    playerObjects[i].SetActive(true);
                }
            }
        }
    }
}
