using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class WitchBrewController : MonoBehaviourPun  {

    public int pos;

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void SetPosOfBrew(int position) {
        //Debug.Log(position);
        this.pos = position;
    }

}