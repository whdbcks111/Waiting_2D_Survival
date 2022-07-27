using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]
    private string _id;
    public string ID => _id;

    [HideInInspector]
    public bool IsDefault = true;
    
    private bool _isEntered;
    internal void PlayerEnter()
    {
        if(_isEntered)
        {
            PlayerStay2d();
            return;
        }
        PlayerEnter2d();
        _isEntered = true;
    }

    internal void PlayerExit()
    {
        PlayerExit2d();
        _isEntered = false;
    }

    protected virtual void PlayerEnter2d() { }

    protected virtual void PlayerStay2d() { }

    protected virtual void PlayerExit2d() { }
}
