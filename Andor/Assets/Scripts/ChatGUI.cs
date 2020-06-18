using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Chat;
using Photon.Realtime;
using Photon.Pun;

public class ChatGUI : MonoBehaviour, IChatClientListener
{

	public string[] ChannelsToJoinOnConnect; // set in inspector. Demo channels to join automatically.

	public int HistoryLengthToFetch; // set in inspector. Up to a certain degree, previously sent messages can be fetched for context

	public string UserName { get; set; }

	private string selectedChannelName; // mainly used for GUI/input

	public ChatClient chatClient;

    #if !PHOTON_UNITY_NETWORKING
    [SerializeField]
    #endif
    protected internal AppSettings chatAppSettings;

	public GameObject panel; 
    public GameObject button;
//	public GameObject ConnectingLabel;
	public RectTransform ChatPanel;     // set in inspector (to enable/disable panel)
	public GameObject UserIdFormPanel;
	public InputField InputFieldChat;   // set in inspector
	public Text CurrentChannelText;     // set in inspector
	public Toggle ChannelToggleToInstantiate; // set in inspector
	public GameObject exitButton;
 	bool state;

	private readonly Dictionary<string, Toggle> channelToggles = new Dictionary<string, Toggle>();

	public bool ShowState = true;
	public GameObject Title;
//	public Text StateText; // set in inspector
	public Text UserIdText; // set in inspector

	private static string WelcomeText = "Welcome to chat. Type \\help to list commands.";
	
	public void Start()
	{
		Debug.Log("chat GUI start function");
		DontDestroyOnLoad(this.gameObject);

	    this.UserIdText.text = "";
	   // this.StateText.text  = "";	//error says this is null ref - COULD THIS BE THE PROB?
	   // this.StateText.gameObject.SetActive(true);
	    this.UserIdText.gameObject.SetActive(true);
	    this.Title.SetActive(true);
	    this.ChatPanel.gameObject.SetActive(false);
	  //  this.ConnectingLabel.SetActive(false);
		//this.InputFieldChat.gameObject.SetActive(false);

		Debug.Log("chat GUI start function 3");

		if (string.IsNullOrEmpty(this.UserName))
		{
		    this.UserName = "user" + Environment.TickCount%99; //made-up username
		}

        #if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings;
        #endif

        bool appIdPresent = !string.IsNullOrEmpty(this.chatAppSettings.AppIdChat);

		this.UserIdFormPanel.gameObject.SetActive(appIdPresent);

		if (!appIdPresent)
		{
			Debug.LogError("You need to set the chat app ID in the PhotonServerSettings file in order to continue.");
		}
		Debug.Log("chat GUI start function 2");
	}

	public void Connect()
	{
		this.UserIdFormPanel.gameObject.SetActive(false);

		this.chatClient = new ChatClient(this);
        #if !UNITY_WEBGL
        this.chatClient.UseBackgroundWorkerForSending = true;
        #endif
		if(this.chatAppSettings.AppIdChat==null) {
			Debug.Log("chat app id is null");
		}


		this.chatClient.Connect(this.chatAppSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(this.UserName));

		this.ChannelToggleToInstantiate.gameObject.SetActive(false);
		Debug.Log("Connecting as: " + this.UserName);

	//    this.ConnectingLabel.SetActive(true);
	}

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnDestroy.</summary>
    public void OnDestroy()
    {
        if (this.chatClient != null)
        {
            this.chatClient.Disconnect();
        }
    }

    /// <summary>To avoid that the Editor becomes unresponsive, disconnect all Photon connections in OnApplicationQuit.</summary>
    public void OnApplicationQuit()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Disconnect();
		}
	}

	public void Update()
	{
		if (this.chatClient != null)
		{
			this.chatClient.Service(); // make sure to call this regularly! it limits effort internally, so calling often is ok!
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			Debug.Log("Sending Message");
			this.OnEnterSend();
		}

		// check if we are missing context, which means we got kicked out to get back to the Photon Demo hub.
	//	if ( this.StateText == null)
	//	{
	//		Destroy(this.gameObject);
	//		return;
	//	}

	//	this.StateText.gameObject.SetActive(this.ShowState); // this could be handled more elegantly, but for the demo it's ok.
	}

	public void OnEnterSend()
	{
		if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
		{
		    this.SendChatMessage(this.InputFieldChat.text);
			this.InputFieldChat.text = "";
		}
	}


	public int TestLength = 2048;
	private byte[] testBytes = new byte[2048];

	private void SendChatMessage(string inputLine)
	{
		if (string.IsNullOrEmpty(inputLine))
		{
			return;
		}
		this.chatClient.PublishMessage(this.selectedChannelName, inputLine);
	}

	public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
	{
		if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
		{
			Debug.LogError(message);
		}
		else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
		{
			Debug.LogWarning(message);
		}
		else
		{
			Debug.Log(message);
		}
	}

	public void OnConnected()
	{
		if (this.ChannelsToJoinOnConnect != null && this.ChannelsToJoinOnConnect.Length > 0)
		{
			this.chatClient.Subscribe(this.ChannelsToJoinOnConnect, this.HistoryLengthToFetch);
		}

	  //  this.ConnectingLabel.SetActive(false);

	    this.UserIdText.text = "Connected as "+ this.UserName;

		this.ChatPanel.gameObject.SetActive(true);

		this.chatClient.SetOnlineStatus(ChatUserStatus.Online); // You can set your online state (without a mesage).
	}

	public void OnDisconnected()
	{
	   // this.ConnectingLabel.SetActive(false);
	}

	public void OnChatStateChange(ChatState state)
	{
		// use OnConnected() and OnDisconnected()
		// this method might become more useful in the future, when more complex states are being used.

	//	this.StateText.text = state.ToString();
	}

	public void OnSubscribed(string[] channels, bool[] results)
	{
		// in this demo, we simply send a message into each channel. This is NOT a must have!
		foreach (string channel in channels)
		{
			this.chatClient.PublishMessage(channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.
			this.selectedChannelName = channel;
	/*		if (this.ChannelToggleToInstantiate != null)
			{
				this.InstantiateChannelButton(channel);

			}*/
		}

		Debug.Log("OnSubscribed: " + string.Join(", ", channels));

		
        // select first subscribed channel in alphabetical order
        if (this.chatClient.PublicChannels.Count > 0)
        {
            var l = new List<string>(this.chatClient.PublicChannels.Keys);
            l.Sort();
            string selected = l[0];
            if (this.channelToggles.ContainsKey(selected))
            {
                ShowChannel(selected);
                foreach (var c in this.channelToggles)
                {
                    c.Value.isOn = false;
                }
                this.channelToggles[selected].isOn = true;
                AddMessageToSelectedChannel(WelcomeText);
            }
        }
        

		// Switch to the first newly created channel
	    this.ShowChannel(channels[0]);
	}

	private void InstantiateChannelButton(string channelName)
	{
		if (this.channelToggles.ContainsKey(channelName))
		{
			Debug.Log("Skipping creation for an existing channel toggle.");
			return;
		}

		Toggle cbtn = (Toggle)Instantiate(this.ChannelToggleToInstantiate);
		cbtn.gameObject.SetActive(true);
		cbtn.GetComponentInChildren<ChannelSelector>().SetChannel(channelName);
		cbtn.transform.SetParent(this.ChannelToggleToInstantiate.transform.parent, false);

		this.channelToggles.Add(channelName, cbtn);
	}

	public void OnUnsubscribed(string[] channels)
	{
		foreach (string channelName in channels)
		{
			if (this.channelToggles.ContainsKey(channelName))
			{
				Toggle t = this.channelToggles[channelName];
				Destroy(t.gameObject);

				this.channelToggles.Remove(channelName);

				Debug.Log("Unsubscribed from channel '" + channelName + "'.");

				// Showing another channel if the active channel is the one we unsubscribed from before
				if (channelName == this.selectedChannelName && this.channelToggles.Count > 0)
				{
					IEnumerator<KeyValuePair<string, Toggle>> firstEntry = this.channelToggles.GetEnumerator();
					firstEntry.MoveNext();

				    this.ShowChannel(firstEntry.Current.Key);

					firstEntry.Current.Value.isOn = true;
				}
			}
			else
			{
				Debug.Log("Can't unsubscribe from channel '" + channelName + "' because you are currently not subscribed to it.");
			}
		}
	}

	public void OnGetMessages(string channelName, string[] senders, object[] messages)
	{
		if (channelName.Equals(this.selectedChannelName))
		{
			// update text
		    this.ShowChannel(this.selectedChannelName);
		}
	}

	public void OnPrivateMessage(string sender, object message, string channelName)
	{
		// as the ChatClient is buffering the messages for you, this GUI doesn't need to do anything here
		// you also get messages that you sent yourself. in that case, the channelName is determinded by the target of your msg
	//	this.InstantiateChannelButton(channelName);

		byte[] msgBytes = message as byte[];
		if (msgBytes != null)
		{
			Debug.Log("Message with byte[].Length: "+ msgBytes.Length);
		}
		if (this.selectedChannelName.Equals(channelName))
		{
		    this.ShowChannel(channelName);
		}
	}
	public void OnStatusUpdate(string user, int status, bool gotMessage, object message){
	}

    public void OnUserSubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserSubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        Debug.LogFormat("OnUserUnsubscribed: channel=\"{0}\" userId=\"{1}\"", channel, user);
    }

    public void AddMessageToSelectedChannel(string msg)
	{
		ChatChannel channel = null;
		bool found = this.chatClient.TryGetChannel(this.selectedChannelName, out channel);
		if (!found)
		{
			Debug.Log("AddMessageToSelectedChannel failed to find channel: " + this.selectedChannelName);
			return;
		}

		if (channel != null)
		{
			channel.Add("Bot", msg,0); //TODO: how to use msgID?
		}
	}

	public void ShowChannel(string channelName)
	{
		if (string.IsNullOrEmpty(channelName))
		{
			return;
		}

		ChatChannel channel = null;
		bool found = this.chatClient.TryGetChannel(channelName, out channel);
		if (!found)
		{
			Debug.Log("ShowChannel failed to find channel: " + channelName);
			return;
		}

		this.selectedChannelName = channelName;
		this.CurrentChannelText.text = channel.ToStringMessages();
		Debug.Log("ShowChannel: " + this.selectedChannelName);

		foreach (KeyValuePair<string, Toggle> pair in this.channelToggles)
		{
			pair.Value.isOn = pair.Key == channelName ? true : false;
		}
	}
}
