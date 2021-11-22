using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMoreTargetsScript : MonoBehaviour
{
    public Material TileIndicator;

    public GameObject SourceMenu;
    public GameObject[,] SourceGrid;
    public List<GridObject> Targets;

    private GameControls control;

    List<GameObject> decalProjectors = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        control = new GameControls();
        control.MapCraftControls.Enable();
        control.MapCraftControls.LeftClick.performed += _ => AddTarget();
        control.MapCraftControls.RightClick.performed += _ => SubmitSelection();
    }

    private void OnDisable()
    {
        control.MapCraftControls.Disable();
    }

    void Start()
    {
        UpdateTargetDisplay();
    }

    void SubmitSelection()
    {
        SourceMenu.SetActive(true);
        BlockRangeDisplay.ClearDisplay(decalProjectors);
        decalProjectors = new List<GameObject>();
        Destroy(gameObject);
    }

    void AddTarget()
    {
        Vector2Int grid_pos = GridCrafter.BlockAtMouse();
        if (SourceGrid[grid_pos.x, grid_pos.y] != null)
        {
            if (!Targets.Contains(SourceGrid[grid_pos.x, grid_pos.y].GetComponent<GridObject>()))
            {
                Targets.Add(SourceGrid[grid_pos.x, grid_pos.y].GetComponent<GridObject>());
            } else
            {
                Targets.Remove(SourceGrid[grid_pos.x, grid_pos.y].GetComponent<GridObject>());
            }
        }
        UpdateTargetDisplay();
    }

    void UpdateTargetDisplay()
    {
        BlockRangeDisplay.ClearDisplay(decalProjectors);
        decalProjectors = new List<GameObject>();
        foreach (GridObject gridObject in Targets)
        {
            decalProjectors.AddRange(BlockRangeDisplay.RectangleDisplay(TileIndicator, GridCrafter.blockGrid, gridObject.pos, gridObject.TileSize));
        }
    }
}
