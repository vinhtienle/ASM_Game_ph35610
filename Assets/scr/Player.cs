using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.U2D.Sprites;
using UnityEngine.SceneManagement;

public class chay : MonoBehaviour
{
    public int coinCount = 0; // Biến để lưu trữ số coin
    public TextMeshProUGUI coinText; // Đối tượng TextMeshPro để hiển thị số coin
    [SerializeField] int coin;
    [SerializeField] TMP_Text vip;
    public Rigidbody2D rb; //private Rigidbody2D rb;
    
    //Khai báo biến tham số
    //Tốc độ di chuyển  
    public float moveSpeed;
    //Tốc độ nhảy
    public float jumpSpeed;
    private Animator animator;
    public float jumpForce = 35f; // Lực nhảy của nhân vật
    public int jumpMax = 5; // Số lần nhảy tối đa
    private int jumpsRemaining; // Số lần nhảy còn lại
    private bool canJump = true; // Biến kiểm tra xem có thể nhảy không
    public GameObject panelEndGame;
   
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "vang")
        {
            coinCount += 10;
            coin += 10;
            Destroy(collision.gameObject);
            UpdateCoinUI();

        }
       if(collision.gameObject.tag == "hoa")
        {
            Destroy(collision.gameObject);

        }
      

    }
    void Start()
    {
        //Gán giá trị mặc định ban đầu cho tốc độ di chuyển, nhảy
        moveSpeed = 7f;
        
        animator = GetComponent<Animator>();
        jumpsRemaining = jumpMax;
       
        //Khi chạy, tự tìm 1 Rigidbody2D để gắn vào,
        //Chỉ tìm các component bên trong nó
        rb = GetComponent<Rigidbody2D>();
        coinText.text = "Coins: " + coinCount.ToString(); // Khởi tạo hiển thị số coin

    }

    // Update is called once per frame
    void Update()
    {

       
        //Nếu phím 
        /*        if (Input.GetKeyDown(KeyCode.Space)) playerJump(jumpSpeed);*/

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;
            Jump();
            
            animator.SetBool("isJumping", !canJump);

        }

        vip.SetText("Điểm : " + coin);
       
    }

    private void FixedUpdate()
    {
        playerRun(moveSpeed);
        animator.SetFloat("aVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("bVelocity", rb.velocity.x);
    }

void playerRun(float moveSpeed)
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        // Áp dụng lực nhảy lên nhân vật
        /*rb.velocity = new Vector2(rb.velocity.x, jumpForce);*/
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpsRemaining--; // Giảm số lần nhảy còn lại
        animator.SetTrigger("Jump");
        // Nếu không còn lần nhảy nào, vô hiệu hóa khả năng nhảy
        if (jumpsRemaining == 0)
        {
            canJump = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Reset lại số lần nhảy và khả năng nhảy khi tiếp đất
            jumpsRemaining = jumpMax;
            canJump = true;
            animator.SetBool("isJumping", !canJump);
        }
        else if(collision.gameObject.tag == "vat")
        {
            
            Time.timeScale = 0;
            panelEndGame.SetActive(true);
        
        }
       
       
    }
    public void Restar()
    {
        panelEndGame.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
    void UpdateCoinUI()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }

}
