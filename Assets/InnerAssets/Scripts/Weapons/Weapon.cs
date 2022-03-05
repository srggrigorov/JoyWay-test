using UnityEngine;
using UnityEngine.InputSystem;

namespace JoyWayTest.Scripts.Weapons
{
    [System.Serializable]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected float _damage;
        [SerializeField] protected float _timeBetweenShots;
        [SerializeField] protected Transform _firePoint;
        protected bool _canFire;
        protected Camera _camera;

        public abstract void Fire(InputAction.CallbackContext context);
    }
}
