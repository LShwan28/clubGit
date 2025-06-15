using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    private enum TileState { Untilled, Tilled, Planted, Watered }

    private CropData data;
    private Crop crop;
    public Tilemap farmTilemap;
    public TileBase untiledTile;
    public TileBase tilledTile;
    public TileBase wateredTile;

  
    public CropData cropToPlant;
    public GameObject cropPrefab;

    // 내부 상태 관리
    private Dictionary<Vector3Int, TileState> tileStates = new();
    private Dictionary<Vector3Int, GameObject> plantedCrops = new();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = 10f; 

            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            Vector3Int cellPos = farmTilemap.WorldToCell(worldPos);

            string tool = PlayerTool.currentTool;

            if (!tileStates.ContainsKey(cellPos))
            {
                tileStates[cellPos] = TileState.Untilled;
            }

            TileState state = tileStates[cellPos];

            switch (tool)
            {
                case "Hoe":
                    if (state == TileState.Untilled)
                    {
                        farmTilemap.SetTile(cellPos, tilledTile);
                        tileStates[cellPos] = TileState.Tilled;
                        Debug.Log("밭을 갈았습니다.");
                    }
                    break;

                case "Seed":
                    if (state == TileState.Tilled)
                    {
                        
                        //CropData에서 cropPrefab을 사용
                         GameObject crop = Instantiate(cropToPlant.cropPrefab, farmTilemap.GetCellCenterWorld(cellPos), Quaternion.identity);
                        Crop cropScript = crop.GetComponent<Crop>();
                        cropScript.SetData(cropToPlant, cellPos);
                        plantedCrops[cellPos] = crop;
                        cropScript.farmManager = this;
                        Debug.Log("씨앗을 심었습니다.");
                    }
                    break;

                case "Water":
                    if (state == TileState.Tilled && plantedCrops.ContainsKey(cellPos))
                    {
                        farmTilemap.SetTile(cellPos, wateredTile);
                        tileStates[cellPos] = TileState.Watered;
                        Crop crop = plantedCrops[cellPos].GetComponent<Crop>();
                        
                        crop.Water();
                        
                    }
                    break;

                case "Hand":
                    if (state == TileState.Watered && plantedCrops.ContainsKey(cellPos))
                    {
                        Crop crop = plantedCrops[cellPos].GetComponent<Crop>();
                        if (crop.IsGrown())
                        {
                            crop.Harvest();
                            Destroy(plantedCrops[cellPos]);
                            plantedCrops.Remove(cellPos);

                            farmTilemap.SetTile(cellPos, tilledTile);
                            tileStates[cellPos] = TileState.Tilled;
                            Debug.Log("수확했습니다.");
                        }
                    }
                    break;
            }
        }

    }
    public void ResetTileToTilled(Vector3Int cellPos)
    {

        if (plantedCrops.ContainsKey(cellPos))
        {
            Crop crop = plantedCrops[cellPos].GetComponent<Crop>();

            if (!crop.isFullyGrown)
            {
                farmTilemap.SetTile(cellPos, tilledTile);
                tileStates[cellPos] = TileState.Tilled;
                Debug.Log("타일을 다시 갈린 상태로 되돌렸습니다.");
            }
        }
        Debug.Log("타일을 다시 갈린 상태로 되돌렸습니다.");
    }
}
