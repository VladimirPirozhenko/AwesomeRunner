using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[Range(-0.1f, 0.1f)]
	public float curveStrengthY = 0.01f;
	[Range(-0.1f, 0.1f)]
	public float curveStrengthX = 0.01f;

	private int curveStrengthXID;
	private int curveStrengthYID;
	private void OnEnable()
    {
		curveStrengthYID = Shader.PropertyToID("_CurveStrength_y");
		curveStrengthXID = Shader.PropertyToID("_CurveStrength_x");
	}

	void Update()
	{
		Shader.SetGlobalFloat(curveStrengthYID, curveStrengthY);
		Shader.SetGlobalFloat(curveStrengthXID, curveStrengthX);
	}
}
