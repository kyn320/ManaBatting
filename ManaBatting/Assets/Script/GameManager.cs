using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public UIInGame ui;
    public HandCard[] handCards;
    public BatchCardGroup[] batchCardGroups;

    public bool isBatch = false;

    public int baseMana;

    public int[] manaBet;
    public bool[] giveUpBet;
    public bool[] readyForBatch;


    public List<PhotonPlayer> playerList;
    public int myID = -1;

    public Transform spinCoin;
    public int whoFirst = -1;
    public int currentBetTurn = -1;
    public int countBetTurn = 0;

    public int currentTurn = 1;

    PhotonView photonView;

    void Awake()
    {
        instance = this;
        photonView = PhotonView.Get(this);
    }

    void Start()
    {
        manaBet = new int[PhotonNetwork.room.MaxPlayers];
        giveUpBet = new bool[PhotonNetwork.room.MaxPlayers];
        readyForBatch = new bool[PhotonNetwork.room.MaxPlayers];

        playerList = PhotonNetwork.playerList.OfType<PhotonPlayer>().ToList();
        playerList.Sort();

        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("GameStart", PhotonTargets.All, null);
        }
    }

    [PunRPC]
    void GameStart()
    {
        //기본 마나를 <baseMana>개 지급
        for (int i = 0; i < handCards.Length; ++i)
        {
            handCards[i].mana += baseMana;
        }
        //플레이어 A와 B는 자신의 덱에서 카드를 n개 배치 시작
        isBatch = true;
    }

    public int FindIndexPlayerList(string _name)
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            if (playerList[i].NickName == _name)
                return i;
        }
        return -1;
    }

    public void SendReadyBatch(int _id)
    {
        //서버로 전송
        photonView.RPC("OnReadyBatch", PhotonTargets.AllBuffered, _id, batchCardGroups[myID].batchList);
    }

    [PunRPC]
    void OnReadyBatch(int _id, CardBehaviour[] batchCard)
    {
        readyForBatch[_id] = true;
        batchCardGroups[_id].batchList = batchCard;

        //플레이어 A와 B가 모두 준비 완료
        if (readyForBatch[0] && readyForBatch[1])
        {
            //플레이어 A와 B의 첫번째 카드를 오픈
            for (int i = 0; i < batchCardGroups.Length; ++i)
            {
                batchCardGroups[i].OpenCard(0);
                //마스터 클라이언트에서 동전을 뒤집음
                if (PhotonNetwork.isMasterClient)
                    SendSpinCoin();
            }
        }
    }

    void SendSpinCoin()
    {
        //서버로 회전 상태를 전송
        OnSpinCoin();
    }

    [PunRPC]
    void OnSpinCoin()
    {
        StartCoroutine(SpinCoin());

        if (PhotonNetwork.isMasterClient)
            StartCoroutine(RandomBetTurn());
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
        ui.manaBet.View();
    }

    void SendRandomBetTurn(int _whoFirst)
    {
        //서버로 전송
        photonView.RPC("OnRandomBetTurn", PhotonTargets.AllBuffered, _whoFirst);
    }

    [PunRPC]
    void OnRandomBetTurn(int _whoFirst)
    {
        whoFirst = _whoFirst;
        currentBetTurn = whoFirst;
        countBetTurn = 1;
    }

    IEnumerator RandomBetTurn()
    {
        yield return new WaitForSeconds(3f);

        int whoFirst = Random.Range(0, 2);
        SendRandomBetTurn(whoFirst);
    }

    void SendNextBetTurn()
    {
        // 베팅 종료
        if (countBetTurn == 2 && (giveUpBet[0] || giveUpBet[1]))
        {
            print("open All Card");
        }

        //서버로 전송
        photonView.RPC("OnNextBetTurn", PhotonTargets.AllBuffered, currentBetTurn + 1);
    }

    [PunRPC]
    void OnNextBetTurn(int _nextTurn)
    {
        //베팅 카운트 
        countBetTurn = ((countBetTurn + 1) / 2) + 1;
        //현재 베팅 가능한 유저
        currentBetTurn = _nextTurn > 1 ? 0 : _nextTurn;


        if (myID == currentTurn)
            ui.manaBet.ViewBet();
    }

    public void SendManaBet(int _manaBet)
    {
        //서버로 전송
        photonView.RPC("OnManaBet", PhotonTargets.AllBuffered, myID, _manaBet);
    }

    [PunRPC]
    public void OnManaBet(int _id, int _manaBet)
    {
        if (_manaBet == -1)
        {
            //give up
            giveUpBet[_id] = true;
        }
        else if (_manaBet > 0)
        {
            manaBet[_id] += _manaBet;
        }

        if (_id == myID)
        {
            SendNextBetTurn();
        }

    }

    public int GetOtherBet()
    {
        if (myID == 0)
            return manaBet[1];
        else if (myID == 1)
        {
            return manaBet[0];
        }
        else
            return -1;
    }

    public int GetMyBet()
    {
        return manaBet[myID];
    }

    public int GetTotalBet()
    {
        return manaBet[0] + manaBet[1];
    }

    void OnDestroy()
    {
        instance = null;
    }

}
