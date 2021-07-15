using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BlockRangeDisplay
{
    public static List<GameObject> RectangleDisplay(Material projectorMaterial, GameObject[,] blockGrid, Vector2Int pos, Vector2Int shape)
    {
        List<GameObject> decalProjectors = new List<GameObject>();
        for(int x = pos.x; x < pos.x + shape.x; x++)
        {
            if (x < blockGrid.GetLength(0))
            {
                for (int y = pos.y; y < pos.y + shape.y; y++)
                {
                    if (y < blockGrid.GetLength(1))
                    {
                        GameObject newProjector = new GameObject("Projector");
                        DecalProjector projector = newProjector.AddComponent<DecalProjector>();
                        projector.material = projectorMaterial;
                        newProjector.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        newProjector.transform.position = blockGrid[x, y].transform.position;
                        decalProjectors.Add(newProjector);
                    }
                }
            }
        }
        return decalProjectors;
    }

    public static void ClearDisplay(List<GameObject> decalProjectors)
    {
        foreach (GameObject decalProjector in decalProjectors)
        {
            Object.Destroy(decalProjector);
        }
    }
}
