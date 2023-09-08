using UnityEngine;

public class DisableInfoBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DisableCheck();
    }


    void DisableCheck()
    {
        var evolLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataEvolution.level_evolution;
        if (evolLevel > 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        DisableCheck();
    }

}
