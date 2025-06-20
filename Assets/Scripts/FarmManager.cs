using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    private enum TileState { Untilled, Tilled, Watered }
    private Dictionary<Vector3Int, TileState> tileStates = new Dictionary<Vector3Int, TileState>();
    private Dictionary<Vector3Int, Crop> plantedCrops = new Dictionary<Vector3Int, Crop>();

    [Header("타일맵 & 타일")]
    public Tilemap farmTilemap;
    public TileBase untiledTile;
    public TileBase tilledTile;
    public TileBase wateredTile;

    [Header("작물 설정")]
    public CropData cropToPlant;

    private PlayerStats playerStats;

    void Awake()
    {
        // 플레이어 스탯 참조
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int cellPos = GetMouseCellPosition();
            if (!tileStates.ContainsKey(cellPos))
                tileStates[cellPos] = TileState.Untilled;

            TileState currentState = tileStates[cellPos];
            string tool = PlayerTool.currentTool;

            switch (tool)
            {
                case "Hoe": HandleTilling(cellPos, currentState); break;
                case "Seed": HandlePlanting(cellPos, currentState); break;
                case "Water": HandleWatering(cellPos, currentState); break;
                case "Hand": HandleHarvest(cellPos, currentState); break;
            }
        }
    }

    // 마우스 위치로 타일 좌표 계산
    private Vector3Int GetMouseCellPosition()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;
        return farmTilemap.WorldToCell(worldPos);
    }

    // 밭 갈기
    private void HandleTilling(Vector3Int pos, TileState state)
    {
        if (state == TileState.Untilled && playerStats.UseStamina())
        {
            farmTilemap.SetTile(pos, tilledTile);
            tileStates[pos] = TileState.Tilled;
            CanvasManager.Instance.ShowStatusMessage("밭을 갈았습니다.");
        }
        else if (state == TileState.Untilled)
        {
            CanvasManager.Instance.ShowStatusMessage("스테미너가 부족합니다.");
        }
    }

    // 씨앗 심기
    private void HandlePlanting(Vector3Int pos, TileState state)
    {
        if (state == TileState.Tilled && playerStats.UseStamina())
        {
            Vector3 spawnPos = farmTilemap.GetCellCenterWorld(pos) + new Vector3(0, 0.25f, 0);
            GameObject cropGO = Instantiate(cropToPlant.cropPrefab, spawnPos, Quaternion.identity);
            Crop crop = cropGO.GetComponent<Crop>();
            crop.SetData(cropToPlant, pos);
            crop.farmManager = this;
            plantedCrops[pos] = crop;
            tileStates[pos] = TileState.Watered;
            CanvasManager.Instance.ShowStatusMessage("씨앗을 심었습니다.");
        }
        else if (state == TileState.Tilled)
        {
            CanvasManager.Instance.ShowStatusMessage("스테미너가 부족합니다.");
        }
    }

    // 물 주기
    private void HandleWatering(Vector3Int pos, TileState state)
    {
        if (state == TileState.Tilled && plantedCrops.ContainsKey(pos) && playerStats.UseStamina())
        {
            farmTilemap.SetTile(pos, wateredTile);
            tileStates[pos] = TileState.Watered;
            plantedCrops[pos].Water();
            CanvasManager.Instance.ShowStatusMessage("물을 주었습니다.");
        }
        else if (state == TileState.Tilled)
        {
            CanvasManager.Instance.ShowStatusMessage("스테미너가 부족합니다.");
        }
    }

    // 수확
    private void HandleHarvest(Vector3Int pos, TileState state)
    {
        if (state == TileState.Watered && plantedCrops.ContainsKey(pos) && playerStats.UseStamina())
        {
            Crop crop = plantedCrops[pos];
            if (crop.IsGrown())
            {
                crop.Harvest();
                plantedCrops.Remove(pos);
                farmTilemap.SetTile(pos, tilledTile);
                tileStates[pos] = TileState.Tilled;
                CanvasManager.Instance.ShowStatusMessage("수확했습니다.");
            }
            else
            {
                CanvasManager.Instance.ShowStatusMessage("아직 자라지 않았습니다.");
            }
        }
        else if (state == TileState.Watered)
        {
            CanvasManager.Instance.ShowStatusMessage("스테미너가 부족합니다.");
        }
    }
    public void ResetTileToTilled(Vector3Int pos)
{
    if (plantedCrops.ContainsKey(pos) && !plantedCrops[pos].IsGrown())
    {
        farmTilemap.SetTile(pos, tilledTile);
        tileStates[pos] = TileState.Tilled;
    }
}
}
