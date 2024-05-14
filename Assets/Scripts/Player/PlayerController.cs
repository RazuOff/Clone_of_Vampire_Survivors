using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
  public static PlayerController instance;

  private void Awake()
  {
    instance = this;
  }

  public float moveSpeed;
  public Rigidbody2D theRB;

  public float pickupRange = 0.5f;

  private Animator animator;
  private enum AnimationState { Player_Idle, Player_Moving}
  private AnimationState animationState = AnimationState.Player_Idle;

  //public Weapon activeWeapon;
  public List<Weapon> unassignedWeapons, assignedWeapons;
  public int maxWeapons = 3;
  [HideInInspector]
  public List<Weapon> maxedWeapons = new List<Weapon>();

  public LayerMask enemyMask;
  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
    AddWeapon(Random.Range(0,unassignedWeapons.Count));
    moveSpeed = PlayerStatController.instance.moveSpeed[0].value;
    pickupRange = PlayerStatController.instance.pickupRange[0].value;
    maxWeapons = Mathf.RoundToInt(PlayerStatController.instance.maxWeapons[0].value);


  }

  // Update is called once per frame
  void Update()
  {
    Vector3 moveInput = new Vector3(0f,0f,0f);
    moveInput.x = Input.GetAxisRaw("Horizontal");
    moveInput.y = Input.GetAxisRaw("Vertical");
    if (NNManager.instance.turnOnNeuralNetworkEducation)
    {
      NNPlayerMove();
    }
    else
      PlayerMove(moveInput);
    


  }
  
  void PlayerMove(Vector3 moveInput)
  {     

    if (moveInput == Vector3.zero)
      ChangeAnimation(AnimationState.Player_Idle);
    else
      ChangeAnimation(AnimationState.Player_Moving);

    moveInput.Normalize();
    transform.position += moveInput * moveSpeed * Time.deltaTime;
    
  }
  void NNPlayerMove()
  {    
    RaycastHit2D enemy = Physics2D.CircleCast(transform.position, 5f,transform.position, 0.1f, enemyMask);
    if (enemy)
      theRB.velocity = -(enemy.transform.position - transform.position).normalized * moveSpeed;
    else
      theRB.velocity = Vector3.zero;
  }



  void ChangeAnimation(AnimationState newState)
  {
    if (animationState == newState) return;

    animationState = newState;    
    animator.Play(animationState.ToString());

  }

  public void AddWeapon(int weaponNumber)
  {
    if (weaponNumber < unassignedWeapons.Count)
    {
      assignedWeapons.Add(unassignedWeapons[weaponNumber]);
      unassignedWeapons[weaponNumber].gameObject.SetActive(true);
      unassignedWeapons.RemoveAt(weaponNumber);
    }
  }

  public void AddWeapon(Weapon weapon)
  {
    weapon.gameObject.SetActive(true);
    assignedWeapons.Add(weapon);
    unassignedWeapons.Remove(weapon);
  }
}
