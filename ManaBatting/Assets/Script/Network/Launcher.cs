using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Game.Network
{

    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        string gameVersion = "1";

        [SerializeField]
        private byte maxPlayersRoom = 4;

        bool isConnecting = false;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            PhotonNetwork.NickName = PlayDataManager.instance.playerName;
        }

        public void Connect()
        {
            isConnecting = true;

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }


        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connect the masterServer");


            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log("Disconnected" + cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("Failed Join Random Room");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersRoom , PublishUserId = true });
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Connect the Room");
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.LoadLevel("prototype");
            }
        }

    }



}


