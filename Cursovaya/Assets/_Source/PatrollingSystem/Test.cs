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
        public float attackWaitTime = 2f; // Время ожидания после атаки в секундах
        public Transform attackPoint;
        public LayerMask playerLayers; // Слой для игрока
        public LayerMask wallLayers;   // Слой для стен
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

            // Поиск игрока
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Не найден объект с тегом 'Player'!");
                enabled = false;
                return;
            }
            _playerTransform = player.transform;
        }

        void Update()
        {
            // Обнаружение игрока
            if (!_isChasing)
            {
                CheckForPlayer();
            }

            // Патрулирование или преследование
            if (!_isChasing)
                Patrol();
            else
                ManageChaseAttackState();
        }

        private void CheckForPlayer()
        {
            // Проверка нахождения игрока в радиусе
            if (Vector3.Distance(transform.position, _playerTransform.position) <= detectionRange)
            {
                // Проверка на наличие стен между врагом и игроком
                Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
                float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayers);

                // Если луч не пересекает стену, игрок виден
                if (hit.collider == null)
                {
                    _isChasing = true;
                    _isWaitingAfterAttack = false;
                }
            }
        }

        private void Patrol()
        {
            // Перемещение к текущей точке патруля
            transform.position = Vector3.MoveTowards(transform.position, _currentPatrolTarget, patrolSpeed * Time.deltaTime);

            // Если достигли цели патруля
            if (Vector3.Distance(transform.position, _currentPatrolTarget) < 0.1f)
            {
                // Изменяем точку патрулирования на противоположную
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
                    // Во время ожидания не двигаемся
                    return;
                }
            }

            ChaseAndAttack();
        }
        private void ChaseAndAttack()
        {
            // Двигаемся к игроку
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, chaseSpeed * Time.deltaTime);

            // Если игрок в радиусе атаки
            if (Vector3.Distance(transform.position, _playerTransform.position) <= attackRange)
            {
                if (Time.time - _lastAttackTime >= attackCooldown)
                {
                    Attack();
                    _isWaitingAfterAttack = true;
                    _timeSinceLastAttack = 0; // Начинаем отсчет времени ожидания после атаки
                }
            }

            // Если игрок вышел из радиуса преследования или за стеной, возвращаем патрулирование
            if (Vector3.Distance(transform.position, _playerTransform.position) > detectionRange || IsPlayerBehindWall())
            {
                _isChasing = false;
                _isWaitingAfterAttack = false;
            }
        }

        private bool IsPlayerBehindWall()
        {
            // Проверка на наличие стен между врагом и игроком
            Vector2 directionToPlayer = (_playerTransform.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayers);

            // Если луч пересекает стену, игрок скрыт
            return hit.collider != null;
        }

        private void Attack()
        {
            // Анимация атаки
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            // Нанесение урона игроку
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
            foreach (Collider2D player in hitPlayers)
            {
                // Проверка на наличие компонента Damageable у игрока
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
            // Визуализация радиуса обнаружения и атаки
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            // Визуализация луча для проверки стен
            if (_playerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, _playerTransform.position);
            }

            // Визуализация точки атаки
            if (attackPoint != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(attackPoint.position, attackRange);
            }
        }
    }
}