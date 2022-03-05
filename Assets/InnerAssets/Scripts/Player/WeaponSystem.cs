using UnityEngine;
using JoyWayTest.Scripts.Weapons;
using JoyWayTest.Scripts.Enemy;

namespace JoyWayTest.Scripts.Player
{
    public class WeaponSystem : MonoBehaviour
    {
#nullable enable
        private Weapon? _leftWeapon;
        private Weapon? _rightWeapon;
#nullable disable
        [SerializeField] private Transform _leftHand;
        [SerializeField] private Transform _rightHand;
        [SerializeField] private LayerMask _pickupMask;

        private PlayerControls _playerControls;
        private Transform _cameraTransform;

        [Space(10)]
        [SerializeField] private GameObject _scarerowPrefab;
        [SerializeField] private GameObject _pickupPrefab;

        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerControls.Player.Enable();
        }

        private void Start()
        {
            _cameraTransform = Camera.main.transform;

            _playerControls.Player.ShootRight.performed += ((context) =>
            {
                _rightWeapon?.Fire(context);
            });
            _playerControls.Player.ShootRight.canceled += ((context) =>
            {
                _rightWeapon?.Fire(context);
            });
            _playerControls.Player.ShootLeft.performed += ((context) =>
            {
                _leftWeapon?.Fire(context);
            });
            _playerControls.Player.ShootLeft.canceled += ((context) =>
            {
                _leftWeapon?.Fire(context);
            });
            _playerControls.Player.ReloadScarecrow.performed += context => ReloadScarecrow();
            _playerControls.Player.EquipLeft.performed += context => EquipOrThrowWeapon(false);
            _playerControls.Player.EquipRight.performed += context => EquipOrThrowWeapon(true);
        }

        private void ReloadScarecrow()
        {
            Scarecrow scarecrow = FindObjectOfType<Scarecrow>();
            if (scarecrow != null) { scarecrow.Reload(); scarecrow.HealthChanged.Invoke(); }
            else Instantiate(_scarerowPrefab, new Vector3(0, 0.05f, 6), Quaternion.Euler(Vector3.up * 180));
        }

        private void EquipOrThrowWeapon(bool isRightHand)
        {
            Transform currentHand = (isRightHand) ? _rightHand : _leftHand;
            Weapon currentWeapon = (isRightHand) ? _rightWeapon : _leftWeapon;
            if (currentWeapon != null)
            {
                GameObject newPickup = Instantiate(_pickupPrefab, transform.position + Vector3.up * 1.3f, Quaternion.identity);
                currentWeapon.gameObject.transform.SetParent(newPickup.transform);
                currentWeapon.gameObject.transform.localPosition = Vector3.zero;
                currentWeapon.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                if (isRightHand) _rightWeapon = null;
                else _leftWeapon = null;
            }

            Ray ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1.5f, _pickupMask))
            {
                Weapon weapon = hitInfo.collider.GetComponentInChildren<Weapon>();
                weapon.gameObject.transform.SetParent(currentHand);
                weapon.gameObject.transform.localPosition = Vector3.zero;
                weapon.gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
                if (isRightHand) _rightWeapon = weapon;
                else _leftWeapon = weapon;
                Destroy(hitInfo.collider.gameObject);
            }
        }

    }

}
