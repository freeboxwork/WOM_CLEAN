using System.Collections.Generic;
using System.Text;
using UnityEngine;

// �ڳ� SDK namespace �߰�
using BackEnd;

public class Post {
    public bool isCanReceive = false;

    public string title; // ���� ����
    public string content; // ���� ����
    public string inDate; // ���� inDate

    // string�� ���� ������ �̸�, int�� ����
    public Dictionary<string, int> postReward = new Dictionary<string, int>();

    public override string ToString() {
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive) {
            result += "���� ������\n";

            foreach (string itemKey in postReward.Keys) {
                result += $"| {itemKey} : {postReward[itemKey]}��\n";
            }
        }
        else {
            result += "�������� �ʴ� ���� �������Դϴ�.";
        }

        return result;
    }
}

public class BackendPost {
    private static BackendPost _instance = null;

    public static BackendPost Instance {
        get {
            if (_instance == null) {
                _instance = new BackendPost();
            }

            return _instance;
        }
    }

    private List<Post> _postList = new List<Post>();

    public void SavePostToLocal(LitJson.JsonData item) {
        foreach (LitJson.JsonData itemJson in item) {
            if (itemJson["item"].ContainsKey("itemType")) {
                int itemId = int.Parse(itemJson["item"]["itemId"].ToString());
                string itemType = itemJson["item"]["itemType"].ToString();
                string itemName = itemJson["item"]["itemName"].ToString();
                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                if (BackendGameData.userData.inventory.ContainsKey(itemName)) {
                    BackendGameData.userData.inventory[itemName] += itemCount;
                }
                else {
                    BackendGameData.userData.inventory.Add(itemName, itemCount);
                }

                Debug.Log($"�������� �����߽��ϴ�. : {itemName} - {itemCount}��");
            }
            else {
                Debug.LogError("�������� �ʴ� item�Դϴ�.");
            }
        }
    }

public void PostListGet(PostType postType) {
        var bro = Backend.UPost.GetPostList(postType);

        // ���� ��Ʈ �̸��� '������ ��Ʈ'�� �������� �ʾ��� ���,
        // �ش� ���� �ֿܼ��� ������ ��Ʈ �̸��� �����ϰ� �������ּ���
        string chartName = "������ ��Ʈ";

        if (bro.IsSuccess() == false) {
            Debug.LogError("���� �ҷ����� �� ������ �߻��߽��ϴ�.");
            return;
        }

        Debug.Log("���� ����Ʈ �ҷ����� ��û�� �����߽��ϴ�. : " + bro);

        if (bro.GetFlattenJSON()["postList"].Count <= 0) {
            Debug.LogWarning("���� ������ �������� �ʽ��ϴ�.");
            return;
        }

        foreach (LitJson.JsonData postListJson in bro.GetFlattenJSON()["postList"]) {
            Post post = new Post();

            post.title = postListJson["title"].ToString();
            post.content = postListJson["content"].ToString();
            post.inDate = postListJson["inDate"].ToString();

            // ������ ������
            if (postType == PostType.User) {
                if (postListJson["itemLocation"]["tableName"].ToString() == "USER_DATA") {
                    if (postListJson["itemLocation"]["column"].ToString() == "inventory") {
                        foreach (string itemKey in postListJson["item"].Keys) post.postReward.Add(itemKey, int.Parse(postListJson["item"][itemKey].ToString()));
                    } else {
                        Debug.LogWarning("���� �������� �ʴ� �÷� ���� �Դϴ�. : " + postListJson["itemLocation"]["column"].ToString());
                    }
                } else {
                    Debug.LogWarning("���� �������� �ʴ� ���̺� ���� �Դϴ�. : " + postListJson["itemLocation"]["tableName"].ToString());
                }
            } else {
                foreach (LitJson.JsonData itemJson in postListJson["items"]) {
                    if (itemJson["chartName"].ToString() == chartName) {
                        string itemName = itemJson["item"]["itemName"].ToString();
                        int itemCount = int.Parse(itemJson["itemCount"].ToString());

                        if (post.postReward.ContainsKey(itemName)) {
                            post.postReward[itemName] += itemCount;
                        } else {
                            post.postReward.Add(itemName, itemCount);
                        }

                        post.isCanReceive = true;
                    } else {
                        Debug.LogWarning("���� �������� �ʴ� ��Ʈ ���� �Դϴ�. : " + itemJson["chartName"].ToString());
                        post.isCanReceive = false;
                    }
                }
            }
            _postList.Add(post);
        }

        for (int i = 0; i < _postList.Count; i++) {
            Debug.Log($"{i}�� ° ����\n" + _postList[i].ToString());
        }
    }

public void PostReceive(PostType postType, int index) {
        if (_postList.Count <= 0) {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        if (index >= _postList.Count) {
            Debug.LogError($"�ش� ������ �������� �ʽ��ϴ�. : ��û index{index} / ���� �ִ� ���� : {_postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}�� {_postList[index].inDate} ���������� ��û�մϴ�.");

        var bro = Backend.UPost.ReceivePostItem(postType, _postList[index].inDate);

        if (bro.IsSuccess() == false) {
            Debug.LogError($"{postType.ToString()}�� {_postList[index].inDate} �������� �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log($"{postType.ToString()}�� {_postList[index].inDate} �������ɿ� �����߽��ϴ�. : " + bro);

        _postList.RemoveAt(index);

        if (bro.GetFlattenJSON()["postItems"].Count > 0) {
            SavePostToLocal(bro.GetFlattenJSON()["postItems"]);
        } else {
            Debug.LogWarning("���� ������ ���� �������� �������� �ʽ��ϴ�.");
        }

        BackendGameData.Instance.GameDataUpdate();
    }
public void PostReceiveAll(PostType postType) {
        if (_postList.Count <= 0) {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        Debug.Log($"{postType.ToString()} ���� ��� ������ ��û�մϴ�.");

        var bro = Backend.UPost.ReceivePostItemAll(postType);

        if (bro.IsSuccess() == false) {
            Debug.LogError($"{postType.ToString()} ���� ��� ���� �� ������ �߻��߽��ϴ� : " + bro);
            return;
        }

        Debug.Log("���� ��� ���ɿ� �����߽��ϴ�. : " + bro);

        _postList.Clear();

        foreach (LitJson.JsonData postItemsJson in bro.GetFlattenJSON()["postItems"]) {
            SavePostToLocal(postItemsJson);
        }

        BackendGameData.Instance.GameDataUpdate();
    }
}