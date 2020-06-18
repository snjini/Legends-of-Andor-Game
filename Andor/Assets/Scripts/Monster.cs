using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class Monster : MonoBehaviourPun  {
//NEED TO ADD SKIPPING IF THERE IS ALREADY A MONSTER ON THAT REGION
    public string type;
    public int wp;
    public int sp;
    public int pos;
    public SpriteRenderer token;

    public void updateRegion() {
        int newRegion = Graph.monsterGraph[this.pos];
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        bool alreadyMonster = Array.Exists(monsters, m => m.GetComponent<Monster>().pos == newRegion && newRegion != 0);
        this.pos = newRegion;
        Vector3 newPos = GameBoardRegion.getCoordinates(newRegion);
        updatePos(newPos);
        if(alreadyMonster) {
            updateRegion();
        }
    }
    public void updatePos(Vector3 newPos) {
        gameObject.transform.position = newPos;
    } 

    [PunRPC]
    public void AdvanceMonster() {
        this.updateRegion();
    }

    public static void UpdateMonstersAtCastle() {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        int num = Array.FindAll(monsters, m => m.GetComponent<Monster>().pos == 0).Length;
        PhotonHashtable h = new PhotonHashtable();
        h.Add("MonstersAtCastle", num);
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateGameStats", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }
    
    [PunRPC]
    public void Destroy() {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void SetPosition(int key) {
        this.pos = key;
    }
}