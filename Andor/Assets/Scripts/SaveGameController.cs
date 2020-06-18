using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;

using UnityEngine.SceneManagement;
using System.Collections;


public class SaveGameController : MonoBehaviourPunCallbacks {

    private InputField inputField;

    void Start() {
        this.inputField = transform.GetChild(1).GetComponent<InputField>();
        transform.GetChild(1).gameObject.SetActive(false);
    }

    void Update() {
        if(Input.GetKey(KeyCode.Return) && inputField.gameObject.activeSelf) {
            ActivateInputField();
        }
    }

    public void ActivateInputField() {
        transform.GetChild(1).gameObject.SetActive(true);
        string name = inputField.text;
        inputField.text = "";
        SaveTheGame(name);
    }

   // IEnumerator Wait() {
    //    yield return new WaitForSeconds(5);
    //    SaveTheGame("Game5");
    //}

    public void SaveTheGame(string filename) {
        
        WriteToFile(filename);
        SceneManager.LoadScene("Menu");
        PhotonNetwork.Disconnect();
    }

    public void WriteToFile(string filename) {
        Debug.Log("Beginning SaveTheGame");
        string path = System.IO.Directory.GetCurrentDirectory();
        using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(path + filename + ".txt"))
        {
            int index = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
            string[] players = (string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"];
            int gameSize = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"];
            int narratorPos = (int)PhotonNetwork.CurrentRoom.CustomProperties["NarratorPosition"];
            int numMonsters = (int)PhotonNetwork.CurrentRoom.CustomProperties["MonstersAtCastle"];
            int numFarmers = (int)PhotonNetwork.CurrentRoom.CustomProperties["FarmersAtCastle"];
            bool herbsAtCastle = (bool)PhotonNetwork.CurrentRoom.CustomProperties["HerbAtCastle"];
            bool skralDefeated = (bool)PhotonNetwork.CurrentRoom.CustomProperties["SkralAtCastleDefeated"];
            file.WriteLine(index.ToString());
            file.WriteLine(narratorPos.ToString());
            file.WriteLine(numMonsters.ToString());          
            file.WriteLine(numFarmers.ToString());          
            file.WriteLine(herbsAtCastle.ToString());          
            file.WriteLine(skralDefeated.ToString());          
            foreach (Player p in PhotonNetwork.PlayerList) {
                string type = (string)p.CustomProperties["Type"];
                int pos = (int)p.CustomProperties["pos"];
                int hours = (int)p.CustomProperties["Hours"];
                int wp = (int)p.CustomProperties["wp"];
                int sp = (int)p.CustomProperties["sp"];
                int farmer = (int)p.CustomProperties["farmer"];
                int gold = (int)p.CustomProperties["gold"];
                bool herb = (bool)p.CustomProperties["herb"];
                int wineskin = (int)p.CustomProperties["wineskin"];
                int shield = (int)p.CustomProperties["shield"];
                int falcon = (int)p.CustomProperties["falcon"];
                int telescope = (int)p.CustomProperties["telescope"];
                int bow = (int)p.CustomProperties["bow"];
                int helm = (int)p.CustomProperties["helm"];
                int brew = (int)p.CustomProperties["brew"];
                int yellowStone = (int)p.CustomProperties["yellowStone"];
                int blueStone = (int)p.CustomProperties["blueStone"];
                int greenStone = (int)p.CustomProperties["greenStone"];

                string ln = type + "\t" + pos + "\t" + hours + "\t" + wp + "\t" + sp + "\t" + farmer + "\t" + gold + "\t" + herb + "\t" + wineskin + "\t" + shield + "\t" + falcon + "\t" + telescope + "\t" + bow + "\t" + helm + "\t" + brew + "\t" + yellowStone + "\t" + blueStone + "\t" + greenStone;
                file.WriteLine(ln);

            }  
            GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
            foreach (GameObject obj in monsters) {
                Monster m = obj.GetComponent<Monster>();
                file.WriteLine(m.type + "\t" + m.pos);
            }
            GameObject[] wells = GameObject.FindGameObjectsWithTag("Well");
            foreach (GameObject obj in wells) {
                Wells w = obj.GetComponent<Wells>();
                file.WriteLine("well" + "\t" + w.pos + "\t" + w.wellStatus.ToString());
            }
            GameObject[] fogs = GameObject.FindGameObjectsWithTag("Fog");
            foreach (GameObject obj in fogs) {
                GameBoardRegion regionWithFog = obj.GetComponent<GameBoardRegion>();
                file.WriteLine("fog" + "\t" + regionWithFog.num);
            }
            string line = "";
            foreach (string f in UnFlippedFog.fogOutcomes) {
                line = line + f + "\t";
            }
            file.WriteLine(line);
            GameObject[] golds = GameObject.FindGameObjectsWithTag("GoldToken");
            foreach (GameObject obj in golds) {
                GoldToken g = obj.GetComponent<GoldToken>();
                file.WriteLine("gold" + "\t" + g.pos + "\t" + g.amt);
            }
            GameObject[] yellow = GameObject.FindGameObjectsWithTag("RuneStoneYellow");
            foreach (GameObject obj in yellow) {
                RuneStone r = obj.GetComponent<RuneStone>();
                file.WriteLine("yellow" + "\t" + r.pos);
            }
            GameObject[] green = GameObject.FindGameObjectsWithTag("RuneStoneGreen");
            foreach (GameObject obj in green) {
                RuneStone r = obj.GetComponent<RuneStone>();
                file.WriteLine("green" + "\t" + r.pos);
            }
             GameObject[] blue = GameObject.FindGameObjectsWithTag("RuneStoneBlue");
            foreach (GameObject obj in blue) {
                RuneStone r = obj.GetComponent<RuneStone>();
                file.WriteLine("blue" +"\t"+ r.pos);
            }
            GameObject[] farmers = GameObject.FindGameObjectsWithTag("Farmer");
            foreach (GameObject obj in farmers) {
                Farmer f = obj.GetComponent<Farmer>();
                file.WriteLine("farmer" +"\t" + f.pos);
            }
            GameObject herbToken = GameObject.Find("Herb");
            if (herbToken == null) herbToken = GameObject.Find("Herb(Clone)");
            else {
                file.WriteLine("herb" + "\t" + herbToken.GetComponent<HerbController>().pos);
            }
            GameObject witch = GameObject.Find("WitchBrew");
            if (witch == null) witch = GameObject.Find("WitchBrew(Clone)");
            else {
                file.WriteLine("witch" + "\t" + witch.GetComponent<WitchBrewController>().pos);
            }
            Debug.Log("Closing Stream Writer");
            
            file.Close();
        }
    }
    
    
}