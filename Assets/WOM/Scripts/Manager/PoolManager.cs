using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolUIType
{
    //사용할 풀종류의 이름을 정하세요.
    Popup,
    Max
}

public class PoolManager : MonoBehaviour
{

    [System.Serializable]
    public class PoolUI
    {
        public PoolUIType poolName;
        public int quantity;
        public GameObject prefab;
        public Transform poolTr;
        public Queue<GameObject> poolingObjectQueue = new Queue<GameObject>();
    }

    [SerializeField] public PoolUI[] pools;

    public GameObject touchRound;
    private List<GameObject> touchRoundPool = new List<GameObject>();

    #region internal
    public static PoolManager instance;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CreatePoolsTr();
        CreateTouchRound();
    }

    //CreatePoolsTr 풀이 될 부모객체를 생성
    private void CreatePoolsTr()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            // GameObject poolTr = new GameObject(pools[i].poolName.ToString());
            // pools[i].poolTr = poolTr.transform;
            // poolTr.transform.SetParent(this.transform);
            PooiInitialize(pools[i], pools[i].quantity);
        }
    }
    //풀의 오브젝트 큐 리스트에 원하는 수량만큼 Init시킴
    void PooiInitialize(PoolUI pool, int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            pool.poolingObjectQueue.Enqueue(CreateNewObject(pool));
        }
    }

    //풀에 들어갈 객체들을 생성하여 반환
    GameObject CreateNewObject(PoolUI pool)
    {
        var newObj = Instantiate(pool.prefab);
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(pool.poolTr, false);
        return newObj;
    }


    //풀타입에 따른 풀을 찾아옵니다
    PoolUI FindPoolByPoolType(PoolUIType poolType)
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].poolName == poolType)
            {
                return pools[i];
            }
        }
        return null;
    }

    #endregion internal

    public GameObject GetPool(PoolUIType poolName)
    {
        PoolUI currentPool = FindPoolByPoolType(poolName);
        if (currentPool == null) Debug.LogError($"<color=red>[{poolName}]</color>의 풀을 찾지 못했습니다. 이름을 확인해주세요  </color>");

        if (currentPool.poolingObjectQueue.Count > 0)
        {
            var obj = currentPool.poolingObjectQueue.Dequeue();
            //obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(currentPool);
            newObj.gameObject.SetActive(true);
            //newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public void ReturnPool(GameObject obj, PoolUIType poolName)
    {
        PoolUI currentPool = FindPoolByPoolType(poolName);
        if (currentPool == null) Debug.LogError($"<color=red>[{poolName}]</color>의 풀을 찾지 못했습니다. 이름을 확인해주세요.  </color>");
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(currentPool.poolTr.transform, false);
        currentPool.poolingObjectQueue.Enqueue(obj);
    }



    public GameObject GetTouchRound()
    {
        foreach (GameObject t in touchRoundPool)
        {
            if (!t.activeInHierarchy)
            {
                return t;
            }
        }
        return null;
    }

    void CreateTouchRound()
    {
        for (int i = 0; i < 20; i++)
        {
            var newObj = Instantiate(touchRound);
            newObj.gameObject.SetActive(false);
            newObj.transform.SetParent(this.transform);
            touchRoundPool.Add(newObj);
        }


    }



}