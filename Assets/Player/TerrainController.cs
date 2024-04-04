using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainController : MonoBehaviour
{
    [SerializeField] private Tilemap terrainNeutral;
    public Enums.ColourState currentShoeColor;
    [SerializeField] private Tilemap terrainRed;
    [SerializeField] private Tilemap terrainBlue;
    [SerializeField] private Tilemap terrainGreen;

    public enum ColourState { red, blue, green }

    private void Start()
    {
        SetColliderActive(ColourState.red, true);
        currentShoeColor = Enums.ColourState.red;
    }

    private void Update()
    {
        
    }

    public void SetColliderActive(ColourState colour, bool active)
    {
        switch (colour)
        {
            case ColourState.red:
                SetTilemapCollidersEnabled(terrainRed, active);
                SetTilemapCollidersEnabled(terrainBlue, !active);
                SetTilemapCollidersEnabled(terrainGreen, !active);
                break;
            case ColourState.green:
                SetTilemapCollidersEnabled(terrainRed, !active);
                SetTilemapCollidersEnabled(terrainBlue, !active);
                SetTilemapCollidersEnabled(terrainGreen, active);
                break;
            case ColourState.blue:
                SetTilemapCollidersEnabled(terrainRed, !active);
                SetTilemapCollidersEnabled(terrainBlue, active);
                SetTilemapCollidersEnabled(terrainGreen, !active);
                break;
        }
    }

    private void SetTilemapCollidersEnabled(Tilemap tilemap, bool active)
    {
        tilemap.GetComponent<TilemapCollider2D>().enabled = active;
    }

    public void TurnOffTerrain(ColourState colour)
    {
        switch (colour)
        {
            case ColourState.red:
                SetColliderActive(ColourState.red, false);
                break;
            case ColourState.green:
                SetColliderActive(ColourState.green, false);
                break;
            case ColourState.blue:
                SetColliderActive(ColourState.blue, false);
                break;
        }
    }
}
