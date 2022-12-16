using UnityEngine;

public class DestructorForAnim : MonoBehaviour
{
    [SerializeField] GameObject rootNode;
    public void DestroyRoot() => Destroy(rootNode);
}
