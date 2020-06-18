using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;



public class CollectiveDecisionController : MonoBehaviourPun {
    private static Text dwarfText;
    private static Text wizardText;
    private static Text archerText;
    private static Text warriorText;
    private static int sum;
    private static GameObject canvas;

    void Start() {
        canvas = GameObject.Find("CollectiveDecisionCanvas");
        if (canvas==null) canvas = GameObject.Find("CollectiveDecisionCanvas(Clone)");
        if (canvas==null) {
            Destroy(transform.gameObject);
            return;
        }
        dwarfText = GameObject.Find("DwarfGoldAmount").GetComponent<Text>();
        warriorText = GameObject.Find("WarriorGoldAmount").GetComponent<Text>();
        archerText = GameObject.Find("ArcherGoldAmount").GetComponent<Text>();
        wizardText = GameObject.Find("WizardGoldAmount").GetComponent<Text>();
        if (PhotonNetwork.IsMasterClient) {
         //   InstantiateRunestones();
            GameObject stats = GameObject.Find("StatsPanel");
            Debug.Log("stats is null" + (stats == null));
            stats.GetPhotonView().RPC("DontDestroy", RpcTarget.AllBuffered);
        }
    }

     

     public static void UpdateDwarfCount(int i) {
        if(Array.Exists(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), s => s =="Dwarf")) {
            int oldInt = Convert.ToInt16(dwarfText.text);
            int newInt = oldInt+i;
            if(newInt>=0 && sum-oldInt+newInt<=5){
                sum -= oldInt;
                sum += newInt;
                dwarfText.text = newInt.ToString();
            }
        } else {
            Debug.Log("Dwarf not in game so no items can be given to them.");
        }
     }

     // add to if statements in Update____Count methods: 
     //((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]).Length == (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"]     -> enforce the
    public static void UpdateWarriorCount(int i) {
        if(Array.Exists(((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]), s => s =="Warrior")) {
            int oldInt = Convert.ToInt16(warriorText.text);
            int newInt = oldInt+i;
            if(newInt>=0 && sum-oldInt+newInt<=5){
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
            if(newInt>=0 && sum-oldInt+newInt<=5){
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
            if(newInt>=0 && sum-oldInt+newInt<=5){
                sum -= oldInt;
                sum += newInt;
                archerText.text = newInt.ToString();
            }
        } else {
            Debug.Log("Archer not in game so no items can be given to them.");
        } 

    }

    public static void OKButtonClicked() {
        if(sum==5) {
            canvas.GetPhotonView().RPC("Destroy", RpcTarget.All);

            if(PhotonNetwork.LocalPlayer.IsMasterClient) {
                //set the stats and display
                for (int i = 0; i< ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"]).Length; i++) {
                    int amt;
                    string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
                    if (s == "Dwarf") {
                        amt = Convert.ToInt16(dwarfText.text);
                        PhotonHashtable h = new PhotonHashtable();
                        h.Add("gold", amt);
                        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                    } else if (s == "Warrior") {
                        amt = Convert.ToInt16(warriorText.text);
                        PhotonHashtable h = new PhotonHashtable();
                        h.Add("gold", amt);
                        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                    } else if (s == "Wizard") {
                        amt = Convert.ToInt16(wizardText.text);
                        PhotonHashtable h = new PhotonHashtable();
                        h.Add("gold", amt);
                        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                    } else {    //Archer
                        amt = Convert.ToInt16(archerText.text);
                        PhotonHashtable h = new PhotonHashtable();
                        h.Add("gold", amt);
                        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
                        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

                    }
                }

                //makes the wineskin controller
                GameObject n = (GameObject)PhotonNetwork.InstantiateSceneObject("WineskinCanvas", new Vector3(0, 0, 15), Quaternion.identity);
                Button ok =GameObject.Find("OKWineskinButton").GetComponent<Button>();
                ok.onClick.AddListener(() => WineskinDistributionController.OKButtonClicked());
                Button dwarfYes =GameObject.Find("AddWineskinDwarfButton").GetComponent<Button>();
                dwarfYes.onClick.AddListener(() => WineskinDistributionController.UpdateDwarfCount(1));
                Button dwarfNo =GameObject.Find("SubtractWineskinDwarfButton").GetComponent<Button>();
                dwarfNo.onClick.AddListener(() => WineskinDistributionController.UpdateDwarfCount(-1));
                Button wizardYes =GameObject.Find("AddWineskinWizardButton").GetComponent<Button>();
                wizardYes.onClick.AddListener(() => WineskinDistributionController.UpdateWizardCount(1));
                Button wizardNo =GameObject.Find("SubtractWineskinWizardButton").GetComponent<Button>();
                wizardNo.onClick.AddListener(() => WineskinDistributionController.UpdateWizardCount(-1));
                Button warriorYes =GameObject.Find("AddWineskinWarriorButton").GetComponent<Button>();
                warriorYes.onClick.AddListener(() => WineskinDistributionController.UpdateWarriorCount(1));
                Button warriorNo =GameObject.Find("SubtractWineskinWarriorButton").GetComponent<Button>();
                warriorNo.onClick.AddListener(() => WineskinDistributionController.UpdateWarriorCount(-1));
                Button archerYes =GameObject.Find("AddWineskinArcherButton").GetComponent<Button>();
                archerYes.onClick.AddListener(() => WineskinDistributionController.UpdateArcherCount(1));
                Button archerNo =GameObject.Find("SubtractWineskinArcherButton").GetComponent<Button>();
                archerNo.onClick.AddListener(() => WineskinDistributionController.UpdateArcherCount(-1));
                WineskinDistributionController.Construct();
        //GameObject g = GameObject.Find("CollectiveDecisionCanvas");
        
        //GameObject h = GameObject.Find("CollectiveDecisionController");
        //if (h==null) h = GameObject.Find("CollectiveDecisionController(Clone)");
        //Destroy(g); //because you only need these once at the start of the game
        //Destroy(h);
            }
        } else {
            Debug.Log("Gold count must add to 5");
        }
    }
}

