using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/CurveProfile")]
public class CurveProfile : ScriptableObject
{
    [SerializeField]
    private AnimationCurve _timeCurve;
    public AnimationCurve Curve 
    {
        get {  return _timeCurve; }
    }

    protected void Reset()
    {
        _timeCurve = new AnimationCurve
        {
            keys = new Keyframe[]
            {
                new Keyframe( -1.0f,   0.0f,   0.0f,   0.0f ),
                new Keyframe( -0.5f,   1.0f,   0.0f,   0.0f ),
                new Keyframe(  0.0f,   0.0f,   0.5f,   0.5f ),
                new Keyframe(  0.5f,   1.0f,   0.0f,   0.0f ),
                new Keyframe(  1.0f,   0.0f,   0.0f,   0.0f )
            }
        };
    }
}
