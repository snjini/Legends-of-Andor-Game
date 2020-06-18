using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class StarController : MonoBehaviourPun  {
    public int location;

    [PunRPC]
    public void DontDestroy() {
        DontDestroyOnLoad(this.gameObject);
    }

    [PunRPC]
    public void SetPosOfStar(int pos) {
        this.location = pos;
    }

    [PunRPC]
    public void Destroy() {
        Destroy(this.gameObject);
    }
}