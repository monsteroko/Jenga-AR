using UnityEngine;

public class MovePiece : MonoBehaviour
{
    public MeshRenderer _renderer;
    private Rigidbody _rigidbody;
    private RigidbodyConstraints originalConstraints;
    private bool pieceGrabbed;
    private Vector3 screenPoint;
    private Vector3 offset;

    private float maxVelocity = 10f;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        originalConstraints = _rigidbody.constraints;
        pieceGrabbed = false;
        JengaManager.Instance.canMove = true;
    }

    private void Update()
    {
        if (_rigidbody.velocity.magnitude > maxVelocity)
        {
            var v = _rigidbody.velocity;
            _rigidbody.velocity = v.normalized * maxVelocity;
        }

        if (JengaManager.Instance.canMove && pieceGrabbed)
        {
            if (Input.touchCount == 2)
            {
                Touch touch = Input.GetTouch(1);
                if (touch.phase == TouchPhase.Began)
                {
                    transform.Rotate(0, 90, 0);
                }
            }
            if (Input.touchCount == 1)
                DragPiece();
        }
    }
    private void OnMouseExit()
    {
        _renderer.material.color = Color.white;
    }
    private void OnMouseDrag()
    {
        _renderer.material.color = Color.yellow;
    }
    private void OnMouseDown()
    {
        if (Input.touchCount == 1)
        {
            if (!JengaManager.Instance.pieceSelected && JengaManager.Instance.canMove)
            {
                Touch touchbb = Input.GetTouch(0);
                if (touchbb.phase == TouchPhase.Began)
                {
                    pieceGrabbed = true;
                    _rigidbody.freezeRotation = true;
                    _rigidbody.useGravity = false;
                    JengaManager.Instance.pieceSelected = true;

                    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(
                        new Vector3(touchbb.position.x, touchbb.position.y, screenPoint.z));
                }
            }
        }
    }
    private void OnMouseUp()
    {
        if (pieceGrabbed)
        {
            pieceGrabbed = false;
            _rigidbody.freezeRotation = false;
            _rigidbody.useGravity = true;
            _rigidbody.constraints = originalConstraints;
            JengaManager.Instance.pieceSelected = false;

        }
    }
    private void DragPiece()
    {
        Touch touchbb = Input.GetTouch(0);
        _rigidbody.Sleep();
        Vector3 curScreenPoint = new Vector3(touchbb.position.x, touchbb.position.y, screenPoint.z);
        Vector3 translatedPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        Vector3 newPosition = transform.position;
        newPosition = translatedPosition;
        transform.position = newPosition;
    }
}