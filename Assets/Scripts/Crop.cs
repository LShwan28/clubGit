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
        Debug.Log($"[SetData] 작물 데이터 설정됨. 성장 시간 : {growTimer}");
        
    }

    public void Water()
    {
        if (!isWatered)
        {
            isWatered = true;
            Debug.Log("���� �ְ� �۹� ������ �����߽��ϴ�.");
        }
        else if (isMidGrown && !isFullyWatered)
        {
            isFullyWatered = true;
            Debug.Log("�ٽ� ���� �־� ���� ������ �����մϴ�.");

        }
    }

        void GrowToMid()
    {
        isMidGrown = true;
        sr.sprite = data.midGrowSprite;
        Debug.Log("�۹��� �߰� �ܰ�� �����߽��ϴ�.");
        farmManager.ResetTileToTilled(cellPosition);
    }

    void GrowToFull()
    {
        isFullyGrown = true;
        sr.sprite = data.fullGrownSprite;
        Debug.Log("�۹��� ������ �ڶ����ϴ�.");
        farmManager.ResetTileToTilled(cellPosition);
    }

    public bool IsGrown() => isFullyGrown;

    public void Harvest()
    {
        if (isFullyGrown)
        {
            Debug.Log($"[{data.cropName}] ��Ȯ �Ϸ�!");
            Destroy(gameObject);
        }
    }
}
