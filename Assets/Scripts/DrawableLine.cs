using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawableLine : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LineRenderer eternalLine;

    [SerializeField] private GameObject linePrefab;
    private LineRenderer temporaryLine;

    private void Start()
    {
        //Все точки в LineRenderer должны добавляться при нажатии на экран
        //Поэтому убираем лишние точки если они есть

        if (eternalLine.positionCount != 0)
            eternalLine.positionCount = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 newMousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lastMousePosition = temporaryLine.GetPosition(temporaryLine.positionCount - 1);

            //Чтобы в массив LineRenderer не добавлялись все позиции мыши на экране нужно это ограничение

            if (Vector2.Distance(newMousePosition, lastMousePosition) > .1f)
                ContinueLine(newMousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            ConnectLine();
        }
    }

    private void CreateLine()
    {
        temporaryLine = Instantiate(linePrefab, transform).GetComponent<LineRenderer>();

        temporaryLine.SetPosition(0,
            mainCamera.ScreenToWorldPoint(Input.mousePosition));
    }

    //Эта функция добавляет новые позиции мыши в LineRenderer
    private void ContinueLine(Vector2 newMousePosition)
    {
        temporaryLine.positionCount++;
        temporaryLine.SetPosition(temporaryLine.positionCount - 1, newMousePosition);
    }

    //Насколько я понял Unity проще рендерить одну длинную линию,
    //Чем несколько маленьких
    //Эта функция добавляет точки новой линии к eternalLine
    private void ConnectLine()
    {
        for (int i = 0; i < temporaryLine.positionCount; i++)
        {
            eternalLine.positionCount++;
            eternalLine.SetPosition(eternalLine.positionCount - 1, temporaryLine.GetPosition(i));
        }

        Destroy(temporaryLine.gameObject);
    }
}
