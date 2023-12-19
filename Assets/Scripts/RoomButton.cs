using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomButton : MonoBehaviour
{
    public TMP_Text buttonText;
    public RoomInfo info;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void SetButtonDetails(RoomInfo roomInfo)
    {
        info = roomInfo;
        buttonText.text = roomInfo.Name.ToString();
    }

    public void JoinRoom()
    {
        launcher.instance.JoinRoom(info);
    }
}