using System.Collections.Generic;
using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerController : MonoBehaviour
    {
        [SerializeField] private GameObject bike;

        private List<GameObject> bikes = new List<GameObject>();

        private void OnJoinedRoom()
        {
            if (bike.activeSelf) bike.SetActive(false);
            CreatePlayerObject();
        }

        private void CreatePlayerObject()
        {
            var position = bike.transform.position;

            var newBike = PhotonNetwork.Instantiate(bike.gameObject.name, position, Quaternion.identity, 0);

            bikes.Add(newBike);
            bikes.ForEach(b => b.GetComponentInChildren<Camera>().gameObject.SetActive(b.GetComponent<PhotonView>().isMine));
        }
    }
}
