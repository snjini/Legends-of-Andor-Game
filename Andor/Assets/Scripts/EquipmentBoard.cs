using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class EquipmentBoard : MonoBehaviour
{
    // Start is called before the first frame update
    int i;
    int gold;
    Text goldText;
    GameObject goldPopup;

    void Start()
    {
        i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        gold = (int)PhotonNetwork.PlayerList[i].CustomProperties["gold"];

        GameObject wineSkinNum = GameObject.Find("WineSkinNumber");
        //wine skin
       	Button wineSkinAdd = GameObject.Find("Wineskinbuy").GetComponent<Button>();
       	wineSkinAdd.onClick.AddListener(() => this.add(wineSkinNum));
       	Button wineSkinMinus = GameObject.Find("Wineskinsubtract").GetComponent<Button>();
        wineSkinMinus.onClick.AddListener(() => this.subtract(wineSkinNum));

        //shield
        GameObject ShieldNumber = GameObject.Find("ShieldNumber");
        Button shieldAdd = GameObject.Find("ShieldBuy").GetComponent<Button>();
        shieldAdd.onClick.AddListener(() =>  this.add(ShieldNumber));
        Button shieldMinus = GameObject.Find("ShieldSubtract").GetComponent<Button>();
        shieldMinus.onClick.AddListener(() => this.subtract(ShieldNumber));

        //Falcon
        GameObject FalconNumber = GameObject.Find("FalconNumber");
        Button falconAdd = GameObject.Find("FalconBuy").GetComponent<Button>();
        falconAdd.onClick.AddListener(() =>  this.add(FalconNumber));
        Button falconMinus = GameObject.Find("FalconSubtract").GetComponent<Button>();
        falconMinus.onClick.AddListener(() => this.subtract(FalconNumber));

        //Bow
        GameObject BowNumber = GameObject.Find("BowNumber");
        Button bowAdd = GameObject.Find("BowBuy").GetComponent<Button>();
        bowAdd.onClick.AddListener(() =>  this.add(BowNumber));
        Button bowMinus = GameObject.Find("BowSubtract").GetComponent<Button>();
        bowMinus.onClick.AddListener(() => this.subtract(BowNumber));

        //Helm
        GameObject HelmNumber = GameObject.Find("HelmNumber");
        Button helmAdd = GameObject.Find("HelmBuy").GetComponent<Button>();
        helmAdd.onClick.AddListener(() =>  this.add(HelmNumber));
        Button helmMinus = GameObject.Find("HelmSubtract").GetComponent<Button>();
        helmMinus.onClick.AddListener(() => this.subtract(HelmNumber));

        //Telescope
        GameObject TelescopeNumber = GameObject.Find("TelescopeNumber");
        Button telescopeAdd = GameObject.Find("TelescopeBuy").GetComponent<Button>();
        telescopeAdd.onClick.AddListener(() =>  this.add(TelescopeNumber));
        Button telescopeMinus = GameObject.Find("TelescopeSubtract").GetComponent<Button>();
        telescopeMinus.onClick.AddListener(() => this.subtract(TelescopeNumber));

        //StrengthPoint
        GameObject StrengthNumber = GameObject.Find("StrengthNumber");
        Button strengthAdd = GameObject.Find("StrengthBuy").GetComponent<Button>();
        strengthAdd.onClick.AddListener(() =>  this.add(StrengthNumber));
        Button strengthMinus = GameObject.Find("StrengthSubtract").GetComponent<Button>();
        strengthMinus.onClick.AddListener(() => this.subtract(StrengthNumber));

        //Gold Number
        goldPopup = GameObject.Find("goldCanvas");
        goldText = goldPopup.transform.GetChild(2).GetComponent<Text>();
        goldText.text = (gold).ToString();

    }

    void add(GameObject o){
    	Text t = o.GetComponent<Text>();
    	int num = int.Parse(t.text);
    	++num;
    	t.text = (num).ToString();
        gold = gold -2;
        goldText.text = (gold).ToString();
    }

    void subtract(GameObject o){
    	Text t = o.GetComponent<Text>();
    	int num = int.Parse(t.text);
    	--num;
    	t.text = (num).ToString();

        gold = gold +2;
        goldText.text = (gold).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
