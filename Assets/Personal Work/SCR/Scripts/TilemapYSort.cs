using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteAlways]
[RequireComponent(typeof(TilemapRenderer), typeof(Tilemap))]
public class TilemapYSort : MonoBehaviour
{
    private Tilemap tilemap;
    private TilemapRenderer renderer;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        renderer = GetComponent<TilemapRenderer>();
    }

    void LateUpdate()
    {
        // 현재 타일맵의 최하단 타일 위치를 가져오기
        Vector3Int? bottomCell = GetBottomCell();
        if (bottomCell.HasValue)
        {
            // 셀 좌표를 월드 좌표로 변환
            Vector3 bottomWorldPos = tilemap.CellToWorld(bottomCell.Value);

            // 그 위치의 Y값 기준으로 sortingOrder 계산
            renderer.sortingOrder = Mathf.RoundToInt(-bottomWorldPos.y * 100);
        }
    }

    Vector3Int? GetBottomCell()
    {
        // boundsInt는 현재 타일이 존재하는 범위
        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int? bottomCell = null;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cell = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cell))
                {
                    if (!bottomCell.HasValue || cell.y < bottomCell.Value.y)
                        bottomCell = cell;
                }
            }
        }

        return bottomCell;
    }
}
