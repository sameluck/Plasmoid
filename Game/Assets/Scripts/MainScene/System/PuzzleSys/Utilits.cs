using UnityEngine;

public class Utilits
{
    public static Transform FindParent(Transform otherParent)
    {
        while (otherParent.parent != null)
        {
            otherParent = otherParent.parent;
        }
        return otherParent;
    }
    
    public static Transform FindParentWithRigedbody(Transform otherParent)
    {
        while (otherParent.parent != null && otherParent.GetComponent<Rigidbody>() == null)
        {
            otherParent = otherParent.parent;
        }
        return otherParent;
    }

    public static bool CheckForward(Vector3 forwardVector, Vector3 checkedVector)
    {
        Vector3 chekerVect = forwardVector;
        float chek = chekerVect.x * (checkedVector.x) + chekerVect.y * (checkedVector.y) +
                     chekerVect.z * (checkedVector.z);
        return chek < 0;
    }

    
}
