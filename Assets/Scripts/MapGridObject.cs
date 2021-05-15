using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Represents a single Grid position
 * */
public class MapGridObject {

    public enum Type {
        Empty,
        Mine,
        MineNum_1,
        MineNum_2,
        MineNum_3,
        MineNum_4,
        MineNum_5,
        MineNum_6,
        MineNum_7,
        MineNum_8,
    }

    private Grid<MapGridObject> grid;
    private int x;
    private int y;
    private Type type;
    private bool isRevealed;
    private bool isFlagged;

    public MapGridObject(Grid<MapGridObject> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
        type = Type.Empty;
        isRevealed = false;
    }

    public int GetX() => x;
    public int GetY() => y;

    public Type GetGridType() {
        return type;
    }

    public void SetGridType(Type type) {
        this.type = type;
    }

    public void SetFlagged() {
        isFlagged = true;
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool IsFlagged() {
        return isFlagged;
    }

    public void Reveal() {
        isRevealed = true;
        grid.TriggerGridObjectChanged(x, y);
    }

    public bool IsRevealed() {
        return isRevealed;
    }

    public override string ToString() {
        return type.ToString();
    }

}
