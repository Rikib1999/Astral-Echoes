using UnityEngine;

public class AssignPlayerToGenerator : MonoBehaviour
{
    void Start()
    {
        var planetGenerator = FindFirstObjectByType<PlanetGenerator>();
        if (planetGenerator != null) planetGenerator.player = gameObject.transform;
    }
}