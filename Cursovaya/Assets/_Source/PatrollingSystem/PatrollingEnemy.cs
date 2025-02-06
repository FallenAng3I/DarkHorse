using PlayerSystem;
using UnityEngine;

namespace PatrollingSystem
{
    public class PatrollingEnemy : MonoBehaviour
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
        public LayerMask playerLayers;
        public Animator animator;

        private Transform _playerTransform;
        private Vector3 _startPosition;
        private Vector3 _currentPatrolTarget;
        private bool _isChasing = false;
        private float _lastAttackTime;
        private float _timeSinceLastAttack = 0f;
        private bool _isWaitingAfterAttack = false;
        private Player _player;

        private void Awake()
        {
            _startPosition = transform.position;
            _currentPatrolTarget = _startPosition + Vector3.right * patrolRange;

            //����� ������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("�� ������ ������ � ����� 'Player'!");
                enabled = false;
                return;
            }
            _playerTransform = player.transform;
            _player = player.GetComponent<Player>();
        }

        private void Update()
        {
            // ����������� ������
            if (!_isChasing)
            {
                CheckForPlayer();
            }

            //�������������� ��� �������������
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
                _isChasing = true;
                _isWaitingAfterAttack = false; // ����� ���������, ��� ���� �������� �������������, � �� �����
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

            // ���� ����� ����� �� ������� �������������, ���������� ��������������
            if (Vector3.Distance(transform.position, _playerTransform.position) > detectionRange)
            {
                _isChasing = false;
                _isWaitingAfterAttack = false;
            }
        }

        private void Attack()
        {
            //animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
            
            // �������� �� ������� ���������� Damageable ����� �������� ��������� �����
            if (_player != null)
            {
                _player.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("��������� Damageable �� ������ �� ������!");
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}