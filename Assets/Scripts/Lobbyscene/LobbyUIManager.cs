using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobbyscene
{
    public class LobbyUIManager : MonoBehaviourPunCallbacks
    {
        public static LobbyUIManager Instance;

        [Header("UI Elements")]
        [SerializeField] TMP_InputField roomNameInput;
        [SerializeField] Button createRoomButton;
        [SerializeField] Button joinRoomButton;
        [SerializeField] Button startGameButton;
        [SerializeField] TMP_Text[] playerListTexts;

        [SerializeField] GameObject[] lobbyUI;

        PhotonView photonView;

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

        void Start()
        {
            photonView = GetComponent<PhotonView>();

            createRoomButton.onClick.AddListener(CreateRoom);
            joinRoomButton.onClick.AddListener(JoinRoom);
            startGameButton.onClick.AddListener(StartGame);
        }

        void CreateRoom()
        {
            string roomName = roomNameInput.text.Trim();
            if (string.IsNullOrEmpty(roomName)) return;

            RoomOptions options = new RoomOptions
            {
                MaxPlayers = 4
            };

            PhotonNetwork.CreateRoom(roomName, options);
        }

        void JoinRoom()
        {
            string roomName = roomNameInput.text.Trim();
            if (string.IsNullOrEmpty(roomName)) return;

            PhotonNetwork.JoinRoom(roomName);
        }

        void StartGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("LoadGameScene", RpcTarget.All);
            }
        }

        [PunRPC]
        void LoadGameScene()
        {
            PhotonNetwork.LoadLevel("Gameplay");
        }

        public void UpdatePlayerListUI()
        {
            lobbyUI[0].SetActive(false);
            lobbyUI[1].SetActive(true);

            switch (PhotonNetwork.PlayerList.Length)
            {
                case 1:
                    PhotonNetwork.NickName = "RedPlayer";
                    break;
                case 2:
                    PhotonNetwork.NickName = "GreenPlayer";
                    break;
                case 3:
                    PhotonNetwork.NickName = "YellowPlayer";
                    break;
                case 4:
                    PhotonNetwork.NickName = "BluePlayer";
                    break;
            }

            for (int i = 0; i < playerListTexts.Length; i++)
            {
                if (i < PhotonNetwork.PlayerList.Length)
                {
                    playerListTexts[i].text = PhotonNetwork.PlayerList[i].NickName;
                    playerListTexts[i].gameObject.SetActive(true);
                }
                else
                {
                    playerListTexts[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
