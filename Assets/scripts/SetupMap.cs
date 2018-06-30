using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupMap : MonoBehaviour {
	[SerializeField] private int tilesWide;
	[SerializeField] private int tilesLong;
	[SerializeField] private float widthOffset;
	[SerializeField] private float lengthOffset;
	[SerializeField] private float maxHeight;
	[SerializeField] private Transform tilePrefab;
	[SerializeField] private Transform treePrefab;
	[SerializeField] private Transform waterPrefab;
	[SerializeField] private Texture2D heightMap;
	[SerializeField] private float noiseXFactor;
	[SerializeField] private float noiseZFactor;
	[SerializeField] private float waterHeight;
	[SerializeField] private float seed;

	private float width=0;
	private float length=0;

	private Transform [,] tiles;

	void Awake() {
		if (!heightMap) {
			fillFromPerlinNoise();
		}
		else {
			fillFromHeightMap();
		}
		createWater();
	}

	// Use this for initialization
	void Start () {


	}

	void fillFromHeightMap() {
		//int previousLevel=-1;
		//Color previousColor = Color.black;
		tiles = new Transform[heightMap.width, heightMap.height];
		for (int x=0; x<heightMap.width;x++) {
			for (int z=0; z<heightMap.height;z++) {
				Color c = heightMap.GetPixel(x,z);

				float y=c.r*maxHeight;
				/*
				if (previousLevel >= 0) { // baseline the height
					// use the Red attribute for height
					if (c.r>previousColor.r) {
						level = previousLevel+1;
					}
					else if (c.r<previousColor.r) {
						level = previousLevel-1;
					}
					else {
						level = previousLevel;
					}
				}
				*/
				createTile(x,z,y);
				//previousColor = c;
				//previousLevel = level;
			}
		}
		width = heightMap.width*widthOffset;
		length = heightMap.height*lengthOffset;
	}

	void createWater() {
		if (! waterPrefab) {
			return;
		}
		else {
			Transform obj = Instantiate(waterPrefab, transform);
			obj.transform.localPosition = new Vector3 (0, waterHeight, 0);
			obj.transform.localScale = new Vector3(200,1,200);
		}
	}

	void createTile( int x, int z, float yPos) {
		float xPos = x*widthOffset+(z%2*widthOffset/2);
		float zPos = z*lengthOffset;

		Transform obj = Instantiate(tilePrefab, transform);
		obj.transform.localPosition = new Vector3 (xPos, yPos, zPos);

		float rotate = Mathf.PerlinNoise(xPos*yPos,xPos*yPos) * 7.0F;

		rotate = Mathf.Floor(rotate);

		Vector3 rotation = new Vector3(0,rotate*60,0);
		obj.transform.Rotate(rotation,Space.World);

		tiles[x,z] = obj;
		// to do - finish setting up the tiles
		
		if (tileHasTree(x, z)) {
			Transform treeObj = Instantiate(treePrefab, transform);
			treeObj.transform.localPosition = new Vector3 (xPos, yPos+7, zPos);
		}

	}

	void fillArbitraryTiles() {
		for (int x=0; x<tilesWide; x++) {
			for (int z=0; z<tilesLong; z++) {
				createTile(x,z,0);
			}
		}
	}

	void fillFromPerlinNoise () {
		tiles = new Transform[tilesWide, tilesLong];
		for (int x=0; x<tilesWide; x++) {
			for (int z=0; z<tilesLong; z++) {
				float height = Mathf.PerlinNoise((float)x*noiseXFactor+seed, (float)z*noiseZFactor+seed);
				createTile(x,z,height*maxHeight);
			}
		}
		width = tilesWide*widthOffset;
		length = tilesLong*lengthOffset;
	}

	bool tileHasTree(int x, int z) {
		// just based on simple random number, get more creative later
		return Random.value > 0.5f;
	}

	// Update is called once per frame
	void Update () {

	}
}
