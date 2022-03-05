using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JoyWayTest.Scripts.Weapons
{
    public class WaterStone : Weapon
    {
        [SerializeField] private GameObject _waterDrop;
        [SerializeField] private float _throwForce;
        public float ThrowForce { get => _throwForce; }
        public float WaterDamage { get => _damage; }

        private void Start()
        {
            _canFire = true;
        }

        public override void Fire(InputAction.CallbackContext context)
        {
            if (!_canFire || context.canceled) return;
            Instantiate(_waterDrop, _firePoint.position, transform.rotation);
            _canFire = false;
            StartCoroutine(EnableFiring());
        }

        private IEnumerator EnableFiring()
        {
            yield return new WaitForSeconds(_timeBetweenShots);
            _canFire = true;
        }
    }
}
