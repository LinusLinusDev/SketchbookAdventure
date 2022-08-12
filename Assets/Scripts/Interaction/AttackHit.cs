using System.Collections;
using UnityEngine;

public class AttackHit : MonoBehaviour
{
    private enum AttacksWhat { EnemyBase, NewPlayer };
    [SerializeField] private AttacksWhat attacksWhat;
    [SerializeField] private bool oneHitKill;
    [SerializeField] private float startCollisionDelay; //Some enemy types, like EnemyBombs, should not be able blow up until a set amount of time
    private int targetSide = 1; //Is the attack target on the left or right side of this object?
    [SerializeField] private GameObject parent; //This must be specified manually, as some objects will have a parent that is several layers higher
    [SerializeField] private bool isBomb = false; //Is the object a bomb that blows up when touching the player?
    [SerializeField] private int hitPower = 200; 

    void Start()
    {
        if (isBomb) StartCoroutine(TempColliderDisable());
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (parent.transform.position.x < col.transform.position.x)
        {
            targetSide = 1;
        }
        else
        {
            targetSide = -1;
        }

        if (attacksWhat == AttacksWhat.NewPlayer)
        {
            if (col.GetComponent<NewPlayer>() != null)
            {
                NewPlayer.Instance.GetHurt(targetSide, hitPower);
                if (isBomb) transform.parent.GetComponent<EnemyBase>().Die(); 
            }
        }

        else if (attacksWhat == AttacksWhat.EnemyBase && col.GetComponent<EnemyBase>() != null)
        {
            col.GetComponent<EnemyBase>().GetHurt(targetSide, hitPower);
        }

        else if (attacksWhat == AttacksWhat.EnemyBase && col.GetComponent<EnemyBase>() == null && col.GetComponent<Breakable>() != null)
        {
            col.GetComponent<Breakable>().GetHurt(hitPower);
        }

        if (isBomb && col.gameObject.layer == 8)
        {
            transform.parent.GetComponent<EnemyBase>().Die();
        }
    }
    
    IEnumerator TempColliderDisable()
    {
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        yield return new WaitForSeconds(startCollisionDelay);
        GetComponent<Collider2D>().enabled = true;
    }
}
