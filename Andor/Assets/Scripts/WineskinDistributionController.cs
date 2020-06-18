using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;



public class WineskinDistributionController : MonoBehaviourPun {
    private static Text dwarfText;
    private static Text wizardText;
    private static Text archerText;
    private static Text warriorText;
    private static int sum;
    private static GameObject canvas;

    public static void Construct() {
        canvas = GameObject.Find("WineskinCanvas");
        if (canvas==null) canvas = GameObject.Find("WineskinCanvas(Clone)");
       // if (canvas==null) {
       //     Destroy(transform.gameObject);
       //     return;
       // }
        dwarfText = GameObject.Find("DwarfWineskinAmount").GetComponent<Text>();
        warriorText = GameObject.Find("WarriorWineskinAmount").GetComponent<Text>();
        archerText = GameObject.Find("ArcherWineskinAmount").GetComponent<Text>();
        wizardText = GameObject.Find("WizardWineskinAmount").GetComponent<Text>();
    }

    public void EndHeroDay() {
        PhotonHashtable h = new PhotonHashtable();
        h.Add("DayIsEnded", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        Debug.Log("ending day");
    }

     public static void UpdateDwarfCount(int i) {
        if(Array.Exists(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), s => s =="Dwarf")) {
            int oldInt = Convert.ToInt16(dwarfText.text);
            int newInt = oldInt+i;
            if(newInt>=0 && sum-oldInt+newInt<=2){
                sum -= oldInt;
                sum += newInt;
                dwarfText.text = newInt.ToString();
            }
        } else {
            Debug.Log("Dwarf not in game so no items can be given to them.");
        }
     }
    public static void UpdateWarriorCount(int i) {
        if(Array.Exists(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), s => s =="Warrior")) {
            int oldInt = Convert.ToInt16(warriorText.text);
            int newInt = oldInt+i;
            if(newInt>=0 && sum-oldInt+newInt<=2){
                sum -= oldInt;
                sum += newInt;
                warriorText.text = newInt.ToString();
            }
        } else {
            Debug.Log("Warrior not in game so no items can be given to them.");
        }
    }
    public static void UpdateWizardCount(int i) {
        if(Array.Exists(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), s => s =="Wizard")) {
            int oldInt = Convert.ToInt16(wizardText.text);
            int newInt = oldInt+i;
            if(newInt>=0 && sum-oldInt+newInt<=2){
                sum -= oldInt;
                sum += newInt;
                wizardText.text = newInt.ToString();
            }
        } else {
            Debug.Log("Wizard not in game so no items can be given to them.");

        }
    }
    public static void UpdateArcherCount(int i) {
        if(Array.Exists(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), s => s =="Archer")) {
            int oldInt = Convert.ToInt16(archerText.text);
            int newInt = oldInt+i;
            if(newInt>=0 && sum-oldInt+newInt<=2){
                sum -= oldInt;
                sum += newInt;
                archerText.text = newInt.ToString();
            }
        } else {
            Debug.Log("Archer not in game so no items can be given to them.");
        }
    }

    public static void OKButtonClicked() {
        if(sum==2) {
            //set the stats and display 
            for (int i = 0; i< ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]).Length; i++) {
                int amt;
                string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
                if (s == "Dwarf") {
                    amt = Convert.ToInt16(dwarfText.text);
                    PhotonHashtable h = new PhotonHashtable();
                    h.Add("wineskin", amt);
                    PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                    StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                } else if (s == "Warrior") {
                    amt = Convert.ToInt16(warriorText.text);
                    PhotonHashtable h = new PhotonHashtable();
                    h.Add("wineskin", amt);
                    PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                    StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                } else if (s == "Wizard") {
                    amt = Convert.ToInt16(wizardText.text);
                    PhotonHashtable h = new PhotonHashtable();
                    h.Add("wineskin", amt);
                    PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                    StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                } else {    //Archer
                    amt = Convert.ToInt16(archerText.text);
                    PhotonHashtable h = new PhotonHashtable();
                    h.Add("wineskin", amt);
                    PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                    StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                }
            }

            canvas.GetPhotonView().RPC("Destroy", RpcTarget.All);
        //GameObject g = GameObject.Find("CollectiveDecisionCanvas");
        
        //GameObject h = GameObject.Find("CollectiveDecisionController");
        //if (h==null) h = GameObject.Find("CollectiveDecisionController(Clone)");
        //Destroy(g); //because you only need these once at the start of the game
        //Destroy(h);
        } else {
            Debug.Log("wineskin total must add to 2.");
        }
    }
}
