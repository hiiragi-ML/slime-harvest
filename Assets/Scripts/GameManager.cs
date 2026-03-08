using UnityEngine;
using TMPro;
using System.IO;

// セーブするデータをまとめる箱
[System.Serializable]
public class SaveData{
    public int totalCoins = 0;
    public int totalSlimes = 0;
}

public  class GameManager : MonoBehaviour
{
    [Header("プレイヤーのデータ")]
    public SaveData saveData = new SaveData();

    [Header("UIの関連付け")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI slimeCountText;

    [Header("ビンの着せ替え設定")]
    public BottleData currentBottle;
    public SpriteRenderer bottleRenderer;
    public ParentSlime parentSlime;

    // セーブファイルを置く場所のパス
    private string saveFilePath;

    void Start(){
        saveFilePath = Application.persistentDataPath + "/SlimeSaveData.json";
        LoadData();
        UpdateUI();
        ApplyBottleData(currentBottle);
    }

    void Update(){
        // 開発中にRキーを押したらリセット
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.R)){
            ResetSaveData();
        }
        #endif
    }

    public void ResetSaveData(){
        saveData = new SaveData();

        SaveDataToFile();

        UpdateUI();

        Debug.Log("セーブデータをリセットしました");
    }

    public void AddReward(int coinAmount, int slimeAmount){
        saveData.totalCoins += coinAmount;
        saveData.totalSlimes += slimeAmount;

        UpdateUI();
    }

    void UpdateUI(){
        if(coinText != null) coinText.text = "コイン: " + saveData.totalCoins.ToString();
        if(slimeCountText != null) slimeCountText.text = "スライム: " + saveData.totalSlimes.ToString();
    }

    public void ApplyBottleData(BottleData newBottleData){
        if(newBottleData == null) return;

        currentBottle = newBottleData;

        // 1. ビンの画像を，カードに書かれている画像に変える
        if(bottleRenderer != null){
            bottleRenderer.sprite = currentBottle.bottleSprite;
        }

        // 2. 親スライムにカードに書かれている5段階の画像を渡す
        if(parentSlime != null){
            parentSlime.ChangeSprites(currentBottle.parentSlimeSprites, currentBottle.maxCapacitySlime);
        }

        Debug.Log("ビンの着せ替え完了: " + currentBottle.bottleName);
    }

    // セーブ，ロード関係の関数
    // セーブ関係の関数
    public void SaveDataToFile(){
        // 1. saveDataの中身をjsonに変換
        string json = JsonUtility.ToJson(saveData);

        // 2. その文字を，ファイルとして書き込む
        File.WriteAllText(saveFilePath, json);

        Debug.Log("セーブ完了: " + saveFilePath);
    }

    // ロード関係の関数
    public void LoadData(){
        if(File.Exists(saveFilePath)){
            // 1. ファイルから文字を読みだす
            string json = File.ReadAllText(saveFilePath);

            // 2. 文字から，元のsaveDataの形に復元して上書き
            saveData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("ロード完了: " + saveFilePath);
        }else{
            Debug.Log("ロード失敗: ファイルが存在しません");
        }
    }

    // オートセーブ関係の関数
    void OnApplicationPause(bool pauseStatus){
        if(pauseStatus == true){
            SaveDataToFile();
        }
    }

    // タスクキル時の関数
    void OnApplicationQuit(){
        SaveDataToFile();
    }

    // インスペクターの値をいじったときに呼ばれる
    void OnValidate(){
        // ゲームが再生中かつ，枠が空っぽじゃないときだけ
        if(Application.isPlaying && currentBottle != null){
            ApplyBottleData(currentBottle);
        }
    }
}
