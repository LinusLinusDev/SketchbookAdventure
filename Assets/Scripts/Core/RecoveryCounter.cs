using UnityEngine;

public class RecoveryCounter : MonoBehaviour
{
    public float recoveryTime = 1f;
    [System.NonSerialized] public float counter;
    [System.NonSerialized] public bool recovering = false;

    void Update()
    {
        if(counter <= recoveryTime)
        {
            counter += Time.deltaTime;
            recovering = true;
        }
        else
        {
            recovering = false;
        }
    }
}
