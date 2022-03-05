using UnityEngine;
using JoyWayTest.Scripts.Enemy;

namespace JoyWayTest.Scripts.Weapons
{
    public class FireParticles : MonoBehaviour
    {
        private ParticleSystem _fireParticles;

        private void Start()
        {
            _fireParticles = GetComponent<ParticleSystem>();
        }
        private void OnParticleCollision(GameObject other)
        {

            if (other.TryGetComponent<Scarecrow>(out Scarecrow scarecrow))
            {
                scarecrow.SetOnFire();
            }
        }
    }
}
