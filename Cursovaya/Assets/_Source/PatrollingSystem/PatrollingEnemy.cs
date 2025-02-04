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
        public Transform attackPoint;
        public LayerMask playerLayers;
        ///public Animator animator;///

        private Transform _playerTransform;
        private Vector3 _startPosition;
        private Vector3 _currentPatrolTarget;
        private bool _isChasing = false;
        private float _lastAttackTime;
        private Damageable _playerDamageable;

        void Start()
        {
            _startPosition = transform.position;
            _currentPatrolTarget = _startPosition + Vector3.right * patrolRange; // �������� ������� ������

            //����� ������
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("�� ������ ������ � ����� 'Player'!");
                enabled = false;
                return;
            }
            _playerTransform = player.transform;
            _playerDamageable = player.GetComponent<Damageable>();
        }

        void Update()
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
                Chase();

            // ���������� �����
            if (_isChasing)
                TryAttack();
        }

        private void CheckForPlayer()
        {
            // �������� ���������� ������ � �������
            if (Vector3.Distance(transform.position, _playerTransform.position) <= detectionRange)
            {
                _isChasing = true;
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

        private void Chase()
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, chaseSpeed * Time.deltaTime);

            // ���� ����� ����� �� ������� �������������, ���������� ��������������
            if (Vector3.Distance(transform.position, _playerTransform.position) > detectionRange)
            {
                _isChasing = false;
            }
        }

        private void TryAttack()
        {
            // ���� ����� � ������� �����
            if (Vector3.Distance(transform.position, _playerTransform.position) <= attackRange)
            {
                if (Time.time - _lastAttackTime >= attackCooldown)
                {
                    Attack();
                }
            }
        }

        private void Attack()
        {
            ///animator.SetTrigger("Attack");///
            _lastAttackTime = Time.time;

            // �������� �� ������� ���������� Damageable ����� �������� ��������� �����
            if (_playerDamageable != null)
            {
                _playerDamageable.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("��������� Damageable �� ������ �� ������!");
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
