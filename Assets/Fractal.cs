using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {
	public Mesh mesh;
	public Material material;
	public int maxDepth;
	private int depth;
	public float childScale;

	public float maxTwist;

	public float spawnProbability;

	public Mesh[] meshes;

	private Material[] materials;

	public float maxRotationSpeed;

	private float rotationSpeed;

	private void InitializeMaterials () {
		materials = new Material[maxDepth + 1];
		for (int i = 0; i <= maxDepth; i++) {
			float t = i / (maxDepth - 1f);
			t *= t;
			materials[i] = new Material(material);
			materials[i].color = Color.Lerp(Color.white, Color.yellow, t);
		}
		materials[maxDepth].color = Color.magenta;
	}

	// Use this for initialization
	void Start () {
		transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);
		rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
		if (materials == null) {
			InitializeMaterials();
		}
		gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];;
		gameObject.AddComponent<MeshRenderer>().material = materials[depth];
		GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.yellow, (float)depth / maxDepth);
		if (depth < maxDepth) {
			StartCoroutine (CreateChildren());
		}
	}
	private IEnumerator CreateChildren(){
		for (int i = 0; i < childDirections.Length; i++) {
			if (Random.value < spawnProbability) {
				yield return new WaitForSeconds (Random.Range (0.1f, 0.5f));
				new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize (this, i);
			}
		}
	}
	private static Vector3[] childDirections = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back
	};
	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0f, 0f, -90f),
		Quaternion.Euler (0f, 0f, 90f),
		Quaternion.Euler(90f, 0f, 0f),
		Quaternion.Euler(-90f, 0f, 0f)
	};

	private void Initialize(Fractal parent, int childIndex){
		maxTwist = parent.maxTwist;
		maxRotationSpeed = parent.maxRotationSpeed;
		spawnProbability = parent.spawnProbability;
		meshes = parent.meshes;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		depth = parent.depth + 1;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}
	
	// Update is called once per frame
	private void Update () {
		transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

	}
}
