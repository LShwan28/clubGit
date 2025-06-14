using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropData data;
    private bool isWatered = false;
    private bool isGrown = false;
    private float growTimer = 0f;

    public Sprite grownSprite;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isWatered && !isGrown)
        {
            growTimer -= Time.deltaTime;
            if (growTimer <= 0f)
            {
                Grow();
            }
        }
    }

    public void SetData(CropData cropData)
    {
        data = cropData;
        growTimer = data.growTime;
    }

    public void Water()
    {
        if (!isWatered)
        {
            isWatered = true;
            Debug.Log("작물에 물을 주었습니다. 성장 시작!");
        }
    }

    void Grow()
    {
        isGrown = true;
        sr.sprite = grownSprite;
        Debug.Log("작물이 자랐습니다!");
    }

    public bool IsGrown()
    {
        return isGrown;
    }

    public void Harvest()
    {
        if (isGrown)
        {
            Debug.Log($"[{data.cropName}] 수확 완료!");
            Destroy(gameObject);
        }
    }
}
