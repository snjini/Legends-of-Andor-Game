using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class GoldToken : MonoBehaviourPun  {

    public int pos;
    public int amt;

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void SetPosOfGoldToken(int position) {
        //Debug.Log(position);
        this.pos = position;
    }

    [PunRPC]
    public void Destroy() {
        Destroy(this.gameObject);
    }

    [PunRPC]
    public void AddAmt(int toAdd) {
        this.amt += toAdd;
    }

}