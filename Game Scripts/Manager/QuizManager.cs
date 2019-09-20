using UnityEngine;
using QuizRunner.Enum;

public class QuizManager : Singleton<QuizManager>
{
    //Struct definition
    [System.Serializable]
    public struct QuizData
    {
        public string name;
        public QuizType quizType;
        [Multiline] public string quizText;
        public Sprite quizSprite;
        public int ans; //Map.instance.mapLimit 값을 따라야 합니다.
    }

    private int nextProblem = 0;

    [SerializeField] private float quizDelay = 2f;

    public QuizData[] quizData;

    private void OnEnable()
    {
        GameManager.GameManagerInitialized += OnInitialized;
    }
    private void OnDisable()
    {
        GameManager.GameManagerInitialized -= OnInitialized;
    }

    private void OnInitialized()
    {
        RegisterProblem();
    }

    public void RegisterProblem()
    {
        //Elect quiz number
        for (; ; )
        {
            if (quizData.Length <= 1)
            {
                nextProblem = Random.Range(0, quizData.Length);
                break;
            }
            var tmp = Random.Range(0, quizData.Length);
            if (tmp != nextProblem)
            {
                nextProblem = tmp;
                break;
            }
        }

        //Check if problem is unique
        if (quizData[nextProblem].quizType == QuizType.sequence)
        {
            Problem1(ref quizData[nextProblem].quizText, ref quizData[nextProblem].ans);
        }

        //Show quiz banner
        if (quizData[nextProblem].quizSprite == null)
        {
            UI.instance.ShowQuizBanner(quizData[nextProblem].quizText);
        }
        else
        {
            UI.instance.ShowQuizBanner(null, quizData[nextProblem].quizSprite);
        }

        //print("답을 기다리는 중입니다. 정답은 " + (nextProblem + 1) + " 입니다.");
    }
    public bool ChallengeProblem(int ans)
    {
        Invoke("RegisterProblem", quizDelay);

        if (ans == quizData[nextProblem].ans)
        {
            SkillManager.instance.UpdateSkill(1, true);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnGameOver()
    {

    }

    #region Quiz Method
    public void Problem1(ref string text, ref int ans)
    {
        int index = Random.Range(0, 500);
        text = "네모안에 들어갈 숫자는?\n";
        int n = 1;
        for (int i = 0; i < 5; i++)
        {
            text += (index - n) + ", ";
            index -= n;
            n++;
        }
        text += "□\n";
        ans = Random.Range(0, Map.instance.mapLimit);
        int real = index - n;
        var distance = Random.Range(1, 20);
        for (int i = 0; i < Map.instance.mapLimit; i++)
        {
            if (ans == i)
            {
                text += (i + 1) + "번 : " + real + "  ";
            }
            else
            {
                text += (i + 1) + "번 : " + (real + ((i - 2) * distance)) + "  ";
            }
        }
    }
    #endregion
}