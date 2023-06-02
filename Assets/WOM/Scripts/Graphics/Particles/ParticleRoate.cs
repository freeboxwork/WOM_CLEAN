using UnityEngine;

namespace ProjectGraphics
{
    public class ParticleRoate : MonoBehaviour
    {
        //파티클 호출시 이 메소드 실행해 주세요.
        public void SetParticleRotateDirection(float zRot)
        {
            transform.rotation = Quaternion.Euler(0, 0, zRot);
        }

        private void OnDisable()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
