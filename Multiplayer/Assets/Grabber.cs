using UnityEngine;
using UnityEngine.EventSystems;

public class Grabber : MonoBehaviour
{
    public static Transform _selectedTransform;
    static Vector3 screenPoint, offset;

    [SerializeField] bool _hasMultipleGrab;

    [Header("Object Screen Clamp")]
    [SerializeField] bool _hasScreenClamp;
    public Vector2 _objectWidthAndHeight;
    public static Vector2 _screenWidthAndHeight;

    private void Start() => _screenWidthAndHeight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

    void LateUpdate()
    {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit3D;
            Ray ray3D = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray3D, out hit3D))
            {
                if (hit3D.collider.gameObject.CompareTag("Grab"))
                {
                    // Store the grabbed object's transform and the mouse's offset
                    _selectedTransform = hit3D.transform;
                    screenPoint = Camera.main.WorldToScreenPoint(_selectedTransform.position);
                    offset = _selectedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                }
            }
            else
            {
                // this is only work when the user camera has Orthographic view
                RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit2D.collider != null && hit2D.collider.gameObject.CompareTag("Grab"))
                {
                    // Store the grabbed object's transform and the mouse's offset
                    _selectedTransform = hit2D.transform;
                    screenPoint = Camera.main.WorldToScreenPoint(_selectedTransform.position);
                    offset = _selectedTransform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                }
            }
        }
        else if (Input.GetMouseButton(0) && _selectedTransform != null && !_hasMultipleGrab) _selectedTransform.position = FreeMovement();
        else if (Input.GetMouseButtonUp(0)) _selectedTransform = null;

        if (_hasScreenClamp && _selectedTransform != null) ObjectScreenClamp(_selectedTransform, _objectWidthAndHeight);
    }

    public static Vector3 FreeMovement()
    {
        if (_selectedTransform == null) return Vector3.zero;

        // Get the new position of the grabbed object in world coordinates
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        // Update the grabbed object's x and y position
        return new Vector3(curPosition.x, curPosition.y, curPosition.z);
    }

    public static void ObjectScreenClamp(Transform objectTransform, Vector2 _objectScale)
    {
        objectTransform.position = new Vector3(Mathf.Clamp(objectTransform.position.x, -_screenWidthAndHeight.x + _objectScale.x, _screenWidthAndHeight.x - _objectScale.x), Mathf.Clamp(objectTransform.position.y, -_screenWidthAndHeight.y + _objectScale.y, _screenWidthAndHeight.y - _objectScale.y), objectTransform.position.z);
    }
}