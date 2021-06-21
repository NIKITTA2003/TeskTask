using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GameManager : MonoBehaviour
{
    public  GameObject Cell;
    public  GameObject BackGround;
    public GameObject TaskText;

    [Serializable]
    public class Task
    {
        [SerializeField]
        public Data[] Database;
    }

    [Serializable]
    public class Data
    {
            [SerializeField]
            public string Task;
            [SerializeField]
            public Sprite sprite;
            [SerializeField]
            public float RotateAngle;
    }

    [SerializeField]
    private Task[] Tasks;

    static List<string> PreviousAnswers;

    Queue<GameMode> gameModes = new Queue<GameMode>();

    //Events
    public delegate void NextLevelEvent(string str);
    public event NextLevelEvent OnNextLevel;

    public delegate void ReloadEvent();
    public event ReloadEvent OnReload;

    public delegate void ReloadStartEvent();
    public event ReloadStartEvent OnReloadStart;

    public delegate void ReloadCompleteEvent();
    public event ReloadCompleteEvent OnReloadComlete;

    private void HandlerFadeIn()
    {
        StartGame();
        OnReloadComlete();
    }


    static string Answer;
    public bool isAnimate;

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Data))]
    public class TaskPropertyDrawer : PropertyDrawer
    {
        private const float space = 5;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var firstLineRect = new Rect(
                x: rect.x,
                y: rect.y,
                width: rect.width,
                height: EditorGUIUtility.singleLineHeight
            );
            DrawMainProperties(firstLineRect, property);

            EditorGUI.indentLevel = indent;
        }

        private void DrawMainProperties(Rect rect,
                                        SerializedProperty human)
        {
            rect.width = (rect.width - 2 * space) / 3;
            DrawProperty(rect, human.FindPropertyRelative("Task"));
            rect.x += rect.width + space;
            DrawProperty(rect, human.FindPropertyRelative("sprite"));
            rect.x += rect.width + space;
            DrawProperty(rect, human.FindPropertyRelative("RotateAngle"));

        }

        private void DrawProperty(Rect rect, SerializedProperty property)
        {
            EditorGUI.PropertyField(rect, property, GUIContent.none);
        }
    }
#endif


    public void StartGame()
    {
        gameModes.Enqueue(new EasyMode(Cell, BackGround, Tasks[UnityEngine.Random.Range(0,Tasks.Length)].Database));
        gameModes.Enqueue(new MediumMode(Cell, BackGround, Tasks[UnityEngine.Random.Range(0, Tasks.Length)].Database));
        gameModes.Enqueue(new HardMode(Cell, BackGround, Tasks[UnityEngine.Random.Range(0, Tasks.Length)].Database));
        NextLevel();
        isAnimate = true;
    }
    public void Reload()
    {
        OnReloadStart();
    }
    public void NextLevel()
    {
        isAnimate = false;
        if (gameModes.Count != 0)
        {
            foreach (Transform child in BackGround.transform)
            {
                Destroy(child.gameObject);
            }
            Answer = gameModes.Dequeue().StartMode();
            
            OnNextLevel(Answer);
        }
        else
            OnReload();
    }
    public bool ClickCell(string value)
    {
        if (value == Answer)
            return true;
        else
            return false;
    }
    
    public interface GameMode
    {
        string StartMode();
    }

    class EasyMode : GameMode
    {
        int HCount = 3;
        int VCount = 1;

        public GameObject C;
        public GameObject B;
        Data[] D;

       public EasyMode(GameObject Cell, GameObject BackGround, Data[] Data)
        {
            C = Cell;
            B = BackGround;
            D = Data;
        }
        public string StartMode()
        {
            return GenerateLevel(HCount, VCount, D, C, B);
        }

    }

    class MediumMode : GameMode
    {
        int HCount = 3;
        int VCount = 2;

        public GameObject C;
        public GameObject B;
        Data[] D;

       public MediumMode(GameObject Cell, GameObject BackGround, Data[] Data)
        {
            C = Cell;
            B = BackGround;
            D = Data;
        }
        public string StartMode()
            {
                return GenerateLevel(HCount, VCount, D, C, B);
            }
        
    }

    class HardMode : GameMode
    {
       static int HCount = 3;
       static int VCount = 3;

        public GameObject C;
        public GameObject B;
        Data[] D;

       public  HardMode(GameObject Cell, GameObject BackGround, Data[] Data)
        {
            C = Cell;
            B = BackGround;
            D = Data;
        }
        public string StartMode()
        {
            return GenerateLevel(HCount, VCount, D, C, B);
        }
    }

    public static string GenerateLevel(int HCount, int VCount, Data[] Data, GameObject Cell, GameObject BackGround)  
    {
        string A;
        Data[] ShuffleData = Shuffle(Data);

        A = ShuffleData[UnityEngine.Random.Range(0, (HCount * VCount ))].Task;
        while (PreviousAnswers.Contains(A))
        {
            A = ShuffleData[UnityEngine.Random.Range(0, (HCount* VCount ))].Task;
        }

        int CurrentItem = 0;
        GameObject CurrentCell;
        
        for (int j = 0; j < VCount; j++)
            for (int i = 0; i < HCount; i++)
            {
                CurrentCell = Instantiate(Cell);
                CurrentCell.transform.SetParent(BackGround.transform);
                CurrentCell.GetComponent<CellButton>().Value = ShuffleData[CurrentItem].Task;
                CurrentCell.GetComponent<CellButton>().Media = ShuffleData[CurrentItem].sprite;
                CurrentCell.GetComponent<CellButton>().Rotation = ShuffleData[CurrentItem].RotateAngle;
                ++CurrentItem;
            }

        return A;
    }

    static T[] Shuffle<T>(T[] Mas)
    {
        T[] arr=(T[])Mas.Clone();
        

        for (int i = arr.Length - 1; i >= 1; i--)
        {
            int j = UnityEngine.Random.Range(0,i-1);

            T tmp = arr[j];
            arr[j] = arr[i];
            arr[i] = tmp;
        }
        return arr;
    }

    void Start()
    {
        ReloadGame RL;
        RL = FindObjectOfType<ReloadGame>();
        RL.OnFadeIn += HandlerFadeIn;
       
        PreviousAnswers = new List<string>();
        
        StartGame();
    }

}
