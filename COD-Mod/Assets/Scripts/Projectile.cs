namespace BlackRece
{
    using UnityEngine;
    
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 20.0f;
        [SerializeField] private float _maxLifeTime = 5.0f;
        [SerializeField] private int _damage = 1;
        private float _lifeTime;

        public int Damage => _damage;
        
        public void Init(Transform tParent)
        {
            Transform tTransform = transform;
            tTransform.position = tParent.position;
            tTransform.rotation = tParent.rotation;
            transform.Translate(tTransform.forward * 2, Space.World);
            
            _lifeTime = _maxLifeTime;
            gameObject.SetActive(true);
        }
        
        public void Init(Vector3 pos, Vector3 rot)
        {
            transform.position = pos;
            transform.rotation = Quaternion.Euler(rot);
            transform.Translate(transform.forward * 2, Space.World);
            
            _lifeTime = _maxLifeTime;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            transform.Translate(transform.forward * _speed * Time.deltaTime, Space.World);

            _lifeTime -= Time.deltaTime;
            if (_lifeTime % 2 == 0)
                Debug.Break();

            if (_lifeTime <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            gameObject.SetActive(false);
        }
    }
}
