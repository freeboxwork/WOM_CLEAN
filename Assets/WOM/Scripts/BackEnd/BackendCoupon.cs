using System.Collections.Generic;
using System.Text;
using UnityEngine;

// �ڳ� SDK namespace �߰�
using BackEnd;

public class BackendCoupon {
    private static BackendCoupon _instance = null;

    public static BackendCoupon Instance {
        get {
            if (_instance == null) {
                _instance = new BackendCoupon();
            }

            return _instance;
        }
    }

public void CouponUse(string couponNumber) {
        var bro = Backend.Coupon.UseCoupon(couponNumber);

        if (bro.IsSuccess() == false) {
            Debug.LogError("���� ��� �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("���� ��뿡 �����߽��ϴ�. : " + bro);

        if (BackendGameData.userData == null) {
            BackendGameData.Instance.GameDataGet();
        }

        if (BackendGameData.userData == null) {
            BackendGameData.Instance.GameDataInsert();
        }

        if (BackendGameData.userData == null) {
            Debug.LogError("userData�� �������� �ʽ��ϴ�.");
            return;
        }

        foreach (LitJson.JsonData item in bro.GetFlattenJSON()["itemObject"]) {
            if (item["item"].ContainsKey("itemType")) {
                int itemId = int.Parse(item["item"]["itemId"].ToString());
                string itemType = item["item"]["itemType"].ToString();
                string itemName = item["item"]["itemName"].ToString();
                int itemCount = int.Parse(item["itemCount"].ToString());

                if (BackendGameData.userData.inventory.ContainsKey(itemName)) {
                    BackendGameData.userData.inventory[itemName] += itemCount;
                } else {
                    BackendGameData.userData.inventory.Add(itemName, itemCount);
                }
            } else {
                Debug.LogError("�������� �ʴ� item�Դϴ�.");
            }
        }

        BackendGameData.Instance.GameDataUpdate();
    }
}