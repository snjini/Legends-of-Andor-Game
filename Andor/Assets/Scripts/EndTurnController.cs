using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EndTurnController : MonoBehaviour {

    public void ExecuteEndTurn() {
        int index = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        string type = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[index];
        GameObject g = GameObject.Find(type);
        if (g == null) g = GameObject.Find(type+"(Clone)");
        
        Hero hero = g.GetComponent<Hero>();

        //if on fog, flip the fog - move this to the end turn button
        if (UnFlippedFog.fogs.Contains(hero.getPos())) {
            GameObject r = GameObject.Find("Region"+hero.getPos().ToString());
            PhotonView fogView = r.GetPhotonView();
            fogView.RPC("FlipFog", RpcTarget.AllBuffered);
        }
        hero.endTurn();
    }
}