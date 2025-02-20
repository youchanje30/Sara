using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class RoomFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField] int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField] int mapWidth = 20, mapHeight = 20;

    [Range(0, 10)]
    [SerializeField] int offset = 1;
    [SerializeField] bool randomWalkRooms = false;

    [SerializeField] MapBalanceSO[] mapBalanceParameter;
    [SerializeField] Transform player;

    [SerializeField] GameObject[] monsterPrefabs;

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }


    public void CreateRooms()
    {
        var roomsList = ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition, new Vector3Int(mapWidth, mapWidth, 0)), minRoomWidth, minRoomHeight);

        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        HashSet<Vector2Int> playerSpawnRoom = new HashSet<Vector2Int>();
        Queue<int> count = new Queue<int>();

        if(randomWalkRooms)
        {
            playerSpawnRoom = CreateRoomRandomly(roomsList[0]);
            
            for (int i = 1; i < roomsList.Count; i++)
            {
                HashSet<Vector2Int> roomFloor = CreateRoomRandomly(roomsList[i]);
                count.Enqueue(roomFloor.Count);
                foreach (var position in roomFloor)
                {
                    floor.Add(position);
                }
            }
            // floor = CreateRoomsRandomly(roomsList);
            
        }
        else
        {
            floor = CreateSimpleRooms(roomsList);
        }

        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach (var room in roomsList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
        }
        // floor.UnionWith(corridors);
        

        // 어찌보면 floor는 각 방의 데이터를 모두 소유하고 있다
        // 각 방을 분류하는 기능은 따로 없다
        // 방을 생성할 때 처리를 하는 것이 좋다 보인다.
        // 몬스터를 소환하기 위해서는 해당 값을 받아 제작할 수 있을 듯 하다.
        // 몬스터와 간단한 오브젝트 (이동과 상관없는) 배치를 위해서 규칙을 정립할 필요가 있다.

        // 몬스터는 8방향을 확인 후 모두 땅임이 확인 된다면 배치를 한다
        // 오브젝트는 8방향을 확인 후 5개 이상 땅임이 확인 된다면 배치를 한다

        // 배치 개수는 '난이도' SO를 통해서 조절이 가능하도록 하자.

        
        SpawnPrefabs(floor, count);
        SpawnPlayer(playerSpawnRoom);

        List<List<Vector2Int>> corridors = ConnectRooms(roomCenters);
        for (int i = 0; i < corridors.Count; i++)
        {
            corridors[i] = IncreaseCorridorBrush3By3(corridors[i]);
            floor.UnionWith(corridors[i]);
        }

        foreach (var position in playerSpawnRoom)
        {
            floor.Add(position);
        }

        tilemapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private void SpawnPlayer(HashSet<Vector2Int> playerSpawnRoom)
    {
        List<Vector2Int> pos = playerSpawnRoom.ToList<Vector2Int>();
        Vector2Int spawnPos = pos[UnityEngine.Random.Range(0, pos.Count)];
        player.position = (Vector3Int)spawnPos;
        Camera.main.transform.position = (Vector3Int)spawnPos;
    }

    private void SpawnPrefabs(HashSet<Vector2Int> floor, Queue<int> queue)
    {
        int cur = 0;
        int i = 0;
        
        MapBalanceSO parameter = mapBalanceParameter[UnityEngine.Random.Range(0, mapBalanceParameter.Length)];
        int spawnMonster = parameter.spawnMonsterNum;
        int spawnObject = parameter.spawnObjectNum;
        
        foreach (var position in floor)
        {
            if(cur <= i && queue.Count > 0)
            {
                parameter = mapBalanceParameter[UnityEngine.Random.Range(0, mapBalanceParameter.Length)];
                spawnMonster = parameter.spawnMonsterNum;
                spawnObject = parameter.spawnObjectNum;
                cur += queue.Dequeue();
            }

            int cnt = 0;

            foreach (var direction in Direction2D.eightDirectionsList)
            {
                var neighbourPosition = position + direction;
                if (floor.Contains(neighbourPosition))
                    cnt ++;
            }

            if(spawnMonster > 0 && cnt == 8 && UnityEngine.Random.value <= parameter.spawnMonsterRate)
            {
                // 대충 소환하는 코드
                // GameObject _monster = Instantiate(monster, transform);
                GameObject _monster = Instantiate(monsterPrefabs[UnityEngine.Random.Range(0, monsterPrefabs.Length)], transform);
                _monster.transform.position = (Vector3Int)position;
                // DBG.DebugerNN.Debug("적기 소환" + position.ToString());
                spawnMonster --;
            }
            else if(spawnObject > 0 && cnt > 4 && UnityEngine.Random.value <= parameter.spawnObjectRate)
            {
                // 대충 소환하는 코드
                // DBG.DebugerNN.Debug("오브젝트 소환" + position.ToString());
                spawnObject --;
            }

            i ++;
        }


    }

    private HashSet<Vector2Int> CreateRoomsRandomly(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomsList.Count; i++)
        {
            var roomBounds = roomsList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
            foreach (var position in roomFloor)
            {
                if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset)
                    && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(position);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> CreateRoomRandomly(BoundsInt room)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        var roomBounds = room;
        var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
        var roomFloor = RunRandomWalk(randomWalkParameters, roomCenter);
        foreach (var position in roomFloor)
        {
            if(position.x >= (roomBounds.xMin + offset) && position.x <= (roomBounds.xMax - offset)
                && position.y >= (roomBounds.yMin + offset) && position.y <= (roomBounds.yMax - offset))
            {
                floor.Add(position);
            }
        }
        return floor;
    }

    private List<Vector2Int> IncreaseCorridorBrush3By3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x ,y));
                }
            }
            // previousDirection = directionFromCell;
        }
        
        return newCorridor;
    }

    private List<List<Vector2Int>> ConnectRooms(List<Vector2Int> roomCenters)
    {
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            List<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            // corridors.UnionWith(newCorridor);
            corridors.Add(newCorridor);
        }
        return corridors;
    }

    private List<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            if(destination.y > position.y)
                position += Vector2Int.up;
            else if(destination.y < position.y)
                position += Vector2Int.down;

            corridor.Add(position);

        }

        while (position.x != destination.x)
        {
            if(destination.x > position.x)
                position += Vector2Int.right;
            else if(destination.x < position.x)
                position += Vector2Int.left;

            corridor.Add(position);
        }

        return corridor;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;

        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }

        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomsList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        foreach (var room in roomsList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }

        return floor;
    }
}
