using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ProjectGraphics
{
    public enum BuildingType
    {
        CAMP, FACTORY, MINE, LAB
    }
    public class CastleController : MonoBehaviour
    {
        public GameObject castleObject; //캐슬오브젝트
        [SerializeField] GameObject[] campBuild;//소환건물(캠프)
        [SerializeField] GameObject[] factoryBuild;//뼛조각 생산건물(가공소)
        [SerializeField] GameObject[] mineBuild;//골드 생산건물(광산)
        [SerializeField] GameObject[] labBuild;//던전업그레이드 건물(연구소)
        [SerializeField] GameObject dustEffect;
        List<GameObject> buildEffects = new List<GameObject>();

        //임시
        int factoryBuildLv = 0;
        int mineBuildLv = 0;
        int campBuildLv = 0;

        int labBuildLv = 0;


  
        private void Awake()
        {
            SetMineBuild(mineBuildLv);
            SetLabBuild(labBuildLv);
            SetCampBuild(campBuildLv);
            SetFactoryBuild(factoryBuildLv);
        }

        public void SetBuildUpgrade(BuildingType type, int level)
        {
            switch (type)
            {
                case BuildingType.CAMP: SetCampBuild(level); break;
                case BuildingType.FACTORY: SetFactoryBuild(level); break;
                case BuildingType.MINE: SetMineBuild(level); break;
                case BuildingType.LAB: SetLabBuild(level); break;
            }

            if (!castleObject.activeSelf) castleObject.SetActive(true);
        }

        private void SetCampBuild(int level)
        {
            for (int i = 0; i < campBuild.Length; i++)
            {
                if (i <= level) campBuild[i].SetActive(true);
                else campBuild[i].SetActive(false);
            }

            SetBuildEffect(campBuild[level].transform.position);
        }

        public void SetFactoryBuild(int level)
        {
            int lv = level / 4;

            for (int i = 0; i < factoryBuild.Length; i++)
            {
                if (i <= lv) factoryBuild[i].SetActive(true);
                else factoryBuild[i].SetActive(false);
            }

            SetBuildEffect(factoryBuild[lv].transform.position);
        }

        public void SetMineBuild(int level)
        {
            int lv = level / 4;
            for (int i = 0; i < mineBuild.Length; i++)
            {
                if (i <= lv) mineBuild[i].SetActive(true);
                else mineBuild[i].SetActive(false);
            }

            SetBuildEffect(mineBuild[lv].transform.position);
        }

        public void SetLabBuild(int level)
        {
            for (int i = 0; i < labBuild.Length; i++)
            {
                if (i <= level) labBuild[i].SetActive(true);
                else labBuild[i].SetActive(false);
            }

            SetBuildEffect(labBuild[level].transform.position);
        }

        private void SetBuildEffect(Vector3 pos)
        {
            for (int i = 0; i < buildEffects.Count; i++)
            {
                if (!buildEffects[i].activeSelf)
                {
                    buildEffects[i].SetActive(true);
                    buildEffects[i].transform.position = pos;
                    buildEffects[i].GetComponent<ParticleSystem>().Play();
                    return;
                }
            }

            GameObject e =
                Instantiate(dustEffect, pos, Quaternion.identity);
            e.transform.SetParent(castleObject.transform);

            buildEffects.Add(e);
        }
    }
}