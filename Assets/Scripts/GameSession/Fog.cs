using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameSession
{
    public class Fog : ImageFilter
    { 
        
        [SerializeField] private Color _nearColor;
        [SerializeField] private Color _farColor;
        [SerializeField] private float _nearValue;
        [SerializeField] private float _farValue;
        [SerializeField] private float _density;
        protected override void Init()
        {
            
        }
       
        protected override void Tick()
        {
            mat.SetColor("_NearColor", _nearColor);
            mat.SetColor("_FarColor", _farColor);
            mat.SetFloat("_NearValue", _nearValue);
            mat.SetFloat("_FarValue", _farValue);
            mat.SetFloat("_Density", _density);
        }
    }
}