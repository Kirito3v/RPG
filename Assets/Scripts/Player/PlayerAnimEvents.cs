using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger() 
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger() 
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats entity = hit.GetComponent<EnemyStats>();
                player.stats.DoDamage(entity);
            }
        }
    }
}
