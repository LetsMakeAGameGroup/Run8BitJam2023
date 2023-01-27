using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] List<LevelSegment> segments = new List<LevelSegment>();

    //We need to track the player and camera position to do the segment switching
    public Transform player;
    public Transform playerCamera;
    public int segmentSwapIndex;

    public LevelSegmentData SegmentSpawnData;
    public int segmentToSpawnCount;
    public float segmentSwitchXPosition;

    public DangerZone dangerZone;

    public float DangerZoneCurrentDistance;
    public float DangerZoneMinDistance;
    public float DangerZoneMaxDistance;

    public GameObject segmentPrefab;

    public bool firstSegmentSpawned;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < segmentToSpawnCount; i++) 
        {
            GameObject newSegmentObject = Instantiate(segmentPrefab, transform, false);
            newSegmentObject.transform.position = new Vector3(SegmentSpawnData.levelSegmentXExtend * i, 0, 0);

            LevelSegment segment = newSegmentObject.GetComponent<LevelSegment>();

            segment.InitializeSegment(this, SegmentSpawnData);

            segments.Add(segment);
        }

        segmentSwitchXPosition = segments[4].transform.position.x + 9;

    }

    private void Update()
    {
        //Vector2 dangerZonePosition = new Vector2(player.position.x + DangerZoneCurrentDistance, 0);
        //dangerZone.SetDeadZonePosition(dangerZonePosition);

        if (player.position.x >= segmentSwitchXPosition)
        {
            Vector2 PlayerLocalPositionInSegment = segments[4].transform.InverseTransformPoint(player.position);
            Vector3 CameraLocalPositionInSegment = segments[4].transform.InverseTransformPoint(playerCamera.position);
            Vector2 dagnerZoneNewPostion = segments[4].transform.InverseTransformPoint(dangerZone.transform.position);

            //Grabs last two segments and swift them to the front
            segments[4].SetLevelSegmentLocation(new Vector3(SegmentSpawnData.levelSegmentXExtend * 0, 0, 0));
            segments[5].SetLevelSegmentLocation(new Vector3(SegmentSpawnData.levelSegmentXExtend * 1, 0, 0));

            //moves the rest forward
            segments[0].SetLevelSegmentLocation(new Vector3(SegmentSpawnData.levelSegmentXExtend * 2, 0, 0));
            segments[0].ResetLevelSegment();
            segments[1].SetLevelSegmentLocation(new Vector3(SegmentSpawnData.levelSegmentXExtend * 3, 0, 0));
            segments[1].ResetLevelSegment();
            segments[2].SetLevelSegmentLocation(new Vector3(SegmentSpawnData.levelSegmentXExtend * 4, 0, 0));
            segments[2].ResetLevelSegment();
            segments[3].SetLevelSegmentLocation(new Vector3(SegmentSpawnData.levelSegmentXExtend * 5, 0, 0));
            segments[3].ResetLevelSegment();

            //Reorders the list based on their X position
            segments.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));

            //Move player and camera to the calculated position
            player.position = PlayerLocalPositionInSegment;

            //Moves DangerZone to new position
            dangerZone.SetDeadZonePosition(dagnerZoneNewPostion);

            CameraLocalPositionInSegment.z = -10;
            playerCamera.position = CameraLocalPositionInSegment;

        }
    }

    public void ResetAllSegments()
    {
        foreach (LevelSegment levelSegment in segments)
        {
            levelSegment.ResetLevelSegment();
        }
    }
}
