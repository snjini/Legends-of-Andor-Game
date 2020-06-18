using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class JoinFightController : MonoBehaviourPun  {

[PunRPC]
public void SetActive(bool status) {
    for (int i = 0; i < 4; i++) {
        this.gameObject.transform.GetChild(i).gameObject.SetActive(status);
    }
    if (!status) {
        GameObject fightController = GameObject.Find("FightController");
        fightController.GetComponent<Fight>().numDecided ++;
    }
}

}