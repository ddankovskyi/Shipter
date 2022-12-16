using UnityEngine;

public class ShipsManager : MonoBehaviour
{

    [SerializeField] int maxShipsCount = 15;

    static ShipsManager _instance;
    public static ShipsManager Instance => _instance;

    int _registeredShipsCount;

    public void ReleaseShip() => _registeredShipsCount--;

    public bool RequestSpawnApproval()
    {
        if (_registeredShipsCount < maxShipsCount)
        {
            _registeredShipsCount++;
            return true;
        }
        return false;
    }

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
        {
            Debug.LogWarning("Multiple instances of ShipsManager");
            enabled = false;
        }
    }
    void OnDestroy()
    {
        if (_instance == this)
            _instance = null;
    }
}
