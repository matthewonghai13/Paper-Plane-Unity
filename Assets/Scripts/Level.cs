using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    public List<Transform> floorList;
    public List<Transform> wallList;
    private float floorSpawnTimer;
    private float floorSpawnTimerMax;
    private float wallSpawnTimer;
    private float wallSpawnTimerMax;
    private int floorsSpawned;
    private int floorsPassed;

    private const float FLOOR_MOVE_SPEED = 8f;
    private const float GAP_WIDTH_EASY = 25f;
    private const float GAP_WIDTH_MEDIUM = 20f;
    private const float GAP_WIDTH_HARD = 18f;
    private const float GAP_LEFT_LIMIT = -35f;
    private const float GAP_RIGHT_LIMIT = 15f;
    private const float FLOOR_DESTROY_YPOS = 65f;
    private const float FLOOR_SPAWN_YPOS = -75f;
    private const float WALL_DESTROY_YPOS = 85f; 
    private const float WALL_SPAWN_YPOS = -89.5f;
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
        floorSpawnTimerMax = 3f;
        wallSpawnTimerMax = 3f;
        floorsSpawned = 0;
        state = State.WaitingToStart;
    }

    // Start is called before the first frame update
    private void Start(){
        instance = this;
    }

    // Update is called once per frame
    void Update() {
        if(state == State.Playing) {
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

        if(passedFloor) floorsPassed++;
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

    private void CreateWall() {
        Transform wall = Instantiate(GameAssets.GetInstance().pfbgBrick);
        wall.position = new Vector3(0, WALL_SPAWN_YPOS);
        wallList.Add(wall);
    }


    public void StartLevel() {
        state = State.Playing;
    }

    public void StopLevel() {
        // TODO: Fix this
        state = State.GameOver;
        GameOverWindow.GetInstance().DisplayGameOverScreen();
    }
}
