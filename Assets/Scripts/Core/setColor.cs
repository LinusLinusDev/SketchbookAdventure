using UnityEngine;

public class setColor : MonoBehaviour
{
    private SpriteRenderer sr;
    private NewPlayer playerScript;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<NewPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerScript.color)
        {
            case 0:
                sr.color = new Color(0.25f, 0.7f, 0.25f, 1);
                break;
            case 1:
                sr.color = new Color(0.7f, 0.25f, 0.25f, 1);
                break;
            case 2:
                sr.color = new Color(0.25f, 0.25f, 0.7f, 1);
                break;
            case 3:
                sr.color = new Color(0.7f, 0.7f, 0.25f, 1);
                break;
        }
    }
}
