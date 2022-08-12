namespace Matrix;

using System;

public class MatrixCalc
{
    // Định dạng lại input từ dạng [][]  -->  [,] để dễ xử lí
    private T[,] JaggedToMultidimensional<T>(T[][] jaggedArray)
    {
        int rows = jaggedArray.Length;
        int cols = jaggedArray.Max(subArray => subArray.Length);
        T[,] array = new T[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            cols = jaggedArray[i].Length;
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = jaggedArray[i][j];
            }
        }

        return array;
    }
        
    // Hàm nhập input từ file, sau đó chuyển thành định dạng [,]    
    public double[,] GetMatrix(string s)
    {
        double[][] list = File.ReadAllLines(s)
            .Select(l => l.Split(' ').Select(i => double.Parse(i)).ToArray())
            .ToArray();
        double[,] array = JaggedToMultidimensional(list);
        return array;
    }
    // Hàm in ma trận ra màn hình, làm tròn 3 cs sau dấu ,
    public void Print(double[,]? arr)
    {
        var customCulture = (System.Globalization.CultureInfo)
            System.Globalization.CultureInfo.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ",";
        customCulture.NumberFormat.NumberGroupSeparator = ".";
        int m = arr.GetLength(0);
        int n = arr.GetLength(1);
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(arr[i, j].ToString("#,##0.####", customCulture) + "\t");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }
    // Hàm cộng 2 ma trận cùng kích cỡ, có kiểm tra input
    public double[,]? Addition(double[,] matrix1, double[,] matrix2)
    {
        int m = matrix1.GetLength(0);
        int n = matrix1.GetLength(1);
        int p = matrix2.GetLength(0);
        int q = matrix2.GetLength(1);
        double[,] result = new double[m, n];
        if (m == p && n == q)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }
        }
        else
        {
            Console.WriteLine("Error!");
            Console.WriteLine("2 input matrix is not at the same size");
            return null;
        }

        return result;
    }


    // Hàm nhân 2 ma trận
    // Nhân ma trận 1 vào bên trái ma trận 2
    // Lưu ý thứ tự tham số để nhận kết quả đúng
    // Có kiểm tra input.
    public double[,] Multiply(double[,] matrix1, double[,] matrix2)
    {
        int mt1_row = matrix1.GetLength(0);
        int mt1_col = matrix1.GetLength(1);
        int mt2_row = matrix2.GetLength(0);
        int mt2_col = matrix2.GetLength(1);
        double[,] result = new double[mt1_row, mt2_col];
        if (mt1_col != mt2_row)
        {
            Console.WriteLine("Error!");
            Console.WriteLine("Invalid Input!");
            System.Environment.Exit(0);
        }
        else
        {
            for (int i = 0; i < mt1_row; i++)
            {
                for (int j = 0; j < mt2_col; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < mt1_col; k++)
                    {
                        result[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }
        }

        return result;
    }
    
    //Hàm trừ 2 ma trận cùng cỡ.
    //Có kiểm tra input.

    public double[,]? Subtract(double[,] matrix1, double[,] matrix2)
    {
        int m = matrix1.GetLength(0);
        int n = matrix1.GetLength(1);
        int p = matrix2.GetLength(0);
        int q = matrix2.GetLength(1);
        double[,] result = new double[m, n];
        if (m == p && n == q)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }
        }
        else
        {
            Console.WriteLine("Error!");
            Console.WriteLine("Invalid input!");
            return null;
        }

        return result;
    }
    
    //Hàm biến đổi ma trận tam giác trên.
    public double[,] Triangular(double[,] matrix)
    {
        int key = 0;
        double k;
        int m = matrix.GetLength(0);
        int n = matrix.GetLength(1);
        double[,] result = new double[m, n];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result[i, j] = matrix[i, j];
            }
        }

        while (key < m - 1)
        {
            for (int i = key + 1; i < m; i++)
            {
                if (result[key, key] == 0)
                {
                    double[] temp = new double[result.GetLength(1)];
                    for (int z = 0; z < result.GetLength(1); z++)
                    {
                        temp[z] = result[key, z];
                        result[key, z] = result[i, z];
                        result[i, z] = temp[z];
                    }
                }

                k = result[i, key] / result[key, key];

                for (int j = 0; j < n; j++)
                {
                    result[i, j] -= result[key, j] * k;
                }
            }

            key++;
        }

        return result;
    }
    
    // Hàm trả về hạng của ma trận tham số
    public int Rank(double[,] matrix)
    {
        int m = matrix.GetLength(0);
        int n = matrix.GetLength(1);
        double[,] result = new double[m, n];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                result[i, j] = Triangular(matrix)[i, j];
            }
        }

        int rank = m;
        for (int i = 0; i < m; i++)
        {
            int count = matrix.GetLength(1);
            for (int j = 0; j < n; j++)
            {
                if (result[i, j] == 0) count--;
            }

            if (count == 0) rank--;
        }

        return rank;
    }
    // Hàm trả về ma trận chuyển vị của ma trận tham số
    public double[,] Transpose(double[,] matrix)
    {
        int n = matrix.GetLength(0);
        double[,] result = new Double[n, n];
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                result[i, j] = matrix[j, i];
            }
        }

        return result;
    }
    // Hàm trả về định thức của ma trận đầu vào
    // Có kiểm tra tham số.
    public double Determinant(double[,] matrix)
    {
        double det;
        int m = matrix.GetLength(0);
        int n = matrix.GetLength(1);
        if (m == n)
        {
            double[,] res = new double[m, n];
            res = matrix;
            res = Triangular(res);
            if (Rank(res) == m)
            {
                det = 1;
                for (int i = 0; i < m; i++)
                {
                    det *= res[i, i];
                }
            }
            else return 0;
        }
        else
        {
            Console.WriteLine("Error!");
            Console.WriteLine("The input matrix is not in NxN format!");
            return Convert.ToDouble(null);
        }


        return det;
    }

    // Hàm trả về ma trận xóa bỏ 1 hàng / cột chỉ định
    // Chủ yếu phục vụ kiểm tra tính xác định dương và tính ma trận nghịch đảo
    // Có kiểu tra đầu vòa.
    public double[,] TrimArray(int rowToRemove, int columnToRemove, double[,] originalArray)
    {
        if (columnToRemove > originalArray.GetLength(1) || rowToRemove > originalArray.GetLength(0)
                                                        || rowToRemove < 0 || columnToRemove < 0)
        {
            Console.WriteLine("Invalid input!");
            return originalArray;
        }
        double[,] result = new double[originalArray.GetLength(0) - 1, originalArray.GetLength(1) - 1];

        for (int i = 0, j = 0; i < originalArray.GetLength(0); i++)
        {
            if (i == rowToRemove)
                continue;

            for (int k = 0, u = 0; k < originalArray.GetLength(1); k++)
            {
                if (k == columnToRemove)
                    continue;

                result[j, u] = originalArray[i, k];
                u++;
            }

            j++;
        }

        return result;
    }

    public double[,]? Inverse(double[,] matrix)
    {
        if (Determinant(matrix) != 0)
        {
            int n = matrix.GetLength(0);
            double[,] result = new double[n, n];
            double deter = Determinant(matrix);
            double[,] Pre = new double[n, n];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    double[,] adg = TrimArray(i, j, matrix);
                    Pre[i, j] = Math.Pow(-1, (i + j)) * Determinant(adg);
                }
            }

            result = Transpose(Pre);
            

            return result;
        }
        else
        {
            return null;
        }
    }


    public double[,] Pow(double[,] matrix, int n)
    {
        double[,] result = matrix;

        for (int i = 1; i <= n; i++)
        {
            result = Multiply(result, matrix);
        }

        return result;
    }

    public double[]? Solve(double[,] a, double[,] b)
    {
        int m = a.GetLength(0);
        int n = a.GetLength(1);
        double[] X = new double[a.GetLength(0)];
        double[,] val = new double[m, n + 1];
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                val[i, j] = a[i, j];
            }

            val[i, n] = b[i,0];
        }

        if (Determinant(a) == 0)
        {
            Console.WriteLine("He phuong trinh ko co nghiem duy nhat");
        }

        else
        {
            val = Triangular(val);
            for (int i = n - 1; i >= 0; i--)
            {
                X[i] = 0;
                {
                    for (int j = n - 1; j > i; j--)
                    {
                        val[i, n] -= val[i, j] * X[j];
                    }
                }
                X[i] = val[i, n] / val[i, i];
            }

            for (int i = 0; i < n; i++)
            {
                Console.Write(X[i] + "\t");
            }

            Console.WriteLine();
            return X;
        }

        double[] sub = { 0 };
        return null;
    }
}