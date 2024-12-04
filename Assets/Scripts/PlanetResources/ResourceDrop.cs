using Assets.Scripts.Enums;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Netcode;

namespace Assets.Scripts.PlanetResources
{
    public class ResourceDrop : NetworkBehaviour
    {
        [SerializeField] public eResourceType resourceType;

        private new ParticleSystem particleSystem;
        private int health;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            var particleMain = particleSystem.main;
            var particleEmission = particleSystem.emission;

            particleMain.startColor = GetColor();
            health = particleEmission.burstCount = GetParticleCount();
        }

        public void Damage(int damage)
        {
            if (!IsServer) return;
            
            health -= damage;
            if (health > 0) return;

            DieClientRpc();

            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            if (TryGetComponent<CircleCollider2D>(out var circleCollider)) circleCollider.enabled = false;

            particleSystem.Play();
        }
        [ClientRpc]
        private void DieClientRpc()
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            if (TryGetComponent<CircleCollider2D>(out var circleCollider)) circleCollider.enabled = false;

            particleSystem.Play();
        }

        private Color GetColor()
        {
            switch (resourceType)
            {
                case eResourceType.Water: return Color.blue;
                case eResourceType.Food: return Color.green;
                case eResourceType.Energy: return Color.white;
                case eResourceType.Fuel: return Color.red;
                case eResourceType.Metals: return Color.yellow;
                default: return Color.white;
            }
        }

        private int GetParticleCount()
        {
            return (int)(GetComponent<SpriteRenderer>().sprite.rect.size.x / 4);
        }
    }
}