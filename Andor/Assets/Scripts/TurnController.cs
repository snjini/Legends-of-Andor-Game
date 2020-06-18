using UnityEngine;
using Photon.Pun;
using System;
using System.Collections.Generic;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;


public class TurnController : MonoBehaviourPun  {

    void Start() {
        if (PhotonNetwork.IsMasterClient) {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    
    [PunRPC]
    public void Enable() {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }
    [PunRPC]
    public void Disable() {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }
}