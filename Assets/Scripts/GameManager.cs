using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("Giris Paneli")]
    public GameObject GirisPanel;
    public TMP_InputField Oyuncuİsimİnput;

    [Header("Secim Paneli")]
    public GameObject SecimPanel;

    [Header("Oda Olusturma Paneli")]
    public GameObject OdaOlusturPanel;
    public TMP_InputField OdaAdiİnput;
    public TMP_InputField Ozellik_1_Input_key;
    public TMP_InputField Ozellik_1_Input_value;
    public TMP_InputField Ozellik_2_Input_key;
    public TMP_InputField Ozellik_2_Input_value;

    [Header("Genel tanımlamalar")]
    public GameObject OdalistePanel;
    public TMP_Text OdalistesiText;

    public GameObject OdaicPanel;    
    public TMP_Text OdainfobilgileriText;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;      
       
    }

    public override void OnConnectedToMaster()
    {

        SetActivePanel(SecimPanel.name);

    }
    public override void OnLeftRoom()
    {
        SetActivePanel(SecimPanel.name);

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }
    public override void OnJoinedRoom()
    {
        SetActivePanel(OdaicPanel.name);
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("özellik", out object ozellikler);
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("renk", out object renkDegeri);
        OdainfobilgileriText.text = "Odanın Adı : " + PhotonNetwork.CurrentRoom.Name + " | Odayı kuran kişi :  " + PhotonNetwork.MasterClient.NickName + " " + ozellikler + " Renk: "+(string)renkDegeri+"\n";
    }
    public void GeriDon()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();

        }
        SetActivePanel(SecimPanel.name);
    }

    public void odadanCik()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OdaOlustur()
    {
        string odaAdi = OdaAdiİnput.text;
        string[] LobyOptions = new string[2];
        LobyOptions[0] = "özellik";
        LobyOptions[1] = "renk";
        Hashtable props = new Hashtable()
        {
            {"özellik", Ozellik_1_Input_value.text},
            {"renk", Ozellik_2_Input_value.text}
        };

        RoomOptions options = new RoomOptions()
        {
            MaxPlayers = 3,
            CustomRoomPropertiesForLobby = LobyOptions,
            CustomRoomProperties = props
        };

        PhotonNetwork.CreateRoom(odaAdi, options, TypedLobby.Default);
    }          

    public void GirisYap()
    {
        string playerName = Oyuncuİsimİnput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();

        }
        else
        {

            Debug.LogError("Oyuncu isimi tanımsız");
        }



    }

    public void OdaOzellikListesi()
    {      

        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();

        }
        SetActivePanel(OdalistePanel.name);
       
    }

    public void SetActivePanel(string activePanel)
    {
        GirisPanel.SetActive(activePanel.Equals(GirisPanel.name));
        SecimPanel.SetActive(activePanel.Equals(SecimPanel.name));
        OdaOlusturPanel.SetActive(activePanel.Equals(OdaOlusturPanel.name));       
        OdalistePanel.SetActive(activePanel.Equals(OdalistePanel.name));
        OdaicPanel.SetActive(activePanel.Equals(OdaicPanel.name));      
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        OdainfobilgileriText.text = "";
        foreach (RoomInfo info in roomList)
        {
            info.CustomProperties.TryGetValue("özellik", out object ozellikler);
            info.CustomProperties.TryGetValue("renk", out object renkDegeri);
            OdainfobilgileriText.text = "Oda adı: " + info.Name + " " +ozellikler+"\n";
            OdalistesiText.text = "Oda adı: " + info.Name + " " +(string)ozellikler+ " Renk: "+(string)renkDegeri+"\n";
        }

    }

    public void odaKur()
    {
        SetActivePanel(OdaOlusturPanel.name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Hashtable props = new Hashtable()
            {
                {"özellik", "Sıradan"},
                {"renk", "Beyaz"}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("özellik", out object ozellikler);
            PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("renk", out object renkDegeri);
            OdainfobilgileriText.text = "Odanın Adı : " + PhotonNetwork.CurrentRoom.Name + " | Odayı kuran kişi :  " + PhotonNetwork.MasterClient.NickName + " " + ozellikler + " Renk: "+(string)renkDegeri+"\n";

        }
    }
}
