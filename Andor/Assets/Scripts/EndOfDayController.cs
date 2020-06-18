using UnityEngine;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;



public class EndOfDayController : MonoBehaviourPun {

    public static bool IsEndOfDay() {
        var l = PhotonNetwork.PlayerList;
        foreach(Player player in PhotonNetwork.PlayerList) {
            bool end = (bool)player.CustomProperties["DayIsEnded"];
           // int hours = (int)player.CustomProperties["Hours"];
            if (!end) {
                Debug.Log("Is end of day:" + false);
                return false;
            }
        }
        Debug.Log("Is end of day:" + true);
        return true;
    }


    public static void ExecuteEndOfDay() {
        //make it player 1's turn again
        PhotonHashtable h = new PhotonHashtable();
        h.Add("CurrentPlayerIndex", 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        ResetTimeTrack();
        MoveMonsters();
        ReplenishWells();
        MoveNarrator();
        GameBoardRegion.wellPopup.gameObject.GetPhotonView().RPC("ResetPopUpListeners", RpcTarget.AllBuffered);
        Monster.UpdateMonstersAtCastle();
        int numberAllowed = 5 - (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"];
        if ((int)PhotonNetwork.CurrentRoom.CustomProperties["MonstersAtCastle"] - (int)PhotonNetwork.CurrentRoom.CustomProperties["FarmersAtCastle"] > numberAllowed) {
            Debug.Log("GAME LOST! TOO MANY MONSTERS AT THE CASTLE!");
            PhotonView panelView =  EndGameController.monstersAtCastlePanel.GetPhotonView();
            panelView.RPC("EndGameMonsters", RpcTarget.AllBuffered);
        } else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["NarratorPosition"] >= 13 && (bool)PhotonNetwork.CurrentRoom.CustomProperties["HerbAtCastle"]) {
            if ((bool)PhotonNetwork.CurrentRoom.CustomProperties["SkralAtCastleDefeated"]) {
                Debug.Log("GAME WON!");
                PhotonView panelView =  EndGameController.gameWonPanel.GetPhotonView();
                panelView.RPC("EndGameWin", RpcTarget.AllBuffered);
            }
        } else if ((int)PhotonNetwork.CurrentRoom.CustomProperties["NarratorPosition"] >= 13) {    
            Debug.Log("GAME LOST! NARRATOR AT POSITION N!");
            PhotonView panelView =  EndGameController.narratorAtNPanel.GetPhotonView();
            panelView.RPC("EndGameNarrator", RpcTarget.AllBuffered);
        } 
        
        //move narrator
        //legend card update
    }


    public static void ResetTimeTrack() {
        foreach (Player player in PhotonNetwork.PlayerList) {
            //reset hours
            PhotonHashtable h = new PhotonHashtable();
            h.Add("Hours", 0);
            h.Add("DayIsEnded", false);
            player.SetCustomProperties(h);
            //reset time track
            string type = (string)player.CustomProperties["Type"];
            GameObject token = GameObject.Find(type+"TimeTracker");
            if (token==null) {
                token = GameObject.Find(type+"TimeTracker(Clone)");
            }
            PhotonView view = token.GetPhotonView();
            view.RPC("ResetTokens", RpcTarget.AllBuffered);
        }

        string master = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[0];
        GameObject masterToken = GameObject.Find(master+"TimeTracker");
        if (masterToken==null) {
            masterToken = GameObject.Find(master+"TimeTracker(Clone)");
        }
        PhotonView masterView = masterToken.GetPhotonView();
        masterView.RPC("ResetTokens", RpcTarget.AllBuffered);
    }


    public static void MoveMonsters() {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach(GameObject m in monsters) {
            PhotonView v = m.GetPhotonView();
            v.RPC("AdvanceMonster", RpcTarget.All);
        }
    }

    public static void ReplenishWells() {
        GameObject[] wells = GameObject.FindGameObjectsWithTag("Well");
        foreach(GameObject w in wells) {
            PhotonView v = w.GetPhotonView();
            v.RPC("RefillWell", RpcTarget.All);
        }
    }

    public static void MoveNarrator() {
        GameObject[] g = GameObject.FindGameObjectsWithTag("Narrator");
        GameObject narrator = g[0];
        narrator.GetPhotonView().RPC("AdvanceNarrator", RpcTarget.AllBuffered);
    }

}
