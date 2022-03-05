using UnityEngine;
using UnityEngine.InputSystem;

namespace JoyWayTest.Scripts.Weapons
{
    public class FireStone : Weapon
    {
        [SerializeField] private ParticleSystem _firePS;
        public override void Fire(InputAction.CallbackContext context)
        {
            if (context.performed) _firePS.enableEmission = true;
            else if (context.canceled) _firePS.enableEmission = false;
        }
    }
}
