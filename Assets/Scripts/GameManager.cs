using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGOL {
    public enum OnGameState {
        Initizialing,Running,
    }
    public class GameManager : MonoBehaviour {
        #region Variables

 
        public static GameManager Instance { get; private set; }
        public Cell Cellprefab;


        public Color[] cellColours;

        OnGameState gameState;
        #endregion
        public OnGameState GameState { get => gameState; set => gameState = value; }
        void Awake() {

            //make a singleton
            Instance = this;
            //Instantiate the prefab from editor 
            Cellprefab = Resources.Load<Cell>(typeof(Cell).Name);
            cellColours = new Color[] { Color.yellow, Color.black };

        }


    }
}