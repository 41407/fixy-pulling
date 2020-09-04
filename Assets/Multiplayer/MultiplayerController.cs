using UnityEngine;

namespace Multiplayer
{
    public class MultiplayerController : MonoBehaviour
    {
        [SerializeField] private GameObject bike;

        private void OnJoinedRoom()
        {
            if (bike.activeSelf) bike.SetActive(false);
            CreatePlayerObject();
        }

        private void CreatePlayerObject()
        {
            var position = bike.transform.position + Vector3.up;

            PhotonNetwork.Instantiate(bike.gameObject.name, position, Quaternion.identity, 0);
        }
    }
}
