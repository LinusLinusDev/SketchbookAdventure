using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Range(-1f, 1f)]
    public float parallaxAmount; 
    [System.NonSerialized] public Vector3 newPosition;
    private bool adjusted = false;

    public void MoveLayer(float positionChangeX, float positionChangeY)
    {
        newPosition = transform.localPosition;
        newPosition.x -= positionChangeX * (-parallaxAmount * 40) * (Time.deltaTime);
        newPosition.y -= positionChangeY * (-parallaxAmount * 40) * (Time.deltaTime);
        transform.localPosition = newPosition;
    }

}
