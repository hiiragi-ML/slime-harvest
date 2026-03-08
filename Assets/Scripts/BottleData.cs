using UnityEngine;

[CreateAssetMenu(fileName = "NewBottleData", menuName = "SlimeGame/BottleData (ビンの設定カード)")]
public class BottleData : ScriptableObject{
    [Header("基本情報")]
    public string bottleID;
    public string bottleName;
    public int price;
    public Sprite bottleSprite;

    [Header("親スライムの見た目(5段階)")]
    public Sprite[] parentSlimeSprites;

    [Header("MAX時のスライム(パンパン状態)")]
    public Sprite maxCapacitySlime;

    [Header("生まれる子スライム")]
    public Sprite normalSlime;
    public Sprite rareSlime;
    public Sprite superRareSlime;
}
