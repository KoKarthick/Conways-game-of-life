using System.Collections;
using UnityEngine;

namespace CGOL {
    public class Board : MonoBehaviour {

        #region Variables

        [Range((3), 50)]
        [Tooltip("Width of the board")]
        [SerializeField] int _row;

        [Range(3, 50)]
        [Tooltip("Height of the board")]
        [SerializeField] int _column;

        [Range(1, 10)]
        [Tooltip("Set the Camera border")]
        [SerializeField] int _camBorder;

        [Range(0.1f, 1.0f)]
        [Tooltip("Set game speed")]
        [SerializeField] float _generationDuration;

        [Header("RequiredComp")]
        [SerializeField] Transform _board;

        private Cell[,] _cells;

        Coroutine SimulationRoutine;

        #endregion

        void Awake() {
            _cells = new Cell[_row, _column];
        }
        private void Start() {
            SetupCamera();
            DrawBoard();
            GameManager.Instance.GameState = OnGameState.Initizialing;
        }

        void DrawBoard() {
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    CreatingBoard(i, j);
                    _cells[i, j] = Instantiate(GameManager.Instance.Cellprefab, new Vector3(i, j, 0), Quaternion.identity);
                    _cells[i, j].gameObject.name = "Cell (x =" + i.ToString() + ", y=" + j.ToString() + ")";
                    _cells[i, j].transform.parent = this.transform;
                    _cells[i, j].Initialize(i, j);
                }
            }
        }
        //creating board for each cells
        private void CreatingBoard(int i, int j) {
            Transform clone = Instantiate(_board, new Vector3(i, j, 0), Quaternion.identity) as Transform;
            clone.name = "board_Clone (x =" + i.ToString() + ", y=" + j.ToString() + ")";
            clone.transform.parent = this.transform;
        }

        //setups the camera for players view
        void SetupCamera() {
            Camera.main.transform.position = new Vector3((float)(_row - 1) / 2f, (float)(_column - 1) / 2f, -5f);//-5f camera depth
            //Need to find aspect ratio of the screen
            float aspectRatio = (float)Screen.width / (float)Screen.height;

            float verticalSize = ((float)_column / 2f) + (float)_camBorder;
            float horizontalSize = (((float)_row / 2f) + (float)_camBorder) / aspectRatio;

            Camera.main.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        }
        void UpdateCells() {
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    var neighboursSum = _cells[i, j].CalculateSum(_cells, i, j, _row, _column);
                    var CellState = _cells[i, j].CellState;

                    //if cell wasnt alive changing to alive
                    //if cell was already alive nochange in its state
                    if (CellState == 0 && neighboursSum == 3)
                    {
                        _cells[i, j].nextcellState = _cells[i, j].IsAlive ? NextCellState.NoChange : NextCellState.ALIVE;

                    }
                    //if cell was alive changing to dead
                    //if cell was already dead nochange in its state
                    else if (CellState == 1 && (neighboursSum < 2 || neighboursSum > 3))
                    {
                        _cells[i, j].nextcellState = _cells[i, j].IsAlive ? NextCellState.DEAD : NextCellState.NoChange;
                    }
                    else
                    {
                        _cells[i, j].nextcellState = NextCellState.NoChange;
                    }
                }
            }
        }
        void RefreshBoard() {
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    if (_cells[i, j].nextcellState == NextCellState.DEAD)
                    {
                        _cells[i, j].IsAlive = false;
                    }
                    else if (_cells[i, j].nextcellState == NextCellState.ALIVE)
                    {
                        _cells[i, j].IsAlive = true;
                    }
                }
            }
        }

        private void Update() {

            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return)) &&
                GameManager.Instance.GameState == OnGameState.Initizialing)
            {
                GameManager.Instance.GameState = OnGameState.Running;
                StartSimulation();
            }
        }
        void StartSimulation() {
            if (SimulationRoutine != null)
            {
                StopCoroutine(SimulationRoutine);
            }
            SimulationRoutine = StartCoroutine(RunSimulation());

        }
        IEnumerator RunSimulation() {

            while (true)
            {
                UpdateCells();
                RefreshBoard();
                yield return new WaitForSeconds(_generationDuration);
            }
        }
    }
}
