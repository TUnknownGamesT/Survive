using UnityEngine.InputSystem;

public class PlayerAnimation : AnimationController
{
    private PlayerInput _playerInput;
    private InputAction _inputAction;
    private WeaponAnimations _myWeapon;


    private void OnEnable()
    {
        PlayerHealth.onPlayerDeath += Die;
    }

    private void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= Die;
    }

    protected override void Start()
    {
        base.Start();
        _myWeapon = new WeaponAnimations(this);
        _myWeapon.SetWeaponType(1);
    }

    void Update()
    {
        SetWalkingAnimation();
    }

    public void ChangeWeapon(int hands)
    {
        _myWeapon.SetWeaponType(hands);
    }
    
    public void Shoot()
    {
        _myWeapon.Shoot();
    }
    public void StopShooting()
    {
    }

    public void Reload()
    {
        _myWeapon.StartReload();
    }

    public void ReloadComplete()
    {
        _myWeapon.ReloadComplete();
    }
    
}
