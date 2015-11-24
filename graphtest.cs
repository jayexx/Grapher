using UnityEngine;
using System.Collections;

public class Grapher2 : MonoBehaviour {
	
	//initial variables
	//sets graph resolution
	[Range(10,100)]
	public int resolution = 100;
	//should set length of graph, may need to be edited later
	[Range(1,10)]
	public float length = 1;
	private int currentResolution;
	private ParticleSystem.Particle[] points;
    public Color graphColor = new Color();
    
	
	//various function types, user input needs to be included
	public enum FunctionOption{
		Linear,
		Exponential,
		Parabola,
		Sine,
		Parabola2
	}
	
	//delegate(method storage) management
	private delegate float FunctionDelegate (Vector3 p);
	private static FunctionDelegate[] functionDelegates = {
		Linear,
		Exponential,
		Parabola,
		Sine,
		Parabola2
	};
	
	public FunctionOption function;
	
	//init
	void Start () {
        graphColor = Color.cyan;
		CreatePoints3D ();
	}
	
	//creates graph
	private void CreatePoints3D() {
		currentResolution = resolution;
		points = new ParticleSystem.Particle[8 * resolution * resolution];
		///int incresolution = length * resolution;
		float increment = 1f / (resolution - 1);
		int i = 0;
		for(int x = -resolution; x < resolution; x++){
            for (int z = -resolution; z < resolution; z++)
            {
                Vector3 p = new Vector3(x * increment, 0f, z * increment);
                points[i].position = p;
                points[i].color = graphColor;
                points[i++].size = 0.1f;
                points[i].velocity = new Vector3(0f, 0f, 0f);
                points[i].lifetime = 0.99f;
                //GetComponent<ParticleSystem>().Emit (points [i].position, points [i].velocity, points [i].size, points [i].lifetime, points [i].color);
            }
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (currentResolution != resolution || points == null) {
			CreatePoints3D();
		}
		FunctionDelegate f = functionDelegates [(int)function];
		for (int i = 0; i < points.Length; i++) {
			Vector3 p = points[i].position;
			p.y = f(p);
			points [i].position = p;
            /*
			Color c = points[i].color;
			c.g = Mathf.Abs(p.y);
			points[i].color = c;
            */
			GetComponent<ParticleSystem>().Emit (points [i].position, points [i].velocity, points [i].size, points [i].lifetime, points [i].color);
		}
		
	}
	
	
	//preset functions for Graph
	private static float Linear (Vector3 p) {
		return p.x;
	}
	
	private static float Parabola (Vector3 p){
		return 4*p.x*p.x - 4*p.x + 1;
	}
	
	private static float Exponential (Vector3 p){
		return p.x * p.x;
	}
	
	private static float Sine (Vector3 p){
		return 0.5f + 0.5f * Mathf.Sin (2 * Mathf.PI * p.x);
	}

	private static float Parabola2 (Vector3 p){
		p.x = 2f * p.x - 1f;
		p.z = 2f * p.z - 1f;
		return 1f - p.x * p.x * p.z * p.z;
	}
}
