const int N = 10;
const int THREADS_NUMBER = 10;

int[,] serialMulRes = new int[N, N];
int[,] threadMulRes = new int[N, N];

int[,] firstMatrix = MatrixGeneratir(N, N);
int[,] secondMatrix = MatrixGeneratir(N, N);

ShowArray(firstMatrix);
System.Console.WriteLine();
ShowArray(secondMatrix);

Console.WriteLine(EqualityMatrix(serialMulRes, threadMulRes));

SerialMatrixMul(firstMatrix, secondMatrix);
PrepareParallelMatrixMuL(firstMatrix, secondMatrix);

int[,] MatrixGeneratir(int rows, int colums)
{
    int[,] res = new int [rows, colums];

    for (int i = 0; i < res.GetLength(0); i++)
    {
        for (int j = 0; j < res.GetLength(1); j++)
        {
            res[i,j] = new Random().Next(-100, 100);
        }
    }
    return res;
}

void SerialMatrixMul(int[,] a, int[,] b)
{
    if(a.GetLength(1) != b.GetLength(0))throw new Exception("Нельзя умножить такие матрицы:(");

    for (int i = 0; i < a.GetLength(0); i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            for (int k = 0; k < b.GetLength(0); k++)
            {
                serialMulRes[i,j] += a[i,k] * b[k,j];
            }
        }
    }
}

void PrepareParallelMatrixMuL(int[,] a, int[,] b)
{
    if(a.GetLength(1) != b.GetLength(0))throw new Exception("Нельзя умножить такие матрицы:(");
    int eachThreadCalc = N / THREADS_NUMBER;
    var threadsList = new List<Thread>();
    for (int i = 0; i < THREADS_NUMBER; i++)
    {
        int startPos = i * eachThreadCalc;
        int endPos = (i + 1) * eachThreadCalc;

        if (i == THREADS_NUMBER -1) endPos = N;
        threadsList.Add(new Thread(() => ParallelMatrixMul(a, b, startPos, endPos)));
        threadsList[i].Start();
    }
    for(int i = 0; i< THREADS_NUMBER; i++)
    {
        threadsList[i].Join();
    }
}

void ParallelMatrixMul(int[,] a, int[,] b , int startPos , int endPos)
{
    for (int i = startPos ; i < endPos; i++)
    {
        for (int j = 0; j < b.GetLength(1); j++)
        {
            for (int k = 0; k < b.GetLength(0); k++)
            {
                serialMulRes[i,j] += a[i,k] * b[k,j];
            }
        }
    }
}

bool EqualityMatrix(int[,] fmatrix, int[,] smatrix)
{
    bool res = true;

    for (int i = 0; i < fmatrix.GetLength(0); i++)
    {
        for (int j = 0; j < fmatrix.GetLength(1); j++)
        {
            res = res && (fmatrix[i,j] == smatrix[i,j]);
        }
    }
    return res;
}

void ShowArray (int[,] array)
{
    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            System.Console.Write(array[i, j] + " ");
        }
        System.Console.WriteLine();
    }
}

