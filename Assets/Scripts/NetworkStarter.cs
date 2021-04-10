using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{

    [RequireComponent(typeof(NetworkManager))]
    public class NetworkStarter : MonoBehaviour
    {

        NetworkManager manager;
        public bool isServer;

        void Start()
        {
            manager = GetComponent<NetworkManager>(); 

            if (!NetworkClient.active) {
                if (isServer) {
                    manager.StartServer();
                } else {
                    manager.StartClient();
                }
            }
        }

    }
}
