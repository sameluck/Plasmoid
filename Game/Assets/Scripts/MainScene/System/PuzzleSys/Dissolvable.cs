using System.Collections;
using UnityEngine;

public class Dissolvable : MonoBehaviour
{
    
    [SerializeField] private Transform[] itemWithMaterial;

    private Material[] _materialToChange; 
    private float _dissolveSpeed;
    private float _dissolveProcent;
    private bool _dissolve;
    private static readonly int ProcentOfTransparency = Shader.PropertyToID("_ProcentOfTransparency");
    
    private void Start()
    {
        int count = itemWithMaterial.Length;
        _materialToChange = new Material[count];
        
        for (var i = 0; i < itemWithMaterial.Length; i++)
        {
            _materialToChange[i] = itemWithMaterial[i].GetComponent<Renderer>().material;
        }

        _dissolveProcent = 0;
        _dissolve = false;
        _dissolveSpeed = 1f / UnificControl.TimeToTeleport;
    }

    
    public void Dissolve()
    {
        _dissolve = true;
    }
    
    public void Undissolve()
    {
        _dissolve = false;
    }
    
    private void Update()
    {
        
        if (_dissolve)
        {
            if (_dissolveProcent < 1)
            {
                _dissolveProcent += _dissolveSpeed * Time.deltaTime;
                //TODO play dissolve sound
                foreach (var material in _materialToChange)
                {
                    material.SetFloat(ProcentOfTransparency, _dissolveProcent);
                }
            }

        }
        else
        {
            if (_dissolveProcent > 0)
            {
                _dissolveProcent -= _dissolveSpeed * Time.deltaTime;
                //TODO play undissolve sound
                foreach (var material in _materialToChange)
                {
                    material.SetFloat(ProcentOfTransparency, _dissolveProcent);
                }
            }
        }
    
    }
    
}
