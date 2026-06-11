using System;
using System.Collections.Generic;
using UnityEngine;

namespace FCG
{
    /// <summary>
    /// نظام cache للـ GameObjects للتقليل من استخدام GameObject.Find()
    /// </summary>
    /// <remarks>
    /// GameObject.Find() عملية مكلفة جداً (O(n)) تسبح في شجرة المشهد كاملة.
    /// هذا الـ class يوفر:
    /// - Lazy loading: البحث عند الحاجة فقط
    /// - Caching: حفظ النتيجة لإعادة استخدام
    /// - Invalidation: تحديث الـ cache عند الحاجة
    /// - Safe access: معالجة الحالات حيث قد لا يكون الكائن موجود
    /// 
    /// استخدام:
    /// <code>
    /// Transform cityMaker = ObjectCache.Instance.GetTransform("City-Maker");
    /// if (cityMaker != null)
    ///     cityMaker.position = Vector3.zero;
    /// </code>
    /// </remarks>
    public class ObjectCache : MonoBehaviour
    {
        private static ObjectCache _instance;
        public static ObjectCache Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject cacheGO = new GameObject("_ObjectCache");
                    _instance = cacheGO.AddComponent<ObjectCache>();
                    DontDestroyOnLoad(cacheGO);
                }
                return _instance;
            }
        }

        private Dictionary<string, Transform> _transformCache = new Dictionary<string, Transform>();
        private Dictionary<string, GameObject> _gameObjectCache = new Dictionary<string, GameObject>();

        /// <summary>
        /// الحصول على Transform من الـ cache أو البحث عنه
        /// </summary>
        /// <param name="name">اسم الكائن في المشهد</param>
        /// <param name="forceLookup">إذا كان true، سيتجاهل الـ cache ويبحث مجدداً</param>
        /// <returns>Transform إذا وُجد، null إذا لم يُوجد</returns>
        public Transform GetTransform(string name, bool forceLookup = false)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            // إذا كانت موجودة في الـ cache وليس هناك forceLookup
            if (_transformCache.TryGetValue(name, out Transform cached) && cached != null && !forceLookup)
                return cached;

            // البحث عن الكائن
            GameObject go = GameObject.Find(name);
            if (go != null)
            {
                _transformCache[name] = go.transform;
                return go.transform;
            }

            // إزالة من الـ cache إذا كان موجود لكن تم حذف الكائن
            if (_transformCache.ContainsKey(name))
                _transformCache.Remove(name);

            return null;
        }

        /// <summary>
        /// الحصول على GameObject من الـ cache أو البحث عنه
        /// </summary>
        public GameObject GetGameObject(string name, bool forceLookup = false)
        {
            Transform t = GetTransform(name, forceLookup);
            return t != null ? t.gameObject : null;
        }

        /// <summary>
        /// تخزين Transform مباشرة في الـ cache
        /// </summary>
        public void CacheTransform(string name, Transform transform)
        {
            if (string.IsNullOrEmpty(name) || transform == null)
                return;

            _transformCache[name] = transform;
        }

        /// <summary>
        /// تحديث الـ cache لكائن معين
        /// </summary>
        public void InvalidateCache(string name)
        {
            if (_transformCache.ContainsKey(name))
                _transformCache.Remove(name);
            if (_gameObjectCache.ContainsKey(name))
                _gameObjectCache.Remove(name);
        }

        /// <summary>
        /// مسح جميع الـ cache (يُستدعى عند تغيير المشهد)
        /// </summary>
        public void ClearAll()
        {
            _transformCache.Clear();
            _gameObjectCache.Clear();
        }

        /// <summary>
        /// الحصول على إحصائيات الـ cache
        /// </summary>
        public string GetCacheStats()
        {
            return $"Cached Transforms: {_transformCache.Count}, Cached GameObjects: {_gameObjectCache.Count}";
        }
    }

    /// <summary>
    /// نظام قائمة الحذف (Destroy Queue) لتأخير حذف الكائنات
    /// </summary>
    /// <remarks>
    /// DestroyImmediate() عملية خطرة تحذف الكائن فوراً أثناء تنفيذ الكود.
    /// هذا يمكن أن يسبب:
    /// - Exceptions إذا حاول كود آخر استخدام الكائن
    /// - Performance spikes بسبب garbage collection فوري
    /// - Stability issues في حلقات معقدة
    /// 
    /// هذا الـ class يجمع الكائنات المراد حذفها ويحذفها دفعة واحدة
    /// في نهاية الـ frame باستخدام Destroy() الآمن.
    /// </remarks>
    public class DestroyQueue : MonoBehaviour
    {
        private static DestroyQueue _instance;
        public static DestroyQueue Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject queueGO = new GameObject("_DestroyQueue");
                    _instance = queueGO.AddComponent<DestroyQueue>();
                    DontDestroyOnLoad(queueGO);
                }
                return _instance;
            }
        }

        private List<GameObject> _destroyQueue = new List<GameObject>();
        private int _totalDestroyed = 0;

        private void LateUpdate()
        {
            if (_destroyQueue.Count > 0)
            {
                int count = _destroyQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    if (_destroyQueue[i] != null)
                    {
                        Destroy(_destroyQueue[i]);
                        _totalDestroyed++;
                    }
                }
                _destroyQueue.Clear();
                ObjectCache.Instance.ClearAll();
            }
        }

        /// <summary>
        /// إضافة كائن إلى قائمة الحذف
        /// </summary>
        /// <remarks>
        /// الكائن سيتم حذفه في نهاية الـ frame بشكل آمن
        /// </remarks>
        public void Queue(GameObject obj)
        {
            if (obj != null && !_destroyQueue.Contains(obj))
                _destroyQueue.Add(obj);
        }

        /// <summary>
        /// إضافة مجموعة من الكائنات
        /// </summary>
        public void QueueRange(GameObject[] objects)
        {
            if (objects == null)
                return;

            foreach (GameObject obj in objects)
            {
                if (obj != null && !_destroyQueue.Contains(obj))
                    _destroyQueue.Add(obj);
            }
        }

        /// <summary>
        /// إضافة مجموعة من الكائنات من List
        /// </summary>
        public void QueueRange(List<GameObject> objects)
        {
            if (objects == null)
                return;

            foreach (GameObject obj in objects)
            {
                if (obj != null && !_destroyQueue.Contains(obj))
                    _destroyQueue.Add(obj);
            }
        }

        /// <summary>
        /// مسح القائمة (بدون حذف الكائنات)
        /// </summary>
        public void Clear()
        {
            _destroyQueue.Clear();
        }

        /// <summary>
        /// الحصول على عدد الكائنات المعلقة للحذف
        /// </summary>
        public int GetQueuedCount()
        {
            return _destroyQueue.Count;
        }

        /// <summary>
        /// الحصول على إحصائيات الحذف
        /// </summary>
        public string GetStats()
        {
            return $"Queued for Destroy: {_destroyQueue.Count}, Total Destroyed: {_totalDestroyed}";
        }
    }

    /// <summary>
    /// فئة مساعدة لتحسين الأداء
    /// </summary>
    public static class PerformanceHelper
    {
        /// <summary>
        /// بديل آمن لـ GameObject.Find()
        /// </summary>
        public static Transform FindTransformSafe(string name)
        {
            return ObjectCache.Instance.GetTransform(name);
        }

        /// <summary>
        /// بديل آمن لـ DestroyImmediate()
        /// </summary>
        public static void DestroyAsync(GameObject obj)
        {
            DestroyQueue.Instance.Queue(obj);
        }

        /// <summary>
        /// حذف مجموعة من الكائنات بشكل آمن
        /// </summary>
        public static void DestroyAsyncRange(GameObject[] objects)
        {
            DestroyQueue.Instance.QueueRange(objects);
        }

        /// <summary>
        /// حذف مجموعة من الكائنات من List بشكل آمن
        /// </summary>
        public static void DestroyAsyncRange(List<GameObject> objects)
        {
            DestroyQueue.Instance.QueueRange(objects);
        }

        /// <summary>
        /// تخزين كائن في الـ cache للوصول السريع
        /// </summary>
        public static void CacheTransform(string name, Transform transform)
        {
            ObjectCache.Instance.CacheTransform(name, transform);
        }

        /// <summary>
        /// الحصول على معلومات الأداء
        /// </summary>
        public static string GetPerformanceStats()
        {
            return $"{ObjectCache.Instance.GetCacheStats()} | {DestroyQueue.Instance.GetStats()}";
        }
    }
}
