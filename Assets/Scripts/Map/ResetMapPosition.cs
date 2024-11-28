using UnityEngine;

public class ResetMapPosition : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) Camera.main.transform.position = new Vector3(PlayerPrefs.GetFloat("currentSystemPositionX", 0), PlayerPrefs.GetFloat("currentSystemPositionY", 0), -10);
    }
}