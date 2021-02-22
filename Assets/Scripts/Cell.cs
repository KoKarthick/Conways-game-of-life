using UnityEngine;

namespace CGOL {

    public enum NextCellState : byte { NoChange, DEAD, ALIVE }
    public class Cell : MonoBehaviour {
        private SpriteRenderer _renderer;
        private bool _isAlive;
        private int _cellState;

        public NextCellState nextcellState { get; set; }
        int X, Y;

        public bool IsAlive {
            get { return _isAlive; }
            set {
                _isAlive = value;
                _cellState = _isAlive ? 1 : 0;
                _renderer.color = GameManager.Instance.cellColours[_cellState];
            }
        }

        public int CellState { get => _cellState; set => _cellState = value; }

        public void Initialize(int x, int y) {

            _renderer = GetComponent<SpriteRenderer>();
            IsAlive = (Random.Range(0, 2)) == 0;
            nextcellState = NextCellState.NoChange;
            this.X = x;
            this.Y = y;
        }

        public int CalculateSum(Cell[,] cells, int x, int y, int row, int col) {
            int sum = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                   var a = (x + i + col) % col;
                    var b = (y + j + row) % row;
                    sum += cells[a, b]._cellState;
                }
            }
            sum -= cells[x, y]._cellState;
            return sum;
        }

    }
}