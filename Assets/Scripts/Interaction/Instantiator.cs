using UnityEngine;


public class Instantiator : MonoBehaviour
{
    [SerializeField] GameObject[] objects;
    [SerializeField] float amount;

    public void InstantiateObjects()
    {
        GameObject iObject;

        if (amount == 0)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                iObject = Instantiate(objects[i], transform.position, Quaternion.identity, null);
                if (iObject.GetComponent<Ejector>() != null)
                {
                    iObject.GetComponent<Ejector>().launchOnStart = true;
                }
            }
        }

        else if (objects.Length != 0)
        {
            for (int i = 0; i < amount; i++)
            {
                iObject = Instantiate(objects[0], transform.position, Quaternion.identity, null);
                if (iObject.GetComponent<Ejector>() != null)
                {
                    iObject.GetComponent<Ejector>().launchOnStart = true;
                }
            }

        }
    }
}
