using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private List<string> dialogueText = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        textInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("???");
        if (other.CompareTag("Player"))
        {
            Debug.Log("P");
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("?");
                Debug.Log(dialogueText[0]);
            }
        }
    }


    private void textInit()
    {
        dialogueText.Add("오 드디어 도착하셨군요 구원자님!");
        dialogueText.Add("오시는 동안 몸이 굳으셨을 것 같으니 저 악독한 용을 잡으러 가기 전 몸부터 푸시는게 어떠신지요??");
        dialogueText.Add("먼저 W,A,S,D 를 통해 움직여보시죠! 마우스를 이리 저리 움직이시면 시야도 바꿀 수 있습니다!");
        dialogueText.Add("이제 몸이 조금 풀리셨을 테니 다음으론 갑옷을 입은 저 경비 모형한테 다가가 망치와 방패를 다뤄보시죠");
        dialogueText.Add("좌클릭을 통해 망치를 휘두를 수 있고, 우클릭을 통해 방패로 적의 공격을 막으실 수 있을겁니다!");
        dialogueText.Add("자 그럼 이제 몸이 다 풀리셨으니 마을 밖으로 나가 몬스터들을 무찌르고 동굴안에 살고 있는 무시무시한 용을 잡아 이 재앙을 끝내주십쇼 구원자님!!");
    }
}
