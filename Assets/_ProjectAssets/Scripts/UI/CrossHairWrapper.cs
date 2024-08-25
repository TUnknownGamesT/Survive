using UnityEngine;

public class CrossHairWrapper : MonoBehaviour
{


    #region Singleton


    public static CrossHairWrapper instance;

    private void Awake()
    {
        instance = FindObjectOfType<CrossHairWrapper>();
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion


    [Header("CrossHair")]
    public float maxScaleMultiplayer;

    public float backToNormalSpeed;

    [Header("References")]
    public Transform crossHairOutterCircle;

    private float _scaleMultiplayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetCrossHairPosition();
        if (_scaleMultiplayer > 0)
        {
            _scaleMultiplayer = Mathf.Clamp(_scaleMultiplayer - (backToNormalSpeed * Time.deltaTime), 0, maxScaleMultiplayer); ;
            crossHairOutterCircle.localScale = new Vector3(1 + (1 * _scaleMultiplayer), 1 + (1 * _scaleMultiplayer), 1);
        }
    }

    public void SetCrossHairScale(float scale)
    {
        _scaleMultiplayer = Mathf.Clamp(scale, 0, maxScaleMultiplayer);
    }

    public void IncreaseScaleMultiplayer(float amount)
    {
        _scaleMultiplayer = Mathf.Clamp(_scaleMultiplayer + amount, 0, maxScaleMultiplayer);
    }


    private void SetCrossHairPosition()
    {
        transform.position = Input.mousePosition;
    }
}
