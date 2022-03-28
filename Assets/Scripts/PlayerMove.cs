using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMove : MonoBehaviour
{
    public float Speed;
    Rigidbody2D rigid;
    Animator anim;
    private BoxCollider2D boxCollider;
    public LayerMask LayerMask;
    bool isHorizonMove;// ���� �̵� �Ǵ� ����
    float h;
    float v;
    Vector3 dirVec;

    public TextManager manager;
    public Text talkText;
    public GameObject talkPanel;
    public bool isExecution = false;

    GameObject scanObject;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        h = isExecution ? 0 : Input.GetAxisRaw("Horizontal");//�¿�
        v = isExecution ? 0 : Input.GetAxisRaw("Vertical");//����

        bool hDown = isExecution ? false : Input.GetButtonDown("Horizontal");
        bool hUp = isExecution ? false : Input.GetButtonUp("Horizontal");

        bool vDown = isExecution ? false : Input.GetButtonDown("Vertical");
        bool vUp = isExecution ? false : Input.GetButtonUp("Vertical");

        if (hDown)
            isHorizonMove = true;
        else if (vDown)
            isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = h != 0;


        if (anim.GetInteger("VAxisRaw") != v)//����Ű ������ ��
        {
            anim.SetBool("Change", true);
            anim.SetInteger("VAxisRaw", (int)v);
        }

        else if (anim.GetInteger("HAxisRaw") != h) // �¿�Ű ��������
        {
            anim.SetBool("Change", true);
            anim.SetInteger("HAxisRaw", (int)h);
        }
        else
            anim.SetBool("Change", false);


        //����ĳ��Ʈ ����
        if (vDown && v == 1)
            dirVec = Vector3.up;
        else if (vDown && v == -1)
            dirVec = Vector3.down;
        else if (hDown && h == 1)
            dirVec = Vector3.right;
        else if (hDown && h == -1)
            dirVec = Vector3.left;

        //�����̽��ٷ� ������Ʈ���� ��ȣ�ۿ�
        if (Input.GetButtonDown("Jump") && scanObject != null && (scanObject.layer == LayerMask.NameToLayer("DummyObject")))
        {
            if (isExecution)
            {
                isExecution = false;
            }
            else
            {
                isExecution = true;
                talkText.text = "������ �ʿ���� �� ����... �ٸ��� ã�ƺ���.....";
            }
            talkPanel.SetActive(isExecution);
        }

        else if (Input.GetButtonDown("Jump") && scanObject != null && (scanObject.layer == LayerMask.NameToLayer("Object")))
        {
            if (isExecution)
            {
                isExecution = false;
            }
            else
            {
                isExecution = true;
                talkText.text = manager.ObjText;
            }
            talkPanel.SetActive(isExecution);
        }
    }
    void FixedUpdate()
    {

        Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);


        rigid.velocity = moveVec * Speed;

        //����ĳ��Ʈ �۵��ϴ��� Ȯ��
        Debug.DrawRay(rigid.position, dirVec * 0.9f, new Color(1, 0, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.9f, LayerMask.GetMask("DummyObject"));

        if (rayHit.collider != null)//���� ���� �߰��ϸ�
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
            scanObject = null;
    }

}