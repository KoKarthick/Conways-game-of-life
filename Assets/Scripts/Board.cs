using UnityEngine;

namespace CGOL {
    public class Board : MonoBehaviour {

        [Range((3), 50)]
        [Tooltip("Width of the board")]
        [SerializeField] int _row;

        [Range(3, 50)]
        [Tooltip("Height of the board")]
        [SerializeField] int _column;

        [Range(0.1f, 1.0f)]
        [SerializeField] float GenerationDuration;

        [SerializeField] Transform _board;

        private Cell[,] _cells;
        float delay = .5f;

        void Awake() {
            _cells = new Cell[_row, _column];
        }
        private void Start() {
            DrawBoard();
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

        private void CreatingBoard(int i, int j) {
            Transform clone = Instantiate(_board, new Vector3(i, j, 0), Quaternion.identity) as Transform;
            clone.name = "board_Clone (x =" + i.ToString() + ", y=" + j.ToString() + ")";
            clone.transform.parent = this.transform;
        }

        void UpdateCells() {
            for (int i = 0; i < _row; i++)
            {
                for (int j = 0; j < _column; j++)
                {
                    var neighboursSum = _cells[i, j].CalculateSum(_cells, i, j, _row, _column);
                    var CellState = _cells[i, j].CellState;
                    if (CellState == 0 && neighboursSum == 3)
                    {
                        _cells[i, j].nextcellState = _cells[i, j].IsAlive ? NextCellState.NoChange : NextCellState.ALIVE;

                    }
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

            GenerationDuration += Time.deltaTime;
            if (GenerationDuration > delay)
            {
                UpdateCells();
                RefreshBoard();
                GenerationDuration = 0;
            }

        }
    }
}
