using UnityEngine;
using Reflex;
using Reflex.Scripts;
using Reflex.Scripts.Enums;

public static class ContainerExtension
{
    // TODO: Support all GameObject.Instantiate overloading
    // TODO: Is this a correct way to instantiate a GameObject in Reflex?
    public static GameObject InstantiateGameObject(this Container container, GameObject go, Vector3 position, Quaternion rotation)
    {
        GameObject g = GameObject.Instantiate(go, position, rotation);
        Component c = g.GetComponent<Transform>();
        Debug.Assert(c != null);
        container.InjectMono(c, MonoInjectionMode.Recursive);
        return g;
    }

    public static GameObject InstantiateGameObject(this Container container, GameObject go)
    {
        return container.InstantiateGameObject(go, Vector3.zero, Quaternion.identity);
    }
}
