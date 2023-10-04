using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 뒤끝 SDK namespace 추가
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
            Debug.LogError("쿠폰 사용 중 에러가 발생했습니다. : " + bro);
            return;
        }

        Debug.Log("쿠폰 사용에 성공했습니다. : " + bro);

        if (BackendGameData.userData == null) {
            BackendGameData.Instance.GameDataGet();
        }

        if (BackendGameData.userData == null) {
            BackendGameData.Instance.GameDataInsert();
        }

        if (BackendGameData.userData == null) {
            Debug.LogError("userData가 존재하지 않습니다.");
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
                Debug.LogError("지원하지 않는 item입니다.");
            }
        }

        BackendGameData.Instance.GameDataUpdate();
    }
}