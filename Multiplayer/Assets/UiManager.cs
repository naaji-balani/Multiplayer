using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Button _serverButton, _clientButton, _hostButton;


    private void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
        });


        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });


        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
    }
}
