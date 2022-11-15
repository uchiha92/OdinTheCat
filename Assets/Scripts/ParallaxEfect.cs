using UnityEngine;

public class ParallaxEfect : MonoBehaviour
{
    [SerializeField] 
    private float parallaxMultiplier;
    
    private Transform _cameraTransform;
    private Vector3 _previousCameraPosition;
    private float _spriteWidth, _startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        _startPosition = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraPosition = _cameraTransform.position;
        float deltaX = (cameraPosition.x - _previousCameraPosition.x) * parallaxMultiplier;
        float moveAmount = cameraPosition.x * (1 - parallaxMultiplier);
        transform.Translate(new Vector3(deltaX, 0, 0));
        _previousCameraPosition = cameraPosition;

        if (moveAmount > _startPosition + _spriteWidth)
        {
            transform.Translate(new Vector3(_spriteWidth, 0, 0));
            _startPosition += _spriteWidth;
        }else if (moveAmount < _startPosition - _spriteWidth)
        {
            transform.Translate(new Vector3(-_spriteWidth, 0, 0));
            _startPosition -= _spriteWidth;
        }
    }
}
