using UnityEngine;

public class DogMovement : MonoBehaviour
{
    private float moveSpeed = 5f; // Tốc độ di chuyển
    private float gridSize = 1f; // Kích thước mỗi ô
    private Vector3 targetPosition; // Vị trí đích di chuyển
    private bool isMoving = false; // Kiểm tra di chuyển
    private float rotationSpeed = 360f; // Tốc độ quay (độ/giây)
    private float rotationAngle = 90f; // Góc quay mỗi lần
    private Vector3 previousPosition; // Vị trí trước đó của con chó
    private bool hasWood = true; // Ban đầu con chó đã gặm thanh gỗ

    [SerializeField] private Transform nosePivot; // Tham chiếu đến GameObject mũi
    [SerializeField] private GameObject level2Prefab; // Tham chiếu đến prefab Level2
    [SerializeField] private AudioSource audioSource; // Tham chiếu đến Audio Source
    [SerializeField] private Transform mouthPosition; // Vị trí miệng để gắn thanh gỗ
    private GameObject heldWood; // Tham chiếu đến thanh gỗ đang gặm
    private GameObject woodPrefab;

    void Start()
    {
        targetPosition = transform.position; // Vị trí ban đầu
        previousPosition = transform.position; // Gán vị trí ban đầu làm vị trí trước đó
        if (nosePivot == null)
        {
            Debug.LogError("NosePivot không được gán! Vui lòng gán GameObject mũi trong Inspector.");
        }
        if (level2Prefab == null)
        {
            Debug.LogError("Level2Prefab không được gán! Vui lòng gán prefab Level2 trong Inspector.");
        }
        if (audioSource == null)
        {
            Debug.LogError("AudioSource không được gán! Vui lòng gán Audio Source trong Inspector.");
        }
        if (mouthPosition == null)
        {
            Debug.LogError("MouthPosition không được gán! Vui lòng gán vị trí miệng trong Inspector.");
        }

        // Tìm thanh gỗ trong Prefab và gắn vào mouthPosition
        heldWood = transform.Find("MouthPosition/Wood")?.gameObject;
        if (heldWood != null)
        {
            heldWood.transform.SetParent(mouthPosition, true); // Gắn vào mouthPosition
            Rigidbody woodRb = heldWood.GetComponent<Rigidbody>();
            if (woodRb != null)
            {
                woodRb.isKinematic = true; // Tắt gravity khi gặm
                woodRb.constraints = RigidbodyConstraints.FreezeAll; // Cố định vị trí và xoay
            }
        }
        else
        {
            Debug.LogError("Thanh gỗ (Wood) không được tìm thấy trong Prefab! Đảm bảo Wood nằm dưới MouthPosition.");
        }

        SnapToNearestDirection(); // Đảm bảo góc ban đầu khớp với 4 hướng chính
    }

    void Update()
    {
        // Di chuyển
        if (!isMoving)
        {
            float horizontal = Input.GetAxisRaw("Horizontal"); // A/D hoặc Left/Right
            float vertical = Input.GetAxisRaw("Vertical");     // W/S hoặc Up/Down
            Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

            if (moveDirection != Vector3.zero)
            {
                Vector3 newPosition = transform.position + moveDirection * gridSize;
                StartMove(newPosition);
            }
        }

        // Di chuyển về vị trí đích
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra đã đến đích chưa
        if (transform.position == targetPosition)
        {
            isMoving = false;
            previousPosition = transform.position; // Cập nhật vị trí trước đó
        }

        // Quay khi nhấn Q (trái) hoặc E (phải)
        if (Input.GetKeyDown(KeyCode.Q)) // Quay trái
        {
            Rotate(-rotationAngle);
        }
        else if (Input.GetKeyDown(KeyCode.E)) // Quay phải
        {
            Rotate(rotationAngle);
        }

        // Thả/nhặt thanh gỗ khi nhấn Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hasWood)
            {
                DropWood(); // Thả thanh gỗ
            }
            else
            {
                PickUpWood(); // Nhặt thanh gỗ
            }
        }
    }

    void StartMove(Vector3 newPosition)
    {
        targetPosition = newPosition;
        isMoving = true;
    }

    void Rotate(float angle)
    {
        if (nosePivot != null)
        {
            Vector3 pivotPoint = nosePivot.position;
            float currentAngle = transform.eulerAngles.y;
            float newAngle = currentAngle + angle;
            float snappedAngle = Mathf.Round(newAngle / 90f) * 90f;
            snappedAngle = Mathf.Repeat(snappedAngle, 360f);

            Quaternion targetRotation = Quaternion.Euler(0, snappedAngle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 offset = nosePivot.localPosition;
            transform.position = pivotPoint - (transform.rotation * offset);
        }
    }

    void SnapToNearestDirection()
    {
        if (nosePivot != null)
        {
            float currentAngle = transform.eulerAngles.y;
            float snappedAngle = Mathf.Round(currentAngle / 90f) * 90f;
            transform.rotation = Quaternion.Euler(0, snappedAngle, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GreenPoint")) // Kiểm tra nếu va vào khối xanh
        {
            ReturnToPreviousPosition(); // Quay lại ô trước đó
        }
        else if (other.CompareTag("Goal")) // Kiểm tra nếu va vào GoalPoint
        {
            StartCoroutine(TransitionToLevel2()); // Bắt đầu coroutine chuyển màn
        }
    }

    void ReturnToPreviousPosition()
    {
        targetPosition = previousPosition; // Đặt đích là vị trí trước đó
        isMoving = true; // Bắt đầu di chuyển
    }

    void PickUpWood()
    {
        if (heldWood == null && mouthPosition != null)
        {
            heldWood = Instantiate(woodPrefab, mouthPosition.position, mouthPosition.rotation, mouthPosition);
            Rigidbody woodRb = heldWood.GetComponent<Rigidbody>();
            if (woodRb != null)
            {
                woodRb.isKinematic = true; // Tắt gravity khi gặm
                woodRb.constraints = RigidbodyConstraints.FreezeAll; // Cố định vị trí và xoay
            }
            hasWood = true;
        }
    }

    void DropWood()
    {
        if (hasWood && heldWood != null)
        {
            Rigidbody woodRb = heldWood.GetComponent<Rigidbody>();
            if (woodRb != null)
            {
                woodRb.isKinematic = false; // Bật gravity khi thả
                woodRb.constraints &= ~RigidbodyConstraints.FreezeAll; // Bỏ cố định
            }
            heldWood.transform.parent = null; // Thả khỏi con chó
            heldWood = null;
            hasWood = false;
        }
    }

    System.Collections.IEnumerator TransitionToLevel2()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play(); // Phát âm thanh
            yield return new WaitForSeconds(audioSource.clip.length); // Đợi âm thanh kết thúc
        }
        else
        {
            yield return new WaitForSeconds(1f); // Đợi 1 giây nếu không có âm thanh
        }

        if (level2Prefab != null)
        {
            // Hủy toàn bộ level cũ (trừ chính con chó)
            Transform parent = transform.parent;
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (child != transform) // Không hủy chính con chó
                    {
                        Destroy(child.gameObject);
                    }
                }
            }

            // Hủy con chó hiện tại
            Destroy(gameObject);

            // Khởi tạo Level2 từ prefab
            GameObject newLevel = Instantiate(level2Prefab, Vector3.zero, Quaternion.identity);

            // Tìm con chó trong Level2Prefab và kích hoạt script (nếu cần)
            DogMovement newDog = newLevel.GetComponentInChildren<DogMovement>();
            if (newDog != null)
            {
                newDog.enabled = true; // Đảm bảo script hoạt động
                // Reset thanh gỗ trong level mới (tùy chọn, có thể thêm logic nhặt lại)
                newDog.hasWood = true;
                newDog.heldWood = newDog.transform.Find("MouthPosition/Wood")?.gameObject;
                if (newDog.heldWood != null)
                {
                    newDog.heldWood.transform.SetParent(newDog.mouthPosition, true);
                    Rigidbody woodRb = newDog.heldWood.GetComponent<Rigidbody>();
                    if (woodRb != null)
                    {
                        woodRb.isKinematic = true;
                        woodRb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                }
            }
        }
    }
}