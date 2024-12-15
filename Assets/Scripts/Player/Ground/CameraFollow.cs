using UnityEngine;

namespace Assets.Scripts.Player
{
    //Script for the Camera following the player
    public class CameraFollow : MonoBehaviour
    {
        private Vector3 velocity;

        [SerializeField] private Transform player;
        [SerializeField] private float smoothTime = 0.2f;

        private void LateUpdate()
        {
            if(player == null){return;}
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.position.x, player.position.y, transform.position.z), ref velocity, smoothTime);
        }

        public Transform Target
        {
            get { return player;}
            set {
                player = value;
                LateUpdate();
            }
        }
    }
}