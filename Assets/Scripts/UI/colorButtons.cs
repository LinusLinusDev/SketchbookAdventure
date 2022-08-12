using UnityEngine;

public class colorButtons : MonoBehaviour
{
    private NewPlayer playerScript;
    [SerializeField] private int colorNumber;
    [SerializeField] private GameObject e;
    [SerializeField] private GameObject q;
    public int ammo;
    void Start()
    {
        playerScript = GameObject.FindWithTag("Player").GetComponent<NewPlayer>();
    }

    void Update()
    {
        ammo = playerScript.colorAmmo[colorNumber];
        if (playerScript.color == colorNumber || ammo <= 0)
        {
            e.SetActive(false);
            q.SetActive(false);
        }
        else if ((playerScript.color + 1) % 3 == colorNumber)
        {
            e.SetActive(true);
            q.SetActive(false);
        }
        else
        {
            e.SetActive(false);
            q.SetActive(true);
        }
    }
}
