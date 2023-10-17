using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPopup : CastlePopupBase
{
    [SerializeField] List<DungeonSlot> dungeonSlots;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
        Init();
    }
    public override void HidePopup()
    {
        base.HidePopup();
    }

    public void Init()
    {
        foreach (var item in dungeonSlots)
        {
            item.UpdateUI();
        }
    }

}
