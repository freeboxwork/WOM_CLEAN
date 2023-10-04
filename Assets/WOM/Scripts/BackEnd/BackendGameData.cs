using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class UserData {
    public int level = 1;
    public float atk = 3.5f;
    public string info = string.Empty;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<string> equipment = new List<string>();

    // 데이터를 디버깅하기 위한 함수입니다.(Debug.Log(UserData);)
    public override string ToString() {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"level : {level}");
        result.AppendLine($"atk : {atk}");
        result.AppendLine($"info : {info}");
        
        result.AppendLine($"inventory");
        foreach (var itemKey in inventory.Keys) {
            result.AppendLine($"| {itemKey} : {inventory[itemKey]}개");
        }

        result.AppendLine($"equipment");
        foreach (var  equip in equipment) {
            result.AppendLine($"| {equip}");
        }

        return result.ToString();
    }
}

public class BackendGameData {
    private static BackendGameData _instance = null;

    public static BackendGameData Instance {
        get {
            if (_instance == null) {
                _instance = new BackendGameData();
            }

            return _instance;
        }
    }

    public static UserData userData;

    private string gameDataRowInDate = string.Empty;
    
public void GameDataInsert() {
        if (userData == null) {
            userData = new UserData();
        }

        Debug.Log("데이터를 초기화합니다.");
        userData.level = 1;
        userData.atk = 3.5f;
        userData.info = "친추는 언제나 환영입니다.";

        userData.equipment.Add("전사의 투구");
        userData.equipment.Add("강철 갑옷");
        userData.equipment.Add("헤르메스의 군화");

        userData.inventory.Add("빨간포션", 1);
        userData.inventory.Add("하얀포션", 1);
        userData.inventory.Add("파란포션", 1);

        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("atk", userData.atk);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);


        Debug.Log("게임정보 데이터 삽입을 요청합니다.");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if (bro.IsSuccess()) {
            Debug.Log("게임정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임정보의 고유값입니다.  
            gameDataRowInDate = bro.GetInDate();
        } else {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }
    public void GameDataGet() {
        Debug.Log("게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if (bro.IsSuccess()) {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);


            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.  

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.  
            if (gameDataJson.Count <= 0) {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            } else {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임정보의 고유값입니다.  

                userData = new UserData();

                userData.level = int.Parse(gameDataJson[0]["level"].ToString());
                userData.atk = float.Parse(gameDataJson[0]["atk"].ToString());
                userData.info = gameDataJson[0]["info"].ToString();

                foreach (string itemKey in gameDataJson[0]["inventory"].Keys) {
                    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                }

                foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"]) {
                    userData.equipment.Add(equip.ToString());
                }

                Debug.Log(userData.ToString());
            }
        } else {
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }


int userDataCallCount = 0;
string userDataIndate = string.Empty;


//기존 플레이 데이터가 존재하지만, 게임 데이터 불러오기 요청 실패 후 새로운 row 착오 insert (초기화) 
//게임 데이터 불러오기시 네트워크 연결상태가 좋지않거나 서버에 부하가 발생하였을 때에는 에러 발생
public void GetUserData()
{
    userDataCallCount++;
    if (userDataCallCount > 3)
    {
        //3번 시도 후 안될 경우 이상 현상으로 분류 게임 재시작 권장
        //ShowUI("데이터 불러오기중 오류가 발생했습니다. 타이틀화면으로 돌아갑니다.");
        //SceneManager.ChangeScene("TitleScene");
        return;
    }
    SendQueue.Enqueue(Backend.GameData.GetMyData, "UserData", new Where(), getCallback =>
    {
        if (getCallback.IsSuccess())
        {
            LitJson.JsonData json = getCallback.FlattenRows();
            if (json.Count <= 0)
            {
                Debug.Log("데이터가 존재하지 않습니다");
                SendQueue.Enqueue(Backend.GameData.Insert, "UserData", insertCallback =>
               {
                   //데이터 생성
                   if (insertCallback.IsSuccess())
                   {
                       userDataIndate = insertCallback.GetInDate(); // 삽입한 row의 indate가져오기
                       //InitUserData(); // UserData 초기화
                   }
               });
            }
            else
            {
                userDataIndate = json[0]["inDate"].ToString();
                //SetUserData(json[0]); // [임의의 함수] 서버에서 불러온 json 데이터를 변수에 저장해주는 함수
            }
        }
        else
        {
            string errorMessage = getCallback.GetMessage();

            Param param = new Param();
            param.Add("errorMessage", errorMessage);

            Backend.GameLog.InsertLog("GetUserData", param);

            if (errorMessage.Contains("maintenance"))
            {
                //서버 점검중
            }
            else if (errorMessage.Contains("accessToken"))
            {
                //액세스 토큰 재시도
            }
            else
            {
                GetUserData(); // 같은 함수 재호출(3번까지 되도록 limit 필요)
            }
        }
    });
}


    public void LevelUp() {
        Debug.Log("레벨을 1 증가시킵니다.");
        userData.level += 1;
        userData.atk += 3.5f;
        userData.info = "내용을 변경합니다.";
    }

    // 게임정보 수정하기
    public void GameDataUpdate() {
        if (userData == null) {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
            return;
        }

        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("atk", userData.atk);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);

        BackendReturnObject bro = null;

        if (string.IsNullOrEmpty(gameDataRowInDate)) {
            Debug.Log("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update( "USER_DATA", new Where(), param);
        } else {
            Debug.Log($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2( "USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if (bro.IsSuccess()) {
            Debug.Log("게임정보 데이터 수정에 성공했습니다. : " + bro);
        } else {
            Debug.LogError("게임정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
}