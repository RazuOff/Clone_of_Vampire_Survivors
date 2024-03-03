using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix : MonoBehaviour
{
  public int Rows { get; }
  public int Columns { get; }

  public float[,] Cells { get; }

  public Matrix(int rows, int columns)
  {
    Rows = rows;
    Columns = columns;

    Cells = new float[Rows, Columns];
  }
  public static Matrix RandomValues(int rows, int columns)
  {
    Matrix matrix = new Matrix(rows, columns);
    for (int i = 0; i < rows; i++)
      for (int j = 0; j < columns; j++)
        matrix[i, j] = Random.Range(-1, 1); 
    return matrix;
  }
  public Matrix(float[,] cells)
  {
    Rows = cells.GetLength(0);
    Columns = cells.GetLength(1);

    Cells = cells;
  }

  public float this[int i, int j]
  {
    get => Cells[i, j];
    set => Cells[i, j] = value;
  }

  public static Matrix operator *(Matrix a, Matrix b)
  {
    if (a.Columns != b.Rows)
      Debug.LogError("Размеры не стыкуются");
    int m = a.Rows;
    int n = a.Columns;
    int p = b.Columns;
    float[,] cells = new float[m, p];
    for (int i = 0; i < m; i++)
      for (int j = 0; j < p; j++)
      {
        float sum = 0;
        for (int k = 0; k < n; k++)
          sum += a[i, k] * b[k, j];
        cells[i, j] = sum;
      }
    return new Matrix(cells);
  }

  public static Matrix Cross(Matrix a, Matrix b, float mutationChance)
  {
    if (a.Rows != b.Rows || a.Columns != b.Columns)
      Debug.LogError("Размеры не совпадают");

    float[,] cells = new float[a.Rows, a.Columns];
    for (int i = 0; i < a.Rows; i++)
      for (int j = 0; j < a.Columns; j++)
      {
        if (Random.value < 0.5f)
          cells[i, j] = a[i, j];
        else
          cells[i, j] = b[i, j];
        if (Random.value < mutationChance)
          cells[i, j] = Random.value;
      }

    return new Matrix(cells);
  }
}


