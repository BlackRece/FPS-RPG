using System;

namespace BlackRece.ProjectilePooler
{
    using System.Collections.Generic;

    using UnityEngine;

    public class ProjectilePooler : MonoBehaviour
    {
        [SerializeField] private int _batchAmount = 10;
        [SerializeField] private GameObject _prefab;
        private GameObject _container;
        private List<GameObject> _pool;

        private GameObject CreatePrefabInstance() {
            var instance = Instantiate(_prefab, _container.transform);
            instance.SetActive(false);
            return instance;
        }
        
        public GameObject GetGameObject(bool bAddedObjects = false) {
            foreach (var inactiveObject in _pool) 
                if (!inactiveObject.activeSelf) 
                    return inactiveObject;

            if (bAddedObjects)
                throw new Exception("ERROR: Can't return an inactive object!");
                
            IncreasePool();
            return GetGameObject(true);
        }
        
        private void IncreasePool()
        {
            for (var i = 0; i < _batchAmount; i++)
                _pool.Add(CreatePrefabInstance());
        }
        
        public void Init() 
        {
            _container = new GameObject();
            _pool = new List<GameObject>();

            _container.name = "Projectiles";

            IncreasePool();
        }
        
        private void OnDestroy()
        {
            for (int i = 0; i < _pool.Count; i++)
                Destroy(_pool[i]);
            
            _pool.Clear();

            Destroy(_container);
        }
    }
}
