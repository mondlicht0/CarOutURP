using UnityEngine;

public abstract class View : MonoBehaviour
{
    protected Presenter Presenter;

    public void Init(Presenter presenter) 
    {
        Presenter = presenter;
    }
}
