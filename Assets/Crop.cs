using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropData data;
    public Vector3Int cellPosition;
    private bool isWatered = false;
    private bool isMidGrown = false;
    public bool isFullyWatered { get; private set; } = false;
    public bool isFullyGrown { get; private set; } = false;
    public FarmManager farmManager;
    private float growTimer;
    private float fullygrowTimer;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isWatered && !isMidGrown)
        {
            growTimer -= Time.deltaTime;
            if (growTimer <= 0f)
            {
                GrowToMid();
            }
        }
        else if(isFullyWatered && !isFullyGrown)
        {
            fullygrowTimer -= Time.deltaTime;
            if (fullygrowTimer <= 0f)
            {
                GrowToFull();
            }
        }
    }

    public void SetData(CropData cropData, Vector3Int cellPos)
    {
        data = cropData;
        growTimer = data.growTime;
        fullygrowTimer = data.growTime;
        cellPosition = cellPos;
        Debug.Log($"[SetData] 작물 데이터 설정됨. 성장 시간: {growTimer}");
        
    }

    public void Water()
    {
        if (!isWatered)
        {
            isWatered = true;
            Debug.Log("물을 주고 작물 성장을 시작했습니다.");
        }
        else if (isMidGrown && !isFullyWatered)
        {
            isFullyWatered = true;
            Debug.Log("다시 물을 주어 완전 성장을 시작합니다.");

        }
    }

        void GrowToMid()
    {
        isMidGrown = true;
        sr.sprite = data.midGrowSprite;
        Debug.Log("작물이 중간 단계로 성장했습니다.");
        farmManager.ResetTileToTilled(cellPosition);
    }

    void GrowToFull()
    {
        isFullyGrown = true;
        sr.sprite = data.fullGrownSprite;
        Debug.Log("작물이 완전히 자랐습니다.");
        farmManager.ResetTileToTilled(cellPosition);
    }

    public bool IsGrown() => isFullyGrown;

    public void Harvest()
    {
        if (isFullyGrown)
        {
            Debug.Log($"[{data.cropName}] 수확 완료!");
            Destroy(gameObject);
        }
    }
}
