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

public class GameBoardRegion : MonoBehaviourPun
{
    public static Dictionary<int, Vector3> regionCoords = new Dictionary<int, Vector3>() {

        {12, new Vector3(-7.4f, 7.1f, 15.0f)},

        {9, new Vector3(-14.0f, 12.3f, 15.0f)},

        {49, new Vector3(-4.1f, 8.1f, 15.0f)},

        {52, new Vector3(10.4f, 10.4f, 15.0f)},

        {15, new Vector3(-17.8f, 12.7f, 15.0f)},

        {41, new Vector3(-6.3f, -8.4f, 15.0f)},

        {65, new Vector3(12.4f, -0.6f, 15.0f)},

        {27, new Vector3(-18.0f, -10.4f, 15.0f)},

        {3, new Vector3(-13.1f, 2.3f, 15.0f)},

        {26, new Vector3(-19.7f, -10.7f, 15.0f)},

        {43, new Vector3(7.1f, -4.3f, 15.0f)},

        {34, new Vector3(-13.1f, -5.3f, 15.0f)},

        {40, new Vector3(-2.7f, -5.9f, 15.3f)},

        {47, new Vector3(4.9f, 4.9f, 15.0f)},

        {29, new Vector3(-9.3f, -5.5f, 15.0f)},

        {1, new Vector3(-14.0f, 4.4f, 15.0f)},

        {80, new Vector3(15.8f, -10.6f, 15.0f)},

        {35, new Vector3(-14.6f, -7.3f, 15.0f)},

        {30, new Vector3(-10.9f, -8.2f, 15.0f)},

        {51, new Vector3(2.8f, 8.3f, 15.0f)},

        {64, new Vector3(11.2f, 1.9f, 15.0f)},

        {14, new Vector3(-10.0f, 2.1f, 15.0f)},

        {11, new Vector3(-11.1f, 7.6f, 15.0f)},

        {55, new Vector3(5.2f, 10.4f, 15.0f)},

        {19, new Vector3(-14.2f, 0.1f, 15.0f)},

        {25, new Vector3(-19.0f, -6.2f, 15.0f)},

        {70, new Vector3(11.6f, -9.9f, 15.0f)},

        {22, new Vector3(-17.2f, -2.4f, 15.0f)},

        {69, new Vector3(12.2f, -8.5f, 15.0f)},

        {4, new Vector3(-16.7f, 3.8f, 15.0f)},

        {82, new Vector3(8.9f, -12.1f, 15.0f)},

        {2, new Vector3(-11.9f, 4.6f, 15.0f)},

        {46, new Vector3(5.5f, 1.5f, 15.0f)},

        {38, new Vector3(-1.3f, -2.1f, 15.3f)},

        {59, new Vector3(13.5f, 9.9f, 15.0f)},

        {17, new Vector3(-6.3f, 1.8f, 15.0f)},

        {44, new Vector3(4.7f, -0.4f, 15.0f)},

        {62, new Vector3(17.6f, 7.2f, 15.0f)},

        {31, new Vector3(-16.5f, -8.9f, 15.0f)},

        {24, new Vector3(-19.2f, -3.0f, 15.0f)},

        {5, new Vector3(-18.8f, 4.4f, 15.0f)},

        {45, new Vector3(9.0f, -0.7f, 15.0f)},

        {66, new Vector3(14.8f, -3.8f, 15.0f)},

        {72, new Vector3(-11.5f, -2.8f, 15.0f)},

        {67, new Vector3(14.7f, -5.6f, 15.0f)},

        {37, new Vector3(-10.6f, -11.8f, 15.0f)},

        {6, new Vector3(-9.7f, 4.9f, 15.0f)},

        {0, new Vector3(-15.7f, 7.8f, 15.0f)},

        {61, new Vector3(16.8f, 3.2f, 15.0f)},

        {8, new Vector3(-11.7f, 10.3f, 15.0f)},

        {20, new Vector3(-16.7f, 0.7f, 15.0f)},

        {10, new Vector3(-12.1f, 0.7f, 15.0f)},

        {42, new Vector3(4.4f, -1.9f, 15.0f)},

        {81, new Vector3(11.3f, -11.5f, 15.0f)},

        {58, new Vector3(15.1f, 7.2f, 15.0f)},

        {56, new Vector3(8.8f, 5.2f, 15.0f)},

        {21, new Vector3(-19.2f, 1.2f, 15.0f)},

        {48, new Vector3(0.4f, 5.6f, 15.0f)},

        {36, new Vector3(-4.8f, -1.1f, 15.0f)},

        {16, new Vector3(-2.5f, 2.3f, 15.0f)},

        {68, new Vector3(13.5f, -7.3f, 15.0f)},

        {13, new Vector3(-5.7f, 5.4f, 15.0f)},

        {50, new Vector3(-1.5f, 9.0f, 15.0f)},

        {84, new Vector3(7.0f, -10.2f, 15.0f)},

        {7, new Vector3(-15.4f, 10.4f, 15.0f)},

        {53, new Vector3(4.4f, 7.3f, 15.0f)},

        {39, new Vector3(14.7f, -5.1f, 15.0f)},

        {33, new Vector3(-13.9f, -9.6f, 15.0f)},

        {23, new Vector3(-16.0f, -3.8f, 15.0f)},

        {54, new Vector3(7.0f, 7.9f, 15.0f)},

        {83, new Vector3(17.2f, -0.7f, 15.0f)},

        {57, new Vector3(10.4f, 9.1f, 15.0f)},

        {60, new Vector3(16.8f, 10.0f, 15.0f)},

        {63, new Vector3(13.0f, 5.1f, 15.0f)},

        {32, new Vector3(0.3f, 1.7f, 15.0f)},

        {71, new Vector3(9.3f, -4.6f, 15.0f)},

        {28, new Vector3(-6.5f, -4.1f, 15.0f)},

        {18, new Vector3(-9.2f, -1.4f, 15.0f)}
    };
    //When the mouse hovers over the GameObject, it turns red
    Color m_MouseOverColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    //This stores the GameObject’s original color (transparent)
    Color m_OriginalColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    //Get the GameObject’s sprite renderer to access the GameObject’s material and color
    SpriteRenderer s_Renderer;
    SortingGroup group;
    PolygonCollider2D s_Collider;
    static Hero hero;
    public int num;
    public static GameObject wellPopup;
    GameObject storePopup;
    public static Button yesButton;
    public static Button noButton;
    public static Button farmerYesButton;
    public static Button farmerNoButton;
    public static Button dropFarmerYesButton;
    public static Button dropFarmerNoButton;
    public static Button dropGoldYesButton;
    public static Button dropGoldNoButton;
    public static Button pickGoldYesButton;
    public static Button pickGoldNoButton;
    public static Button pickBrewYesButton;
    public static Button pickBrewNoButton;
    public static Button pickHerbYesButton;
    public static Button pickHerbNoButton;
    public static Button pickStoneYesButton;
    public static Button pickStoneNoButton;
    public static Button inviteYesButton;
    public static Button inviteNoButton;
    public static Button joinYesButton;
    public static Button joinNoButton;
    Button storeYesButton;
    Button storeNoButton;
    Button fightYesButton;
    Button fightNoButton;

    GameObject fightPopup;
    static private bool popupFalseDone;
    public static GameObject farmerPopup;
    public static GameObject dropFarmerPopup;
    public static GameObject dropGoldPopup;
    public static GameObject pickGoldPopup;
    public static GameObject pickBrewPopup;
    public static GameObject pickHerbPopup;
    public static GameObject pickStonePopup;
    public static GameObject inviteToFightPopup;
    public static GameObject joinFightPopup;

    public static PhotonView eventCard14Popup;
    public static PhotonView eventCard15Popup;
    public static PhotonView eventCard17Popup;
    public static PhotonView eventCard22Popup;
    public static PhotonView eventCardUnknownPopup;


    public bool regionsTrue = true;

    void Start()
    {
        s_Renderer = GetComponent<SpriteRenderer>();
        s_Renderer.material.color = m_OriginalColor;
        s_Collider = GetComponent<PolygonCollider2D>();

        //if fog and not flipped
        if (UnFlippedFog.fogs.Contains(this.num))
        {
            s_Renderer.sprite = Resources.Load("Regions/" + this.num.ToString() + "-fill", typeof(Sprite)) as Sprite;
            s_Renderer = GetComponent<SpriteRenderer>();
            this.transform.gameObject.tag = "Fog";
        }

        popupFalseDone = false;

        storePopup = GameObject.Find("StoreCanvas");
        storeYesButton = storePopup.transform.GetChild(2).GetComponent<Button>();
        storeNoButton = storePopup.transform.GetChild(1).GetComponent<Button>();
        storeYesButton.onClick.AddListener(() => SceneManager.LoadScene("Store"));
        storeNoButton.onClick.AddListener(() => setActiveFalse(storePopup, 4));
        setActiveFalse(storePopup, 4);

        wellPopup = GameObject.Find("WellCanvas");
        yesButton = wellPopup.transform.GetChild(2).GetComponent<Button>();
        noButton = wellPopup.transform.GetChild(1).GetComponent<Button>();
        yesButton.onClick.AddListener(() => setActiveFalse(wellPopup, 4));
        noButton.onClick.AddListener(() => setActiveFalse(wellPopup, 4));
        setActiveFalse(wellPopup, 4);

        fightPopup = GameObject.Find("FightCanvas");
        fightYesButton = fightPopup.transform.GetChild(2).GetComponent<Button>();
        fightNoButton = fightPopup.transform.GetChild(1).GetComponent<Button>();
        //fightYesButton.onClick.AddListener(() => SceneManager.LoadScene("FightScene"));
        fightNoButton.onClick.AddListener(() => setActiveFalse(fightPopup, 4));
        setActiveFalse(fightPopup, 4);

        inviteToFightPopup = GameObject.Find("InviteOptionCanvas");
        inviteYesButton = inviteToFightPopup.transform.GetChild(2).GetComponent<Button>();
        inviteNoButton = inviteToFightPopup.transform.GetChild(1).GetComponent<Button>();
        inviteYesButton.onClick.AddListener(() => SceneManager.LoadScene("FightScene"));
        inviteNoButton.onClick.AddListener(() => SceneManager.LoadScene("FightScene"));
        setActiveFalse(inviteToFightPopup, 4);

        joinFightPopup = GameObject.Find("JoinFightCanvas");
        joinYesButton = joinFightPopup.transform.GetChild(2).GetComponent<Button>();
        joinNoButton = joinFightPopup.transform.GetChild(1).GetComponent<Button>();
        //joinYesButton.onClick.AddListener(() => SceneManager.LoadScene("FightScene"));
        setActiveFalse(joinFightPopup, 4);

        dropFarmerPopup = GameObject.Find("DropCanvas");
        dropFarmerYesButton = dropFarmerPopup.transform.GetChild(2).GetComponent<Button>();
        dropFarmerNoButton = dropFarmerPopup.transform.GetChild(1).GetComponent<Button>();
        dropFarmerYesButton.onClick.AddListener(() => setActiveFalse(dropFarmerPopup, 4));
        dropFarmerNoButton.onClick.AddListener(() => setActiveFalse(dropFarmerPopup, 4));
        setActiveFalse(dropFarmerPopup, 4);

        farmerPopup = GameObject.Find("FarmerCanvas");
        farmerYesButton = farmerPopup.transform.GetChild(2).GetComponent<Button>();
        farmerNoButton = farmerPopup.transform.GetChild(1).GetComponent<Button>();
        farmerYesButton.onClick.AddListener(() => setActiveFalse(farmerPopup, 4));
        farmerNoButton.onClick.AddListener(() => setActiveFalse(farmerPopup, 4));
        setActiveFalse(farmerPopup, 4);

        pickStonePopup = GameObject.Find("PickStoneCanvas");
        pickStoneYesButton = pickStonePopup.transform.GetChild(2).GetComponent<Button>();
        pickStoneNoButton = pickStonePopup.transform.GetChild(1).GetComponent<Button>();
        pickStoneYesButton.onClick.AddListener(() => setActiveFalse(pickStonePopup, 4));
        pickStoneNoButton.onClick.AddListener(() => setActiveFalse(pickStonePopup, 4));
        setActiveFalse(pickStonePopup, 4);

        PolygonCollider2D collide36 = GameObject.Find("Region36").GetComponent<PolygonCollider2D>();
        PolygonCollider2D collide42 = GameObject.Find("Region42").GetComponent<PolygonCollider2D>();
        PolygonCollider2D collide38 = GameObject.Find("Region38").GetComponent<PolygonCollider2D>();
        PolygonCollider2D collide44 = GameObject.Find("Region44").GetComponent<PolygonCollider2D>();

        if (this.num == 0)
        {    //only drop set up the listeners so they are controlled by 1 region
            dropGoldPopup = GameObject.Find("DropGoldCanvas");
            dropGoldYesButton = dropGoldPopup.transform.GetChild(2).GetComponent<Button>();
            dropGoldNoButton = dropGoldPopup.transform.GetChild(1).GetComponent<Button>();
            dropGoldYesButton.onClick.AddListener(() => ExecuteDropGold());
            dropGoldYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide38, collide42, collide44)));
            dropGoldNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide38, collide42, collide44)));
            dropGoldYesButton.onClick.AddListener(() => setActiveFalse(dropGoldPopup, 4));
            dropGoldNoButton.onClick.AddListener(() => setActiveFalse(dropGoldPopup, 4));
            setActiveFalse(dropGoldPopup, 4);

            Button dropGoldButton = GameObject.Find("DropGoldButton").transform.GetChild(0).GetComponent<Button>();
            dropGoldButton.onClick.AddListener(() => setActiveTrue(dropGoldPopup, 4));
            dropGoldButton.onClick.AddListener(() => SetRegionsFalse(collide36, collide38, collide42, collide44));

        }

        pickGoldPopup = GameObject.Find("PickGoldCanvas");
        pickGoldYesButton = pickGoldPopup.transform.GetChild(2).GetComponent<Button>();
        pickGoldNoButton = pickGoldPopup.transform.GetChild(1).GetComponent<Button>();
        pickGoldYesButton.onClick.AddListener(() => setActiveFalse(pickGoldPopup, 4));
        pickGoldNoButton.onClick.AddListener(() => setActiveFalse(pickGoldPopup, 4));
        setActiveFalse(pickGoldPopup, 4);

        pickBrewPopup = GameObject.Find("PickBrewCanvas");
        pickBrewYesButton = pickBrewPopup.transform.GetChild(2).GetComponent<Button>();
        pickBrewNoButton = pickBrewPopup.transform.GetChild(1).GetComponent<Button>();
        pickBrewYesButton.onClick.AddListener(() => setActiveFalse(pickBrewPopup, 4));
        pickBrewNoButton.onClick.AddListener(() => setActiveFalse(pickBrewPopup, 4));
        setActiveFalse(pickBrewPopup, 4);

        pickHerbPopup = GameObject.Find("PickHerbCanvas");
        pickHerbYesButton = pickHerbPopup.transform.GetChild(2).GetComponent<Button>();
        pickHerbNoButton = pickHerbPopup.transform.GetChild(1).GetComponent<Button>();
        pickHerbYesButton.onClick.AddListener(() => setActiveFalse(pickHerbPopup, 4));
        pickHerbNoButton.onClick.AddListener(() => setActiveFalse(pickHerbPopup, 4));
        setActiveFalse(pickHerbPopup, 4);

        if (this.num == 0) {
           eventCard14Popup = GameObject.Find("EventCard14Popup").GetPhotonView();
            eventCard14Popup.gameObject.SetActive(false);

            eventCard15Popup = GameObject.Find("EventCard15Popup").GetPhotonView();
            eventCard15Popup.gameObject.SetActive(false);

            eventCard17Popup = GameObject.Find("EventCard17Popup").GetPhotonView();
            eventCard17Popup.gameObject.SetActive(false);

            eventCard22Popup = GameObject.Find("EventCard22Popup").GetPhotonView();
            eventCard22Popup.gameObject.SetActive(false);

            eventCardUnknownPopup = GameObject.Find("EventCardUnknown").GetPhotonView();
            eventCardUnknownPopup.gameObject.SetActive(false); 
        }

    }

    //this is the hero that will move when a region is clicked on
    //ie. if you are playing as wizard, the wizard will move and not the other heroes. 
    public static void setHero(Hero h)
    {
        hero = h;
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (hero.canMove(num))
            {
                //otherwise, update position to region that was clicked on
                hero.updatePos(s_Collider.bounds.center);
                hero.updateRegion(num);
                if (GameObject.Find("Region36") == null) return;    //game is over and board has been set inactive

                PolygonCollider2D collide36 = GameObject.Find("Region36").GetComponent<PolygonCollider2D>();
                PolygonCollider2D collide42 = GameObject.Find("Region42").GetComponent<PolygonCollider2D>();
                PolygonCollider2D collide38 = GameObject.Find("Region38").GetComponent<PolygonCollider2D>();
                PolygonCollider2D collide44 = GameObject.Find("Region44").GetComponent<PolygonCollider2D>();
                //can drop off a farmer
                if (num == 0 && (int)PhotonNetwork.LocalPlayer.CustomProperties["farmer"] > 0)
                {

                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    dropFarmerYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    dropFarmerYesButton.onClick.AddListener(() => ExecuteDropFarmer());
                    dropFarmerNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(dropFarmerPopup, 4);
                }

                if (num == 0 && (bool)PhotonNetwork.LocalPlayer.CustomProperties["herb"])
                { //at castle with herb
                    ExecuteDropHerb();
                }

                //if witch brew exists and is on that region
                GameObject brew = GameObject.Find("WitchBrew");
                if (brew == null) brew = GameObject.Find("WitchBrew(Clone)");
                if (brew != null && brew.GetComponent<WitchBrewController>().pos == this.num)
                {
                    int gold = (int)PhotonNetwork.LocalPlayer.CustomProperties["gold"];
                    int nbPlayers = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"];
                    if (gold >= nbPlayers + 1)
                    { //if they have enough gold to buy brew
                        //do pop up for picking up witches brew
                        collide36.gameObject.SetActive(false);
                        collide42.gameObject.SetActive(false);
                        collide38.gameObject.SetActive(false);
                        collide44.gameObject.SetActive(false);
                        pickBrewYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                        pickBrewYesButton.onClick.AddListener(() => ExecuteBuyBrew(brew, gold, nbPlayers + 1));
                        pickBrewNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                        setActiveTrue(pickBrewPopup, 4);
                    }
                }

                GameObject herb = GameObject.Find("Herb");
                if (herb == null) herb = GameObject.Find("Herb(Clone)");
                if (herb != null && herb.GetComponent<HerbController>().pos == this.num)
                {
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    pickHerbYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    pickHerbYesButton.onClick.AddListener(() => ExecutePickupHerb(herb));
                    pickHerbNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(pickHerbPopup, 4);
                }

                //check for runestones
                GameObject[] stones2 = GameObject.FindGameObjectsWithTag("RuneStoneBlue");
                GameObject stoneBlue = Array.Find(stones2, f => f.GetComponent<RuneStone>().pos == num);
                if (stoneBlue != null && stoneBlue.GetComponent<RuneStone>().pos == this.num)
                {
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    pickStoneYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    pickStoneYesButton.onClick.AddListener(() => ExecutePickupStone(stoneBlue, "blue"));
                    pickStoneNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(pickStonePopup, 4);
                }

                GameObject[] stones = GameObject.FindGameObjectsWithTag("RuneStoneYellow");
                GameObject stoneYellow = Array.Find(stones, f => f.GetComponent<RuneStone>().pos == num);

                if (stoneYellow != null && stoneYellow.GetComponent<RuneStone>().pos == this.num)
                {
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    pickStoneYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    pickStoneYesButton.onClick.AddListener(() => ExecutePickupStone(stoneYellow, "yellow"));
                    pickStoneNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(pickStonePopup, 4);
                }

                GameObject[] stones3 = GameObject.FindGameObjectsWithTag("RuneStoneGreen");
                GameObject stoneGreen = Array.Find(stones3, f => f.GetComponent<RuneStone>().pos == num);
                if (stoneGreen == null) stoneGreen = GameObject.Find("RuneStoneGreen(Clone)");
                if (stoneGreen != null && stoneGreen.GetComponent<RuneStone>().pos == this.num)
                {
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    pickStoneYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    pickStoneYesButton.onClick.AddListener(() => ExecutePickupStone(stoneGreen, "green"));
                    pickStoneNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(pickStonePopup, 4);
                }

                //at a well
                if (num == 5 || num == 35 || num == 45 || num == 55)
                {
                    List<GameObject> wells = new List<GameObject>(GameObject.FindGameObjectsWithTag("Well"));
                    Wells thisWell = null;
                    foreach (GameObject w in wells)
                    {
                        Wells temp = w.GetComponent<Wells>();
                        if (temp.pos == num && temp.wellFull.activeSelf)
                        {
                            thisWell = temp;
                        }
                    }
                    if (thisWell != null)
                    {
                        PhotonView wellView = thisWell.gameObject.GetPhotonView();
                        setActiveTrue(wellPopup, 4);
                        collide36.gameObject.SetActive(false);
                        collide42.gameObject.SetActive(false);
                        collide38.gameObject.SetActive(false);
                        collide44.gameObject.SetActive(false);
                        yesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                        yesButton.onClick.AddListener(() => wellView.RPC("DrinkWell", RpcTarget.AllBuffered, (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"]));
                        noButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    }
                }

                //at a store
                else if (num == 18 || num == 71 || num == 57)
                {
                    PhotonNetwork.AutomaticallySyncScene = false;
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    storeYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    storeNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(storePopup, 4);
                }

                // check for gold
                GameObject[] golds = GameObject.FindGameObjectsWithTag("GoldToken");
                GameObject foundGold = Array.Find(golds, f => f.GetComponent<GoldToken>().pos == num);
                if (foundGold != null)
                {
                    Debug.Log("Gold PhotonView: " + foundGold.GetPhotonView());
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    pickGoldYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    pickGoldYesButton.onClick.AddListener(() => ExecutePickupGold(foundGold));
                    pickGoldNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(pickGoldPopup, 4);

                }

                // check for farmer
                GameObject[] farmers = GameObject.FindGameObjectsWithTag("Farmer");
                GameObject found = Array.Find(farmers, f => f.GetComponent<Farmer>().pos == num);
                if (found != null)
                {
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);
                    farmerYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    farmerYesButton.onClick.AddListener(() => ExecutePickupFarmer(found));
                    farmerNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    setActiveTrue(farmerPopup, 4);
                }

                //check for monster
                GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");
                GameObject g = Array.Find(monsters, m => m.GetComponent<Monster>().pos == num);
                bool onRegion = true;

                if ((string)PhotonNetwork.LocalPlayer.CustomProperties["Type"] == "Archer" && g == null)
                {
                    //check for adj regions
                    onRegion = false;
                    int[] adjRegions = Graph.getAdjRegions(this.num);
                    foreach (int r in adjRegions)
                    {
                        g = Array.Find(monsters, m => m.GetComponent<Monster>().pos == r);
                        if (g != null) break;
                    }
                }
                else if ((int)PhotonNetwork.LocalPlayer.CustomProperties["bow"] > 0 && g == null)
                {
                    onRegion = false;
                    int[] adjRegions = Graph.getAdjRegions(this.num);
                    foreach (int r in adjRegions)
                    {
                        g = Array.Find(monsters, m => m.GetComponent<Monster>().pos == r);
                        if (g != null)
                        {
                            Debug.Log("Using a Bow");
                            break;
                        }
                    }
                }

                if (g != null && num != 0)
                {

                    if ((int)PhotonNetwork.LocalPlayer.CustomProperties["farmer"] > 0 && onRegion)
                    {
                        PhotonHashtable h = new PhotonHashtable();
                        h.Add("farmer", 0);
                        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
                        Debug.Log("farmer(s) died because on same space as monster");
                    }

                    Monster monst = g.GetComponent<Monster>();
                    PhotonNetwork.AutomaticallySyncScene = false;
                    GameObject fightController = GameObject.Find("FightController");
                    if (fightController == null) fightController = GameObject.Find("FightController(Clone)");
                    collide36.gameObject.SetActive(false);
                    collide42.gameObject.SetActive(false);
                    collide38.gameObject.SetActive(false);
                    collide44.gameObject.SetActive(false);

                    /*
                    fightYesButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    */
                    if (!onRegion && !((string)PhotonNetwork.LocalPlayer.CustomProperties["Type"]).Equals("Archer"))
                    {
                        fightYesButton.onClick.AddListener(() => UseBow());
                    }
                    fightYesButton.onClick.AddListener(() => setActiveTrue(inviteToFightPopup, 4));
                    fightYesButton.onClick.AddListener(() => fightController.GetPhotonView().RPC("AddToFight", RpcTarget.AllBuffered, hero.type));
                    fightNoButton.onClick.AddListener(() => StartCoroutine(SetRegionsToTrue(collide36, collide42, collide38, collide44)));
                    inviteYesButton.onClick.AddListener(() => fightController.GetComponent<Fight>().InviteFighters(g, monst.type, this.num, hero.type));
                    inviteNoButton.onClick.AddListener(() => fightController.GetComponent<Fight>().InitializeFight(g, monst.type, this.num, hero.type));
                    //joinYesButton.onClick.AddListener(() => fightController.GetComponent<Fight>().InitializeFight(monst.type, this.num, ""));


                    setActiveTrue(fightPopup, 4);
                    // SHOULD WE ADD THIS?: fightPopup.GetComponent<TextMeshProUGUI>().text = "Would you like to use a bow \nto fight the monster on the adjacent space?";
                }

                int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
                Debug.Log("Hero at " + (int)PhotonNetwork.PlayerList[i].CustomProperties["Hours"] + " hours");
                if ((int)PhotonNetwork.PlayerList[i].CustomProperties["Hours"] >= 10)
                {
                    hero.endTurn(); //if out of hours, set turn to next player
                }
            }
        }
    }

    public void UseBow()
    {
        int numBows = (int)PhotonNetwork.LocalPlayer.CustomProperties["bow"];
        PhotonHashtable p = new PhotonHashtable();
        p.Add("bow", numBows - 1);
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        string s = (string)PhotonNetwork.PlayerList[i].CustomProperties["Type"];
        PhotonNetwork.PlayerList[i].SetCustomProperties(p);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
    }

    public void SetRegionsFalse(PolygonCollider2D collide36, PolygonCollider2D collide42, PolygonCollider2D collide38, PolygonCollider2D collide44)
    {
        regionsTrue = false;
        collide36.gameObject.SetActive(false);
        collide42.gameObject.SetActive(false);
        collide38.gameObject.SetActive(false);
        collide44.gameObject.SetActive(false);
    }

    IEnumerator SetRegionsToTrue(PolygonCollider2D collide36, PolygonCollider2D collide42, PolygonCollider2D collide38, PolygonCollider2D collide44)
    {
        yield return new WaitUntil(() => popupFalseDone);
        collide36.gameObject.SetActive(true);
        collide42.gameObject.SetActive(true);
        collide38.gameObject.SetActive(true);
        collide44.gameObject.SetActive(true);
        popupFalseDone = false;
        regionsTrue = true;
    }

    IEnumerator EndTurnIndex()
    {
        yield return new WaitUntil(() => popupFalseDone);
        hero.endTurn(); //after moving, set turn to next player
    }

    void OnMouseEnter()
    {
        // Change the color of the GameObject to red when the mouse is over GameObject
        s_Renderer.material.color = m_MouseOverColor;
        s_Renderer.sortingLayerName = "CurrentRegion";

    }

    void OnMouseExit()
    {
        // Reset the color of the GameObject back to normal
        s_Renderer.material.color = m_OriginalColor;
        s_Renderer.sortingLayerName = "Regions";
    }

    public void ExecutePickupFarmer(GameObject found)
    {
        string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        GameObject.Find(s).GetComponent<Hero>().PickupFarmer();
        found.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
        Debug.Log("Farmer picked up");
    }

    public void ExecutePickupStone(GameObject found, String type)
    {
        Debug.Log("AAAA");
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        int itemNum = 0;
        if (type == "blue")
        {
            itemNum = (int)PhotonNetwork.PlayerList[i].CustomProperties["blueStone"];
            itemNum = itemNum + 1;
        }
        else if (type == "yellow")
        {
            itemNum = (int)PhotonNetwork.PlayerList[i].CustomProperties["yellowStone"];
            itemNum = itemNum + 1;
        }
        else if (type == "green")
        {
            itemNum = (int)PhotonNetwork.PlayerList[i].CustomProperties["greenStone"];
            itemNum = itemNum + 1;
        }
        PhotonHashtable h = new PhotonHashtable();
        if (type == "blue")
        {
            h.Add("blueStone", itemNum);
        }
        else if (type == "yellow")
        {
            h.Add("yellowStone", itemNum);
        }
        else if (type == "green")
        {
            h.Add("greenStone", itemNum);
        }
        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
        //destroy object on board
        found.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
    }

    public void ExecutePickupGold(GameObject found)
    {
        string s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        int gold = (int)PhotonNetwork.LocalPlayer.CustomProperties["gold"];
        Debug.Log(found.GetComponent<GoldToken>().amt);
        GameObject.Find(s).GetComponent<Hero>().PickupGold(found.GetComponent<GoldToken>().amt);
        found.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
        Debug.Log("Gold picked up");
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
    }

    public void ExecutePickupHerb(GameObject herb)
    {
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        PhotonHashtable h = new PhotonHashtable();
        h.Add("herb", true);
        PhotonNetwork.PlayerList[i].SetCustomProperties(h);
        //destroy object on board
        herb.GetPhotonView().RPC("Destroy", RpcTarget.AllBuffered);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
    }

    public void ExecuteBuyBrew(GameObject found, int goldAmt, int brewCost)
    {
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        int brew = (int)PhotonNetwork.LocalPlayer.CustomProperties["brew"];

        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];
        PhotonHashtable p = new PhotonHashtable();
        p.Add("brew", brew + 1);
        p.Add("gold", goldAmt - brewCost);
        PhotonNetwork.PlayerList[i].SetCustomProperties(p);
        Debug.Log("brew bought");
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
    }


    public void ExecuteDropHerb()
    {
        PhotonHashtable h = new PhotonHashtable();
        h.Add("herb", false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        String s = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);


        h = new PhotonHashtable();
        h.Add("HerbAtCastle", true);
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateGameStats", RpcTarget.AllBuffered);

    }

    public void ExecuteDropFarmer()
    {
        int heroFarmers = (int)PhotonNetwork.LocalPlayer.CustomProperties["farmer"];
        PhotonHashtable h = new PhotonHashtable();
        h.Add("farmer", 0);
        PhotonNetwork.LocalPlayer.SetCustomProperties(h);
        h = new PhotonHashtable();
        int numFarmers = (int)PhotonNetwork.CurrentRoom.CustomProperties["FarmersAtCastle"];
        numFarmers++;
        h.Add("FarmersAtCastle", numFarmers);
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);

        //Can't instantiate a scene object because not the master client - RPC to call for the master client to do it?
        string s = (string)PhotonNetwork.MasterClient.CustomProperties["Type"];
        GameObject masterHero = GameObject.Find(s);
        if (masterHero == null) masterHero = GameObject.Find(s + "(Clone)");
        masterHero.GetPhotonView().RPC("AddFarmerToCastle", RpcTarget.MasterClient);
        Debug.Log("farmer dropped off. There are now " + numFarmers + " farmers at the castle.");
        String toAdd = (string)PhotonNetwork.LocalPlayer.CustomProperties["Type"];
        StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)toAdd);


    }

    public void ExecuteDropGold()
    {
        int i = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        //int index = i-1;
        //if (index < 0) {
        //    index = (int)PhotonNetwork.CurrentRoom.CustomProperties["GameSize"] -1;
        //}
        string s = ((string[])PhotonNetwork.CurrentRoom.CustomProperties["Players"])[i];

        Hero local = GameObject.Find(s).GetComponent<Hero>();
        int gold = (int)PhotonNetwork.PlayerList[i].CustomProperties["gold"];
        if (gold < 1)
        {
            Debug.Log("You can't deposit more than you currently have");
        }
        else
        {
            GameObject[] golds = GameObject.FindGameObjectsWithTag("GoldToken");
            GameObject foundGold = Array.Find(golds, f => f.GetComponent<GoldToken>().pos == local.getPos());
            if (foundGold == null)
            {
                StartCoroutine(PlaceGold(local));
            }
            else
            {
                foundGold.GetPhotonView().RPC("AddAmt", RpcTarget.AllBuffered, (int)1);
            }
            PhotonHashtable h = new PhotonHashtable();
            h.Add("gold", gold - 1);
            PhotonNetwork.PlayerList[i].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);
        }
    }

    IEnumerator PlaceGold(Hero local)
    {
        yield return new WaitUntil(() => regionsTrue);
        string master = (string)PhotonNetwork.MasterClient.CustomProperties["Type"];
        GameObject masterHero = GameObject.Find(master);
        if (masterHero == null) masterHero = GameObject.Find(master + "(Clone)");
        masterHero.GetPhotonView().RPC("DropGoldToken", RpcTarget.MasterClient, (int)local.getPos());
    }

    public static void setActiveFalse(GameObject obj, int end)
    {
        for (int i = 0; i < end; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(false);
        }
        popupFalseDone = true;
    }

    private void setActiveTrue(GameObject obj, int end)
    {
        for (int i = 0; i < end; i++)
        {
            obj.transform.GetChild(i).gameObject.SetActive(true);
        }

    }

    public static Vector3 getCoordinates(int regionNum)
    {
        GameObject region = GameObject.Find("Region" + regionNum.ToString());
        PolygonCollider2D collider = region.GetComponent<PolygonCollider2D>();
        return collider.bounds.center;
    }

    [PunRPC]
    public void FlipFog()
    {
        //flip fog UI
        string outcome = UnFlippedFog.RemoveFromList(this.num);
        this.s_Renderer.sprite = Resources.Load("Regions/" + this.num.ToString(), typeof(Sprite)) as Sprite;
        this.transform.gameObject.tag = "Untagged";
        //generate fog outcome - FINISH
        Debug.Log(outcome);

        int index = (int)PhotonNetwork.CurrentRoom.CustomProperties["CurrentPlayerIndex"];
        string s = (string)PhotonNetwork.PlayerList[index].CustomProperties["Type"];

        if (outcome == "event")
        {
            //FINISH
            int tempNum = UnityEngine.Random.Range(1, 5);
            if (tempNum == 1)
            {
                //Cards.EventCard22();
                eventCard22Popup.gameObject.SetActive(true);
                eventCard22Popup.RPC("EventCard22", RpcTarget.AllBuffered);
                Debug.Log("eventcard 22 is called");
                eventCard22Popup.RPC("DisplayCard", RpcTarget.AllBuffered);

            }
            else if (tempNum == 2)
            {
                //Cards.EventCard15();
                eventCard15Popup.gameObject.SetActive(true);
                eventCard15Popup.RPC("EventCard15", RpcTarget.AllBuffered);
                Debug.Log("eventcard 15 is called");
                eventCard15Popup.RPC("DisplayCard", RpcTarget.AllBuffered);

            }
            else if (tempNum == 3)
            {
                //Cards.EventCardNumUnknown();
                eventCardUnknownPopup.gameObject.SetActive(true);
                eventCardUnknownPopup.RPC("EventCardNumUnknown", RpcTarget.AllBuffered);
                Debug.Log("eventcard num unknown is called");
                eventCardUnknownPopup.RPC("DisplayCard", RpcTarget.AllBuffered);

            }
            else if (tempNum == 4)
            {
                //Cards.EventCard14();
                eventCard14Popup.gameObject.SetActive(true);
                eventCard14Popup.RPC("EventCard14", RpcTarget.AllBuffered);
                Debug.Log("eventcard 14 is called");
                eventCard14Popup.RPC("DisplayCard", RpcTarget.AllBuffered);

            }
            else
            {
                //Cards.EventCard17();
                eventCard17Popup.gameObject.SetActive(true);
                eventCard17Popup.RPC("EventCard17", RpcTarget.AllBuffered);
                Debug.Log("eventcard 17 is called");
                eventCard17Popup.RPC("DisplayCard", RpcTarget.AllBuffered);

            }

        }
        else if (outcome == "witches")
        {
            //instantiate brew token
            string master = (string)PhotonNetwork.MasterClient.CustomProperties["Type"];
            GameObject masterHero = GameObject.Find(master);
            if (masterHero == null) masterHero = GameObject.Find(master + "(Clone)");

            masterHero.GetPhotonView().RPC("PlaceWitchToken", RpcTarget.MasterClient, (int)this.num);

            //instantiate herb token
            int randomRoll = UnityEngine.Random.Range(0, 6);
            int herbPos;
            if (randomRoll < 3) herbPos = 37;
            else if (randomRoll > 4) herbPos = 67;
            else herbPos = 61;
            Debug.Log(herbPos);
            masterHero.GetPhotonView().RPC("PlaceHerbToken", RpcTarget.MasterClient, (int)herbPos);


            //add the one free brew
            PhotonHashtable h = new PhotonHashtable();
            int oldBrew = (int)PhotonNetwork.PlayerList[index].CustomProperties["brew"];
            h.Add("brew", oldBrew + 1);
            PhotonNetwork.PlayerList[index].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        else if (outcome == "gold")
        {
            int oldGold = (int)PhotonNetwork.PlayerList[index].CustomProperties["gold"];
            PhotonHashtable h = new PhotonHashtable();
            h.Add("gold", oldGold + 3);
            PhotonNetwork.PlayerList[index].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        else if (outcome == "wp2")
        {
            int oldWP = (int)PhotonNetwork.PlayerList[index].CustomProperties["wp"];
            PhotonHashtable h = new PhotonHashtable();
            h.Add("wp", oldWP + 2);
            PhotonNetwork.PlayerList[index].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        else if (outcome == "wp3")
        {
            int oldWP = (int)PhotonNetwork.PlayerList[index].CustomProperties["wp"];
            PhotonHashtable h = new PhotonHashtable();
            h.Add("wp", oldWP + 3);
            PhotonNetwork.PlayerList[index].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        else if (outcome == "strength")
        {
            int oldSP = (int)PhotonNetwork.PlayerList[index].CustomProperties["sp"];
            PhotonHashtable h = new PhotonHashtable();
            h.Add("sp", oldSP + 1);
            PhotonNetwork.PlayerList[index].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        else if (outcome == "wineskin")
        {
            int oldWineskin = (int)PhotonNetwork.PlayerList[index].CustomProperties["wineskin"];
            PhotonHashtable h = new PhotonHashtable();
            h.Add("wineskin", oldWineskin + 1);
            PhotonNetwork.PlayerList[index].SetCustomProperties(h);
            StatsPanelController.statsPanel.GetPhotonView().RPC("UpdateCharStats", RpcTarget.AllBuffered, (string)s);

        }
        else
        { //gor
            GameObject m = (GameObject)PhotonNetwork.InstantiateSceneObject("Gor", getCoordinates(this.num), Quaternion.identity);
            PhotonView v3 = m.GetPhotonView();
            v3.RPC("DontDestroy", RpcTarget.AllBuffered);
            v3.RPC("SetPosition", RpcTarget.AllBuffered, this.num);
        }
    }

    [PunRPC]
    public void DontDestroy()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}