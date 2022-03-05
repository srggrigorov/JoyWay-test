using System.Collections;
using UnityEngine;
using JoyWayTest.Scripts.Interfaces;
using UnityEngine.InputSystem;

namespace JoyWayTest.Scripts.Weapons
{
    public class Gun : Weapon
    {
        private Transform _cameraTransfrom;
        [SerializeField] private Animator _animator;
        [SerializeField] private TrailRenderer _bulletTracer;
        [SerializeField] private LayerMask _mask;
        private int hash_ShotTrigger;
        private void Start()
        {
            _camera = Camera.main;
            _canFire = true;
            _cameraTransfrom = _camera.transform;
            hash_ShotTrigger = Animator.StringToHash("Shot");
        }
        private void OnEnable()
        {
            _canFire = true;
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public override void Fire(InputAction.CallbackContext context)
        {
            if (!_canFire || context.canceled) return;
            Ray ray = new Ray(_cameraTransfrom.position, _cameraTransfrom.forward);
            RaycastHit hitInfo;
            TrailRenderer bulletTracer = Instantiate(_bulletTracer, _firePoint.position, Quaternion.identity);
            bulletTracer.AddPosition(_firePoint.position);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, _mask))
            {
                if (hitInfo.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable scarecrow))
                {
                    scarecrow.TakeDamage(_damage);
                }
                bulletTracer.transform.position = hitInfo.point;
            }
            else bulletTracer.transform.position = _cameraTransfrom.position + _cameraTransfrom.forward * 100;
            _canFire = false;
            StartCoroutine(EnableFiring());
            _animator.SetTrigger(hash_ShotTrigger);
        }

        private IEnumerator EnableFiring()
        {
            yield return new WaitForSeconds(_timeBetweenShots);
            _canFire = true;
        }
    }
}
