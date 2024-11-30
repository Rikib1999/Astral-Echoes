using System.Collections;
using UnityEngine;

public class DeathAlert : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DisableAlert());
    }

    private IEnumerator DisableAlert()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}