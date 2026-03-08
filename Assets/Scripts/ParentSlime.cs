using UnityEngine;

public class ParentSlime : MonoBehaviour
{
    [Header("内部的な成長設定")]
    public int maxInternalStage = 100;
    public float timePerStage = 1.0f;

    private Sprite[] currentStageSprites;
    private Sprite currentMaxSprite;

    [Header("報酬の設定")]
    public int baseCoin = 10;

    [Header("ぷちぷち収穫の設定")]
    public float harvestInterval = 0.15f;

    [Header("ゲームマネージャ")]
    public GameManager gameManager;

    [Header("MAX時の位置調整")]
    [Tooltip("パンパンの時だけY座標をずらす(マイナスなら下，プラスなら上)")]
    public float maxYOffset = 1f;

    private Vector3 defaultLocalPosition;

    // 内部変数
    private int currentInternalStage = 0;
    private float growthTimer = 0f;
    private SpriteRenderer spriteRenderer;

    private bool isPressing = false;
    private float harvestTimer = 0f;

    void Awake(){
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultLocalPosition = transform.localPosition;
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        // 内部成長
        if(currentInternalStage < maxInternalStage){
            growthTimer += Time.deltaTime;
            if(growthTimer >= timePerStage){
                currentInternalStage++;
                growthTimer = 0f;
                UpdateVisual();
            }
        }

        // 連続収穫
        if(isPressing && currentInternalStage > 0){
            harvestTimer += Time.deltaTime;

            if(harvestTimer >= harvestInterval){
                HarvestOne();
                harvestTimer = 0f;
            }
        }
    }

    // スマホタップ(瞬間)
    void OnMouseDown(){
        isPressing = true;
        harvestTimer = 0f;

        if(currentInternalStage > 0){
            HarvestOne();
        }else{
            Debug.Log("まだ小さくて分裂できない！");
        }
    }

    void OnMouseUp(){
        isPressing = false;
    }

    void HarvestOne(){
        currentInternalStage--;

        // １匹分のガチャ
        int roll = Random.Range(0, 100);
        string rarity = "";

        if(roll < 5){
            rarity = "超激レア";
        }else if(roll < 20){
            rarity = "レア";
        }else{
            rarity = "ノーマル";
        }

        Debug.Log($"ポンッ！ {rarity}の子スライムと{baseCoin}コインを獲得！");

        if(gameManager != null){
            gameManager.AddReward(baseCoin, 1);
        }

        UpdateVisual();
    }

    public void FeedItem(int amount){
        currentInternalStage += amount;
        if(currentInternalStage > maxInternalStage){
            currentInternalStage = maxInternalStage;
        }
        UpdateVisual();
        Debug.Log("餌を食べて一気に成長！ 現在ステージ: " + currentInternalStage);
    }

    public void ChangeSprites(Sprite[] newSprites, Sprite maxSprite){
        currentStageSprites = newSprites;
        currentMaxSprite = maxSprite;
        UpdateVisual();
    }

    void UpdateVisual(){
        if(currentInternalStage >= maxInternalStage && currentMaxSprite != null){
            spriteRenderer.sprite = currentMaxSprite;
            transform.localScale = new Vector3(0.6f, 0.6f, 1f);

            transform.localPosition = defaultLocalPosition + new Vector3(0f, maxYOffset, 0f);
        }else if(currentStageSprites != null && currentStageSprites.Length > 0){
            float ratio = (float)currentInternalStage / maxInternalStage;
            int visualIndex = Mathf.FloorToInt(ratio * (currentStageSprites.Length));

            if(visualIndex >= currentStageSprites.Length){
                visualIndex = currentStageSprites.Length - 1;
            }

            spriteRenderer.sprite = currentStageSprites[visualIndex];

            float scale = 1.0f + (ratio * 0.2f);
            transform.localScale = new Vector3(scale, scale, 1f);

            transform.localPosition = defaultLocalPosition;
        }
    }
}
