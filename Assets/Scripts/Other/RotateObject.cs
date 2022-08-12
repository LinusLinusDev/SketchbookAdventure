using UnityEngine;


public class RotateObject : MonoBehaviour
{

    [SerializeField] float speed = 1;
    void Update()
    {
        transform.Rotate(Vector3.back * Time.deltaTime * speed);
    }
}
