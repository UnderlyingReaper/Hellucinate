using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PuzzleOneController : MonoBehaviour
{
    public Sockets[] sockets;
    public int socketsConnected = 0;
    public GameObject wires;
    public bool allWiresConnected = false;
    public Light2D redLight, greenLight;

    public Transform lever;


    void Start()
    {
        wires.SetActive(true);

        for(int i = 0; i < sockets.Length; i++)
        {
            sockets[i].OnConnected += OnSocketConnected;
        }
    }

    public void OnSocketConnected(object sender, EventArgs e)
    {
        socketsConnected++;
        if(socketsConnected == sockets.Length) allWiresConnected = true;
    }
}
