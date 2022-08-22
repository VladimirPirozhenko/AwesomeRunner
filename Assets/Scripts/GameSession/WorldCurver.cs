using UnityEngine;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[field: Range(-0.1f, 0.1f)]
	[field: SerializeField] public float CurveStrengthY { get; private set; }

	[field:Range(-0.1f, 0.1f)]
	[field: SerializeField] public float CurveStrengthX { get; private set; }

	[SerializeField] private Transform curveOrigin;

	private int curveStrengthXID;
	private int curveStrengthYID;
	private int curveOriginID;

	
	private void OnEnable()
    {
		curveStrengthYID = Shader.PropertyToID("_CurveStrength_y");
		curveStrengthXID = Shader.PropertyToID("_CurveStrength_x");
		curveOriginID = Shader.PropertyToID("_CurveOrigin");
	}

	public void Tick()
	{
		Shader.SetGlobalFloat(curveStrengthYID, CurveStrengthY);
		Shader.SetGlobalFloat(curveStrengthXID, CurveStrengthX);
		Shader.SetGlobalVector(curveOriginID, curveOrigin.position);
	}

	public void TurnWorldToLeft()
    {
		var startRange = -0.003f;   
		var endRange = 0.003f;   
		var oscilationRange = (endRange - startRange) / 2;
		var oscilationOffset = oscilationRange + startRange;
		//float time = Mathf.Lerp(Time.time * 0.05f, 1);
		CurveStrengthX = Mathf.Lerp(startRange, endRange, Time.time);
	}
	public void SinCurveX()
    {
		var startRange = -0.003f;    //your chosen start value
		var endRange = 0.003f;    //your chose end value
		var oscilationRange = (endRange - startRange) / 2;
		var oscilationOffset = oscilationRange + startRange;

		CurveStrengthX = oscilationOffset + Mathf.Sin(Time.time) * oscilationRange;
		//curveStrengthX = Mathf.Clamp(Mathf.Sin(Time.time),-0.001f,0.001f);	
    }
	public void SinCurveY()
	{
		var startRange = -0.003f;    //your chosen start value
		var endRange = 0.003f;    //your chose end value
		var oscilationRange = (endRange - startRange) / 2;
		var oscilationOffset = oscilationRange + startRange;

		//CurveStrengthY = oscilationOffset + Mathf.Sin(Time.time) * oscilationRange;

		//curveStrengthY = Mathf.Lerp(-0.003f, 0.003f, Time.deltaTime);
		//CurveStrengthY = Mathf.PingPong(Time.time, endRange - startRange);
		//PingPong between 0 and 1
		float time = Mathf.PingPong(Time.time * 0.05f, 1);
		CurveStrengthX = Mathf.Lerp(startRange, endRange, time);
		//curveStrengthX = Mathf.Clamp(Mathf.Sin(Time.time),-0.001f,0.001f);	
	}
}
