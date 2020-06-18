using System.Collections.Generic;
using System;

public class Graph {

    private static readonly Dictionary<int, int[]> boardGraph = new Dictionary<int, int[]>() {
        {0, new int[] {1,2,4,5,6,7,11}},
        {1, new int[] {0,2,3,4} },
        {2, new int[] {0,1,3,6,14}},
        {3,new int[] {1,2,4,10,14,19,20}},
        {4,new int[] {0,1,3,5,20,21}},
        {5,new int[] {0,4,21}},
        {6, new int[] {0,2,11,13,14,17}},
        {7, new int[] {0,8,9,11,15}},
        {8, new int[] {7,9,11}},
        {9, new int[] {7,8,15}},
        {10, new int[] {3,14,18,19}},
        {11, new int[] {0,6,7,8,12,13}},
        {12, new int[] {11,13}},
        {13, new int[] {6,11,12,16,17}},
        {14, new int[] {2,3,6,10,17,18}},
        {15, new int[] {7,9}},
        {16, new int[] {13,17,32,36,38,48}},
        {17, new int[] {6,13,14,16,18,36}},
        {18, new int[] {10,14,17,19,28,36,72}},
        {19, new int[] {3,10,18,20,22,23,72}},
        {20, new int[] {3,4,19,21,22}},
        {21, new int[] {4,5,20,22,24}},
        {22, new int[] {19,20,21,23,24,}},
        {23, new int[] {19,22,24,25,31,34,35,72}},
        {24, new int[] {21,22,23,25}},
        {25, new int[] {23,24,26,27,31}},
        {26, new int[] {25,27}},
        {27, new int[] {25,26,31}},
        {28, new int[] {18,29,36,38,72}},
        {29, new int[] {28,30,34,72}},
        {30, new int[] {29,33,34,35}},
        {31, new int[] {23,25,27,33,35}},
        {32, new int[] {16,38}},
        {33, new int[] {30,31,35}},
        {34, new int[] {23,29,30,35,72}},
        {35, new int[] {23,30,31,33,34}},
        {36, new int[] {16,17,18,28,38}},
        {37, new int[] {41}},
        {38, new int[] {16,28,32,36,39}},
        {39, new int[] {38,40,42,43}},
        {40, new int[] {39,41}},
        {41, new int[] {37,40}},
        {42, new int[] {39,43,44}},
        {43, new int[] {39,42,44,45,71}},
        {44, new int[] {42,43,45,46}},
        {45, new int[] {43,44,46,64,65}},
        {46, new int[] {44,45,47,64}},
        {47, new int[] {46,48,53,54,56}},
        {48, new int[] {16,47,49,50,51,53}},
        {49, new int[] {48,50}},
        {50, new int[] {48,49,51,52}},
        {51, new int[] {48,50,52,53,55}},
        {52, new int[] {50,51,55}},
        {53, new int[] {47,48,51,54,55}},
        {54, new int[] {47,53,55,56,57}},
        {55, new int[] {51,52,53,54,57}},
        {56, new int[] {47,54,57,63}},
        {57, new int[] {54,55,56,58,59,63}},
        {58, new int[] {57,59,60,61,62,63}},
        {59, new int[] {57,58,60}},
        {60, new int[] {58,59,62}},
        {61, new int[] {58,62,63,64}},
        {62, new int[] {58,60,61}},
        {63, new int[] {56,57,58,61,64}},
        {64, new int[] {45,46,61,63,65}},
        {65, new int[] {45,64,66}},
        {66, new int[] {65,67}},
        {67, new int[] {66,68}},
        {68, new int[] {67,69}},
        {69, new int[] {68,70}},
        {70, new int[] {69,81}},
        {71, new int[] {43}},
        {72, new int[] {18,19,23,28,29,34}},
        {80, new int[] {}},
        {81, new int[] {70,82}},
        {82, new int[] {81,84}},
        {83, new int[] {}},
        {84, new int[] {82}}
    };

    public static readonly Dictionary<int, int> monsterGraph = new Dictionary<int, int>() {
        {0,0},
        {1,0},
        {2,0},
        {3,1},
        {4,0},
        {5,0},
        {6,0},
        {7,0},
        {8,7},
        {9,7},
        {10,3},
        {11,0},
        {12,11},
        {13,6},
        {14,2},
        {15,7},
        {16,13},
        {17,6},
        {18,14},
        {19,3},
        {20,3},
        {21,4},
        {22,19},
        {23,19},
        {24,21},
        {25,24},
        {26,25},
        {27,25},
        {28,18},
        {29,28},
        {30,29},
        {31,23},
        {32,16},
        {33,30},
        {34,23},
        {35,23},
        {36,16},
        {37,41},
        {38,16},
        {39,38},
        {40,39},
        {41,40},
        {42,39},
        {43,39},
        {44,42},
        {45,43},
        {46,44},
        {47,46},
        {48,16},
        {49,48},
        {50,48},
        {51,48},
        {52,50},
        {53,47},
        {54,47},
        {55,51},
        {56,47},
        {57,54},
        {58,57},
        {59,57},
        {60,59},
        {61,58},
        {62,58},
        {63,56},
        {64,45},
        {65,45},
        {66,65},
        {67,66},
        {68,67},
        {69,68},
        {70,69},
        {71,43},
        {72,18},
        {80,80},
        {81,70},
        {82,81},
        {83,83},
        {84,82}
    };

    public static int[] getAdjRegions(int currRegion) {
        int[] adj = boardGraph[currRegion];
        return adj;
    }

    public static int BFS(int src, int dest) {
        if (src==dest) return 0;
        Dictionary<int,int> pred = new Dictionary<int,int>();
        List<int> visited = new List<int>();
        visited.Add(src);
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(src);

        while (queue.Count > 0) {
            int vertex = queue.Dequeue();
            if (vertex == dest) {
                int curr = dest;
                int count = 1;
                while(pred[curr] != src) {
                    curr = pred[curr];
                    count++;
                }
                return count;
            }
            foreach(int neighbour in boardGraph[vertex]) {
                if (!visited.Contains(neighbour)) {
                    queue.Enqueue(neighbour);
                    visited.Add(neighbour);
                    pred[neighbour] = vertex;
                }
            }
        }
        return -1;
    }
}