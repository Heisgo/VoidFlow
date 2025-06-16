using UnityEngine;
using VoidFlow;

public class EnemyChaseState : FlowState
{
    Transform player;

    public override void OnEnter()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Debug.Log("Enemy chasing you (pretend that there's a skull emoji here)!");
    }

    public override void OnUpdate()
    {
        if (player == null) return;

        float dist = Vector3.Distance(machine.transform.position, player.position);
        if (dist > machine.data.forgetDistance)
        {
            Debug.Log("Shit, we lost sight of player. Going idle");
            machine.TransitionTo<EnemyIdleState>();
            return;
        }

        machine.transform.position = Vector3.MoveTowards(
            machine.transform.position,
            player.position,
            machine.data.moveSpeed * Time.deltaTime
        );
    }
}
