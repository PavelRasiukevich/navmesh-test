using UnityEngine;

public class DeathParticle : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1.0f);
    }
}
