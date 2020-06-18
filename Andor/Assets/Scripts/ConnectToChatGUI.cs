using UnityEngine;
using UnityEngine.UI;
//test
[RequireComponent(typeof (ChatGUI))]
public class ConnectToChatGUI : MonoBehaviour
{
    private const string UserNamePlayerPref = "NamePickUserName";

    public ChatGUI chatNewComponent;

    public InputField idInput;

    public void Start()
    {
        this.chatNewComponent = GameObject.Find("Scripts").GetComponent<ChatGUI>();
        Debug.Log("connect to chat gui start");
     //   GameObject canvas = GameObject.Find("ChatCanvas");
     //   canvas.SetActive(false);
    }


    // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
    public void Update()
    {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            this.StartChat();
        }
    }

    public void StartChat()
    {
        Debug.Log("starting chat");
        ChatGUI chatNewComponent = FindObjectOfType<ChatGUI>();
        chatNewComponent.UserName = this.idInput.text.Trim();
		chatNewComponent.Connect();
        enabled = false;

       // PlayerPrefs.SetString(ConnectToChatGUI.UserNamePlayerPref, chatNewComponent.UserName);
    }
}