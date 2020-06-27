using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
{
    [Header("Players")]
    public string playerPrefabPath;
    public Transform[] spawnPoints;
    public float respawnTime;
    private int playersInGame;
    public PlayerController[] players;

    // instance
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    // keep track of how many players have joined the game so we can 
    // spawn player characters when everyone's in
    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabPath, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        // initialize the player
        playerObj.GetComponent<PhotonView>().RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
