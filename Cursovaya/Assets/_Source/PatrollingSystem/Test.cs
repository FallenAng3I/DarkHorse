using PlayerSystem;
using UnityEngine;

namespace PatrollingSystem
{
    public class Test : MonoBehaviour
    {
        public float patrolSpeed = 3f;
        public float chaseSpeed = 6f;
        public float patrolRange = 5f;
        public float detectionRange = 8f;
        public float attackRange = 2f;
        public int attackDamage = 20;
        public float attackCooldown = 1f;
        public float attackWaitTime = 2f; // ����� �������� ����� ����� � ��������
        public Transform attackPoint;
        public LayerMask playerLayers; // ���� ��� ������
        public LayerMask wallLayers;   // ���� ��� ����
        public Animator animator;

        private Transform _playerTransform;
        private Vector3 _startPosition;
        private Vector3 _currentPatrolTarget;
        private bool _isChasing = false;
        private float _lastAttackTime;
        private float _timeSinceLastAttack = 0f;
        private bool _isWaitingAfterAttack = false;

        void Start()
        {
            _startPosition = transform.position;
            _currentPatrolTarget = _startPosition + Vector3.right * patrolRange;

            // ����� ������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("�� ������ ������ � ����� 'Player'!");
                enabled = false;
                return;
            }
            _playerTransform = player.transform;
        }

        void Update()
        {
            // ����������� ������
            if (!_isChasing)
            {
                CheckForPlayer();
            }

            // �������������� ��� �������������
            if (!_isChasing)
                Patrol();
            else
                ManageChaseAttackState();
        }

        private void CheckForPlayer()
        {
            // �������� ���������� ������ � �������
            if (Vector3.Distance(transform.position, _playerTransform.position) <= detectionRange)
            {
                // �������� �� ������� ���� ����� ������ � �������
                Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
                float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayers);

                // ���� ��� �� ���������� �����, ����� �����
                if (hit.collider == null)
                {
                    _isChasing = true;
                    _isWaitingAfterAttack = false;
                }
            }
        }

        private void Patrol()
        {
            // ����������� � ������� ����� �������
            transform.position = Vector3.MoveTowards(transform.position, _currentPatrolTarget, patrolSpeed * Time.deltaTime);

            // ���� �������� ���� �������
            if (Vector3.Distance(transform.position, _currentPatrolTarget) < 0.1f)
            {
                // �������� ����� �������������� �� ���������������
                if (_currentPatrolTarget == _startPosition + Vector3.right * patrolRange)
                    _currentPatrolTarget = _startPosition + Vector3.left * patrolRange;
                else
                    _currentPatrolTarget = _startPosition + Vector3.right * patrolRange;
            }
        }

        private void ManageChaseAttackState()
        {
            if (_isWaitingAfterAttack)
            {
                _timeSinceLastAttack += Time.deltaTime;
                if (_timeSinceLastAttack >= attackWaitTime)
                {
                    _isWaitingAfterAttack = false;
                    _timeSinceLastAttack = 0;
                }
                else
                {
                    // �� ����� �������� �� ���������
                    return;
                }
            }

            ChaseAndAttack();
        }
        private void ChaseAndAttack()
        {
            // ��������� � ������
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, chaseSpeed * Time.deltaTime);

            // ���� ����� � ������� �����
            if (Vector3.Distance(transform.position, _playerTransform.position) <= attackRange)
            {
                if (Time.time - _lastAttackTime >= attackCooldown)
                {
                    Attack();
                    _isWaitingAfterAttack = true;
                    _timeSinceLastAttack = 0; // �������� ������ ������� �������� ����� �����
                }
            }

            // ���� ����� ����� �� ������� ������������� ��� �� ������, ���������� ��������������
            if (Vector3.Distance(transform.position, _playerTransform.position) > detectionRange || IsPlayerBehindWall())
            {
                _isChasing = false;
                _isWaitingAfterAttack = false;
            }
        }

        private bool IsPlayerBehindWall()
        {
            // �������� �� ������� ���� ����� ������ � �������
            Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayers);

            // ���� ��� ���������� �����, ����� �����
            return hit.collider != null;
        }

        private void Attack()
        {
            // �������� �����
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            // ��������� ����� ������
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach (Collider2D player in hitPlayers)
            {
                // �������� �� ������� ���������� Damageable � ������
                Player playerDamageable = player.GetComponent<Player>();
                if (playerDamageable != null)
                {
                    playerDamageable.TakeDamage(attackDamage);
                }
            }

            _lastAttackTime = Time.time;
        }

        void OnDrawGizmosSelected()
        {
            // ������������ ������� ����������� � �����
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // ������������ ���� ��� �������� ����
            if (_playerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, _playerTransform.position);
            }

            // ������������ ����� �����
            if (attackPoint != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            }
        }
    }
}