using System.Collections;
using UnityEngine;
using static EnumDefinition;

public class InsectSpwanTimer : MonoBehaviour
{
    public EnumDefinition.InsectType insectType;
    public bool isTimerReady;
    public InsectManager insectManager;
    public InsectSpwanManager insectSpwanManager;

    void Start()
    {
        insectManager = GlobalData.instance.insectManager;
    }

    public void TimerStart()
    {
        isTimerReady = true;
        StartCoroutine(SpwanTimer());
    }

    public void TimerStop()
    {
        isTimerReady = false;
        StopAllCoroutines();
    }

    IEnumerator SpwanTimer()
    {
        while (isTimerReady)
        {
            var insect = insectManager.GetDisableST_InsectByType(insectType);
            var waitTime = GlobalData.instance.statManager.GetInsectSpwanTime(insectType);

            //Debug.Log(" T Y P E " + insectType + " -- " +waitTime);

            if (insect != null)
            {
                // set position
                var randomPos = insectSpwanManager.GetRandomPos();
                insect.gameObject.transform.position = randomPos;

                yield return new WaitForSeconds(waitTime);
                insect.gameObject.SetActive(true);
                GlobalData.instance.insectManager.AddEnableInsects(insect);

                yield return new WaitForEndOfFrame();

                if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.insectDamageUp))
                    insect.effectContoller.AuraEffect(true);
                // if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.unionDamageUp))
                //     insect.effectContoller.FireEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.allUnitSpeedUp))
                    insect.effectContoller.TrailEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.glodBonusUp))
                    insect.effectContoller.GoldEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(SkillType.allUnitCriticalChanceUp))
                    insect.effectContoller.ThunderEffect(true);

            }
            else
                yield return new WaitForSeconds(waitTime);

        }
    }
}
