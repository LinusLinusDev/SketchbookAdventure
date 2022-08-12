using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(float cameraPositionChangeX, float cameraPositionChangeY);
    public ParallaxCameraDelegate onCameraMove;
    private Vector2 oldCameraPosition;
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        onCameraMove += MoveLayer;
        FindLayers();
        oldCameraPosition.x = cam.transform.position.x;
        oldCameraPosition.y = cam.transform.position.y;
    }

    private void FixedUpdate()
    {
        if (cam.transform.position.x != oldCameraPosition.x || (cam.transform.position.y) != oldCameraPosition.y)
        {
            if (onCameraMove != null)
            {
                Vector2 cameraPositionChange;
                cameraPositionChange = new Vector2(oldCameraPosition.x - cam.transform.position.x, oldCameraPosition.y - cam.transform.position.y);
                onCameraMove(cameraPositionChange.x, cameraPositionChange.y);
            }

            oldCameraPosition = new Vector2(cam.transform.position.x, cam.transform.position.y);
        }
    }

    void FindLayers()
    {
        parallaxLayers.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            if (layer != null)
            {
                parallaxLayers.Add(layer);
            }
        }
    }

    void MoveLayer(float positionChangeX, float positionChangeY)
    {
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.MoveLayer(positionChangeX, positionChangeY);
        }
    }
}