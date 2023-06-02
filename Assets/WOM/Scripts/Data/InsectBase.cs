using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectBase : MonoBehaviour
{
    public string name;
    public EnumDefinition.InsectType insectType;
    public float damage;
    public float damageRate;
    public float criticalChance;
    public float criticalDamage;
    public float speed;
    public float goldBonus;
    public float bossDamage;

    public void Attack()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


//[System.Serializable]
//public class InsectBee : InsectBase
//{

//}

//[System.Serializable]
//public class InsectBeetle : InsectBase
//{

//}

//[System.Serializable]
//public class InsectMentis : InsectBase
//{

//}