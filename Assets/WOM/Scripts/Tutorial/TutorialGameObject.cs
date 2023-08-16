using UnityEngine;

public class TutorialGameObject : MonoBehaviour
{
    public int id;

    public void SetActiveObj(bool active)
    {
        gameObject.SetActive(active);
    }

}
