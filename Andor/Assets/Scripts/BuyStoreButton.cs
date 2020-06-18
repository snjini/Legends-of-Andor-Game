using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class BuyStoreButton : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameObject storePopup;
    Button closeButton;

    void Start()
    {
        Button b = transform.GetComponent<Button>();
    	b.onClick.AddListener(delegate() { this.buyStore(); });
        storePopup = GameObject.Find("closeCanvas");
        closeButton = storePopup.transform.GetChild(1).GetComponent<Button>();
        closeButton.onClick.AddListener(() => setActiveFalse(storePopup, 3));
        setActiveFalse(storePopup, 3);

    }

    void buyStore()
    {
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        int gold = (int)PhotonNetwork.PlayerList[i].CustomProperties["gold"];
        int wineSkinNum = getObjectInt(GameObject.Find("WineSkinNumber"));
        int ShieldNumber = getObjectInt(GameObject.Find("ShieldNumber"));
        int FalconNumber = getObjectInt(GameObject.Find("FalconNumber"));
        int BowNumber = getObjectInt(GameObject.Find("BowNumber"));
        int HelmNumber = getObjectInt(GameObject.Find("HelmNumber"));
        int TelescopeNumber = getObjectInt(GameObject.Find("TelescopeNumber"));
        int StrengthNumber = getObjectInt(GameObject.Find("StrengthNumber"));

        List<int> totalObj = new List<int>{wineSkinNum,ShieldNumber,FalconNumber,BowNumber,HelmNumber,TelescopeNumber, StrengthNumber};
        int cost = 0;
        bool notEnough = false;
        foreach(var num in totalObj){
            cost += num*2;
            if (cost > gold){
                notEnough = true;
                setActiveTrue(storePopup, 3);
            }
        }
        if (!notEnough) {
            int sp = (int)PhotonNetwork.PlayerList[i].CustomProperties["sp"];
            int wineskin = (int)PhotonNetwork.PlayerList[i].CustomProperties["wineskin"];
            int shield = (int)PhotonNetwork.PlayerList[i].CustomProperties["shield"];
            int bow = (int)PhotonNetwork.PlayerList[i].CustomProperties["bow"];
            int falcon = (int)PhotonNetwork.PlayerList[i].CustomProperties["falcon"];
            int telescope = (int)PhotonNetwork.PlayerList[i].CustomProperties["telescope"];
            int helm = (int)PhotonNetwork.PlayerList[i].CustomProperties["helm"];

            int newGoldAmount = gold-cost;
            PhotonHashtable p = new PhotonHashtable();
            p.Add("sp", sp+StrengthNumber);
            p.Add("gold", newGoldAmount);
            p.Add("wineskin",wineskin+wineSkinNum);
            p.Add("shield",shield+ShieldNumber);
            p.Add("falcon",bow+BowNumber);
            p.Add("bow",falcon+FalconNumber);
            p.Add("telescope",telescope+TelescopeNumber);
            p.Add("helm",helm+HelmNumber);
            PhotonNetwork.PlayerList[i].SetCustomProperties(p);
            LoadGame loading = new LoadGame();
            loading.LoadGameScene();
        }
    }

    int getObjectInt(GameObject o){
        Text t = o.GetComponent<Text>();
        int num = int.Parse(t.text);
        return num;
    }

    public static void setActiveFalse(GameObject obj, int end)
    {
        for (int i = 0; i < end; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void setActiveTrue(GameObject obj, int end)
    {
        for (int i = 0; i < end; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(true);
        }

    }

}
