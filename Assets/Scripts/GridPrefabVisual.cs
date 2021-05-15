using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class GridPrefabVisual : MonoBehaviour {

    public static GridPrefabVisual Instance { get; private set; }

    [SerializeField] private Transform pfGridPrefabVisualNode;
    [SerializeField] private Sprite flagSprite;
    [SerializeField] private Sprite mineSprite;

    private List<Transform> visualNodeList;
    private Transform[,] visualNodeArray;
    private Grid<MapGridObject> grid;
    private bool revealEntireMap;
    private bool updateVisual;

    private void Awake() {
        Instance = this;
        visualNodeList = new List<Transform>();
    }

    public void SetRevealEntireMap(bool revealEntireMap) {
        this.revealEntireMap = revealEntireMap;
        UpdateVisual(grid);
    }

    public void Setup(Grid<MapGridObject> grid) {
        this.grid = grid;
        visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight()];

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                Vector3 gridPosition = new Vector3(x, y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f;
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }

        HideNodeVisuals();
        UpdateVisual(grid);

        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }

    private void Update() {
        if (updateVisual) {
            updateVisual = false;
            UpdateVisual(grid);
        }
    }

    private void Grid_OnGridObjectChanged(object sender, Grid<MapGridObject>.OnGridObjectChangedEventArgs e) {
        updateVisual = true;
    }

    public void UpdateVisual(Grid<MapGridObject> grid) {
        HideNodeVisuals();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                MapGridObject gridObject = grid.GetGridObject(x, y);
                
                Transform visualNode = visualNodeArray[x, y];
                visualNode.gameObject.SetActive(true);
                SetupVisualNode(visualNode, gridObject);
            }
        }
    }

    private void HideNodeVisuals() {
        foreach (Transform visualNodeTransform in visualNodeList) {
            visualNodeTransform.gameObject.SetActive(false);
        }
    }

    private Transform CreateVisualNode(Vector3 position) {
        Transform visualNodeTransform = Instantiate(pfGridPrefabVisualNode, position, Quaternion.identity);
        return visualNodeTransform;
    }

    private void SetupVisualNode(Transform visualNodeTransform, MapGridObject mapGridObject) {
        SpriteRenderer iconSpriteRenderer = visualNodeTransform.Find("iconSprite").GetComponent<SpriteRenderer>();
        TextMeshPro indicatorText = visualNodeTransform.Find("mineIndicatorText").GetComponent<TextMeshPro>();
        Transform hiddenTransform = visualNodeTransform.Find("hiddenSprite");

        if (mapGridObject.IsRevealed() || revealEntireMap) {
            // Node is revealed
            hiddenTransform.gameObject.SetActive(false);

            switch (mapGridObject.GetGridType()) {
            default:
            case MapGridObject.Type.Empty:
                indicatorText.gameObject.SetActive(false);
                iconSpriteRenderer.gameObject.SetActive(false);
                break;
            case MapGridObject.Type.Mine:
                indicatorText.gameObject.SetActive(false);
                iconSpriteRenderer.gameObject.SetActive(true);
                iconSpriteRenderer.sprite = mineSprite;
                break;
            case MapGridObject.Type.MineNum_1:
            case MapGridObject.Type.MineNum_2:
            case MapGridObject.Type.MineNum_3:
            case MapGridObject.Type.MineNum_4:
            case MapGridObject.Type.MineNum_5:
            case MapGridObject.Type.MineNum_6:
            case MapGridObject.Type.MineNum_7:
            case MapGridObject.Type.MineNum_8:
                indicatorText.gameObject.SetActive(true);
                iconSpriteRenderer.gameObject.SetActive(false);
                switch (mapGridObject.GetGridType()) {
                default:
                case MapGridObject.Type.MineNum_1: indicatorText.SetText("1"); indicatorText.color = UtilsClass.GetColorFromString("2F58EF"); break;
                case MapGridObject.Type.MineNum_2: indicatorText.SetText("2"); indicatorText.color = UtilsClass.GetColorFromString("4DE700"); break;
                case MapGridObject.Type.MineNum_3: indicatorText.SetText("3"); indicatorText.color = UtilsClass.GetColorFromString("E53144"); break;
                case MapGridObject.Type.MineNum_4: indicatorText.SetText("4"); indicatorText.color = UtilsClass.GetColorFromString("000000"); break;
                case MapGridObject.Type.MineNum_5: indicatorText.SetText("5"); indicatorText.color = UtilsClass.GetColorFromString("000000"); break;
                case MapGridObject.Type.MineNum_6: indicatorText.SetText("6"); indicatorText.color = UtilsClass.GetColorFromString("000000"); break;
                case MapGridObject.Type.MineNum_7: indicatorText.SetText("7"); indicatorText.color = UtilsClass.GetColorFromString("000000"); break;
                case MapGridObject.Type.MineNum_8: indicatorText.SetText("8"); indicatorText.color = UtilsClass.GetColorFromString("000000"); break;
                }
                break;
            }
        } else {
            // Node is hidden
            hiddenTransform.gameObject.SetActive(true);

            if (mapGridObject.IsFlagged()) {
                iconSpriteRenderer.gameObject.SetActive(true);
                iconSpriteRenderer.sprite = flagSprite;
            } else {
                iconSpriteRenderer.gameObject.SetActive(false);
            }
        }
    }
    
}

