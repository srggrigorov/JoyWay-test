using UnityEngine;

namespace JoyWayTest.Scripts
{

    public class Pickup : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.up * 25 * Time.deltaTime);
        }
    }
}
