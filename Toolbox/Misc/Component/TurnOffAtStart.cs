using System.Collections;
using UnityEngine;

public class TurnOffAtStart : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        gameObject.SetActive(false);
    }
}
