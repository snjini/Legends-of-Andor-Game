using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class HerbController : MonoBehaviourPun  {

    public int pos;

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void SetPosOfHerb(int position) {
        //Debug.Log(position);
        this.pos = position;
    }

    [PunRPC]
    public void Destroy() {
        Destroy(this.gameObject);
    }


}