using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UIパネル")]
    public GameObject shopPanel;
    public GameObject bottlePanel;
    public GameObject dexPanel;
    public GameObject settingsPanel;

    void Start(){
        AllClose();
    }

    public void AllClose(){
        if(shopPanel != null){
            shopPanel.SetActive(false);
        }
        if(bottlePanel != null){
            bottlePanel.SetActive(false);
        }
        if(dexPanel != null){
            dexPanel.SetActive(false);
        }
        if(settingsPanel != null){
            settingsPanel.SetActive(false);
        }
    }

    public void OnClickShopButton(){
        AllClose();
        Debug.Log("ショップ画面を開きます！");
        shopPanel.SetActive(true);
    }

    public void OnClickBottleButton(){
        AllClose();
        Debug.Log("ビン選択画面を開きます！");
        bottlePanel.SetActive(true);
    }

    public void OnClickDexButton(){
        AllClose();
        Debug.Log("スライム図鑑を開きます！");
        dexPanel.SetActive(true);
    }

    public void OnClickSettingsButton(){
        AllClose();
        Debug.Log("設定画面を開きます！");
        settingsPanel.SetActive(true);
    }

    public void OnClickCloseButton(){
        AllClose();
        Debug.Log("ショップ画面を閉じます！");
        shopPanel.SetActive(false);
    }

    // 音量設定
    public void OnBGMVolumeChanged(float value){
        Debug.Log("BGMの音量を変更しました: " + value);
    }

    public void OnSEVolumeChanged(float value){
        Debug.Log("SEの音量を変更しました: " + value);
    }
}
