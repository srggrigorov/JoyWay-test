using UnityEngine;
using JoyWayTest.Scripts.Enemy;

namespace JoyWayTest.Scripts.Weapons
{
    public class WaterDrop : MonoBehaviour
    {
        [SerializeField] private WaterStone _waterStone;
        [SerializeField] private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody.AddForce(transform.forward * _waterStone.ThrowForce, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent<Scarecrow>(out Scarecrow scarecrow))
            {
                scarecrow.GetWet(_waterStone.WaterDamage);
            }
            Destroy(gameObject);
        }
    }
}
