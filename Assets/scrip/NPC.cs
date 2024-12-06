using System.Collections;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject NPCPanel; // Panel hiển thị
    public GameObject Mission;
    public TextMeshProUGUI MissionContent;
    public GameObject Missioncomplet;
    public TextMeshProUGUI MissioncompletContent;
    public GameObject NPCcomplet;
    public TextMeshProUGUI NPCcompletContent;

    public int kill; // Số lượng kẻ địch cần tiêu diệt
    public int playerkill = 0; // Lượng kẻ địch mà người chơi đã tiêu diệt
    public TextMeshProUGUI NPCContent; // Nội dung NPC

    public string[] content; // Nội dung đối thoại khi nhiệm vụ chưa hoàn thành
    public string[] NPCcompletcontent; // Nội dung khi NPC hoàn thành nhiệm vụ

    private Coroutine coroutine;

    void Start()
    {
        NPCPanel.SetActive(false);
        NPCcomplet.SetActive(false);
        Mission.SetActive(false);
        Missioncomplet.SetActive(false);
        MissionContent.text = $"{playerkill} / {kill}";
        MissioncompletContent.text = "Done";
        NPCcompletContent.text = "";
    }

    void Update()
    {
        // Kiểm tra nếu nhiệm vụ đã hoàn thành và cập nhật bảng nhiệm vụ
        MissionContent.text = $"{playerkill} / {kill}";

        if (playerkill >= kill)
        {
            Mission.SetActive(false);
            Missioncomplet.SetActive(true);
        }

        // Kiểm tra sự thay đổi của số lượng kẻ địch bị tiêu diệt
        if (Enemy.playerKill != playerkill)
        {
            playerkill = Enemy.playerKill; // Cập nhật số lượng kẻ địch đã tiêu diệt
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("va cham");
        if (other.gameObject.CompareTag("Player"))
        {
            // Nếu nhiệm vụ chưa hoàn thành, hiển thị nội dung đối thoại NPCContent
            if (playerkill < kill)
            {
                NPCPanel.SetActive(true); // Hiển thị bảng đối thoại NPC
                coroutine = StartCoroutine(ReadContent());
                NPCcomplet.SetActive(false);
            }
            // Nếu nhiệm vụ đã hoàn thành, hiển thị đối thoại NPC hoàn thành nhiệm vụ
            else if (playerkill == kill) // Đảm bảo kiểm tra `else if` và không thay đổi giá trị `playerkill` ở đây
            {
                Mission.SetActive(false); // Tắt bảng nhiệm vụ
                Missioncomplet.SetActive(true); // Hiển thị bảng nhiệm vụ hoàn thành
                NPCcomplet.SetActive(true); // Hiển thị NPC hoàn thành nhiệm vụ
                coroutine = StartCoroutine(NPCcompletconten());
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCPanel.SetActive(false);
            StopCoroutine(coroutine);
            NPCcomplet.SetActive(false);
        }
    }

    // Coroutine hiển thị nội dung đối thoại NPC khi nhiệm vụ chưa hoàn thành
    private IEnumerator ReadContent()
    {
        foreach (var line in content)
        {
            NPCContent.text = "";
            foreach (var item in line)
            {
                NPCContent.text += item;
                yield return new WaitForSeconds(0.2f); // Thời gian hiển thị mỗi ký tự
            }
            yield return new WaitForSeconds(0.5f); // Thời gian nghỉ giữa các câu
        }
    }

    // Coroutine hiển thị nội dung khi NPC hoàn thành nhiệm vụ
    private IEnumerator NPCcompletconten()
    {
        foreach (var line in NPCcompletcontent)
        {
            NPCcompletContent.text = "";
            foreach (var item in line)
            {
                NPCcompletContent.text += item;
                yield return new WaitForSeconds(0.2f); // Thời gian hiển thị mỗi ký tự
            }
            yield return new WaitForSeconds(0.5f); // Thời gian nghỉ giữa các câu
        }
    }

    // Hàm để kết thúc cuộc trò chuyện
    public void endContent()
    {
        // Ẩn bảng đối thoại NPC
        NPCPanel.SetActive(false);
        Mission.SetActive(true); // Hiển thị bảng nhiệm vụ khi nhấn nút kết thúc
        StopCoroutine(coroutine);
    }

    // Hàm để kết thúc nhiệm vụ và reset số lượng kẻ địch đã tiêu diệt
    public void Endmission()
    {
        // Tắt bảng nhiệm vụ hoàn thành
        Missioncomplet.SetActive(false);
        NPCcomplet.SetActive(false);

        // Reset lại số lượng kẻ địch đã tiêu diệt
        Enemy.playerKill = 0;
        playerkill = 0;

        // Cập nhật lại nội dung bảng nhiệm vụ
        MissionContent.text = $"{playerkill} / {kill}";

        // Nếu cần, bạn có thể kiểm tra xem collider có được kích hoạt lại không.
        Collider npcCollider = GetComponent<Collider>();
        if (npcCollider != null)
        {
            npcCollider.enabled = true; // Đảm bảo collider vẫn được kích hoạt khi trả nhiệm vụ
        }
    }



}
