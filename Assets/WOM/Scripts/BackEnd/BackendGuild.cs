using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// �ڳ� SDK namespace �߰�
using BackEnd;

public class BackendGuild {
    private static BackendGuild _instance = null;

    public static BackendGuild Instance {
        get {
            if (_instance == null) {
                _instance = new BackendGuild();
            }

            return _instance;
        }
    }
    
    public void CreateGuild(string guildName) {
        // 10���� ���� goodsCount�� ���, ��� ������ ���� ������ �����̸� ���� ������ �Ұ����ϴ�.  
        var bro = Backend.Guild.CreateGuildV3("���ϴ�_���_�̸�", 10);

        if (bro.IsSuccess() == false) {
            Debug.LogError("��带 �����ϴ��� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��尡 �����Ǿ����ϴ�. : " + bro);
    }

    public void RequestGuildJoin(string guildName) {
        var bro = Backend.Guild.GetGuildIndateByGuildNameV3(guildName);

        if (bro.IsSuccess() == false) {
            Debug.LogError($"{guildName}�� �˻��ϴ� �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        string guildInDate = bro.GetFlattenJSON()["guildInDate"].ToString();

        bro = Backend.Guild.ApplyGuildV3(guildInDate);

        if (bro.IsSuccess() == false) {
            Debug.LogError($"{guildName}({guildInDate})���� ���� ��û�� ������ �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log($"{guildName}({guildInDate})�� ��� ������ ���������� ��û�Ǿ����ϴ�. : " + bro);
    }
    public void AcceptGuildJoinRequest(int index) {
        var bro = Backend.Guild.GetApplicantsV3();

        if (bro.IsSuccess() == false) {
            Debug.LogError("��� ���� ��û ���� ����Ʈ�� �ҷ����� �� ������ �߻��߽��ϴϴ�. : " + bro);
            return;
        }

        Debug.Log("��� ���� ��û ���� ����Ʈ�� ���������� �ҷ��Խ��ϴ�. : " + bro);


        if (bro.FlattenRows().Count <= 0) {
            Debug.LogError("������ ��û�� ������ �������� �ʽ��ϴ�. : " + bro);
            return;
        }

        List<Tuple<string, string>> requestUserList = new List<Tuple<string, string>>();

        foreach (LitJson.JsonData requestJson in bro.FlattenRows()) {
            requestUserList.Add(new Tuple<string, string>(requestJson["nickname"].ToString(), requestJson["inDate"].ToString()));
        }

        string userString = "���� ��û ���\n";

        for (int i = 0; i < requestUserList.Count; i++) {
            userString += $"{index}. {requestUserList[i].Item1}({requestUserList[i].Item2})\n";
        }

        Debug.Log(userString);

        bro = Backend.Guild.ApproveApplicantV3(requestUserList[index].Item2);
        if (bro.IsSuccess() == false) {
            Debug.LogError($"{requestUserList[index].Item1}({requestUserList[index].Item2})�� ���� ��û�� �����ϴ� �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log($"{requestUserList[index].Item1}({requestUserList[index].Item2})�� ���� ��û ��û ������ �����߽��ϴ�.: " + bro);
    }
    public void ContributeGoods() {
        var bro = Backend.Guild.ContributeGoodsV3(goodsType.goods1, 100);

        if (bro.IsSuccess() == false) {
            Debug.LogError("��� ����� ������ �߻��߽��ϴ� . : " + bro);
        }

        Debug.Log("��� ���� ��ΰ� ���������� ����Ǿ����ϴ�. : " + bro);
    }
}