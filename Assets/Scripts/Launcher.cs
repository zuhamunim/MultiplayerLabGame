using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using System.Collections.Generic;

public class launcher : MonoBehaviourPunCallbacks
{
    public static launcher instance;
    public GameObject LoadingScene;
    public TMP_Text loadingtext;
    public GameObject createRoomScreen;
    public GameObject createdRoomScreen;
    public TMP_Text roomnameText;
    public TMP_InputField roomNameInputField;

    public GameObject roomBrowserScreen;
    public RoomButton roomButton;
    public List<RoomButton> allRoomButtons;

    public GameObject errorScreen;
    public TMP_Text errorText;

    public TMP_Text playerNameLabel;
    public List<TMP_Text> playerNameList;

    public void Start()
    {
        instance = this;
        LoadingScene.SetActive(true);
        loadingtext.text = "Connecting to Server....";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingtext.text = "Joining Lobby";
    }

    public override void OnJoinedLobby()
    {
        LoadingScene.SetActive(false);
    }

    public void openCreateRoomScreen()

    {
        createRoomScreen.SetActive(true);
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInputField.text))
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 10;

            PhotonNetwork.CreateRoom(roomNameInputField.text, roomOptions);
            LoadingScene.SetActive(true);
            loadingtext.text = "Creating Room....";
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Failed to create Room: " + message;
        errorScreen.SetActive(true);
    }

    public void CloseErrorScreen()
    {
        errorScreen.SetActive(false);
    }

    public override void OnCreatedRoom()
    {
        LoadingScene.SetActive(false);
        createdRoomScreen.SetActive(true);
        roomnameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void CloseMenus()
    {
        createdRoomScreen.SetActive(false);
        createRoomScreen.SetActive(false);
        LoadingScene.SetActive(false);
        roomBrowserScreen.SetActive(false);
    }

    public void LeaveRoom()
    {
        CloseMenus();

        LoadingScene.SetActive(true);
        loadingtext.text = "Leaving Room.....";
        PhotonNetwork.LeaveRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var item in allRoomButtons)
        {
            Destroy(item.gameObject);
        }
        allRoomButtons.Clear();

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                RoomButton newButton = Instantiate(roomButton, roomButton.transform.parent);
                newButton.SetButtonDetails(roomList[i]);
                newButton.gameObject.SetActive(true);
                allRoomButtons.Add(newButton);
            }
        }
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        CloseMenus();
        LoadingScene.SetActive(true);
        loadingtext.text = "Joining Roon...";
    }

    public override void OnJoinedRoom()
    {
        CloseMenus();
        createdRoomScreen.SetActive(true);
        PhotonNetwork.NickName = Random.Range(0, 1000).ToString();
        ListAllPlayerss();
        roomnameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ListAllPlayerss();
    }

    public void ListAllPlayerss()
    {
        Player[] playerslist = PhotonNetwork.PlayerList;

        foreach (var item in playerNameList)
        {
            Destroy(item.gameObject);
        }

        playerNameList.Clear();
        for (int i = 0; i < playerslist.Length; i++)
        {
            TMP_Text newPlayerLable = Instantiate(playerNameLabel, playerNameLabel.transform.parent);
            newPlayerLable.text = playerslist[i].NickName;
            // newPlayerLable.text = playerslist[i].UserId;
            newPlayerLable.gameObject.SetActive(true);
            playerNameList.Add(newPlayerLable);
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("FPS");
    }
}