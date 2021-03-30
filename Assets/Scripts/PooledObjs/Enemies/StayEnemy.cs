using System.Collections;
using System.Text;
using UnityEngine;

public class StayEnemy: EnemyBase
{
    [SerializeField] Animator animator;


    private void OnEnable()
    {
        animator.SetBool("walk", false);
    }
}
