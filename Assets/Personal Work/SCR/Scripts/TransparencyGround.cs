using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparencyGround : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private float fadeSpeed = 4f;
    private Vector3Int lastCell;
    private void Start()
    {
        var bounds = tilemap.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;

            tilemap.SetTileFlags(pos, TileFlags.None);
            Color c = tilemap.GetColor(pos);
            c.a = 0f;
            tilemap.SetColor(pos, c);
        }

        Vector3Int testCell = new Vector3Int(1, -7, 0);
        Debug.Log("Init color at " + testCell + " = " + tilemap.GetColor(testCell));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Vector3Int cell = tilemap.WorldToCell(collision.transform.position);
        if (cell == lastCell) return;
        lastCell = cell;

        if (!tilemap.HasTile(cell)) return;

        StartCoroutine(FadeInTile(cell));
    }

    private IEnumerator FadeInTile(Vector3Int cell)
    {
        Debug.Log(cell);
        tilemap.SetTileFlags(cell, TileFlags.None);

        Color c = tilemap.GetColor(cell);
        Debug.Log(c);
        float startA = c.a;
        float t = 0f;

        while (c.a < 0.99f)
        {
            t += Time.deltaTime * fadeSpeed;
            c.a = Mathf.Lerp(startA, 1f, t);
            tilemap.SetColor(cell, c);
            yield return null;
        }

        c.a = 1f;
        tilemap.SetColor(cell, c);
    }
}
