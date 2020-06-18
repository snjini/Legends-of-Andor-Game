using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using UnityEngine.SceneManagement;

public class StatsPanelController : MonoBehaviourPun  {

    public static GameObject statsPanel;

    void Start() {
        statsPanel = this.gameObject;
        //StatsPanelController.statsPanel
        this.gameObject.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"]);
        this.gameObject.GetPhotonView().RPC("UpdateGameStats", RpcTarget.AllBuffered);
    }

    public void UpdateStats() {
        foreach (string s in (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]) {
            this.gameObject.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
        }
        this.gameObject.GetPhotonView().RPC("UpdateGameStats", RpcTarget.AllBuffered);
    
    }

    [PunRPC]
    public void UpdateCharStats(string charType) {
        GameObject g = GameObject.Find(charType);
        if (g == null) g = GameObject.Find(charType+"(Clone)");
        PhotonView v = g.GetPhotonView();
        int wp = (int)v.Owner.CustomProperties["wp"];
        int sp = (int)v.Owner.CustomProperties["sp"];
        int farmer = (int)v.Owner.CustomProperties["farmer"];
        int gold = (int)v.Owner.CustomProperties["gold"];
        bool herb = (bool)v.Owner.CustomProperties["herb"];
        int items = (int)v.Owner.CustomProperties["wineskin"];
        items = items + (int)v.Owner.CustomProperties["shield"];
        items = items + (int)v.Owner.CustomProperties["falcon"];
        items = items + (int)v.Owner.CustomProperties["telescope"];
        items = items + (int)v.Owner.CustomProperties["bow"];
        items = items + (int)v.Owner.CustomProperties["helm"];
        items = items + (int)v.Owner.CustomProperties["brew"];
        items = items + (int)v.Owner.CustomProperties["yellowStone"];
        items = items + (int)v.Owner.CustomProperties["blueStone"];
        items = items + (int)v.Owner.CustomProperties["greenStone"];

        string toWrite = charType + ": " + wp + " wp, " + sp + " sp\n";
        toWrite = toWrite + gold + " gold, ";
        toWrite = toWrite + farmer + " farmer\n";
        if (items > 0) toWrite = toWrite + items + " items";
        if (herb) toWrite = toWrite + "\n has the herb!";
        GameObject.Find(charType + "StatsText").GetComponent<TextMeshProUGUI>().text = toWrite;

    }

    [PunRPC]
    public void UpdateGameStats() {

        int farmers = (int)PhotonNetwork.CurrentRoom.CustomProperties["FarmersAtCastle"];
        int monsters = (int)PhotonNetwork.CurrentRoom.CustomProperties["MonstersAtCastle"];
        bool herb = (bool)PhotonNetwork.CurrentRoom.CustomProperties["HerbAtCastle"];

        string toWrite = "Game Stats:\n" + farmers + " farmers at the castle\n" + monsters + " monsters at the castle";
        if (herb) toWrite = toWrite + "\nHERB AT CASTLE!!";
        GameObject.Find("GameStatsText").GetComponent<TextMeshProUGUI>().text = toWrite;


    }

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject.transform.parent);
    }



}