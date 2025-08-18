using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using System.IO;

/// <summary>
/// MazeBuilder reads a maze summary file and builds a 3-floor maze using prefabs.
/// It also includes raycasting logic for interacting with trap doors and stairs.
/// </summary>
public class MazeBuilder : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject stairPrefab;
    public GameObject trapDoorPrefab;
    public Transform mazeEntrance;

    private void Start()
    {
        BuildMazeFromSummary();
    }

    void BuildMazeFromSummary()
    {
        string[] lines = mazeSummary.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("Floor"))
            {
                string[] parts = line.Replace("Floor ", "").Replace("Col ", "").Replace("Row ", "").Split(',');
                int floor = int.Parse(parts[0].Trim());
                int row = int.Parse(parts[1].Trim());
                int col = int.Parse(parts[2].Trim().Replace(":", ""));

                Vector3 basePos = mazeEntrance.position + new Vector3(col * 4, floor * 5, row * 4);
                Instantiate(floorPrefab, basePos, Quaternion.identity);

                // Read next few lines for cell details
                int index = System.Array.IndexOf(lines, line);
                Dictionary<string, bool> walls = new Dictionary<string, bool>();
                bool hasTrapDoor = false;
                bool hasStairs = false;

                for (int i = 1; i <= 4; i++)
                {
                    if (index + i >= lines.Length) break;
                    string detailLine = lines[index + i].Trim();
                    if (detailLine.StartsWith("Walls"))
                    {
                        string wallData = detailLine.Substring(detailLine.IndexOf("{") + 2).Replace("}", "");
                        string[] wallParts = wallData.Split(',');
                        foreach (string wp in wallParts)
                        {
                            string[] kv = wp.Split(':');
                            walls[kv[0].Trim().Trim('\'')] = bool.Parse(kv[1].Trim());
                        }
                    }
                    else if (detailLine.StartsWith("TrapDoor"))
                        hasTrapDoor = detailLine.Contains("True");
                    else if (detailLine.StartsWith("Stairs"))
                        hasStairs = detailLine.Contains("True");
                }

                // Instantiate walls
                foreach (var wall in walls)
                {
                    if (wall.Value)
                    {
                        Vector3 offset = Vector3.zero;
                        Quaternion rot = Quaternion.identity;
                        if (wall.Key == "N") { offset = new Vector3(0, 0, 2); }
                        if (wall.Key == "S") { offset = new Vector3(0, 0, -2); }
                        if (wall.Key == "E") { offset = new Vector3(2, 0, 0); rot = Quaternion.Euler(0, 90, 0); }
                        if (wall.Key == "W") { offset = new Vector3(-2, 0, 0); rot = Quaternion.Euler(0, 90, 0); }
                        Instantiate(wallPrefab, basePos + offset, rot);
                    }
                }

                // Instantiate trap door
                if (hasTrapDoor)
                {
                    GameObject trap = Instantiate(trapDoorPrefab, basePos + new Vector3(0, 0.1f, 0), Quaternion.identity);
                    trap.tag = "TrapDoor";
                }

                // Instantiate stairs
                if (hasStairs)
                {
                    GameObject stair = Instantiate(stairPrefab, basePos + new Vector3(0, 0.1f, 0), Quaternion.identity);
                    stair.tag = "Stair";
                }
            }
        }
    }

    // Raycasting interaction
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag("TrapDoor"))
                {
                    Animator anim = hit.collider.GetComponent<Animator>();
                    if (anim != null) anim.SetTrigger("Open");
                }
                else if (hit.collider.CompareTag("Stair"))
                {
                    Debug.Log("Stair clicked! You can climb or teleport.");
                }
            }
        }
    }

    // Embed maze summary directly for simplicity
    private string mazeSummary = @"""Floor 0, Row 0, Col 0:
  Walls: {'N': True, 'S': False, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 0, Col 1:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 0, Col 2:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 0, Col 3:
  Walls: {'N': True, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 0, Col 4:
  Walls: {'N': True, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 1, Col 0:
  Walls: {'N': False, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 0, Row 1, Col 1:
  Walls: {'N': True, 'S': False, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 1, Col 2:
  Walls: {'N': True, 'S': False, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 1, Col 3:
  Walls: {'N': False, 'S': False, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 1, Col 4:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 2, Col 0:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 0, Row 2, Col 1:
  Walls: {'N': False, 'S': False, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 2, Col 2:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: True

Floor 0, Row 2, Col 3:
  Walls: {'N': False, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 2, Col 4:
  Walls: {'N': True, 'S': False, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 0, Row 3, Col 0:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 0, Row 3, Col 1:
  Walls: {'N': False, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 3, Col 2:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 3, Col 3:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: True
  Stairs: False
  Landmark: False

Floor 0, Row 3, Col 4:
  Walls: {'N': False, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 4, Col 0:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 4, Col 1:
  Walls: {'N': True, 'S': True, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 0, Row 4, Col 2:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 0, Row 4, Col 3:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: True
  Stairs: False
  Landmark: False

Floor 0, Row 4, Col 4:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 0, Col 0:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 0, Col 1:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 0, Col 2:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 0, Col 3:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 1, Row 0, Col 4:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 1, Row 1, Col 0:
  Walls: {'N': True, 'S': False, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 1, Col 1:
  Walls: {'N': True, 'S': False, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 1, Col 2:
  Walls: {'N': True, 'S': False, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 1, Col 3:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 1, Col 4:
  Walls: {'N': True, 'S': False, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 2, Col 0:
  Walls: {'N': False, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 2, Col 1:
  Walls: {'N': False, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 2, Col 2:
  Walls: {'N': False, 'S': False, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: True

Floor 1, Row 2, Col 3:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 2, Col 4:
  Walls: {'N': False, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 3, Col 0:
  Walls: {'N': False, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 3, Col 1:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: True
  Stairs: False
  Landmark: False

Floor 1, Row 3, Col 2:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 3, Col 3:
  Walls: {'N': True, 'S': False, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 3, Col 4:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 4, Col 0:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 4, Col 1:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 4, Col 2:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 4, Col 3:
  Walls: {'N': False, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 1, Row 4, Col 4:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 0, Col 0:
  Walls: {'N': True, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 2, Row 0, Col 1:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 0, Col 2:
  Walls: {'N': True, 'S': False, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 0, Col 3:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 0, Col 4:
  Walls: {'N': True, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 1, Col 0:
  Walls: {'N': False, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 1, Col 1:
  Walls: {'N': True, 'S': True, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 1, Col 2:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 1, Col 3:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 1, Col 4:
  Walls: {'N': False, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 2, Row 2, Col 0:
  Walls: {'N': True, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 2, Row 2, Col 1:
  Walls: {'N': True, 'S': False, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 2, Col 2:
  Walls: {'N': True, 'S': False, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: True

Floor 2, Row 2, Col 3:
  Walls: {'N': True, 'S': True, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 2, Row 2, Col 4:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: True
  Stairs: False
  Landmark: False

Floor 2, Row 3, Col 0:
  Walls: {'N': False, 'S': False, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 3, Col 1:
  Walls: {'N': False, 'S': True, 'E': False, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 3, Col 2:
  Walls: {'N': False, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 3, Col 3:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: True
  Stairs: False
  Landmark: False

Floor 2, Row 3, Col 4:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 4, Col 0:
  Walls: {'N': False, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: True
  Landmark: False

Floor 2, Row 4, Col 1:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: True
  Stairs: True
  Landmark: False

Floor 2, Row 4, Col 2:
  Walls: {'N': True, 'S': True, 'E': True, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 4, Col 3:
  Walls: {'N': True, 'S': True, 'E': False, 'W': True}
  TrapDoor: False
  Stairs: False
  Landmark: False

Floor 2, Row 4, Col 4:
  Walls: {'N': True, 'S': True, 'E': True, 'W': False}
  TrapDoor: True
  Stairs: False
  Landmark: False

""";
}
