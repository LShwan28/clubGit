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
            Debug.Log("�۹��� ���� �־����ϴ�. ���� ����!");
        }
    }

    void Grow()
    {
        isGrown = true;
        sr.sprite = grownSprite;
        Debug.Log("�۹��� �ڶ����ϴ�!");
    }

    public bool IsGrown()
    {
        return isGrown;
    }

    public void Harvest()
    {
        if (isGrown)
        {
            Debug.Log($"[{data.cropName}] ��Ȯ �Ϸ�!");
            Destroy(gameObject);
        }
    }
}
