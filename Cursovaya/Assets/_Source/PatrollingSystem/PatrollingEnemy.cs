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
            _currentPatrolTarget = _startPosition + Vector3.right * patrolRange; // Начинаем патруль вправо

            //Поиск игрока
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Не найден объект с тегом 'Player'!");
                enabled = false;
                return;
            }
            _playerTransform = player.transform;
            _playerDamageable = player.GetComponent<Damageable>();
        }

        void Update()
        {
            // Обнаружение игрока
            if (!_isChasing)
            {
                CheckForPlayer();
            }

            //Патрулирование или преследование
            if (!_isChasing)
                Patrol();
            else
                Chase();

            // Выполнения атаки
            if (_isChasing)
                TryAttack();
        }

        private void CheckForPlayer()
        {
            // Проверка нахождения игрока в радиусе
            if (Vector3.Distance(transform.position, _playerTransform.position) <= detectionRange)
            {
                _isChasing = true;
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

        private void Chase()
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerTransform.position, chaseSpeed * Time.deltaTime);

            // Если игрок вышел из радиуса преследования, возвращаем патрулирование
            if (Vector3.Distance(transform.position, _playerTransform.position) > detectionRange)
            {
                _isChasing = false;
            }
        }

        private void TryAttack()
        {
            // Если игрок в радиусе атаки
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

            // Проверка на наличие компонента Damageable перед попыткой нанесения урона
            if (_playerDamageable != null)
            {
                _playerDamageable.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogWarning("Компонент Damageable не найден на игроке!");
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
