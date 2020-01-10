using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public List<Transform> floorList;
    public List<Transform> wallList;
    public List<Transform> cloudList;
    private float floorSpawnTimer;
    private float floorSpawnTimerMax;
    private float wallSpawnTimer;
    private float wallSpawnTimerMax;
    private float cloudSpawnTimer;
    private float cloudSpawnTimerMax;
    private int floorsSpawned;
    private int floorsPassed;

    private const float FLOOR_MOVE_SPEED = 8f;
    private const float CLOUD_MOVE_SPEED = 3f;
    private const float GAP_WIDTH_EASY = 25f;
    private const float GAP_WIDTH_MEDIUM = 20f;
    private const float GAP_WIDTH_HARD = 18f;
    private const float GAP_LEFT_LIMIT = -35f;
    private const float GAP_RIGHT_LIMIT = 15f;

    private const float FLOOR_SPAWN_YPOS = -75f;
    private const float FLOOR_DESTROY_YPOS = 75f;
    

    private const float WALL_SPAWN_YPOS = -89.5f;
    private const float WALL_DESTROY_YPOS = 85f; 
   
    private const float CLOUD_SPAWN_XPOS = 100f;
    private const float CLOUD_DESTROY_XPOS = -100f;

    private const float PLANE_YPOS = 0f;
    private static Level instance;
    private State state;

    private enum State {
        WaitingToStart,
        Playing,
        GameOver,
    }

    public static Level GetInstance() {
        return instance;
    }

    private void Awake() {
        floorList = new List<Transform>();
        //wallList = new List<Transform>();
        //cloudList = new List<Transform>();          // ??
        floorSpawnTimerMax = 3f;
        wallSpawnTimerMax = 3f;
        cloudSpawnTimerMax = 25f;
        floorsSpawned = 0;
        state = State.WaitingToStart;
    }

    // Start is called before the first frame update
    private void Start() {
        instance = this;
        GameAssets.GetInstance().wind.Play();
    }

    // Update is called once per frame
    void Update() {
        if (floorsPassed == 1) GameAssets.GetInstance().music.Play();
        HandleCloudMovement();
        HandleCloudSpawning();

        if (state == State.Playing) {
            HandleFloorMovement();
            HandleFloorSpawning();
            HandleWallMovement();
            HandleWallSpawning();
        }
    }

    public int getFloorsSpawned() {
        return floorsSpawned;
    }

    public int getFloorsPassed() {
        return floorsPassed;
    }


    // !!!!!!!!!!!! CLOUDS

    private void HandleCloudSpawning() {
        cloudSpawnTimer -= Time.deltaTime;
        if (cloudSpawnTimer < 0) {
            cloudSpawnTimer += cloudSpawnTimerMax;
            CreateCloud(0);
            CreateCloud(50);
            CreateCloud(-50);
        }
    }

    private void HandleCloudMovement() {
        for (int i = 0; i < cloudList.Count; i++) {
            Transform cloudTransform = cloudList[i];
            if (state == State.Playing) {
                cloudTransform.position += new Vector3(-1, 0.8f, 0) * CLOUD_MOVE_SPEED * Time.deltaTime;
            } else {
                cloudTransform.position += new Vector3(-1, 0, 0) * CLOUD_MOVE_SPEED * Time.deltaTime;
            }

            // clean up
            if (cloudTransform.position.x < CLOUD_DESTROY_XPOS) {
                Destroy(cloudTransform.gameObject);
                cloudList.Remove(cloudTransform);
                i--;
            }
        }
    }

    private void CreateCloud(float ypos) {
        Transform cloud = Instantiate(GameAssets.GetInstance().whiteCloud1);
        cloud.position = new Vector3(CLOUD_SPAWN_XPOS, ypos);
        cloudList.Add(cloud);
    }

    // !!!!!!!!!!!! WALLS

    private void HandleWallSpawning() {
        wallSpawnTimer -= Time.deltaTime;
        if (wallSpawnTimer < 0){
            wallSpawnTimer += wallSpawnTimerMax;
            CreateWall();
        }
    }

    private void HandleWallMovement() {
        for (int i = 0; i < wallList.Count; i++){
            Transform wallTransform = wallList[i];
            wallTransform.position += new Vector3(0, 1, 0) * FLOOR_MOVE_SPEED * Time.deltaTime;

            // clean up
            if (wallTransform.position.y > WALL_DESTROY_YPOS){
                Destroy(wallTransform.gameObject);
                wallList.Remove(wallTransform);
                i--;
            }
        }
    }

    private void CreateWall() {
        Transform wall = Instantiate(GameAssets.GetInstance().pfbgBrick);
        if(floorsSpawned % 2 == 0) {
            wall.localScale = new Vector3(-15, 15, 0);
        }
        wall.position = new Vector3(0, WALL_SPAWN_YPOS);
        wallList.Add(wall);
    }

    // !!!!!!!!!!!!
    // !!!!!!!!!!!! FLOORS

    private void HandleFloorSpawning() { 
        floorSpawnTimer -= Time.deltaTime;
        if(floorSpawnTimer < 0){
            floorSpawnTimer += floorSpawnTimerMax;
            float gapLeftPos = UnityEngine.Random.Range(GAP_LEFT_LIMIT, GAP_RIGHT_LIMIT); 
            CreateFloor(GAP_WIDTH_EASY, gapLeftPos);
        }
    }

    private void HandleFloorMovement() {
        bool passedFloor = false;
        for(int i = 0; i < floorList.Count; i++){
            Transform brickTransform = floorList[i];

            // scoring
            bool isBelowPlane = brickTransform.position.y < PLANE_YPOS;

            // move the floor
            brickTransform.position += new Vector3(0, 1, 0) * FLOOR_MOVE_SPEED * Time.deltaTime;

            //final scoring check
            if (isBelowPlane && brickTransform.position.y >= PLANE_YPOS) passedFloor = true;

            // clean up
            if (brickTransform.position.y > FLOOR_DESTROY_YPOS){
                Destroy(brickTransform.gameObject);
                floorList.Remove(brickTransform);
                i--;
            }
        }

        if (passedFloor) {
            GameAssets.GetInstance().score.Play();
            floorsPassed++;
        }
    }

    private void CreateFloor(float gapWidth, float GapLeftPos) {
        float gapRightPos = GapLeftPos + gapWidth;
        for(float i = -50f; i <= 50f; i += 5f){
            if (!(i > GapLeftPos && i < gapRightPos)){
                CreateBrick(i, FLOOR_SPAWN_YPOS);
            }
        }
        floorsSpawned++;
    }

    private void CreateBrick(float xPos, float yPos) {
        Transform brick = Instantiate(GameAssets.GetInstance().pfBrick);
        brick.position = new Vector3(xPos, yPos);
        floorList.Add(brick);
    }


    // !!!!!!!!!!!!

    public void StartLevel() {
        state = State.Playing;
    }

    public void StopLevel() {
        // TODO: Fix this
        state = State.GameOver;
        GameOverWindow.GetInstance().DisplayGameOverScreen();
    }
}
