using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGOL {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance { get; private set; }
        public Cell Cellprefab;


        public Color[] cellColours;
        void Awake() {
            Instance = this;
            Cellprefab = Resources.Load<Cell>(typeof(Cell).Name);
            cellColours = new Color[] { Color.yellow, Color.black };

        }


    }
}