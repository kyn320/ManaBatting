using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PhotonNetworkManager : PunBehaviour
{
    public static PhotonNetworkManager instance;

    void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = true;
        }
    }

    void Start()
    {
        PhotonNetwork.playerName = PlayDataManager.instance.playerName;

        if (!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings("dev");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        print("is join" + PhotonNetwork.isMasterClient + " / " + PhotonNetwork.room.PlayerCount);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        if (PhotonNetwork.isMasterClient && PhotonNetwork.room.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel("prototype");
        }
    }


    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        // GameManager.Instance.OutGame();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        TypedLobby typedLobby = new TypedLobby();
        typedLobby.Type = LobbyType.Default;

        PhotonNetwork.CreateRoom(null, roomOptions, typedLobby);
    }

    public int GetPlayerCount()
    {
        if (PhotonNetwork.connected)
            return PhotonNetwork.countOfPlayers;
        else
            return 0;
    }
}
