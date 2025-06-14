using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    private enum TileState { Untilled, Tilled, Planted, Watered }


    public Tilemap farmTilemap;
    public TileBase untiledTile;
    public TileBase tilledTile;
    public TileBase plantedTile;
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
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
                        farmTilemap.SetTile(cellPos, plantedTile);
                        tileStates[cellPos] = TileState.Planted;

                        GameObject crop = Instantiate(cropPrefab, farmTilemap.GetCellCenterWorld(cellPos), Quaternion.identity);
                        Crop cropScript = crop.GetComponent<Crop>();
                        cropScript.SetData(cropToPlant);
                        plantedCrops[cellPos] = crop;

                        Debug.Log("씨앗을 심었습니다.");
                    }
                    break;

                case "Water":
                    if (state == TileState.Planted && plantedCrops.ContainsKey(cellPos))
                    {
                        farmTilemap.SetTile(cellPos, wateredTile);
                        tileStates[cellPos] = TileState.Watered;

                        Crop crop = plantedCrops[cellPos].GetComponent<Crop>();
                        crop.Water();
                        Debug.Log("물을 주었습니다.");
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
}
