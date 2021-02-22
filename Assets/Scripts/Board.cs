using UnityEngine;

namespace CGOL {
    public class Board : MonoBehaviour {

        [Range((3), 20)]
        [Tooltip("Width of the board")]
        [SerializeField] int _row;

        [Range(3, 20)]
        [Tooltip("Height of the board")]
        [SerializeField] int _column;

        [Range(0.1f, 1.0f)]
        [SerializeField] float GenerationDuration;

        [SerializeField] Transform _board;

        private Cell[,] _cells;

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

                }
            }
        }

        private void CreatingBoard(int i, int j) {
            Transform clone = Instantiate(_board, new Vector3(i, j, 0), Quaternion.identity) as Transform;
            clone.name = "board_Clone (x =" + i.ToString() + ", y=" + j.ToString() + ")";
            clone.transform.parent = this.transform;
        }
    }
}
