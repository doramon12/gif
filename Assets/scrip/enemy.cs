using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private float radius = 10f; // Bán kính để enemy đuổi theo

    [SerializeField]
    private Transform target; // Player

    [SerializeField]
    private float attackRadius = 2f; // Phạm vi tấn công

    [SerializeField]
    private float maxDistance = 50f; // Khoảng cách lớn nhất mà enemy có thể đi

    [SerializeField]
    private Animator animator; // Tham chiếu đến Animator

    private Vector3 originalPosition;

    public static int playerKill = 0;

    public enum EnemyState
    {
        Normal,Attack,Die
    }

    public EnemyState currentState;

    public int maxHP, currentHP;
    private bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        // Lấy vị trí ban đầu của enemy
        originalPosition = transform.position;
    }

    void Update()
    {
        if ( currentState == EnemyState.Die)
        {
            return;
        }
        // Tính khoảng cách từ enemy đến vị trí ban đầu
        var distanceToOriginal = Vector3.Distance(originalPosition, transform.position);

        // Tính khoảng cách từ enemy đến player
        var distanceToTarget = Vector3.Distance(target.position, transform.position);

        if (distanceToTarget <= attackRadius && distanceToOriginal <= maxDistance)
        {
            // Trong phạm vi tấn công, chuyển sang anim Attack
            navMeshAgent.SetDestination(transform.position); // Dừng di chuyển
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", true);
        }
        else if (distanceToTarget <= radius && distanceToOriginal <= maxDistance)
        {
            // Trong phạm vi đuổi theo, chuyển sang anim Run
            navMeshAgent.SetDestination(target.position);
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttacking", false);
        }
        else
        {
            // Ngoài phạm vi, quay về vị trí ban đầu và chuyển sang anim Idle
            navMeshAgent.SetDestination(originalPosition);
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttacking", false);

            if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
            {
                animator.SetTrigger("Idle");
            }
        }
    }

    public void ChangrState(EnemyState newState)
    {
        switch (currentState)
        {
            case EnemyState.Normal: break;
            case EnemyState.Attack: break;
            case EnemyState.Die: break;
        }

        switch (newState)
        {
            case EnemyState.Normal: break;
            case EnemyState.Attack: break;
            case EnemyState.Die:
                if (!isDead)
                {
                    animator.SetTrigger("die");
                    Destroy(gameObject, 2f);
                    isDead = true;
                    playerKill++;
                }
            break;
        }

        currentState = newState;
    }

    public void TakeDame(int dame)
    {
        currentHP -= dame;
        currentHP = Mathf.Max(0, currentHP);
        if(currentHP <= 0)
        {
            ChangrState(EnemyState.Die);
        }
    }
}
