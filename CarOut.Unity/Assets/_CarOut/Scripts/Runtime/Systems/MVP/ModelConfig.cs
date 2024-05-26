using UnityEngine;

public class ModelConfig : ScriptableObject 
{
	[field: SerializeField] public string Name { get; private set; }
}
