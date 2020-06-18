using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;



public class EndGameController : MonoBehaviourPun {
    static public GameObject monstersAtCastlePanel;
    static public GameObject narratorAtNPanel;
    static public GameObject gameWonPanel;


    static public GameObject board;
    

    void Start() {
        monstersAtCastlePanel = GameObject.Find("MonstersAtCastlePanel");
        monstersAtCastlePanel.SetActive(false);
        narratorAtNPanel = GameObject.Find("NarratorAtNPanel");
        narratorAtNPanel.SetActive(false);
        gameWonPanel = GameObject.Find("GameWonPanel");
        gameWonPanel.SetActive(false);
        board = GameObject.Find("Regions");
    }


    [PunRPC]
    public void EndGameMonsters() {
        monstersAtCastlePanel.SetActive(true);
        board.SetActive(false);
    }

    [PunRPC]
    public void EndGameNarrator() {
        narratorAtNPanel.SetActive(true);
        board.SetActive(false);
    }

    [PunRPC]
    public void EndGameWin() {
        gameWonPanel.SetActive(true);
        board.SetActive(false);
    }

    public void LeaveGame() {
        //if (PhotonNetwork.IsMasterClient) {
        PhotonNetwork.LoadLevel(0);
        //}
        PhotonNetwork.LeaveRoom();
    }


}