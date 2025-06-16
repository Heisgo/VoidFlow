using Codice.CM.Common;
using UnityEngine;
using VoidFlow;

public class EnemyIdleState : FlowState
{
    Transform player;

    public override void OnEnter()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        Debug.Log("Idle mode...");
    }

    public override void OnUpdate()
    {
        if (player == null) return;

        Transform idleTarget = machine.data.idleTarget;
        Vector3 currentPos = machine.transform.position;

        Vector3 targetPos = idleTarget != null ? idleTarget.position : currentPos;

        float distToTarget = Vector3.Distance(currentPos, targetPos);
        float distToPlayer = Vector3.Distance(currentPos, player.position);

        if (idleTarget != null && distToTarget > 0.1f)
        {
            machine.transform.position = Vector3.MoveTowards(
                currentPos,
                targetPos,
                machine.data.moveSpeed * Time.deltaTime
            );
        }

        if (distToPlayer < machine.data.viewDistance)
        {
            machine.TransitionTo<EnemyChaseState>();
        }
    }
}
