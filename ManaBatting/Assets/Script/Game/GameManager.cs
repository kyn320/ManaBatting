using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : SingletonPunCallbacks<GameManager>
{
    CardManager cardManager;

    public UIInGame ui;


    public bool isBatch = false;

    public int baseMana;

    public int[] manaBets;
    public bool[] giveUpBets;
    public bool[] readyForBatches;


    public List<Player> playerList;
    public int myID = -1;

    public Transform spinCoin;
    public int whoFirst = -1;
    public int currentBetTurn = -1;
    public int countBetTurn = 0;

    public int currentTurn = 1;


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        cardManager = CardManager.Instance;

        if (!PhotonNetwork.IsConnected)
            return;

        manaBets = new int[PhotonNetwork.CurrentRoom.MaxPlayers];
        giveUpBets = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];
        readyForBatches = new bool[PhotonNetwork.CurrentRoom.MaxPlayers];

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GameStart", RpcTarget.All, null);
        }
    }

    int FindMyID()
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            if (PlayDataManager.instance.playerName == playerList[i].NickName)
                return i;
        }

        return -1;
    }

    [PunRPC]
    void GameStart()
    {
        //기본 마나를 <baseMana>개 지급
        cardManager.GiveManaAll(baseMana);

        //플레이어 A와 B는 자신의 덱에서 카드를 n개 배치 시작
        isBatch = true;
    }

    public int FindIndexPlayerList(string name)
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            if (playerList[i].NickName == name)
                return i;
        }
        return -1;
    }

    public void RPCReadyBatch(int id)
    {
        //서버로 전송
        photonView.RPC("RemoteReadyBatch", RpcTarget.AllBuffered, id);
    }

    [PunRPC]
    void RemoteReadyBatch(int id)
    {
        readyForBatches[id] = true;
        print(id + "is ready");
        //플레이어 A와 B가 모두 준비 완료
        if (readyForBatches[0] && readyForBatches[1])
        {
            //플레이어 A와 B의 첫번째 카드를 오픈
            //cardManager.OpenCard();

            //마스터 클라이언트에서 동전을 뒤집음
            if (PhotonNetwork.IsMasterClient)
            {
                print("send spin");
                RPCSpinCoin();
            }
        }
    }

    void RPCSpinCoin()
    {
        //서버로 회전 상태를 전송
        //서버로 전송
        photonView.RPC("RemoteSpinCoin", RpcTarget.AllBuffered, null);
    }

    [PunRPC]
    void RemoteSpinCoin()
    {
        StartCoroutine(SpinCoin());

        if (PhotonNetwork.IsMasterClient)
        {
            print("isMasterClient");
            StartCoroutine(RandomBetTurn());
        }
    }

    IEnumerator SpinCoin()
    {
        while (whoFirst == -1)
        {
            spinCoin.Rotate(Vector3.up * 200f * Time.deltaTime);
            yield return null;
        }

        Quaternion rot = Quaternion.Euler(0, 180f * whoFirst, 0);

        while (Quaternion.Angle(spinCoin.rotation, rot) > 0.01f)
        {
            spinCoin.localRotation = Quaternion.Lerp(spinCoin.localRotation, rot, Time.deltaTime * 20f);
            yield return null;
        }
        print("end spin");
    }

    void RPCRandomBetTurn(int whoFirst)
    {
        //서버로 전송
        photonView.RPC("RemoteRandomBetTurn", RpcTarget.AllBuffered, whoFirst);
    }

    [PunRPC]
    void RmoteRandomBetTurn(int whoFirst)
    {
        print("WhoFirst on randomn rpc = " + whoFirst + " / my ID = " + myID);
        this.whoFirst = whoFirst;
        currentBetTurn = whoFirst;
        countBetTurn = 1;

        ui.manaBet.View();

        if (currentBetTurn == myID)
            ui.manaBet.ViewBet();
    }

    IEnumerator RandomBetTurn()
    {
        yield return new WaitForSeconds(3f);

        int whoFirst = Random.Range(0, 2);
        RPCRandomBetTurn(whoFirst);
    }

    void RPCNextBetTurn()
    {
        //서버로 전송
        photonView.RPC("RemoteNextBetTurn", RpcTarget.AllBuffered, currentBetTurn);
    }

    [PunRPC]
    void RemoteNextBetTurn(int nextTurn)
    {
        if (countBetTurn == 2 && (giveUpBets[0] || giveUpBets[1]))
        {
            print("open All Card and action");
            ui.manaBet.Hide();
            cardManager.OpenCard();
        }
        else
        {
            //1 % 2 = 1 | 2 % 2 = 0 | 3 % 2 = 1 | 4 % 2 = 0 |
            countBetTurn = (countBetTurn + 1) > 2 ? 1 : (countBetTurn + 1);
            print("countBet = " + countBetTurn);
            //현재 베팅 가능한 유저
            currentBetTurn = currentBetTurn == 1 ? 0 : 1;

            if (myID == currentBetTurn)
                ui.manaBet.ViewBet();
            else
                ui.manaBet.HideBet();
        }
    }

    public void RPCManaBet(int manaBet)
    {
        //서버로 전송
        photonView.RPC("RemoteManaBet", RpcTarget.AllBuffered, myID, manaBet);
    }

    [PunRPC]
    public void RemoteManaBet(int id, int manaBet)
    {
        print("Set Mana Bet" + id + " = " + manaBet);
        if (manaBet == -1)
        {
            //give up
            giveUpBets[id] = true;
        }
        else if (manaBet > 0)
        {
            manaBets[id] += manaBet;
        }

        ui.manaBet.UpdateAllBet();

        if (id == myID)
        {
            print("is next turn");
            RPCNextBetTurn();
        }

    }

    public int GetOtherBet()
    {
        if (myID == 0)
            return manaBets[1];
        else if (myID == 1)
        {
            return manaBets[0];
        }
        else
            return -1;
    }

    public int GetMyBet()
    {
        return manaBets[myID];
    }

    public int GetTotalBet()
    {
        return manaBets[0] + manaBets[1];
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        playerList = PhotonNetwork.PlayerList.ToList<Player>();

        myID = FindMyID();

        cardManager.SetID(myID);
    }

}
